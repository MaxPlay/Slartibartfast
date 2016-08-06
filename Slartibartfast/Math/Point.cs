using System;

namespace Slartibartfast.Math
{
    public class Point
    {
        #region Private Fields

        private int x;

        private int y;

        #endregion Private Fields

        #region Public Constructors

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion Public Constructors

        #region Public Properties

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        #endregion Public Properties

        #region Public Methods

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.x - p2.x, p1.y - p2.y);
        }

        public static Point operator *(Point p1, int i)
        {
            return new Point(p1.x * i, p1.y * i);
        }

        public static Point operator /(Point p1, int i)
        {
            if (i == 0)
                throw new DivideByZeroException();

            return new Point(p1.x / i, p1.y / i);
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.x + p2.x, p1.y + p2.y);
        }

        #endregion Public Methods
    }
}