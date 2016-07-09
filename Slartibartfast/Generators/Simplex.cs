﻿using System;

namespace Slartibartfast.Generators
{
    public class Simplex
    {

        Octave[] octaves;
        double[] frequencys;
        double[] amplitudes;

        int largestFeature;
        double persistence;
        int seed;

        public Simplex(int largestFeature, double persistence, int seed)
        {
            this.largestFeature = largestFeature;
            this.persistence = persistence;
            this.seed = seed;

            //recieves a number (eg 128) and calculates what power of 2 it is (eg 2^7)
            int numberOfOctaves = (int)Math.Ceiling(Math.Log10(largestFeature) / Math.Log10(2));

            octaves = new Octave[numberOfOctaves];
            frequencys = new double[numberOfOctaves];
            amplitudes = new double[numberOfOctaves];

            Random rnd = new Random(seed);

            for (int i = 0; i < numberOfOctaves; i++)
            {
                octaves[i] = new Octave(rnd.Next());

                frequencys[i] = Math.Pow(2, i);
                amplitudes[i] = Math.Pow(persistence, octaves.Length - i);
            }

        }


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
                double frequency = Math.Pow(2, i);
                double amplitude = Math.Pow(persistence, octaves.Length - i);

                result += octaves[i].Noise(x / frequency, y / frequency, z / frequency) * amplitude;
            }


            return result;

        }

        public double GetNoise(double x, double y, double z, double w)
        {

            double result = 0;

            for (int i = 0; i < octaves.Length; i++)
            {
                double frequency = Math.Pow(2, i);
                double amplitude = Math.Pow(persistence, octaves.Length - i);

                result += octaves[i].Noise(x / frequency, y / frequency, z / frequency, w / frequency) * amplitude;
            }


            return result;

        }

        public double GetTileableNoise(int x, int y, double tileBeginX, double tileEndX, double tileBeginY, double tileEndY, double width, double height, double frequency)
        {
            double s = x / width;
            double t = y / height;
            double dx = tileEndX - tileBeginX;
            double dy = tileEndY - tileBeginY;

            double nx = tileBeginX + Math.Cos(s * 2 * Math.PI) * dx / (2 * Math.PI);
            double ny = tileBeginY + Math.Cos(t * 2 * Math.PI) * dy / (2 * Math.PI);
            double nz = tileBeginX + Math.Sin(s * 2 * Math.PI) * dx / (2 * Math.PI);
            double nw = tileBeginY + Math.Sin(t * 2 * Math.PI) * dy / (2 * Math.PI);

            return GetNoise(nx, ny, nz, nw);
        }

        public double GetTileableFBM(int x, int y, double tileBeginX, double tileEndX, double tileBeginY, double tileEndY, double width, double height, double frequency)
        {
            double s = x / width;
            double t = y / height;
            double dx = tileEndX - tileBeginX;
            double dy = tileEndY - tileBeginY;

            double nx = tileBeginX + Math.Cos(s * 2 * Math.PI) * dx / (2 * Math.PI);
            double ny = tileBeginY + Math.Cos(t * 2 * Math.PI) * dy / (2 * Math.PI);
            double nz = tileBeginX + Math.Sin(s * 2 * Math.PI) * dx / (2 * Math.PI);
            double nw = tileBeginY + Math.Sin(t * 2 * Math.PI) * dy / (2 * Math.PI);

            float h = 0;
            for (int c = 1; c < frequency; c *= 2)
            {
                float dh = (float)(0.5 * (1 + GetNoise(nx * frequency / c, ny * frequency / c, nz * frequency / c, nw * frequency / c)));
                h = dh + 0.5f * h;
            }

            return h;
        }
    }
}