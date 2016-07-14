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
                CoordinateY = 180-CoordinateY;
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
                    surface[x, y] = texel;
                }
            }

            Random rand = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < tectonicPlatesCount; i++)
            {
                tectonicPlates.Add(new TectonicPlate(i));
                int x = rand.Next(0, 360);
                int y = rand.Next(0, 180);

                SurfaceTexel texel = new SurfaceTexel();
                texel.TectonicPlateID = i;
                SetSurfaceTexel(x, y, texel);
            }

            int emptyTiles = 180 * 360 - this.tectonicPlatesCount;
            while (emptyTiles > 0)
            {
                Console.WriteLine(emptyTiles);

                for (int y = -90; y < 90; y++)
                {
                    for (int x = -180; x < 180; x++)
                    {
                        List<int> possiblePlates = new List<int>();
                        if (GetSurfaceTexel(x, y).TectonicPlateID != -1)
                        {
                            continue;
                        }

                        possiblePlates.Add(GetSurfaceTexel(x, y - 1).TectonicPlateID);
                        possiblePlates.Add(GetSurfaceTexel(x, y + 1).TectonicPlateID);
                        possiblePlates.Add(GetSurfaceTexel(x - 1, y).TectonicPlateID);
                        possiblePlates.Add(GetSurfaceTexel(x + 1, y).TectonicPlateID);

                        SurfaceTexel texel = new SurfaceTexel();
                        texel.TectonicPlateID = possiblePlates[rand.Next(4)];
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
    }
}
