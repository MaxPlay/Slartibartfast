using Slartibartfast.Math;

namespace Slartibartfast.Planets
{
    public struct SurfaceTexel
    {
        #region Private Fields

        private Vector2 adjacentDirection;
        private Biome biome;
        private int distance;
        private float height;
        private float moisture;
        private float normalAngle;
        private int tectonicPlateID;

        private float temperature;

        private Vector2 windDirection;

        #endregion Private Fields

        #region Public Properties

        public Vector2 AdjacentDirection
        {
            get { return adjacentDirection; }
            set { adjacentDirection = value; }
        }

        public Biome Biome
        {
            get { return biome; }
            set { biome = value; }
        }

        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// The moisture of the Texel. 0 means completly dry, while 1 means completly wet.
        /// </summary>
        public float Moisture
        {
            get { return moisture; }
            set { moisture = value; }
        }

        /// <summary>
        /// This is the angle of the texel relative to the equator.
        /// </summary>
        public float NormalAngle
        {
            get { return normalAngle; }
            set { normalAngle = value; }
        }

        public int TectonicPlateID
        {
            get { return tectonicPlateID; }
            set { tectonicPlateID = value; }
        }

        /// <summary>
        /// The temperature is measured in Celsius. However, every value is possible, though the regular temperature is somewhere between -40 and +40. Every value under 0 will result in ice.
        /// </summary>
        public float Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }

        public Vector2 WindDirection
        {
            get { return windDirection; }
            set { windDirection = value; }
        }

        #endregion Public Properties
    }
}