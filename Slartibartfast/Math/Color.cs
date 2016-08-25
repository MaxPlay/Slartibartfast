using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slartibartfast.Math
{
    public struct Color
    {
        private byte r;

        public byte R
        {
            get { return r; }
            set { r = value; }
        }

        private byte g;

        public byte G
        {
            get { return g; }
            set { g = value; }
        }

        private byte b;

        public byte B
        {
            get { return b; }
            set { b = value; }
        }

        private byte a;

        public Color(int r, int g, int b) : this(r, g, b, 255)
        {
        }

        public Color(int r, int g, int b, int a) : this()
        {
            this.r = (byte)r;
            this.g = (byte)g;
            this.b = (byte)b;
            this.a = (byte)a;
        }

        public byte A
        {
            get { return a; }
            set { a = value; }
        }

        public static Color Lerp(Color c1, Color c2, float value)
        {
            if (value < 0)
                return c1;
            if (value > 1)
                return c2;

            return new Color(
                (int)(c1.R * value + c2.R * (1 - value)),
                (int)(c1.G * value + c2.G * (1 - value)),
                (int)(c1.B * value + c2.B * (1 - value)),
                (int)(c1.A * value + c2.A * (1 - value))
                );
        }

        public static Color Black { get { return new Color(0, 0, 0, 255); } }
        public static Color White { get { return new Color(255, 255, 255, 255); } }
    }
}
