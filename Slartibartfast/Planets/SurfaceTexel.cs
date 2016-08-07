using Slartibartfast.Math;

namespace Slartibartfast.Planets
{
    public struct SurfaceTexel
    {
        #region Private Fields

        private Vector2 adjacentDirection;
        private int distance;
        private float height;
        private int tectonicPlateID;

        #endregion Private Fields

        #region Public Properties

        public Vector2 AdjacentDirection
        {
            get { return adjacentDirection; }
            set { adjacentDirection = value; }
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

        public int TectonicPlateID
        {
            get { return tectonicPlateID; }
            set { tectonicPlateID = value; }
        }

        private Biome biome;

        public Biome Biome
        {
            get { return biome; }
            set { biome = value; }
        }

        private float temperature;

        /// <summary>
        /// The temperature is measured in Celsius. However, every value is possible, though the regular temperature is somewhere between -40 and +40. Every value under 0 will result in ice.
        /// </summary>
        public float Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }

        private float moisture;

        /// <summary>
        /// The moisture of the Texel. 0 means completly dry, while 1 means completly wet.
        /// </summary>
        public float Moisture
        {
            get { return moisture; }
            set { moisture = value; }
        }

        private float normalAngle;

        /// <summary>
        /// This is the angle of the texel relative to the equator.
        /// </summary>
        public float NormalAngle
        {
            get { return normalAngle; }
            set { normalAngle = value; }
        }

        private Vector2 windDirection;

        public Vector2 WindDirection
        {
            get { return windDirection; }
            set { windDirection = value; }
        }

        #endregion Public Properties
    }
}