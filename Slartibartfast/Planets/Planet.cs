using Slartibartfast.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slartibartfast.Planets
{
    public class Planet
    {
        /// <summary>
        /// The radius of the earth from its core to sealevel in kilometers.
        /// </summary>
        public const float EARTH_RADIUS = 6371;
        /// <summary>
        /// The mass of the earth in earth masses (which is 1 obviously).
        /// </summary>
        public const float EARTH_MASS = 1;
        /// <summary>
        /// The distance of the earth to the sun in AU.
        /// </summary>
        public const float EARTH_DISTANCE_TO_SUN = 1;
        /// <summary>
        /// The distance of the earth to the sun in AU.
        /// </summary>
        public const int EARTH_AGE = 4543;
        /// <summary>
        /// The distance of the earth to the sun in AU.
        /// </summary>
        public const int EARTH_TECTONIC_PLATES_COUNT = 7;

        /// <summary>
        /// The distance to the sun in AU. 1 AU = the earths distance to the sun.
        /// </summary>
        private float distanceToSun;

        public float DistanceToSun
        {
            get { return distanceToSun; }
        }

        private Atmosphere atmosphere;

        /// <summary>
        /// The atmosphere of the planet. If null, the planet does not have an atmosphere.
        /// </summary>
        public Atmosphere Atmosphere
        {
            get { return atmosphere; }
            set { atmosphere = value; }
        }

        private float radius;

        /// <summary>
        /// The radius of the planet in kilometers. The value for earth would be 6371.
        /// </summary>
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        private float mass;

        /// <summary>
        /// The mass of the planet in earth masses. 1 = The planet has the same mass as the earth.
        /// </summary>
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        private int age;

        /// <summary>
        /// The age of the planet in billion years.
        /// </summary>
        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        private int tectonicPlatesCount;

        public int TectonicPlatesCount
        {
            get { return tectonicPlatesCount; }
            set { tectonicPlatesCount = value; }
        }

        private List<TectonicPlate> tectonicPlates;
        private SurfaceTexel[,] surface;

        public Planet(PlanetSettings settings)
        {
            this.age = settings.Age;
            this.atmosphere = settings.Atmosphere;
            this.distanceToSun = settings.DistanceToSun;
            this.mass = settings.Mass;
            this.radius = settings.Radius;
            this.tectonicPlatesCount = 20;//settings.TectonicPlatesCount;

            // One texel for every degree in geographical coordinates.
            surface = new SurfaceTexel[360, 180];
            GenerateTectonicPlates();
            GenerateBorders();
        }

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

        public void GenerateTectonicPlates()
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

            Random rand = new Random();// DateTime.Now.Millisecond);
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
                        texel.Distance = possiblePlates[target].Distance + 1;

                        SetSurfaceTexel(x, y, texel);
                        if (texel.TectonicPlateID != -1)
                            emptyTiles--;
                    }
                }
            }
        }

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
                    int col = 255-(int)((surface[x, y].Distance / max) * 255);
                    surf[x, y] = Color.FromArgb(255, col, col, col);
                }
            }

            return surf;
        }

        public void GenerateBorders()
        {
            int[,] distance = new int[360, 180];

            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    distance[x, y] = -1;
                    y -= 90;
                    x -= 180;
                    SurfaceTexel texel = GetSurfaceTexel(x, y);
                    int plateID = texel.TectonicPlateID;

                    int[] plates = new int[4];

                    plates[0] = GetSurfaceTexel(x - 1, y).TectonicPlateID;
                    plates[1] = GetSurfaceTexel(x + 1, y).TectonicPlateID;
                    plates[2] = GetSurfaceTexel(x, y - 1).TectonicPlateID;
                    plates[3] = GetSurfaceTexel(x, y + 1).TectonicPlateID;

                    for (int i = 0; i < 4; i++)
                    {
                        if (plates[i] != plateID)
                        {
                            distance[x + 180, y + 90] = 0;
                            continue;
                        }
                    }
                    y += 90;
                    x += 180;
                }
            }

            this.border = distance;
        }

        int[,] border;

        public Color[,] GetBorder()
        {
            Color[,] c = new Color[360, 180];
            for (int y = 0; y < 180; y++)
            {
                for (int x = 0; x < 360; x++)
                {
                    c[x, y] = border[x, y] == 0 ? Color.Black : Color.White;
                }
            }
            return c;
        }
    }
}
