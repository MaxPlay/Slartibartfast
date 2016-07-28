using Slartibartfast.Extensions;
using Slartibartfast.Generators;
using Slartibartfast.Planets;
using Slartibartfast.Textures;
using Slartibartfast.Math;
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
            MathHelper.RandomSeed = 1;
            
            Planet planet = new Planet(PlanetSettings.Earth());
            Color[,] plates = planet.GetTectonicPlates();
            Texture tex = new Texture(360, 180, ref plates);
            tex.SaveToFile("plates.png");

            plates = planet.GetDistances();
            tex = new Texture(360, 180, ref plates);
            tex.SaveToFile("plates0.png");

            float[,] height = planet.GetHeight();
            tex = new Texture(360, 180, ref height);
            tex.SaveToFile("height.png");

            plates = planet.GetAdjacentMoveDirection();
            tex = new Texture(360, 180, ref plates);
            tex.SaveToFile("adjacent.png");
        }

        public void Dispose()
        {

        }

        public void Run()
        {

        }
    }
}
