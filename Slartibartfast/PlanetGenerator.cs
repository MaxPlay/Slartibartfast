using Slartibartfast.Generators;
using Slartibartfast.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slartibartfast
{
    public class PlanetGenerator : IDisposable
    {
        public PlanetGenerator()
        {
            DiamondSquare ds = new DiamondSquare();
            for (int i = 0; i < 50; i++)
            {
                float[,] height = ds.Generate(2048, DateTime.Now.Millisecond, 0, 100, i);
                Texture tex = new Texture(2048, 2048, ref height);
                tex.SaveToFile("height" + i + ".png");
            }
        }

        public void Dispose()
        {

        }

        public void Run()
        {

        }
    }
}
