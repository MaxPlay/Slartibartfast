using Slartibartfast.Extensions;
using Slartibartfast.Generators;
using Slartibartfast.Textures;
using Slartibartfast.Vectors;
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
            Simplex simplexNoise = new Simplex(256, 0.1, 5000);

            double xStart = 0;
            double xEnd = 200;
            double yStart = 0;
            double yEnd = 200;

            int xResolution = 256;
            int yResolution = 256;

            int frequency = 128;

            float[,] height = new float[xResolution, yResolution];

            for (int x = 0; x < xResolution; x++)
            {
                for (int y = 0; y < yResolution; y++)
                {
                    /*double s = i / (double)xResolution;
                    double t = j / (double)yResolution;
                    double dx = xEnd - xStart;
                    double dy = yEnd - yStart;

                    double nx = xStart + Math.Cos(s * 2 * Math.PI) * dx / (2 * Math.PI);
                    double ny = yStart + Math.Cos(t * 2 * Math.PI) * dy / (2 * Math.PI);
                    double nz = xStart + Math.Sin(s * 2 * Math.PI) * dx / (2 * Math.PI);
                    double nw = yStart + Math.Sin(t * 2 * Math.PI) * dy / (2 * Math.PI);

                    float h = 0;
                    for (int c = 1; c < frequency; c *= 2)
                    {
                        float dh = (float)(0.5 * (1 + simplexNoise.GetNoise(nx * frequency / c, ny * frequency / c, nz * frequency / c, nw * frequency / c)));
                        h = dh + 0.5f * h;
                    }
                    */
                    height[x, y] = (float)simplexNoise.GetTileableNoise(x, y, xStart, xEnd, yStart, yEnd, xResolution, yResolution, frequency);
                }
            }
            //DiamondSquare ds = new DiamondSquare();
            //float[,] height = ds.Generate(1024, 1024);
            Texture tex = new Texture(256, 256, ref height);
            tex.SaveToFile("height.png");
        }

        public void Dispose()
        {

        }

        public void Run()
        {

        }
    }
}
