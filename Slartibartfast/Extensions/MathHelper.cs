using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slartibartfast.Extensions
{
    public static class MathHelper
    {
        public static float Lerp(float f0, float f1, float t)
        {
            return (1f - t) * f0 + t * f1;
        }

        public static double Lerp(double f0, double f1, double t)
        {
            return (1f - t) * f0 + t * f1;
        }
    }
}
