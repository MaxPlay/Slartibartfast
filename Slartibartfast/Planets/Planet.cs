using Slartibartfast.Extensions;
using Slartibartfast.Generators;
using Slartibartfast.Math;
using Slartibartfast.Textures;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public Planet(PlanetSettings settings)
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
        public Tuple<Texture, Texture, Texture> GenerateTextures(TextureType outputTextures)
        {
            Texture color, height, gloss;

            color = outputTextures.Has(TextureType.Color) ? GenerateColorMap() : null;
            gloss = outputTextures.Has(TextureType.Gloss) ? GenerateGlossMap() : null;
            height = outputTextures.Has(TextureType.Height) ? GenerateHeightMap() : null;

            return new Tuple<Texture, Texture, Texture>(color, height, gloss);
        }

        /// <summary>
        /// Gets the adjacent move directions (other plates adjacent to the current plate) as RGB values.
        /// </summary>
        /// <returns></returns>
        public Color[,] GetAdjacentMoveDirection()
        {
            Color[,] surf = new Color[360, 180];

            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    int r = (int)((surface[x, y].AdjacentDirection.X * 128f) + 128);
                    int g = (int)((surface[x, y].AdjacentDirection.Y * 128f) + 128);
                    surf[x, y] = Color.FromArgb(255, r, g, 255);
                }
            }

            return surf;
        }

        /// <summary>
        /// Gets a generated Color-Representation of the distances to the borders of tectonic plates
        /// (generated with the "GenerateBorders and ExtendBorders" methods).
        /// </summary>
        /// <returns>
        /// Two-dimensional array representing the distances to the borders of tectonic plates.
        /// </returns>
        public Color[,] GetDistances()
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
                    surf[x, y] = Color.FromArgb(255, col, col, col);
                }
            }

            return surf;
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

            if (yRepeat % 2 == 1)
            {
                CoordinateY = 180 - CoordinateY;
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
        public Color[,] GetTectonicPlates()
        {
            Color[,] surf = new Color[360, 180];
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    surf[x, y] = tectonicPlates[surface[x, y].TectonicPlateID].DebugColor;
                }
            }

            return surf;
        }

        public Color[,] GetWindMoveDirection()
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
                    surf[x, y] = Color.FromArgb(255, r == 256 ? 255 : r, g == 256 ? 255 : g, 255); //Bugfix, don't think about it.
                }
            }

            return surf;
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

            if (yRepeat % 2 == 1)
            {
                CoordinateY = 180 - CoordinateY;
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

        internal void GenerateVegetation()
        {
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
                    multiplier *= 0.05f;
                    multiplier++;
                    texel.Height *= multiplier;
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
        /// value. The defaulvalue is 15.
        /// </param>
        private void ExtendBorders(int limit = 25)
        {
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
                Console.WriteLine(emptyTiles);

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
            float[,] height = GetHeight();
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    if (height[x, y] > this.sealevel)
                    {
                        height[x, y] = this.sealevel;
                    }
                }
            }

            return new Texture(360, 180, ref height);
        }

        private Texture GenerateGlossMap()
        {
            float[,] height = GetNormalizedHeight();
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    if (height[x, y] < this.sealevel || surface[x, y].Moisture >= 0.95f)
                    {
                        height[x, y] = 0;
                    }
                    else
                        height[x, y] = 1;
                }
            }

            return new Texture(360, 180, ref height);
        }

        private Texture GenerateHeightMap()
        {
            float[,] height = GetNormalizedHeight();
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    if (height[x, y] < this.sealevel)
                    {
                        height[x, y] = this.sealevel;
                    }
                }
            }

            height = height.Invert();

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

            return new Texture(360, 180, ref height);
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
            int percent = 0;

            for (int x = 0; x < xResolution; x++)
            {
                for (int y = 0; y < yResolution; y++)
                {
                    if (percent < (steps / maxsteps) * 100)
                    {
                        percent++;
                        Console.Clear();
                        Console.WriteLine("Generating Tileable FBM Simplex Noise.");
                        Console.WriteLine("{0}%", percent);
                    }
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

        private void GenerateSurface()
        {
            for (int y = -90; y < 90; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    SurfaceTexel texel = GetSurfaceTexel(x, y);
                    texel.NormalAngle = 1 - y / 90f;
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
                Console.WriteLine(emptyTiles);

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