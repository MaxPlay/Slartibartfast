using Slartibartfast.Extensions;
using Slartibartfast.Generators;
using Slartibartfast.Planets;
using Slartibartfast.Textures;
using Slartibartfast.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slartibartfast
{
    public class PlanetGenerator : IDisposable
    {
        public PlanetGenerator()
        {
            /*Simplex simplexNoise = new Simplex(256, 0.1, 5000);

            int xResolution = 1024;
            int yResolution = 1024;

            int frequency = 128;

            float[,] height = new float[xResolution, yResolution];

            int steps = 0;
            double maxsteps = xResolution * yResolution;
            int percent = 0;

            for (int x = 0; x < xResolution; x++)
            {
                for (int y = 0; y < yResolution; y++)
                {
                    if (percent < (steps / maxsteps) * 100)
                    {
                        percent++;
                        Console.Clear();
                        Console.WriteLine("Generating Tileable FBM Simplex Noise.");
                        Console.WriteLine("{0}%", percent);
                    }
                    steps++;
                    height[x, y] = (float)simplexNoise.GetTileableFBM(x, y, 0, 256, 0, 256, xResolution, yResolution, frequency);
                }
            }
            //DiamondSquare ds = new DiamondSquare();
            //float[,] height = ds.Generate(1024, 1024);
            Console.WriteLine("Writing File.");
            Texture tex = new Texture(1024, 1024, ref height);
            tex.SaveToFile("height.png");*/

            Planet planet = new Planet(PlanetSettings.Earth());
            Color[,] plates = planet.GetDistances();
            Texture tex = new Texture(360, 180, ref plates);
            tex.SaveToFile("plates.png");
        }

        public void Dispose()
        {

        }

        public void Run()
        {

        }
    }
}
