using Slartibartfast.Math;

namespace Slartibartfast.Planets
{
    public struct BiomeColors
    {
        private Color[] colors;

        public Color this[Biome biome]
        {
            get
            {
                if (colors == null)
                    colors = new Color[8];
                return colors[(int)biome];
            }

            set
            {
                if (colors == null)
                    colors = new Color[8];
                colors[(int)biome] = value;
            }
        }
    }
}