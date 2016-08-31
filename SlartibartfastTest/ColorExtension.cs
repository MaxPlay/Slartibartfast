using Slartibartfast.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlartibartfastTest
{
    public static class ColorExtension
    {
        public static System.Drawing.Color ToDrawingsColor(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
