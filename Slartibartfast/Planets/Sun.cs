using Slartibartfast.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slartibartfast.Planets
{
    public enum SpectralClass
    {
        B,
        A,
        F,
        G,
        K,
        M
    }

    public class Sun
    {
        public const float SOL_RADIUS = 1;
        public const float SOL_MASS = 1;
        public const float SOL_ABSOLUTE_MAGNITUDE = 4.83f;
        public const float SOL_INNER_CHZ = 0.95f;
        public const SpectralClass SOL_SPECTRAL_CLASS = SpectralClass.G;


        /// <summary>
        /// Returns the Bolometric Correction Constant of the corresponding Spectral Class. The spectral class of the sun is G.
        /// </summary>
        /// <param name="SpectralClass">The spectral class for the bolometric correction value.</param>
        /// <returns></returns>
        public static float BolometricCorrectionConstant(SpectralClass SpectralClass)
        {
            switch (SpectralClass)
            {
                case SpectralClass.B:
                    return -2f;
                case SpectralClass.A:
                    return -0.3f;
                case SpectralClass.F:
                    return -0.15f;
                case SpectralClass.G:
                    return -0.4f;
                case SpectralClass.K:
                    return -0.8f;
                case SpectralClass.M:
                    return -2f;
                default:
                    return 0;
            }
        }

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

        private SpectralClass spectralClass;

        public SpectralClass SpectralClass
        {
            get { return spectralClass; }
            set { spectralClass = value; }
        }

        /// <summary>
        /// Calculates the habitable zone for the sun in AU.
        /// </summary>
        /// <returns></returns>
        public MinMax<float> GetHabitableZone()
        {
            float correctedBolometricMagnitude = this.absoluteMagnitude - BolometricCorrectionConstant(this.spectralClass);
            float absoluteLuminosity = (float)System.Math.Pow(10, (correctedBolometricMagnitude - 4.72) / -2.5f);

            float min = (float)System.Math.Sqrt(absoluteLuminosity / 1.1);
            float max = (float)System.Math.Sqrt(absoluteLuminosity / 0.53);

            return new MinMax<float>(min, max);
        }
    }
}
