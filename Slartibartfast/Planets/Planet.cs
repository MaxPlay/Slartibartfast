using Slartibartfast.Extensions;
using Slartibartfast.Generators;
using Slartibartfast.Math;
using Slartibartfast.Textures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slartibartfast.Planets
{
    public class Planet
    {
        #region Public Fields

        /// <summary>
        /// The distance of the earth to the sun in AU.
        /// </summary>
        public const int EARTH_AGE = 4543;

        /// <summary>
        /// The distance of the earth to the sun in AU.
        /// </summary>
        public const float EARTH_DISTANCE_TO_SUN = 1;

        /// <summary>
        /// The mass of the earth in earth masses (which is 1 obviously).
        /// </summary>
        public const float EARTH_MASS = 1;

        /// <summary>
        /// The radius of the earth from its core to sealevel in kilometers.
        /// </summary>
        public const float EARTH_RADIUS = 6371;

        /// <summary>
        /// The distance of the earth to the sun in AU.
        /// </summary>
        public const int EARTH_TECTONIC_PLATES_COUNT = 7;

        #endregion Public Fields

        #region Private Fields

        /// <summary>
        /// The count of all the texels of the surface. This is a const field, because the value does never change and saves calculationtime.
        /// </summary>
        private const int TEXEL_COUNT = 64800;

        private int age;

        private Atmosphere atmosphere;

        /// <summary>
        /// The distance to the sun in AU. 1 AU = the earths distance to the sun.
        /// </summary>
        private float distanceToSun;

        private float mass;

        private float radius;

        private float sealevel;
        private SurfaceTexel[,] surface;

        private List<TectonicPlate> tectonicPlates;

        private int tectonicPlatesCount;
        private List<WindPoint> windPoints;

        #endregion Private Fields

        #region Public Constructors

        public Planet(PlanetSettings settings, Sun sun)
        {
            this.age = settings.Age;
            this.atmosphere = settings.Atmosphere;
            this.distanceToSun = settings.DistanceToSun;
            this.mass = settings.Mass;
            this.radius = settings.Radius;
            this.tectonicPlatesCount = settings.TectonicPlatesCount;

            // One texel for every degree in geographical coordinates.
            surface = new SurfaceTexel[360, 180];
            GenerateTectonicPlates();
            GenerateSurface();
            GenerateBorders();
            ExtendBorders();
            GenerateHeightValues();
            ApplyPlateTectonics();
            GenerateWind();
            GenerateHeat(sun);
            GenerateMoisture(sun);
            GenerateOcean(sun);
            GenerateBiomes();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// The age of the planet in billion years.
        /// </summary>
        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        /// <summary>
        /// The atmosphere of the planet. If null, the planet does not have an atmosphere.
        /// </summary>
        public Atmosphere Atmosphere
        {
            get { return atmosphere; }
            set { atmosphere = value; }
        }

        public float DistanceToSun
        {
            get { return distanceToSun; }
        }

        /// <summary>
        /// The mass of the planet in earth masses. 1 = The planet has the same mass as the earth.
        /// </summary>
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        /// <summary>
        /// The radius of the planet in kilometers. The value for earth would be 6371.
        /// </summary>
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public float Sealevel
        {
            get { return sealevel; }
            set { sealevel = value; }
        }

        public int TectonicPlatesCount
        {
            get { return tectonicPlatesCount; }
            set { tectonicPlatesCount = value; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Tuple returns three textures, defaulting to null if not set.
        ///
        /// Item1 = Color
        /// Item2 = Height
        /// Item3 = Gloss
        /// </summary>
        /// <param name="outputTextures"></param>
        /// <returns></returns>
        public TextureSet GenerateTextures(TextureType outputTextures)
        {
            Texture color, height, gloss;

            color = outputTextures.Has(TextureType.Color) ? GenerateColorMap() : null;
            gloss = outputTextures.Has(TextureType.Gloss) ? GenerateGlossMap() : null;
            height = outputTextures.Has(TextureType.Height) ? GenerateHeightMap() : null;

            return new TextureSet(color, height, gloss);
        }

        /// <summary>
        /// Gets the adjacent move directions (other plates adjacent to the current plate) as RGB values.
        /// </summary>
        /// <returns></returns>
        public Texture GetAdjacentMoveDirection()
        {
            Color[,] surf = new Color[360, 180];

            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    int r = (int)((surface[x, y].AdjacentDirection.X * 128f) + 128);
                    int g = (int)((surface[x, y].AdjacentDirection.Y * 128f) + 128);
                    surf[x, y] = new Color(255, r, g, 255);
                }
            }

            return new Texture(360, 180, ref surf);
        }

        /// <summary>
        /// Gets a generated Color-Representation of the distances to the borders of tectonic plates
        /// (generated with the "GenerateBorders and ExtendBorders" methods).
        /// </summary>
        /// <returns>
        /// Two-dimensional array representing the distances to the borders of tectonic plates.
        /// </returns>
        public Texture GetDistances()
        {
            Color[,] surf = new Color[360, 180];
            float max = 0;
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    if (surface[x, y].Distance > max)
                        max = surface[x, y].Distance;
                }
            }

            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    int col = max == 0 ? 0 : (int)((surface[x, y].Distance / max) * 255);
                    surf[x, y] = new Color(col, col, col, 255);
                }
            }

            return new Texture(360, 180, ref surf);
        }

        /// <summary>
        /// Returns a Heatmap of the surface, comparable to the temperaturemaps, you can see on television.
        /// </summary>
        /// <returns></returns>
        public Texture GetHeat()
        {
            Color[,] surf = new Color[360, 180];
            float max = 0;
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    if (surface[x, y].Temperature > max)
                        max = surface[x, y].Temperature;
                }
            }

            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    surf[x, y] = Color.Lerp(new Color(0, 0, 255, 255), new Color(255, 0, 0, 255), surface[x, y].Temperature / max);
                }
            }

            return new Texture(360, 180, ref surf);
        }

        /// <summary>
        /// Gets the heightmap as a two-dimensional array of floats.
        /// </summary>
        /// <returns></returns>
        public float[,] GetHeight()
        {
            float[,] surf = new float[360, 180];
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    surf[x, y] = surface[x, y].Height;
                }
            }

            return surf;
        }

        /// <summary>
        /// Generates a Texturerepresentation of the Moisture of a Planet.
        /// </summary>
        /// <returns></returns>
        public Texture GetMoisture()
        {
            Color[,] surf = new Color[360, 180];

            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    surf[x, y] = Color.Lerp(new Color(128, 128, 0, 255), new Color(0, 0, 255, 255), surface[x, y].Moisture);
                }
            }

            return new Texture(360, 180, ref surf);
        }

        /// <summary>
        /// Gets the heightmap as a two-dimensional array of floats with values between 0 and 1. Slightly slower than GetHeight.
        /// </summary>
        /// <returns></returns>
        public float[,] GetNormalizedHeight()
        {
            float[,] surf = GetHeight();

            float maxValue = surf.GetHighestValue();
            float minValue = surf.GetLowestValue();

            float range = maxValue - minValue;

            if (range != 0)
                for (int y = 0; y < 180; y++)
                {
                    for (int x = 0; x < 360; x++)
                    {
                        surf[x, y] = (surf[x, y] - minValue) / (range);
                    }
                }

            return surf;
        }

        /// <summary>
        /// Gets a spherical surface texel. This means that the range to get from is from -180 to 180
        /// on the x-axis and -90 to 90 on the y-axis. Every value exceeding this limit will be
        /// wrapped around the sphere, as if you were moving around it. If you are exiting on top,
        /// you will enter on top, but with a y-shift of 180.
        /// </summary>
        /// <param name="CoordinateX">The x coordinate.</param>
        /// <param name="CoordinateY">The y coordinate.</param>
        /// <returns>Surface Texel of the given location.</returns>
        public SurfaceTexel GetSurfaceTexel(int CoordinateX, int CoordinateY)
        {
            CoordinateY += 90;

            int yRepeat = 0;

            while (CoordinateY < 0)
            {
                yRepeat++;
                CoordinateY += 180;
                CoordinateX += 180;
            }

            while (CoordinateY >= 180)
            {
                yRepeat--;
                CoordinateY -= 180;
                CoordinateX += 180;
            }

            if (yRepeat >= 0)
            {
                if (yRepeat % 2 == 1)
                {
                    CoordinateY = 180 - CoordinateY;
                }
            }
            else
            {
                yRepeat = System.Math.Abs(yRepeat);
                if (yRepeat % 2 == 1)
                {
                    CoordinateY = (179 - CoordinateY);
                }
            }

            while (CoordinateX < -180)
            {
                CoordinateX += 360;
            }

            int x = (CoordinateX + 180) % 360;
            int y = CoordinateY;
            return surface[x, y];
        }

        /// <summary>
        /// Gets a generated Color-Representation of the tectonic plates.
        /// </summary>
        /// <returns>Two-dimensional array representing the tectonic plates.</returns>
        public Texture GetTectonicPlates()
        {
            Color[,] surf = new Color[360, 180];
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    surf[x, y] = tectonicPlates[surface[x, y].TectonicPlateID].DebugColor;
                }
            }

            return new Texture(360, 180, ref surf);
        }

        public Texture GetWindMoveDirection()
        {
            Color[,] surf = new Color[360, 180];

            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    Vector2 windDirection = surface[x, y].WindDirection;
                    if (windDirection.X != 0 && windDirection.Y != 0)
                        windDirection.Normalize();
                    int r = (int)((windDirection.X * 128f) + 128);
                    int g = (int)((windDirection.Y * 128f) + 128);
                    surf[x, y] = new Color(r == 256 ? 255 : r, g == 256 ? 255 : g, 255, 255); //Bugfix, don't think about it.
                }
            }

            return new Texture(360, 180, ref surf);
        }

        /// <summary>
        /// Sets a spherical surface texel. This means that the range to get from is from -180 to 180
        /// on the x-axis and -90 to 90 on the y-axis. Every value exceeding this limit will be
        /// wrapped around the sphere, as if you were moving around it. If you are exiting on top,
        /// you will enter on top, but with a y-shift of 180.
        /// </summary>
        /// <param name="CoordinateX">The x coordinate.</param>
        /// <param name="CoordinateY">The y coordinate.</param>
        /// <param name="texel">      Surface Texel of the given location.</param>
        public void SetSurfaceTexel(int CoordinateX, int CoordinateY, SurfaceTexel texel)
        {
            CoordinateY += 90;

            int yRepeat = 0;

            while (CoordinateY < 0)
            {
                yRepeat++;
                CoordinateY += 180;
                CoordinateX += 180;
            }

            while (CoordinateY >= 180)
            {
                yRepeat--;
                CoordinateY -= 180;
                CoordinateX += 180;
            }

            if (yRepeat >= 0)
            {
                if (yRepeat % 2 == 1)
                {
                    CoordinateY = 180 - CoordinateY;
                }
            }
            else
            {
                yRepeat = System.Math.Abs(yRepeat);
                if (yRepeat % 2 == 1)
                {
                    CoordinateY = (179 - CoordinateY);
                }
            }

            while (CoordinateX < -180)
            {
                CoordinateX += 360;
            }

            int x = (CoordinateX + 180) % 360;
            int y = CoordinateY;
            surface[x, y] = texel;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void GenerateVegetation(BiomeColors colors)
        {
            for (int y = -90; y < 90; y++)
            {
                for (int x = -180; x < 180; x++)
                {
                    SurfaceTexel texel = GetSurfaceTexel(x, y);
                    switch (texel.Biome)
                    {
                        case Biome.Ocean:
                            texel.GeneratedColors = colors[Biome.Ocean];
                            break;

                        case Biome.Grass:
                        case Biome.Mountain:
                        case Biome.Desert:
                        case Biome.Forest:
                        case Biome.Rainforest:
                        case Biome.Plains:
                        case Biome.Ice:
                            int finalColorR = 0, finalColorG = 0, finalColorB = 0;
                            int count = 0;
                            for (int y2 = -3; y2 < 3; y2++)
                            {
                                for (int x2 = -3; x2 < 3; x2++)
                                {
                                    if (x == 0 && y == 0)
                                        continue;
                                    SurfaceTexel nextTexel = GetSurfaceTexel(x + x, y + y);
                                    if (nextTexel.Biome == Biome.Ocean)
                                        continue;

                                    Color c = colors[nextTexel.Biome];
                                    finalColorB += c.B;
                                    finalColorG += c.G;
                                    finalColorR += c.R;
                                    count++;
                                }
                            }
                            if (count > 0)
                            {
                                finalColorR /= count * 2;
                                finalColorG /= count * 2;
                                finalColorB /= count * 2;
                            }

                            finalColorR += (int)(colors[texel.Biome].R / ((count > 0) ? 2 : 1));
                            finalColorG += (int)(colors[texel.Biome].G / ((count > 0) ? 2 : 1));
                            finalColorB += (int)(colors[texel.Biome].B / ((count > 0) ? 2 : 1));

                            texel.GeneratedColors = new Color(finalColorR, finalColorG, finalColorB);
                            break;
                    }
                    SetSurfaceTexel(x, y, texel);
                }
            }
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Raises and lowers the terrain according to the plates
        /// </summary>
        /// <param name="limit"></param>
        private void ApplyPlateTectonics(int limit = 25)
        {
            for (int y = -90; y < 90; y++)
            {
                for (int x = -180; x < 180; x++)
                {
                    SurfaceTexel texel = GetSurfaceTexel(x, y);
                    Vector2 ownDirection = tectonicPlates[texel.TectonicPlateID].MoveDirection;
                    Vector2 adjacentDirection = texel.AdjacentDirection;
                    // if (adjacentDirection.Length() == 0)
                    //continue;

                    Vector2 cumulatedDirection = ownDirection + adjacentDirection;
                    float multiplier = cumulatedDirection.Length();

                    if (y < 0)
                        multiplier = ((y + 90) / 90f) * multiplier + 1 - ((y + 90) / 90f);

                    if (y > 0)
                        multiplier = ((180 - (y + 90)) / 90f) * multiplier + 1 - ((180 - (y + 90)) / 90f);

                    multiplier--;
                    multiplier *= 0.1f;
                    multiplier++;
                    texel.Height *= multiplier;
                    texel.Height += tectonicPlates[texel.TectonicPlateID].HeightModifier * 0.01f;
                    SetSurfaceTexel(x, y, texel);
                }
            }
        }

        /// <summary>
        /// This method extends the borders until either a certain limit is reached or the whole area
        /// is filled. This is used as a falloff for the plates that work together. The nearer two
        /// plates are, the more they have impact on each other.
        /// </summary>
        /// <param name="limit">
        /// The limit of the extended border. This means that the extension never excedes over this
        /// value. The defaulvalue is 25.
        /// </param>
        private void ExtendBorders(int limit = 25)
        {
            if (this.tectonicPlates.Count < 2)
                return;

            if (limit <= 0)
                limit = 1;

            int emptyTiles = 180 * 360;
            for (int y = -90; y < 90; y++)
            {
                for (int x = -180; x < 180; x++)
                {
                    SurfaceTexel texel = GetSurfaceTexel(x, y);
                    if (texel.Distance > 0)
                        emptyTiles--;
                }
            }

            int targetheight = 1;
            int limitheight = limit;
            while (emptyTiles > 0)
            {
                //Console.WriteLine(emptyTiles);

                for (int y = -90; y < 90; y++)
                {
                    for (int x = -180; x < 180; x++)
                    {
                        SurfaceTexel texel = GetSurfaceTexel(x, y);
                        int dist = texel.Distance;

                        if (dist > 0)
                            continue;

                        SurfaceTexel[] dists = new SurfaceTexel[4];

                        dists[0] = GetSurfaceTexel(x - 1, y);
                        dists[1] = GetSurfaceTexel(x + 1, y);
                        dists[2] = GetSurfaceTexel(x, y - 1);
                        dists[3] = GetSurfaceTexel(x, y + 1);

                        List<Vector2> heightreached = new List<Vector2>();
                        for (int i = 0; i < 4; i++)
                        {
                            if (dists[i].Distance == targetheight)
                            {
                                heightreached.Add(dists[i].AdjacentDirection);
                            }
                        }

                        if (heightreached.Count > 0)
                        {
                            if (targetheight == limitheight)
                                texel.Distance = limitheight;
                            else
                                texel.Distance = targetheight + 1;
                            emptyTiles--;

                            Vector2 adjacentMoveDirection = Vector2.Zero;
                            for (int i = 0; i < heightreached.Count; i++)
                            {
                                adjacentMoveDirection += heightreached[i] / (float)heightreached.Count;
                            }

                            texel.AdjacentDirection = adjacentMoveDirection * (1f - targetheight / (float)limitheight);
                        }
                        SetSurfaceTexel(x, y, texel);
                    }
                }
                targetheight++;
                if (targetheight > limitheight)
                    targetheight = limitheight;
            }
        }

        private Vector2 ExtractWindLocation(int x, WindPoint wp)
        {
            float xDistance = wp.Location.X - x;
            float xCorrection = 0;
            while (System.Math.Abs(xDistance) > 180)
            {
                if (xDistance > 0)
                {
                    xDistance -= 180;
                    xCorrection -= 180;
                }
                else
                {
                    xDistance += 180;
                    xCorrection += 180;
                }
            }

            Vector2 location = new Vector2(wp.Location.X + xCorrection, wp.Location.Y);
            return location;
        }

        /// <summary>
        /// This method is made just to make the result look nice. This is not based on real facts, but on guesses.
        /// </summary>
        private void GenerateBiomes()
        {
            Random rand = new Random(MathHelper.RandomSeed != null ? (int)MathHelper.RandomSeed : 0);

            float[,] height = GetNormalizedHeight();
            
            for (int y = -90; y < 90; y++)
            {
                for (int x = -180; x < 180; x++)
                {
                    SurfaceTexel texel = GetSurfaceTexel(x, y);
                    
                    if (height[x + 180, y + 90] < sealevel)
                        texel.Biome = Biome.Ocean;
                    else
                    {
                        if (texel.Moisture < 0.1f)
                            texel.Biome = texel.Temperature > 5f ? Biome.Desert : Biome.Ice;
                        else if (texel.Moisture < 0.5f)
                            texel.Biome = texel.Temperature > 20f ? Biome.Plains : Biome.Grass;
                        else
                            texel.Biome = texel.Temperature > 20f ? Biome.Rainforest : Biome.Forest;

                        if (texel.Temperature / 70f > texel.Moisture)
                            texel.Biome = Biome.Desert;

                        if (texel.Temperature < 0)
                            texel.Biome = texel.Moisture > 0f ? Biome.Ice : Biome.Desert;

                        if (height[x + 180, y + 90] > 0.8f)
                            texel.Biome = Biome.Mountain;

                        if (height[x + 180, y + 90] > 0.95f)
                            texel.Biome = Biome.Ice;
                        
                        float[] adjTexel = new float[4];

                        adjTexel[0] = GetSurfaceTexel(x - 1, y).Height;
                        adjTexel[1] = GetSurfaceTexel(x + 1, y).Height;
                        adjTexel[2] = GetSurfaceTexel(x, y - 1).Height;
                        adjTexel[3] = GetSurfaceTexel(x, y + 1).Height;

                        for (int i = 0; i < adjTexel.Length; i++)
                        {
                            if (adjTexel[i] - height[x + 180, y + 90] > 0.55f)
                            {
                                texel.Biome = Biome.Mountain;
                                break;
                            }
                        }
                    }

                    SetSurfaceTexel(x, y, texel);
                }
            }

            for (int y = -90; y < 90; y++)
            {
                for (int x = -180; x < 180; x++)
                {
                    SurfaceTexel texel = GetSurfaceTexel(x, y);

                    if (texel.Biome == Biome.Ocean)
                        continue;

                    int count = 0;
                    for (int y2 = -3; y2 < 3; y2++)
                    {
                        for (int x2 = -3; x2 < 3; x2++)
                        {
                            if (x2 == 0 && y2 == 0)
                                continue;
                            SurfaceTexel nextTexel = GetSurfaceTexel(x + x2, y + y2);
                            if (nextTexel.Biome == Biome.Ocean || nextTexel.Biome == texel.Biome)
                                count++;
                        }
                    }

                    if (count < 20)
                    {
                        int _x = rand.Next(-1, 2);
                        int _y = rand.Next(-1, 2);
                        SurfaceTexel adjTexel = GetSurfaceTexel(x + _x, y + _y);
                        if (adjTexel.Biome != Biome.Ocean && adjTexel.Biome != Biome.Mountain && adjTexel.Biome != Biome.Ice)
                            texel.Biome = adjTexel.Biome;
                    }
                    SetSurfaceTexel(x, y, texel);
                }
            }
        }

        /// <summary>
        /// This method generates a border around the area where two plates meet. This line is always
        /// 2 units wide. It also sets the adjacent plates.
        /// </summary>
        private void GenerateBorders()
        {
            for (int y = -90; y < 90; y++)
            {
                for (int x = -180; x < 180; x++)
                {
                    SurfaceTexel texel = GetSurfaceTexel(x, y);
                    int plateID = texel.TectonicPlateID;

                    int[] plates = new int[4];

                    plates[0] = GetSurfaceTexel(x - 1, y).TectonicPlateID;
                    plates[1] = GetSurfaceTexel(x + 1, y).TectonicPlateID;
                    plates[2] = GetSurfaceTexel(x, y - 1).TectonicPlateID;
                    plates[3] = GetSurfaceTexel(x, y + 1).TectonicPlateID;

                    texel.Distance = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (plates[i] != plateID)
                        {
                            texel.Distance = 1;
                            break;
                        }
                    }

                    List<Vector2> otherMoveDirections = new List<Vector2>();
                    for (int i = 0; i < 4; i++)
                    {
                        if (plates[i] != plateID)
                        {
                            otherMoveDirections.Add(tectonicPlates[plates[i]].MoveDirection);
                        }
                    }

                    Vector2 adjacentMoveDirection = Vector2.Zero;
                    for (int i = 0; i < otherMoveDirections.Count; i++)
                    {
                        adjacentMoveDirection += otherMoveDirections[i] / (float)otherMoveDirections.Count;
                    }

                    texel.AdjacentDirection = adjacentMoveDirection;

                    SetSurfaceTexel(x, y, texel);
                }
            }
        }

        private Texture GenerateColorMap()
        {
            Color[,] color = new Color[360, 180];
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    color[x, y] = surface[x, y].GeneratedColors;
                }
            }

            return new Texture(360, 180, ref color);
        }

        private Texture GenerateGlossMap()
        {
            float[,] heightIn = GetNormalizedHeight();
            float[,] heightOut = new float[360, 180];
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    if (heightIn[x, y] < this.sealevel)
                    {
                        heightIn[x, y] = 0;
                    }
                    else
                    {
                        if (surface[x, y].Biome != Biome.Ice)
                        {
                            if (surface[x, y].Moisture >= 0.95f)
                                heightIn[x, y] = 0.2f;
                            else
                                heightIn[x, y] = 1;
                        }
                        else
                        {
                            heightIn[x, y] = 0.5f;
                        }
                    }
                }
            }

            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    int requestedX = x;
                    int requestedY = y;
                    if (y > 90)
                    {
                        if ((x - 180) < 0)
                        {
                            requestedX = x + 180;
                        }
                        else
                        {
                            requestedX = x - 180;
                        }
                    }
                    else
                        requestedX = x;

                    heightOut[x, y] = heightIn[requestedX, y];// y >= 90 ? y - 90 : y + 90];
                }
            }
            return new Texture(360, 180, ref heightIn);
        }

        /// <summary>
        /// Generates heatvalues for the SurfaceTexels based on the Atmosphere and Sun.
        /// </summary>
        /// <param name="sun"></param>
        private void GenerateHeat(Sun sun)
        {
            Random rand = new Random(MathHelper.RandomSeed == null ? 0 : (int)MathHelper.RandomSeed);
            for (int y = -90; y < 90; y++)
            {
                for (int x = -180; x < 180; x++)
                {
                    SurfaceTexel texel = GetSurfaceTexel(x, y);
                    float pressure = (float)(atmosphere.PressureAtSealevel - System.Math.Exp(-atmosphere.ScaleHeight * texel.Height));
                    float virtualDensity = pressure + (0.4f - texel.NormalAngle) * pressure;

                    //This is cheating. What I do here is the following: As long as I stay within the habitable zone, my temperature stays within a range of 15-25 °C, this makes sure that the planet can keep things alive.
                    MinMax<float> habitableZone = sun.GetHabitableZone();

                    float temperature = 0;
                    if (habitableZone.Max > this.distanceToSun && habitableZone.Min < this.distanceToSun)
                    {
                        float habitableZoneLocation = (DistanceToSun - habitableZone.Min) / (habitableZone.Max - habitableZone.Min);
                        temperature = 15 + 10 * habitableZoneLocation;
                    }
                    else if (habitableZone.Max <= this.distanceToSun)
                    {
                        float distance = habitableZone.Max - this.distanceToSun;
                        temperature = 15 * (1 - distance);
                    }
                    else
                    {
                        float distance = -this.distanceToSun * habitableZone.Min;
                        temperature = 30 * (1 - distance);
                    }

                    texel.Temperature = temperature * ((pressure - 0.5f) / (virtualDensity) + rand.Range(-0.3f, 1f));// - (0.7f - texel.NormalAngle) * 10f; //Experiments over an hour. This stays. Sorry.
                    SetSurfaceTexel(x, y, texel);
                }
            }
        }

        private Texture GenerateHeightMap()
        {
            float[,] heightIn = GetNormalizedHeight();
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    if (heightIn[x, y] < this.sealevel)
                    {
                        heightIn[x, y] = this.sealevel;
                    }
                }
            }

            heightIn = heightIn.Invert();

            /*
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    if (y < 90)
                        height[x, y] = (y / 90f) * height[x, y] + 1 - (y / 90f);

                    if (y > 90)
                        height[x, y] = ((180 - y) / 90f) * height[x, y] + 1 - ((180 - y) / 90f);
                }
            }
            */
            float[,] heightOut = new float[360, 180];
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    //heightOut[x, y] = heightIn[x >= 180 ? x - 180 : x + 180, y >= 90 ? y - 90 : y + 90];
                    heightOut[x, y] = heightIn[x, y];// y >= 90 ? y - 90 : y + 90];

                    if (y < 90)
                        heightOut[x, y] = (y / 90f) * heightOut[x, y] + 1 - (y / 90f);

                    if (y > 90)
                        heightOut[x, y] = ((180 - y) / 90f) * heightOut[x, y] + 1 - ((180 - y) / 90f);
                }
            }
            return new Texture(360, 180, ref heightOut);
        }

        /// <summary>
        /// Generates a terrain by using simplex noise. This is merely the basis for every further
        /// calculations and shall be used as a noisey nonlinear terrain. The next step will be a
        /// linear interpolation on both ends of the images to unify this point to a median value.
        /// </summary>
        private void GenerateHeightValues()
        {
            Simplex simplexNoise = new Simplex(256, 0.1);

            int xResolution = 180;
            int yResolution = 180;

            int frequency = 128;

            float[,] height = new float[xResolution, yResolution];

            int steps = 0;
            double maxsteps = xResolution * yResolution;
            //int percent = 0;

            for (int x = 0; x < xResolution; x++)
            {
                for (int y = 0; y < yResolution; y++)
                {
                    /*if (percent < (steps / maxsteps) * 100)
                    {
                        percent++;
                        Console.Clear();
                        Console.WriteLine("Generating Tileable FBM Simplex Noise.");
                        Console.WriteLine("{0}%", percent);
                    }*/
                    steps++;
                    float h = (float)simplexNoise.GetTileableFBM(x, y, 0, 256, 0, 256, xResolution, yResolution, frequency);

                    SurfaceTexel texel = GetSurfaceTexel(x - 180, y - 90);
                    SurfaceTexel texel1 = GetSurfaceTexel(x, y - 90);
                    texel.Height = h;//+ tectonicPlates[texel.TectonicPlateID].HeightModifier;
                    texel1.Height = h;// + tectonicPlates[texel1.TectonicPlateID].HeightModifier;
                    SetSurfaceTexel(x - 180, y - 90, texel);
                    SetSurfaceTexel(x, y - 90, texel1);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void GenerateMoisture(Sun sun)
        {
            if (atmosphere == null)
                return;

            Simplex simplexNoise = new Simplex(100, 0.1, MathHelper.RandomSeed + 20);

            int xResolution = 180;
            int yResolution = 180;

            int frequency = 32;

            float[,] height = new float[xResolution, yResolution];

            double maxsteps = xResolution * yResolution;

            for (int x = 0; x < xResolution; x++)
            {
                for (int y = 0; y < yResolution; y++)
                {
                    float h = (float)simplexNoise.GetTileableFBM(x, y, 0, 256, 0, 256, xResolution, yResolution, frequency);

                    SurfaceTexel texel = GetSurfaceTexel(x - 180, y - 90);
                    SurfaceTexel texel1 = GetSurfaceTexel(x, y - 90);
                    texel.Moisture = System.Math.Max(0, h * MathHelper.Abs(texel.Temperature));
                    texel1.Moisture = System.Math.Max(0, h * MathHelper.Abs(texel1.Temperature));
                    SetSurfaceTexel(x - 180, y - 90, texel);
                    SetSurfaceTexel(x, y - 90, texel1);
                }
            }

            MinMax<float> habitableZone = sun.GetHabitableZone();

            float max = 0;
            for (int y = -90; y < 90; y++)
            {
                for (int x = -180; x < 180; x++)
                {
                    SurfaceTexel texel = GetSurfaceTexel(x, y);
                    if (texel.Moisture > max)
                        max = texel.Moisture;
                }
            }

            for (int y = -90; y < 90; y++)
            {
                for (int x = -180; x < 180; x++)
                {
                    SurfaceTexel texel = GetSurfaceTexel(x, y);
                    texel.Moisture /= max;
                    SetSurfaceTexel(x, y, texel);
                }
            }
        }

        /// <summary>
        /// Generates the sealevel. It is calculated by doubling the middlevalue of the moisture.
        /// </summary>
        private void GenerateOcean(Sun sun)
        {
            MinMax<float> habitableZone = sun.GetHabitableZone();

            if (habitableZone.Max > this.distanceToSun && habitableZone.Min < this.distanceToSun)
            {
                float moisture = 0;
                for (int y = -90; y < 90; y++)
                {
                    for (int x = -180; x < 180; x++)
                    {
                        SurfaceTexel texel = GetSurfaceTexel(x, y);
                        moisture += texel.Moisture;
                    }
                }
                sealevel = moisture / TEXEL_COUNT * 1.1f;
            }
            else
                sealevel = 0;
        }

        private void GenerateSurface()
        {
            for (int y = -90; y < 90; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    SurfaceTexel texel = GetSurfaceTexel(x, y);
                    texel.NormalAngle = 1 - MathHelper.Abs(y / 90f);
                    SetSurfaceTexel(x, y, texel);
                }
            }
        }

        /// <summary>
        /// This method generates tectonic plates using a randomized flood fill algorithm.
        /// </summary>
        private void GenerateTectonicPlates()
        {
            tectonicPlates = new List<TectonicPlate>();

            if (tectonicPlatesCount <= 1)
            {
                SinglePlatePlanet();
                return;
            }

            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    SurfaceTexel texel = new SurfaceTexel();
                    texel.TectonicPlateID = -1;
                    texel.Distance = -1;
                    surface[x, y] = texel;
                }
            }

            Random rand = new Random(MathHelper.RandomSeed != null ? (int)MathHelper.RandomSeed : 0);// DateTime.Now.Millisecond);
            for (int i = 0; i < tectonicPlatesCount; i++)
            {
                tectonicPlates.Add(new TectonicPlate(i));
                int x = rand.Next(0, 360);
                int y = rand.Next(0, 180);

                SurfaceTexel texel = new SurfaceTexel();
                texel.TectonicPlateID = i;
                texel.Distance = 0;
                SetSurfaceTexel(x, y, texel);
            }

            int[,] distance = new int[360, 180];

            int emptyTiles = 180 * 360 - this.tectonicPlatesCount;
            while (emptyTiles > 0)
            {
                //Console.WriteLine(emptyTiles);

                for (int y = -90; y < 90; y++)
                {
                    for (int x = -180; x < 180; x++)
                    {
                        List<SurfaceTexel> possiblePlates = new List<SurfaceTexel>();
                        if (GetSurfaceTexel(x, y).TectonicPlateID != -1)
                        {
                            continue;
                        }

                        possiblePlates.Add(GetSurfaceTexel(x, y - 1));
                        possiblePlates.Add(GetSurfaceTexel(x, y + 1));
                        possiblePlates.Add(GetSurfaceTexel(x - 1, y));
                        possiblePlates.Add(GetSurfaceTexel(x + 1, y));

                        SurfaceTexel texel = new SurfaceTexel();

                        int target = rand.Next(4);

                        texel.TectonicPlateID = possiblePlates[target].TectonicPlateID;

                        SetSurfaceTexel(x, y, texel);
                        if (texel.TectonicPlateID != -1)
                            emptyTiles--;
                    }
                }
            }
        }

        private void GenerateWind()
        {
            Random rand = new Random(MathHelper.RandomSeed != null ? (int)MathHelper.RandomSeed : 0);
            int WindPointCount = rand.Range(5, 20);
            windPoints = new List<WindPoint>();
            for (int i = 0; i < WindPointCount; i++)
            {
                WindPoint wp = new WindPoint();
                wp.Distance = (float)(rand.Range(45, 135));
                wp.Rotation = (float)(rand.NextDouble() * MathHelper.TWO_PI * 2f - MathHelper.TWO_PI);
                wp.Location = new Vector2(rand.Range(0, 360), rand.Range(0, 180));
                windPoints.Add(wp);
            }

            for (int y = -90; y < 90; y++)
            {
                for (int x = -180; x < 180; x++)
                {
                    SurfaceTexel texel = GetSurfaceTexel(x, y);

                    //Let's keep every point out that is within the eye of the storm. (Or to be more precise at the same location as a WindPoint)
                    if (windPoints.Count(wp => wp.Location == new Vector2(x, y)) > 0)
                        continue;

                    List<WindPoint> possiblePoints = GetPossibleWindSources(x, y);
                    texel.WindDirection = GenerateWindDirection(x, y, possiblePoints);

                    SetSurfaceTexel(x, y, texel);
                }
            }
        }

        private Vector2 GenerateWindDirection(int x, int y, List<WindPoint> possiblePoints)
        {
            Vector2 windDirection = Vector2.Zero;
            x += 180;
            y += 90;
            for (int i = 0; i < possiblePoints.Count; i++)
            {
                Vector2 location = ExtractWindLocation(x, possiblePoints[i]);

                float s = (float)System.Math.Sin(windPoints[i].Rotation);
                float c = (float)System.Math.Cos(windPoints[i].Rotation);

                // translate point back to origin:
                float WindTargetX = location.X - x;
                float WindTargetY = location.Y - y;

                // rotate point
                float xnew = WindTargetX * c - WindTargetY * s;
                float ynew = WindTargetX * s + WindTargetY * c;

                float distanceModifier = 1f - Vector2.Distance(location, new Vector2(x, y)) / possiblePoints[i].Distance;

                Vector2 direction = new Vector2(xnew, ynew) * distanceModifier * distanceModifier;

                windDirection += direction;
            }

            return windDirection;
        }

        private List<WindPoint> GetPossibleWindSources(int x, int y)
        {
            List<WindPoint> possiblePoints = new List<WindPoint>();
            x += 180;
            y += 90;
            for (int i = 0; i < windPoints.Count; i++)
            {
                Vector2 location = ExtractWindLocation(x, windPoints[i]);
                float distance = Vector2.Distance(location, new Vector2(x, y));
                if (distance <= windPoints[i].Distance)
                    possiblePoints.Add(windPoints[i]);
            }

            return possiblePoints;
        }

        /// <summary>
        /// This method generates a planet with a single tectonic plate.
        /// </summary>
        private void SinglePlatePlanet()
        {
            tectonicPlates.Add(new TectonicPlate(0));
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    SurfaceTexel texel = new SurfaceTexel();
                    texel.TectonicPlateID = 0;
                    surface[x, y] = texel;
                }
            }
        }

        #endregion Private Methods
    }
}