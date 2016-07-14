namespace Slartibartfast.Planets
{
    public struct PlanetSettings
    {
        public float DistanceToSun;
        public Atmosphere Atmosphere;
        public float Radius;
        public float Mass;
        public int Age;
        public int TectonicPlatesCount;

        public static PlanetSettings Earth()
        {
            PlanetSettings s = new PlanetSettings();
            s.DistanceToSun = Planet.EARTH_DISTANCE_TO_SUN;
            s.Mass = Planet.EARTH_MASS;
            s.Radius = Planet.EARTH_RADIUS;
            s.Age = Planet.EARTH_AGE;
            s.TectonicPlatesCount = Planet.EARTH_TECTONIC_PLATES_COUNT;

            Atmosphere a = new Atmosphere();
            a.PressureAtSealevel = Atmosphere.EARTH_PRESSURE_AT_SEALEVEL;
            a.ScaleHeight = Atmosphere.EARTH_SCALE_HEIGHT;

            s.Atmosphere = a;
            return s;
        }

        public static PlanetSettings Mercury()
        {
            PlanetSettings s = new PlanetSettings();
            s.DistanceToSun = 0.38f;
            s.Mass = 0.055f;
            s.Radius = 2440;
            s.Age = 4500;
            s.TectonicPlatesCount = 1;

            Atmosphere a = null;

            s.Atmosphere = a;
            return s;
        }

        public static PlanetSettings Venus()
        {
            PlanetSettings s = new PlanetSettings();
            s.DistanceToSun = 0.72f;
            s.Mass = 0.81f;
            s.Radius = 6052;
            s.Age = 4500;
            s.TectonicPlatesCount = 1;

            Atmosphere a = new Atmosphere();
            a.PressureAtSealevel = 900;
            a.ScaleHeight = 15.9f;

            s.Atmosphere = a;
            return s;
        }

        public static PlanetSettings Mars()
        {
            PlanetSettings s = new PlanetSettings();
            s.DistanceToSun = 0.63f;
            s.Mass = 0.11f;
            s.Radius = 3396;
            s.Age = 4500;
            s.TectonicPlatesCount = 4;

            Atmosphere a = new Atmosphere();
            a.PressureAtSealevel = 0.01f;
            a.ScaleHeight = 11.1f;

            s.Atmosphere = a;
            return s;
        }
    }
}