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

        #endregion Public Properties
    }
}