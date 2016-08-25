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
        private BiomeColors colors;

        public BiomeColors BiomeColors
        {
            get { return colors; }
            set { colors = value; }
        }


        #endregion Private Fields

        #region Public Constructors

        public PlanetGenerator()
        {
            MathHelper.RandomSeed = 1;

            //Defaults
            colors = new BiomeColors();
            colors[Biome.Desert] = Color.Beige;
            colors[Biome.Forest] = Color.ForestGreen;
            colors[Biome.Grass] = Color.LawnGreen;
            colors[Biome.Plains] = Color.SandyBrown;
            colors[Biome.Ice] = Color.GhostWhite;
            colors[Biome.Mountain] = Color.LightGray;
            colors[Biome.Ocean] = Color.DarkBlue;
            colors[Biome.Rainforest] = Color.DarkSeaGreen;

            sun = Sun.GetSol();
            planetSettings = PlanetSettings.Earth();
        }

        #endregion Public Constructors

        #region Public Properties

        public TextureType OutputTextures
        {
            get { return outputTextures; }
        }

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

            Planet planet = new Planet(planetSettings, sun);
            MinMax<float> habitableZone = sun.GetHabitableZone();
            float habitableZoneLocation = (planet.DistanceToSun - habitableZone.Min) / (habitableZone.Max - habitableZone.Min);
            
            planet.GenerateVegetation(colors);
            outputTextures = outputTextures.Include(TextureType.Gloss);
            outputTextures = outputTextures.Include(TextureType.Height);
            outputTextures = outputTextures.Include(TextureType.Color);
            Tuple<Texture, Texture, Texture> tex = planet.GenerateTextures(outputTextures);

            tex.Item1?.SaveToFile("color.png");
            tex.Item2?.SaveToFile("height.png");
            tex.Item3?.SaveToFile("gloss.png");

            Color[,] plates = planet.GetWindMoveDirection();
            Texture t = new Texture(360, 180, ref plates);
            t.SaveToFile("wind.png");
        }

        #endregion Public Methods
    }
}