using System;
using System.Collections.Generic;
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
            // One texel for every degree in geographical coordinates.
            surface = new SurfaceTexel[190, 360];
        }
    }
}
