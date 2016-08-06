using Slartibartfast.Math;
using System;
using System.Drawing;

namespace Slartibartfast.Extensions
{
    public static class RandomExtension
    {
        #region Public Methods

        public static Color Color(this Random r)
        {
            return System.Drawing.Color.FromArgb(255, r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));
        }

        public static int Range(this Random r, int rMin, int rMax)
        {
            return rMin + r.Next() * (rMax - rMin);
        }

        public static double Range(this Random r, double rMin, double rMax)
        {
            return rMin + r.NextDouble() * (rMax - rMin);
        }

        public static float Range(this Random r, float rMin, float rMax)
        {
            return rMin + (float)r.NextDouble() * (rMax - rMin);
        }

        public static Vector2 Vector2(this Random r)
        {
            return new Vector2((float)r.NextDouble() * 2 - 1, (float)r.NextDouble() * 2 - 1);
        }

        public static Vector3 Vector3(this Random r)
        {
            return new Vector3((float)r.NextDouble() * 2 - 1, (float)r.NextDouble() * 2 - 1, (float)r.NextDouble() * 2 - 1);
        }

        #endregion Public Methods
    }
}