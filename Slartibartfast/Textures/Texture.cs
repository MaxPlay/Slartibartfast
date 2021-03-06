﻿using Slartibartfast.Extensions;
using Slartibartfast.Math;

namespace Slartibartfast.Textures
{
    public class Texture
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
            : this(width, height, ref array, false)
        {
            
        }

        public Texture(int width, int height, ref float[,] array, bool fromNormalized)
        {
            this.width = width;
            this.height = height;
            color = new Color[width * height];

            float highestValue = fromNormalized ? 1: array.GetHighestValue();
            float lowestValue = fromNormalized ? 0 : array.GetLowestValue();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    color[y * width + x] = Math.Color.Lerp(Math.Color.Black, Math.Color.White, (highestValue - lowestValue) != 0 ? (array[x, y] - lowestValue) / (highestValue - lowestValue) : 0);
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

        /*
        public void SaveToFile(string filename, bool overwrite = true)
        {
            if (File.Exists(filename))
                if (!overwrite)
                    return;

            ToBitmap(this.color, width, height).Save(filename, ImageFormat.Png);
        }
        */

        #region Public Methods

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
                    color[y * width + x] = Math.Color.Lerp(Math.Color.Black, Math.Color.White, (array[x, y] - lowestValue) / (highestValue - lowestValue));
                }
            }
        }

        #endregion Private Methods

        /*
        /// <summary>
        /// Taken from one of my own projects. This is originally coming from: http://stackoverflow.com/questions/13511661/create-bitmap-from-double-two-dimentional-array
        /// </summary>
        /// <param name="rawImage">The inputarray</param>
        /// <param name="width">   The width of the array.</param>
        /// <param name="height">  The height of the array.</param>
        /// <returns></returns>
        private unsafe Bitmap ToBitmap(Color[] rawImage, int width, int height)
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

        private struct ColorARGB
        {
            public byte A;
            public byte B;
            public byte G;
            public byte R;

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
        */
    }
}