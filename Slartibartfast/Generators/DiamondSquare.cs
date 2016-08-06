using Slartibartfast.Extensions;
using System;

namespace Slartibartfast.Generators
{
    internal class DiamondSquare
    {
        #region Private Fields

        private Random random;

        #endregion Private Fields

        #region Public Constructors

        public DiamondSquare()
        {
            random = new Random(MathHelper.RandomSeed != null ? (int)MathHelper.RandomSeed : 0);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// This is basically taken from https://github.com/eogas/DiamondSquare, but modified to fit
        /// my needs in terms of readability and usability.
        ///
        /// This generates random noise using the diamond square algorithm.
        /// </summary>
        /// <param name="size">        
        /// The size of the resulting array. Needs to be a Power of 2.
        /// </param>
        /// <param name="minElevation">The min elevation of the terrain.</param>
        /// <param name="maxElevation">The max elevation of the terrain.</param>
        /// <param name="noise">       The noise applied to the result.</param>
        /// <returns></returns>
        public float[,] Generate(int size, float minElevation = 0, float maxElevation = 255, float noise = 0.0f)
        {
            random = new Random(MathHelper.RandomSeed != null ? (int)MathHelper.RandomSeed : 0);

            // Fail if grid size is not of the form (2 ^ n) - 1 or if min/max values are invalid
            size++;
            int s = size - 1;
            if (!IsPow2(s) || minElevation >= maxElevation)
                return null;

            // init the grid
            float[,] grid = new float[size, size];

            // Seed the first four corners
            grid[0, 0] = 0;//random.Range(minElevation, maxElevation);
            grid[s, 0] = 0;//random.Range(minElevation, maxElevation);
            grid[0, s] = 0; //random.Range(minElevation, maxElevation);
            grid[s, s] = 0; //random.Range(minElevation, maxElevation);

            /*
			 * Use temporary named variables to simplify equations
			 *
			 * s0 . d0. s1
			 *  . . . . .
			 * d1 . cn. d2
			 *  . . . . .
			 * s2 . d3. s3
			 *
			 * */
            float s0, s1, s2, s3, d0, d1, d2, d3, center, modNoise;

            for (int i = s; i > 1; i /= 2)
            {
                // reduce the random range at each step
                modNoise = (maxElevation - minElevation) * noise * ((float)i / s);

                // diamonds
                for (int y = 0; y < s; y += i)
                {
                    for (int x = 0; x < s; x += i)
                    {
                        s0 = grid[x, y];
                        s1 = grid[x + i, y];
                        s2 = grid[x, y + i];
                        s3 = grid[x + i, y + i];

                        // center
                        grid[x + (i / 2), y + (i / 2)] = ((s0 + s1 + s2 + s3) / 4.0f)
                            + random.Range(-modNoise, modNoise);
                    }
                }

                // squares
                for (int y = 0; y < s; y += i)
                {
                    for (int x = 0; x < s; x += i)
                    {
                        s0 = grid[x, y];
                        s1 = grid[x + i, y];
                        s2 = grid[x, y + i];
                        s3 = grid[x + i, y + i];
                        center = grid[x + (i / 2), y + (i / 2)];

                        d0 = y <= 0 ? (s0 + s1 + center) / 3.0f : (s0 + s1 + center + grid[x + (i / 2), y - (i / 2)]) / 4.0f;
                        d1 = x <= 0 ? (s0 + center + s2) / 3.0f : (s0 + center + s2 + grid[x - (i / 2), y + (i / 2)]) / 4.0f;
                        d2 = x >= s - i ? (s1 + center + s3) / 3.0f :
                            (s1 + center + s3 + grid[x + i + (i / 2), y + (i / 2)]) / 4.0f;
                        d3 = y >= s - i ? (center + s2 + s3) / 3.0f :
                            (center + s2 + s3 + grid[x + (i / 2), y + i + (i / 2)]) / 4.0f;

                        grid[x + (i / 2), y] = d0 + random.Range(-modNoise, modNoise);
                        grid[x, y + (i / 2)] = d1 + random.Range(-modNoise, modNoise);
                        grid[x + i, y + (i / 2)] = d2 + random.Range(-modNoise, modNoise);
                        grid[x + (i / 2), y + i] = d3 + random.Range(-modNoise, modNoise);
                    }
                }
            }

            return grid;
        }

        #endregion Public Methods

        #region Private Methods

        private bool IsPow2(int a)
        {
            return (a & (a - 1)) == 0;
        }

        #endregion Private Methods
    }
}