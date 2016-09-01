using Slartibartfast.Textures;
using System.Drawing;

namespace SlartibartfastTest
{
    public static class TextureExtension
    {
        public static Bitmap ToBitmap(this Texture texture)
        {
            Color[] colors = new Color[360 * 180];

            for (int i = 0; i < 360 * 180; i++)
            {
                colors[i] = texture.Color[i].ToDrawingsColor();
            }

            return Program.ToBitmap(colors, texture.Width, texture.Height);
        }
    }
}