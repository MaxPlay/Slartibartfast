namespace Slartibartfast.Planets
{
    public class Atmosphere
    {
        public const float EARTH_PRESSURE_AT_SEALEVEL = 1.01325f;
        public const float EARTH_SCALE_HEIGHT = 8.5f;

        private float pressureAtSealevel;

        /// <summary>
        /// The pressure at sealevel in bars.
        /// </summary>
        public float PressureAtSealevel
        {
            get { return pressureAtSealevel; }
            set { pressureAtSealevel = value; }
        }
        
        private float scaleHeight;

        /// <summary>
        /// The scale height of the atmosphere.
        /// </summary>
        public float ScaleHeight
        {
            get { return scaleHeight; }
            set { scaleHeight = value; }
        }

    }
}