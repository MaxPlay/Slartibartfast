using Slartibartfast.Extensions;
using Slartibartfast.Math;
using Slartibartfast.Planets;
using Slartibartfast.Textures;
using System;
using System.Drawing;

namespace Slartibartfast
{
    public class PlanetGenerator : IDisposable
    {
        #region Private Fields

        private TextureType outputTextures;
        private PlanetSettings planetSettings;
        private Sun sun;

        public TextureType OutputTextures
        {
            get { return outputTextures; }
        }

        #endregion Private Fields

        #region Public Constructors

        public PlanetGenerator()
        {
            MathHelper.RandomSeed = 1;

            /*Planet planet = new Planet(PlanetSettings.Earth());

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
            tex.SaveToFile("adjacent.png");*/
        }

        #endregion Public Constructors

        #region Public Properties

        public PlanetSettings PlanetSettings
        {
            get { return planetSettings; }
            set { planetSettings = value; }
        }

        public Sun Sun
        {
            get { return sun; }
            set { sun = value; }
        }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            sun = null;
        }

        public void Run()
        {
            if (sun == null)
                sun = Sun.GetSol();

            Planet planet = new Planet(planetSettings);
            MinMax<float> habitableZone = sun.GetHabitableZone();
            float habitableZoneLocation = (planet.DistanceToSun - habitableZone.Min) / (habitableZone.Max - habitableZone.Min);
            planet.GenerateVegetation();
            outputTextures = outputTextures.Include(TextureType.Gloss);
            outputTextures = outputTextures.Include(TextureType.Height);
            Tuple<Texture, Texture, Texture> tex = planet.GenerateTextures(outputTextures);

            tex.Item1?.SaveToFile("color.png");
            tex.Item2?.SaveToFile("gloss.png");
            tex.Item3?.SaveToFile("height.png");
        }

        #endregion Public Methods
    }
}