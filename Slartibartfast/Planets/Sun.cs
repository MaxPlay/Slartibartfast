using Slartibartfast.Math;

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
        #region Public Fields

        public const float SOL_ABSOLUTE_MAGNITUDE = 4.83f;
        public const float SOL_INNER_CHZ = 0.95f;
        public const float SOL_MASS = 1;
        public const float SOL_RADIUS = 1;
        public const SpectralClass SOL_SPECTRAL_CLASS = SpectralClass.G;

        #endregion Public Fields

        #region Private Fields

        private const double HABITABLE_ZONE_INNER_CONSTANT = 0.53;
        private const double HABITABLE_ZONE_OUTER_CONSTANT = 1.1;
        private const float SOL_CORRECTED_BOLOMETRIC_CONSTANT = 4.72f;
        private float absoluteMagnitude;

        private float mass;

        private float radius;

        private SpectralClass spectralClass;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// The absolute magnitude of the sun. The relative magnitude is the magnitude as perceived
        /// through the atmosphere of the earth.
        /// </summary>
        public float AbsoluteMagnitude
        {
            get { return absoluteMagnitude; }
            set { absoluteMagnitude = value; }
        }

        /// <summary>
        /// The mass of the sun in solar masses. Our sun (Sol) has the value 1.
        /// </summary>
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        /// <summary>
        /// The radius of the sun in solar radii. Our sun (Sol) has the value 1.
        /// </summary>
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public SpectralClass SpectralClass
        {
            get { return spectralClass; }
            set { spectralClass = value; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the Bolometric Correction Constant of the corresponding Spectral Class. The
        /// spectral class of the sun is G.
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

        public static Sun GetSol()
        {
            Sun s = new Sun();
            s.absoluteMagnitude = SOL_ABSOLUTE_MAGNITUDE;
            s.mass = SOL_MASS;
            s.radius = SOL_RADIUS;
            s.spectralClass = SOL_SPECTRAL_CLASS;
            return s;
        }

        /// <summary>
        /// Calculates the habitable zone for the sun in AU.
        /// </summary>
        /// <returns></returns>
        public MinMax<float> GetHabitableZone()
        {
            float correctedBolometricMagnitude = this.absoluteMagnitude - BolometricCorrectionConstant(this.spectralClass);
            float absoluteLuminosity = (float)System.Math.Pow(10, (correctedBolometricMagnitude - SOL_CORRECTED_BOLOMETRIC_CONSTANT) / -2.5f);

            float min = (float)System.Math.Sqrt(absoluteLuminosity / HABITABLE_ZONE_OUTER_CONSTANT);
            float max = (float)System.Math.Sqrt(absoluteLuminosity / HABITABLE_ZONE_INNER_CONSTANT);

            return new MinMax<float>(min, max);
        }

        #endregion Public Methods
    }
}