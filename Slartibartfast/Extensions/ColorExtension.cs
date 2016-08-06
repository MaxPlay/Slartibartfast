using System.Drawing;

namespace Slartibartfast.Extensions
{
    public static class ColorExtension
    {
        #region Public Methods

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

        #endregion Public Methods
    }
}