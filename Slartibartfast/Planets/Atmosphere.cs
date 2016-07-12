namespace Slartibartfast.Planets
{
    public class Atmosphere
    {
        public const float EARTH_PRESSURE_AT_SEALEVEL = 101325;
        public const float EARTH_THICKNESS = 10000;

        private float pressureAtSealevel;

        /// <summary>
        /// The pressure at sealevel in Pa.
        /// </summary>
        public float PressureAtSealevel
        {
            get { return pressureAtSealevel; }
            set { pressureAtSealevel = value; }
        }
        
        private float thickness;

        /// <summary>
        /// The thickness of the atmosphere in kilometers from sealevel to outer space.
        /// </summary>
        public float Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }

    }
}