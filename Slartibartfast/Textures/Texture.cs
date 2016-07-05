using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slartibartfast.Extensions;
using System.IO;
using System.Drawing.Imaging;

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
        int width;
        int height;
        Color[] color;
        TextureType textureType;

        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public Color[] Color { get { return color; } }
        public TextureType Type { get { return textureType; } set { textureType = value; } }

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
                    color[y * width + x] = ColorExtension.Lerp(System.Drawing.Color.Black, System.Drawing.Color.White, (array[x, y] - lowestValue) / (highestValue - lowestValue));
                }
            }
        }

        public Texture(int width, int height)
        {
            this.width = width;
            this.height = height;
            color = new Color[width * height];
        }

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

        public void SaveToFile(string filename, bool overwrite = true)
        {
            if (File.Exists(filename))
                if (!overwrite)
                    return;

            ToBitmap(this.color, width, height).Save(filename, ImageFormat.Png);
        }

        /// <summary>
        /// Taken from one of my own projects. This is originally coming from: http://stackoverflow.com/questions/13511661/create-bitmap-from-double-two-dimentional-array
        /// </summary>
        /// <param name="rawImage">The inputarray</param>
        /// <param name="width">The width of the array.</param>
        /// <param name="height">The height of the array.</param>
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

        private struct ColorARGB
        {
            public byte B;
            public byte G;
            public byte R;
            public byte A;

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

            public System.Drawing.Color ToColor()
            {
                return System.Drawing.Color.FromArgb(A, R, G, B);
            }
        }
    }
}
