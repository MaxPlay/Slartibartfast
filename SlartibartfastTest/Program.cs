using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slartibartfast;

namespace SlartibartfastTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (PlanetGenerator generator = new PlanetGenerator())
            {
                generator.Run();
            }
        }
    }
}
