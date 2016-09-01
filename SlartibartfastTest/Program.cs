using Slartibartfast;
using Slartibartfast.Planets;
using Slartibartfast.Textures;
using System.Drawing;
using System.Drawing.Imaging;

namespace SlartibartfastTest
{
    internal class Program
    {
        #region Public Methods

        /// <summary>
        /// Taken from one of my own projects. This is originally coming from: http://stackoverflow.com/questions/13511661/create-bitmap-from-double-two-dimentional-array
        /// </summary>
        /// <param name="rawImage">The inputarray</param>
        /// <param name="width">   The width of the array.</param>
        /// <param name="height">  The height of the array.</param>
        /// <returns></returns>
        public static unsafe Bitmap ToBitmap(Color[] rawImage, int width, int height)
        {
            System.Drawing.Bitmap Image = new System.Drawing.Bitmap(width, height);
            System.Drawing.Imaging.BitmapData bitmapData = Image.LockBits(
                new System.Drawing.Rectangle(0, 0, width, height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
            );
            ColorARGB* startingPosition = (ColorARGB*)bitmapData.Scan0;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    System.Drawing.Color color = rawImage[x + y * width];

                    ColorARGB* position = startingPosition + x + y * width;
                    position->R = color.A;
                    position->G = color.R;
                    position->B = color.G;
                    position->A = color.B;
                }

            Image.UnlockBits(bitmapData);
            return Image;
        }

        #endregion Public Methods

        #region Private Methods

        private static void Main(string[] args)
        {
            using (PlanetGenerator generator = new PlanetGenerator())
            {
                PlanetSettings settings = PlanetSettings.Earth();

                generator.PlanetSettings = settings;

                Planet planet = generator.RunDebug();

                planet.GetDistances().ToBitmap().Save("distances.png", ImageFormat.Png);
                float[,] height = planet.GetNormalizedHeight();
                new Texture(360, 180, ref height).ToBitmap().Save("heightmap.png", ImageFormat.Png);
                planet.GetTectonicPlates().ToBitmap().Save("plates.png", ImageFormat.Png);
                planet.GetWindMoveDirection().ToBitmap().Save("wind.png", ImageFormat.Png);
                planet.GetHeat().ToBitmap().Save("heat.png", ImageFormat.Png);
                planet.GetMoisture().ToBitmap().Save("moisture.png", ImageFormat.Png);

                generator.ColorTexture.ToBitmap().Save("color.png", ImageFormat.Png);
                generator.HeightTexture.ToBitmap().Save("height.png", ImageFormat.Png);
                generator.GlossTexture.ToBitmap().Save("gloss.png", ImageFormat.Png);
            }
        }

        #endregion Private Methods

        #region Private Structs

        private struct ColorARGB
        {
            #region Public Fields

            public byte A;
            public byte B;
            public byte G;
            public byte R;

            #endregion Public Fields

            #region Public Constructors

            public ColorARGB(System.Drawing.Color color)
            {
                A = color.A;
                R = color.R;
                G = color.G;
                B = color.B;
            }

            public ColorARGB(byte a, byte r, byte g, byte b)
            {
                A = a;
                R = r;
                G = g;
                B = b;
            }

            #endregion Public Constructors

            #region Public Methods

            public System.Drawing.Color ToColor()
            {
                return System.Drawing.Color.FromArgb(A, R, G, B);
            }

            #endregion Public Methods
        }

        #endregion Private Structs
    }
}