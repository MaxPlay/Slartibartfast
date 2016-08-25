using Slartibartfast.Extensions;
using System;

namespace Slartibartfast.Generators
{
    public class Simplex
    {
        #region Private Fields

        private double[] amplitudes;
        private double[] frequencys;
        private int largestFeature;
        private Octave[] octaves;
        private double persistence;

        #endregion Private Fields

        #region Public Constructors

        public Simplex(int largestFeature, double persistence, int? seed = null)
        {
            this.largestFeature = largestFeature;
            this.persistence = persistence;

            //recieves a number (eg 128) and calculates what power of 2 it is (eg 2^7)
            int numberOfOctaves = (int)System.Math.Ceiling(System.Math.Log10(largestFeature) / System.Math.Log10(2));

            octaves = new Octave[numberOfOctaves];
            frequencys = new double[numberOfOctaves];
            amplitudes = new double[numberOfOctaves];

            Random rnd = new Random(seed == null ? MathHelper.RandomSeed != null ? (int)MathHelper.RandomSeed : 0 : (int)seed);

            for (int i = 0; i < numberOfOctaves; i++)
            {
                octaves[i] = new Octave(rnd.Next());

                frequencys[i] = System.Math.Pow(2, i);
                amplitudes[i] = System.Math.Pow(persistence, octaves.Length - i);
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public double GetNoise(int x, int y)
        {
            double result = 0;

            for (int i = 0; i < octaves.Length; i++)
            {
                //double frequency = Math.Pow(2, i);
                //double amplitude = Math.Pow(persistence, octaves.Length - i);

                result += octaves[i].Noise(x / frequencys[i], y / frequencys[i]) * amplitudes[i];
            }

            return result;
        }

        public double GetNoise(int x, int y, int z)
        {
            double result = 0;

            for (int i = 0; i < octaves.Length; i++)
            {
                double frequency = System.Math.Pow(2, i);
                double amplitude = System.Math.Pow(persistence, octaves.Length - i);

                result += octaves[i].Noise(x / frequency, y / frequency, z / frequency) * amplitude;
            }

            return result;
        }

        public double GetNoise(double x, double y, double z, double w)
        {
            double result = 0;

            for (int i = 0; i < octaves.Length; i++)
            {
                double frequency = System.Math.Pow(2, i);
                double amplitude = System.Math.Pow(persistence, octaves.Length - i);

                result += octaves[i].Noise(x / frequency, y / frequency, z / frequency, w / frequency) * amplitude;
            }

            return result;
        }

        public double GetTileableFBM(int x, int y, double tileBeginX, double tileEndX, double tileBeginY, double tileEndY, double width, double height, double frequency)
        {
            double s = x / width;
            double t = y / height;
            double dx = tileEndX - tileBeginX;
            double dy = tileEndY - tileBeginY;

            double nx = tileBeginX + System.Math.Cos(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double ny = tileBeginY + System.Math.Cos(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);
            double nz = tileBeginX + System.Math.Sin(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double nw = tileBeginY + System.Math.Sin(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);

            float h = 0;
            for (int c = 1; c < frequency; c *= 2)
            {
                float dh = (float)(0.5 * (1 + GetNoise(nx * frequency / c, ny * frequency / c, nz * frequency / c, nw * frequency / c)));
                h = dh + 0.5f * h;
            }

            return h;
        }

        public double GetTileableFBM(int x, int y, double width, double height, double frequency)
        {
            double tileBeginX = 0;
            double tileEndX = width;
            double tileBeginY = 0;
            double tileEndY = height;

            double s = x / width;
            double t = y / height;
            double dx = tileEndX - tileBeginX;
            double dy = tileEndY - tileBeginY;

            double nx = tileBeginX + System.Math.Cos(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double ny = tileBeginY + System.Math.Cos(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);
            double nz = tileBeginX + System.Math.Sin(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double nw = tileBeginY + System.Math.Sin(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);

            float h = 0;
            for (int c = 1; c < frequency; c *= 2)
            {
                float dh = (float)(0.5 * (1 + GetNoise(nx * frequency / c, ny * frequency / c, nz * frequency / c, nw * frequency / c)));
                h = dh + 0.5f * h;
            }

            return h;
        }

        public double GetTileableFBM(int x, int y, double width, double height, double frequency, double zoom)
        {
            double tileBeginX = 0;
            double tileEndX = width * zoom;
            double tileBeginY = 0;
            double tileEndY = height * zoom;

            double s = x / width;
            double t = y / height;
            double dx = tileEndX - tileBeginX;
            double dy = tileEndY - tileBeginY;

            double nx = tileBeginX + System.Math.Cos(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double ny = tileBeginY + System.Math.Cos(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);
            double nz = tileBeginX + System.Math.Sin(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double nw = tileBeginY + System.Math.Sin(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);

            float h = 0;
            for (int c = 1; c < frequency; c *= 2)
            {
                float dh = (float)(0.5 * (1 + GetNoise(nx * frequency / c, ny * frequency / c, nz * frequency / c, nw * frequency / c)));
                h = dh + 0.5f * h;
            }

            return h;
        }

        public double GetTileableNoise(int x, int y, double tileBeginX, double tileEndX, double tileBeginY, double tileEndY, double width, double height, double frequency)
        {
            double s = x / width;
            double t = y / height;
            double dx = tileEndX - tileBeginX;
            double dy = tileEndY - tileBeginY;

            double nx = tileBeginX + System.Math.Cos(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double ny = tileBeginY + System.Math.Cos(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);
            double nz = tileBeginX + System.Math.Sin(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double nw = tileBeginY + System.Math.Sin(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);

            return GetNoise(nx, ny, nz, nw);
        }

        public double GetTileableNoise(int x, int y, double width, double height, double frequency, double zoom)
        {
            double tileBeginX = 0;
            double tileEndX = width * zoom;
            double tileBeginY = 0;
            double tileEndY = height * zoom;

            double s = x / width;
            double t = y / height;
            double dx = tileEndX - tileBeginX;
            double dy = tileEndY - tileBeginY;

            double nx = tileBeginX + System.Math.Cos(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double ny = tileBeginY + System.Math.Cos(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);
            double nz = tileBeginX + System.Math.Sin(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double nw = tileBeginY + System.Math.Sin(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);

            return GetNoise(nx, ny, nz, nw);
        }

        public double GetTileableNoise(int x, int y, double width, double height, double frequency)
        {
            double tileBeginX = 0;
            double tileEndX = width;
            double tileBeginY = 0;
            double tileEndY = height;

            double s = x / width;
            double t = y / height;
            double dx = tileEndX - tileBeginX;
            double dy = tileEndY - tileBeginY;

            double nx = tileBeginX + System.Math.Cos(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double ny = tileBeginY + System.Math.Cos(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);
            double nz = tileBeginX + System.Math.Sin(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double nw = tileBeginY + System.Math.Sin(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);

            return GetNoise(nx, ny, nz, nw);
        }

        #endregion Public Methods
    }
}