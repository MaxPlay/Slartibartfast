#region License
/***
* Extension created by Hugo Bonacci. http://hugoware.net/
* This work is licensed under a Creative Commons Attribution 3.0 Unported License.
*
* Source:
* http://hugoware.net/blog/enumeration-extensions-2-0
***/
#endregion License

using System;

namespace Slartibartfast.Extensions
{
    /// <summary>
    /// Extension methods to make working with Enum values easier
    /// </summary>
    public static class EnumerationExtensions
    {
        #region Public Methods

        /// <summary>
        /// Checks if an enumerated type contains a value
        /// </summary>
        public static bool Has<T>(this Enum value, T check)
        {
            Type type = value.GetType();

            //determine the values
            object result = value;
            _Value parsed = new _Value(check, type);
            if (parsed.Signed is long)
            {
                return (Convert.ToInt64(value) &
          (long)parsed.Signed) == (long)parsed.Signed;
            }
            else if (parsed.Unsigned is ulong)
            {
                return (Convert.ToUInt64(value) &
          (ulong)parsed.Unsigned) == (ulong)parsed.Unsigned;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Includes an enumerated type and returns the new value
        /// </summary>
        public static T Include<T>(this Enum value, T append)
        {
            Type type = value.GetType();

            //determine the values
            object result = value;
            _Value parsed = new _Value(append, type);
            if (parsed.Signed is long)
            {
                result = Convert.ToInt64(value) | (long)parsed.Signed;
            }
            else if (parsed.Unsigned is ulong)
            {
                result = Convert.ToUInt64(value) | (ulong)parsed.Unsigned;
            }

            //return the final value
            return (T)Enum.Parse(type, result.ToString());
        }

        /// <summary>
        /// Checks if an enumerated type is missing a value
        /// </summary>
        public static bool Missing<T>(this Enum obj, T value)
        {
            return !EnumerationExtensions.Has<T>(obj, value);
        }

        /// <summary>
        /// Removes an enumerated type and returns the new value
        /// </summary>
        public static T Remove<T>(this Enum value, T remove)
        {
            Type type = value.GetType();

            //determine the values
            object result = value;
            _Value parsed = new _Value(remove, type);
            if (parsed.Signed is long)
            {
                result = Convert.ToInt64(value) & ~(long)parsed.Signed;
            }
            else if (parsed.Unsigned is ulong)
            {
                result = Convert.ToUInt64(value) & ~(ulong)parsed.Unsigned;
            }

            //return the final value
            return (T)Enum.Parse(type, result.ToString());
        }

        #endregion Public Methods

        #region Private Classes

        //class to simplfy narrowing values between
        //a ulong and long since either value should
        //cover any lesser value
        private class _Value
        {
            #region Public Fields

            public long? Signed;

            public ulong? Unsigned;

            #endregion Public Fields

            #region Private Fields

            private static Type _UInt32 = typeof(long);

            //cached comparisons for tye to use
            private static Type _UInt64 = typeof(ulong);

            #endregion Private Fields

            #region Public Constructors

            public _Value(object value, Type type)
            {
                //make sure it is even an enum to work with
                if (!type.IsEnum)
                {
                    throw new
            ArgumentException("Value provided is not an enumerated type!");
                }

                //then check for the enumerated value
                Type compare = Enum.GetUnderlyingType(type);

                //if this is an unsigned long then the only
                //value that can hold it would be a ulong
                if (compare.Equals(_Value._UInt32) || compare.Equals(_Value._UInt64))
                {
                    this.Unsigned = Convert.ToUInt64(value);
                }
                //otherwise, a long should cover anything else
                else
                {
                    this.Signed = Convert.ToInt64(value);
                }
            }

            #endregion Public Constructors
        }

        #endregion Private Classes
    }
}