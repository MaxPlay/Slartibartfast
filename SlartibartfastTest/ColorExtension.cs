using Slartibartfast.Math;

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