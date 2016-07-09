using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slartibartfast.Extensions;

namespace Slartibartfast.Generators.NoiseUNUSED
{
    public class Fractal : Noise
    {
        protected float h;
        protected float lacunarity;
        protected float[] exponent;

        public Fractal() : base()
        {
            exponent = new float[MathHelper.MAX_OCTAVES];
        }

        public Fractal(int dimensions, int seed, float h, float lacunarity) : this()
        {
            Init(dimensions, seed, h, lacunarity);
        }

        public void Init(int dimensions, int seed, float h, float lacunarity)
        {
            Init(dimensions, seed);
            this.h = h;
            this.lacunarity = lacunarity;
            float f = 1;
            for (int i = 0; i < MathHelper.MAX_OCTAVES; i++)
            {
                exponent[i] = MathHelper.Pow(f, -h);
                f *= lacunarity;
            }
        }

        public float fBm(float[] vector, float octaves)
        {
            float value = 0;
            float[] temp = new float[MathHelper.MAX_DIMENSIONS];
            int i;
            for (i = 0; i < dimensions; i++)
            {
                temp[i] = vector[i];
            }

            for (i = 0; i < octaves; i++)
            {
                value += GenerateNoise(temp) * exponent[i];
                for (int j = 0; j < dimensions; j++)
                {
                    temp[j] *= lacunarity;
                }
            }

            octaves -= (int)octaves;
            if (octaves > MathHelper.DELTA)
                value += octaves * GenerateNoise(temp) * exponent[i];
            return MathHelper.Clamp(-0.99999f, 0.99999f, value);
        }

        public float Turbulence(float[] vector, float octaves)
        {
            float value = 0;
            float[] temp = new float[MathHelper.MAX_DIMENSIONS];
            int i;
            for (i = 0; i < dimensions; i++)
            {
                temp[i] = vector[i];
            }

            for (i = 0; i < octaves; i++)
            {
                value += GenerateNoise(temp) * exponent[i];
                for (int j = 0; j < dimensions; j++)
                {
                    temp[j] *= lacunarity;
                }
            }

            octaves -= (int)octaves;
            if (octaves > MathHelper.DELTA)
                value += octaves * Math.Abs(GenerateNoise(temp) * exponent[i]);
            return MathHelper.Clamp(-0.99999f, 0.99999f, value);
        }

        public float Multifractal(float[] vector, float octaves, float Offset)
        {
            float value = 1;
            float[] temp = new float[MathHelper.MAX_DIMENSIONS];
            int i;
            for (i = 0; i < dimensions; i++)
            {
                temp[i] = vector[i];
            }

            for (i = 0; i < octaves; i++)
            {
                value += GenerateNoise(temp) * exponent[i];
                for (int j = 0; j < dimensions; j++)
                {
                    temp[j] *= lacunarity;
                }
            }

            octaves -= (int)octaves;
            if (octaves > MathHelper.DELTA)
                value += octaves * (GenerateNoise(temp) * exponent[i] + Offset);
            return MathHelper.Clamp(-0.99999f, 0.99999f, value);
        }

        public float Heterofractal(float[] vector, float octaves, float Offset)
        {
            float value = GenerateNoise(vector) + Offset;
            float[] temp = new float[MathHelper.MAX_DIMENSIONS];
            int i;
            for (i = 0; i < dimensions; i++)
            {
                temp[i] = vector[i] * lacunarity;
            }

            for (i = 0; i < octaves; i++)
            {
                value += GenerateNoise(temp) * exponent[i];
                for (int j = 0; j < dimensions; j++)
                {
                    temp[j] *= lacunarity;
                }
            }

            octaves -= (int)octaves;
            if (octaves > MathHelper.DELTA)
                value += octaves * (GenerateNoise(temp) + Offset) * exponent[i] * value;
            return MathHelper.Clamp(-0.99999f, 0.99999f, value);
        }

        public float HybridMultifractal(float[] vector, float octaves, float Offset, float gain)
        {
            float value = (GenerateNoise(vector) + Offset) * exponent[0];
            float weight = value;
            float[] temp = new float[MathHelper.MAX_DIMENSIONS];
            int i;
            for (i = 0; i < dimensions; i++)
            {
                temp[i] = vector[i] * lacunarity;
            }

            for (i = 0; i < octaves; i++)
            {
                if (weight > 1)
                    weight = 1;
                float signal = (GenerateNoise(temp) + Offset) * exponent[i];
                value = weight * signal;
                weight *= gain * signal;
                for (int j = 0; j < dimensions; j++)
                {
                    temp[j] *= lacunarity;
                }
            }

            octaves -= (int)octaves;
            if (octaves > MathHelper.DELTA)
            {
                float signal = (GenerateNoise(temp) + Offset) * exponent[i];
                value += octaves * weight * signal;
            }
            return MathHelper.Clamp(-0.99999f, 0.99999f, value);
        }

        public float RidgedMultifractal(float[] vector, float octaves, float Offset, float gain)
        {
            float signal = Offset - Math.Abs(GenerateNoise(vector));
            signal *= signal;
            float value = signal;
            float[] temp = new float[MathHelper.MAX_DIMENSIONS];
            int i;
            for (i = 0; i < dimensions; i++)
            {
                temp[i] = vector[i];
            }

            for (i = 0; i < octaves; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    temp[j] *= lacunarity;
                }
                float weight = MathHelper.Clamp(0, 1, signal * gain);
                signal = Offset - Math.Abs(GenerateNoise(temp));
                signal *= signal;
                signal *= weight;
                value += signal * exponent[i];
            }

            return MathHelper.Clamp(-0.99999f, 0.99999f, value);
        }

        public float fBmTest(float[] vector, float octaves)
        {
            float value = 0;
            float[] temp = new float[MathHelper.MAX_DIMENSIONS];
            int i;
            for (i = 0; i < dimensions; i++)
            {
                temp[i] = vector[i];
            }

            for (i = 0; i < octaves; i++)
            {
                value += GenerateNoise(temp) * exponent[i];
                for (int j = 0; j < dimensions; j++)
                {
                    temp[j] *= lacunarity;
                }
            }

            octaves -= (int)octaves;
            if (octaves > MathHelper.DELTA)
                value += octaves * GenerateNoise(temp) * exponent[i];

            if (value <= 0.0f)
                return (float)-Math.Pow(-value, 0.7f);
            return (float)Math.Pow(value, 1 + GenerateNoise(temp) * value);
        }
    }
}
