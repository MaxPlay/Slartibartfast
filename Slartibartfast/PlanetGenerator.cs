using Slartibartfast.Extensions;
using Slartibartfast.Math;
using Slartibartfast.Planets;
using Slartibartfast.Textures;
using System;

namespace Slartibartfast
{
    public sealed class PlanetGenerator : IDisposable
    {
        #region Private Fields

        private BiomeColors colors;
        private Texture colorTex;
        private Texture glossTex;
        private Texture heightTex;
        private TextureType outputTextures;
        private PlanetSettings planetSettings;
        private Sun sun;

        #endregion Private Fields

        #region Public Constructors

        public PlanetGenerator()
        {
            MathHelper.RandomSeed = 1;

            //Defaults
            colors = new BiomeColors();
            colors[Biome.Desert] = new Color(238, 237, 181);
            colors[Biome.Forest] = new Color(23, 164, 27);
            colors[Biome.Grass] = new Color(117, 236, 120);
            colors[Biome.Plains] = new Color(230, 233, 167);
            colors[Biome.Ice] = new Color(202, 228, 255);
            colors[Biome.Mountain] = new Color(166, 172, 153);
            colors[Biome.Ocean] = new Color(18, 27, 101);
            colors[Biome.Rainforest] = new Color(36, 92, 27);

            sun = Sun.GetSol();
            planetSettings = PlanetSettings.Earth();
        }

        #endregion Public Constructors

        #region Public Properties

        public BiomeColors BiomeColors
        {
            get { return colors; }
            set { colors = value; }
        }

        public Texture ColorTexture
        {
            get { return colorTex; }
            set { colorTex = value; }
        }

        public Texture GlossTexture
        {
            get { return glossTex; }
            set { glossTex = value; }
        }

        public Texture HeightTexture
        {
            get { return heightTex; }
            set { heightTex = value; }
        }

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
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Runs the generator. This may take a while, so this is the method you want to put in another thread.
        /// </summary>
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
            TextureSet tex = planet.GenerateTextures(outputTextures);

            colorTex = tex.ColorTexture;
            heightTex = tex.HeightTexture;
            glossTex = tex.GlossTexture;
            /*
            tex.Item1?.SaveToFile("color.png");
            tex.Item2?.SaveToFile("height.png");
            tex.Item3?.SaveToFile("gloss.png");

            Color[,] plates = planet.GetWindMoveDirection();
            Texture t = new Texture(360, 180, ref plates);
            t.SaveToFile("wind.png");*/
        }

        /*
        /// <summary>
        /// Saves textures if available.
        /// </summary>
        public void SaveTextures()
        {
            colorTex?.SaveToFile("color.png");
            heightTex?.SaveToFile("height.png");
            glossTex?.SaveToFile("gloss.png");
        }
        */

        /// <summary>
        /// Returns the Planet for debugreasons.
        /// </summary>
        /// <returns></returns>
        public Planet RunDebug()
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
            TextureSet tex = planet.GenerateTextures(outputTextures);

            colorTex = tex.ColorTexture;
            heightTex = tex.HeightTexture;
            glossTex = tex.GlossTexture;

            return planet;
        }

        #endregion Public Methods
    }
}