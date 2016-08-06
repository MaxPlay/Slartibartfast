namespace Slartibartfast.Planets
{
    public class Atmosphere
    {
        #region Public Fields

        public const float EARTH_PRESSURE_AT_SEALEVEL = 1.01325f;
        public const float EARTH_SCALE_HEIGHT = 8.5f;

        #endregion Public Fields

        #region Private Fields

        private float pressureAtSealevel;

        private float scaleHeight;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// The pressure at sealevel in bars.
        /// </summary>
        public float PressureAtSealevel
        {
            get { return pressureAtSealevel; }
            set { pressureAtSealevel = value; }
        }

        /// <summary>
        /// The scale height of the atmosphere.
        /// </summary>
        public float ScaleHeight
        {
            get { return scaleHeight; }
            set { scaleHeight = value; }
        }

        #endregion Public Properties
    }
}