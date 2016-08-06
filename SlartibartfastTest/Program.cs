using Slartibartfast;

namespace SlartibartfastTest
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            using (PlanetGenerator generator = new PlanetGenerator())
            {
                generator.Run();
            }
        }

        #endregion Private Methods
    }
}