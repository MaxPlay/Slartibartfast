namespace Slartibartfast.Math
{
    public class MinMax<T>
    {
        #region Private Fields

        private T max;
        private T min;

        #endregion Private Fields

        #region Public Constructors

        public MinMax(T max)
        {
            this.max = max;
        }

        public MinMax(T min, T max)
        {
            this.min = min;
            this.max = max;
        }

        #endregion Public Constructors

        #region Public Properties

        public T Max
        {
            get { return max; }
            set { max = value; }
        }

        public T Min
        {
            get { return min; }
            set { min = value; }
        }

        #endregion Public Properties
    }
}