using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slartibartfast.Math
{
    public class MinMax<T>
    {
        private T min;

        public T Min
        {
            get { return min; }
            set { min = value; }
        }

        private T max;

        public T Max
        {
            get { return max; }
            set { max = value; }
        }

        public MinMax(T max)
        {
            this.max = max;
        }

        public MinMax(T min, T max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
