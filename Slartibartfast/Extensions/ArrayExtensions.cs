namespace Slartibartfast.Extensions
{
    public static class ArrayExtensions
    {
        #region Public Methods

        public static float GetHighestValue(this float[,] f)
        {
            float value = float.MinValue;
            int width = f.GetLength(0);
            int height = f.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (value < f[x, y])
                        value = f[x, y];
                }
            }

            return value;
        }

        public static float GetLowestValue(this float[,] f)
        {
            float value = float.MaxValue;
            int width = f.GetLength(0);
            int height = f.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (value > f[x, y])
                        value = f[x, y];
                }
            }

            return value;
        }

        #endregion Public Methods
    }
}