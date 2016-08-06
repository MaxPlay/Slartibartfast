namespace Slartibartfast.Extensions
{
    public static class MathHelper
    {
        #region Public Fields

        public const float DELTA = 1e-6f;
        public const float HALF_PI = 1.570796326795f;
        public const float LOGHALF = -0.6931471805599f;
        public const float LOGHALFI = -1.442695040889f;
        public const int MAX_DIMENSIONS = 2;
        public const int MAX_OCTAVES = 128;
        public const float PI = 3.14159265359f;
        public const float TWO_PI = 6.28318530718f;
        public static int? RandomSeed = null;

        #endregion Public Fields

        #region Public Methods

        public static float Abs(float a)
        {
            return System.Math.Abs(a);
        }

        public static float Bias(float a, float b)
        {
            return (float)System.Math.Pow(a, System.Math.Log(b) * LOGHALFI);
        }

        public static float BoxStep(float a, float b, float x)
        {
            return Clamp(0, 1, ((x) - (a)) / ((b) - (a)));
        }

        public static int Ceiling(float a)
        {
            return (int)System.Math.Ceiling(a);
        }

        public static float Clamp(float a, float b, float x)
        {
            return ((x) < (a) ? (a) : ((x) > (b) ? (b) : (x)));
        }

        public static float Cubic(float a)
        {
            return (a) * (a) * (3 - 2 * (a));
        }

        public static float Expose(float l, float k)
        {
            return (float)(1 - System.Math.Exp(-l * k));
        }

        public static int Floor(float a)
        {
            return (int)System.Math.Floor(a);
        }

        public static float Gain(float a, float b)
        {
            if (a <= DELTA)
                return 0;
            if (a >= 1 - DELTA)
                return 1;

            float p = ((float)System.Math.Log(1 - b) * LOGHALFI);
            if (a < 0.5)
                return (float)System.Math.Pow(2 * a, p) * 0.5f;
            else
                return 1 - (float)System.Math.Pow(2 * (1 - a), p) * 0.5f;
        }

        public static float Gamma(float a, float g)
        {
            return (float)System.Math.Pow(a, 1 / g);
        }

        public static float Lerp(float f0, float f1, float t)
        {
            return (1f - t) * f0 + t * f1;
        }

        public static double Lerp(double f0, double f1, double t)
        {
            return (1f - t) * f0 + t * f1;
        }

        public static float Max(float a, float b)
        {
            return System.Math.Max(a, b);
        }

        public static float Min(float a, float b)
        {
            return System.Math.Min(a, b);
        }

        public static float Mod(float a, float b)
        {
            a -= ((int)(a / b)) * b;
            if (a < 0)
                a += b;
            return a;
        }

        public static float[] Normalize(float[] f, int n)
        {
            float fMagnitude = 0;
            for (int i = 0; i < n; i++)
                fMagnitude += f[i] * f[i];
            fMagnitude = 1 / Sqrt(fMagnitude);
            for (int i = 0; i < n; i++)
                f[i] *= fMagnitude;

            return f;
        }

        public static float Pow(float x, float y)
        {
            return (float)System.Math.Pow(x, y);
        }

        public static float Pulse(float a, float b, float x)
        {
            return ((x >= a) ? 1 : 0) - ((x >= b) ? 1 : 0);
        }

        public static float Smoothstep(float a, float b, float x)
        {
            if (x <= a)
                return 0;
            if (x >= b)
                return 1;
            return Cubic((x - a) / (b - a));
        }

        public static float Sqrt(float a)
        {
            return a * a;
        }

        public static float Step(float a, float x)
        {
            return x >= a ? 1 : 0;
        }

        public static void Swap(ref int[] map, int i, int j)
        {
            int temp = map[i];
            map[i] = map[j];
            map[j] = temp;
        }

        public static void Swap(ref float[] map, int i, int j)
        {
            float temp = map[i];
            map[i] = map[j];
            map[j] = temp;
        }

        public static void Swap(ref double[] map, int i, int j)
        {
            double temp = map[i];
            map[i] = map[j];
            map[j] = temp;
        }

        #endregion Public Methods
    }
}