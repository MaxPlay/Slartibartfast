using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slartibartfast.Extensions
{
    public static class ColorExtension
    {
        public static Color Lerp(Color c1, Color c2, float value)
        {
            if (value < 0)
                return c1;
            if (value > 1)
                return c2;

            return Color.FromArgb(
                (int)(c1.A * value + c2.A * (1 - value)),
                (int)(c1.R * value + c2.R * (1 - value)),
                (int)(c1.G * value + c2.G * (1 - value)),
                (int)(c1.B * value + c2.B * (1 - value))
                );
        }
    }
}
