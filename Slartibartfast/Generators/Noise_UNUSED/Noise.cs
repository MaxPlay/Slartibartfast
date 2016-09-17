using Slartibartfast.Extensions;
using System;

namespace Slartibartfast.Generators.NoiseUNUSED
{
    internal class Noise
    {
        #region Protected Fields

        protected float[][] buffer;
        protected int dimensions;
        protected int[] map;

        #endregion Protected Fields

        #region Public Constructors

        public Noise()
        {
            map = new int[256];
            buffer = new float[256][];
            for (int i = 0; i < 256; i++)
            {
                buffer[i] = new float[MathHelper.MAX_DIMENSIONS];
            }
        }

        public Noise(int dimensions, int seed) : this()
        {
            Init(dimensions, seed);
        }

        #endregion Public Constructors

        #region Public Methods

        public float GenerateNoise(float[] vector)
        {
            int[] n = new int[MathHelper.MAX_DIMENSIONS];
            float[] r = new float[MathHelper.MAX_DIMENSIONS];
            float[] w = new float[MathHelper.MAX_DIMENSIONS];
            for (int i = 0; i < dimensions; i++)
            {
                n[i] = MathHelper.Floor(vector[i]);
                r[i] = vector[i] - n[i];
                w[i] = MathHelper.Cubic(r[i]);
            }

            float value = 0;
            switch (dimensions)
            {
                case 1:
                    value = MathHelper.Lerp(Lattice(n[0], r[0]),
                                  Lattice(n[0] + 1, r[0] - 1),
                                  w[0]);
                    break;

                case 2:
                    value = MathHelper.Lerp(MathHelper.Lerp(Lattice(n[0], r[0], n[1], r[1]),
                                       Lattice(n[0] + 1, r[0] - 1, n[1], r[1]),
                                       w[0]),
                                  MathHelper.Lerp(Lattice(n[0], r[0], n[1] + 1, r[1] - 1),
                                       Lattice(n[0] + 1, r[0] - 1, n[1] + 1, r[1] - 1),
                                       w[0]),
                                  w[1]);
                    break;

                case 3:
                    value = MathHelper.Lerp(MathHelper.Lerp(MathHelper.Lerp(Lattice(n[0], r[0], n[1], r[1], n[2], r[2]),
                                            Lattice(n[0] + 1, r[0] - 1, n[1], r[1], n[2], r[2]),
                                            w[0]),
                                       MathHelper.Lerp(Lattice(n[0], r[0], n[1] + 1, r[1] - 1, n[2], r[2]),
                                            Lattice(n[0] + 1, r[0] - 1, n[1] + 1, r[1] - 1, n[2], r[2]),
                                            w[0]),
                                       w[1]),
                                  MathHelper.Lerp(MathHelper.Lerp(Lattice(n[0], r[0], n[1], r[1], n[2] + 1, r[2] - 1),
                                            Lattice(n[0] + 1, r[0] - 1, n[1], r[1], n[2] + 1, r[2] - 1),
                                            w[0]),
                                       MathHelper.Lerp(Lattice(n[0], r[0], n[1] + 1, r[1] - 1, n[2] + 1, r[2] - 1),
                                            Lattice(n[0] + 1, r[0] - 1, n[1] + 1, r[1] - 1, n[2] + 1, r[2] - 1),
                                            w[0]),
                                       w[1]),
                                  w[2]);
                    break;
            }

            return MathHelper.Clamp(-0.99999f, 0.99999f, value);
        }

        public void Init(int dimensions, int seed)
        {
            this.dimensions = System.Math.Min(dimensions, MathHelper.MAX_DIMENSIONS);
            Random r = new Random(MathHelper.RandomSeed != null ? (int)MathHelper.RandomSeed : 0);
            int i, j;
            for (i = 0; i < 256; i++)
            {
                map[i] = i;
                for (j = 0; j < dimensions; j++)
                {
                    buffer[i][j] = r.Range(-0.5f, 0.5f);
                }
                MathHelper.Normalize(buffer[i], dimensions);
            }

            while (i != 0)
            {
                i--;
                j = r.Next(0, 255);
                MathHelper.Swap(ref map, i, j);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected float Lattice(int ix, float fx, int iy = 0, float fy = 0, int iz = 0, float fz = 0, int iw = 0, float fw = 0)
        {
            int[] n = new int[] { ix, iy, iz, iw };
            float[] f = new float[] { fx, fy, fz, fw };
            int index = 0;
            for (int i = 0; i < dimensions; i++)
            {
                index = map[(index + n[i]) & 0xFF];
            }
            float value = 0;
            for (int i = 0; i < dimensions; i++)
            {
                value += buffer[index][i] * f[i];
            }
            return value;
        }

        #endregion Protected Methods
    }
}