using Slartibartfast;
using Slartibartfast.Extensions;
using Slartibartfast.Planets;
using Slartibartfast.Textures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

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
            List<TimeSpan> spans = new List<TimeSpan>();
            for (int i = 1; i <= 500; i++)
            {
                DateTime time = DateTime.Now;
                Console.WriteLine("{0}: {1}", i, time);
                using (PlanetGenerator generator = new PlanetGenerator())
                {
                    MathHelper.RandomSeed = 3;
                    PlanetSettings settings = PlanetSettings.Earth();

                    generator.PlanetSettings = settings;

                    Planet planet = generator.RunDebug();

                    planet.GetDistances().ToBitmap().Save("distances.png", ImageFormat.Png);
                    float[,] height = planet.GetNormalizedHeight();
                    new Texture(360, 180, ref height, true).ToBitmap().Save("heightmap.png", ImageFormat.Png);
                    planet.GetTectonicPlates().ToBitmap().Save("plates.png", ImageFormat.Png);
                    planet.GetWindMoveDirection().ToBitmap().Save("wind.png", ImageFormat.Png);
                    planet.GetHeat().ToBitmap().Save("heat.png", ImageFormat.Png);
                    planet.GetMoisture().ToBitmap().Save("moisture.png", ImageFormat.Png);

                    generator.ColorTexture.ToBitmap().Save("color.png", ImageFormat.Png);
                    generator.HeightTexture.ToBitmap().Save("height.png", ImageFormat.Png);
                    generator.GlossTexture.ToBitmap().Save("gloss.png", ImageFormat.Png);
                }

                TimeSpan span = DateTime.Now - time;

                Console.WriteLine("{0}: {1}", i, DateTime.Now);
                Console.WriteLine("{0}: {1}", i, "Finished after: ");
                Console.WriteLine("{0}: {1}", i, span);
                spans.Add(span);
            }

            double doubleAverageTicks = spans.Average(timeSpan => timeSpan.Ticks);
            long longAverageTicks = Convert.ToInt64(doubleAverageTicks);

            using (FileStream stream = File.OpenWrite("t.txt"))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    foreach (TimeSpan item in spans)
                    {
                        writer.WriteLine(item.TotalSeconds);
                    }
                }
            }

            TimeSpan average = new TimeSpan(longAverageTicks);

            Console.WriteLine();
            Console.WriteLine("Average Time:");
            Console.WriteLine(average);
            Console.ReadLine();
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