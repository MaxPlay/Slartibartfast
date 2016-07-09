using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slartibartfast.Extensions
{
    public static class SingleExtension
    {
        public static float[] ToVectorArray(this float f)
        {
            return new float[] { f };
        }
    }
}
