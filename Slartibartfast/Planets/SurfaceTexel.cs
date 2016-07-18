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

    }
}