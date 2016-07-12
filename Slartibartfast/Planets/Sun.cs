using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slartibartfast.Planets
{
    public class Sun
    {
        public const float SOL_RADIUS = 1;
        public const float SOL_MASS = 1;
        public const float SOL_LUMINOSITY = 1;
        public const float SOL_ABSOLUTE_MAGNITUDE = 4.83f;

        private float radius;

        /// <summary>
        /// The radius of the sun in solar radii. Our sun (Sol) has the value 1.
        /// </summary>
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        private float mass;

        /// <summary>
        /// The mass of the sun in solar masses. Our sun (Sol) has the value 1.
        /// </summary>
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        private float luminosity;

        /// <summary>
        /// The luminosity of the sun.
        /// </summary>
        public float Luminosity
        {
            get { return luminosity; }
            set { luminosity = value; }
        }

        private float absoluteMagnitude;

        /// <summary>
        /// The absolute magnitude of the sun.
        /// The relative magnitude is the magnitude as perceived through the atmosphere of the earth.
        /// </summary>
        public float AbsoluteMagnitude
        {
            get { return absoluteMagnitude; }
            set { absoluteMagnitude = value; }
        }

    }
}
