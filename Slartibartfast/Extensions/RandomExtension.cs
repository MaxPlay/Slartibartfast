using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slartibartfast.Extensions
{
    public static class RandomExtension
    {
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
    }
}
