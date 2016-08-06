using Slartibartfast.Extensions;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Slartibartfast.Textures
{
    public enum TextureType
    {
        color,
        height,
        gloss
    }

    internal class Texture
    {
        #region Private Fields

        private Color[] color;
        private int height;
        private TextureType textureType;
        private int width;

        #endregion Private Fields

        #region Public Constructors

        public Texture(int width, int height, ref Color[,] array)
        {
            this.width = width;
            this.height = height;
            color = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    color[y * width + x] = array[x, y];
                }
            }
        }

        public Texture(int width, int height, ref float[,] array)
        {
            this.width = width;
            this.height = height;
            color = new Color[width * height];

            float highestValue = array.GetHighestValue();
            float lowestValue = array.GetLowestValue();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    color[y * width + x] = ColorExtension.Lerp(System.Drawing.Color.Black, System.Drawing.Color.White, (highestValue - lowestValue) != 0 ? (array[x, y] - lowestValue) / (highestValue - lowestValue) : 0);
                }
            }
        }

        public Texture(int width, int height)
        {
            this.width = width;
            this.height = height;
            color = new Color[width * height];
        }

        #endregion Public Constructors

        #region Public Properties

        public Color[] Color { get { return color; } }
        public int Height { get { return height; } }
        public TextureType Type { get { return textureType; } set { textureType = value; } }
        public int Width { get { return width; } }

        #endregion Public Properties

        #region Public Methods

        public void SaveToFile(string filename, bool overwrite = true)
        {
            if (File.Exists(filename))
                if (!overwrite)
                    return;

            ToBitmap(this.color, width, height).Save(filename, ImageFormat.Png);
        }

        public void SetColorData(ref Color[,] array)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    color[y * width + x] = array[x, y];
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void SetHeightData(ref float[,] array)
        {
            float highestValue = array.GetHighestValue();
            float lowestValue = array.GetLowestValue();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    color[y * width + x] = ColorExtension.Lerp(System.Drawing.Color.Black, System.Drawing.Color.White, (array[x, y] - lowestValue) / (highestValue - lowestValue));
                }
            }
        }

        /// <summary>
        /// Taken from one of my own projects. This is originally coming from: http://stackoverflow.com/questions/13511661/create-bitmap-from-double-two-dimentional-array
        /// </summary>
        /// <param name="rawImage">The inputarray</param>
        /// <param name="width">   The width of the array.</param>
        /// <param name="height">  The height of the array.</param>
        /// <returns></returns>
        private unsafe System.Drawing.Bitmap ToBitmap(Color[] rawImage, int width, int height)
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
                    position->A = color.A;
                    position->R = color.R;
                    position->G = color.G;
                    position->B = color.B;
                }

            Image.UnlockBits(bitmapData);
            return Image;
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