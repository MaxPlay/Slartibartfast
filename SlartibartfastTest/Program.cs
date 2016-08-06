using Slartibartfast;
using Slartibartfast.Planets;

namespace SlartibartfastTest
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            using (PlanetGenerator generator = new PlanetGenerator())
            {
                PlanetSettings settings = PlanetSettings.Earth();

                generator.PlanetSettings = settings;

                generator.Run();
            }
        }

        #endregion Private Methods
    }
}