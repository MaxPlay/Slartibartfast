using Slartibartfast.Math;

namespace Slartibartfast.Planets
{
    public struct SurfaceTexel
    {
        private int tectonicPlateID;

        public int TectonicPlateID
        {
            get { return tectonicPlateID; }
            set { tectonicPlateID = value; }
        }

        private int distance;

        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        private float height;

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        private Vector2 adjacentDirection;

        public Vector2 AdjacentDirection
        {
            get { return adjacentDirection; }
            set { adjacentDirection = value; }
        }


    }
}