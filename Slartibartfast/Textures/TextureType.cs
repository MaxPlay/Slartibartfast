using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Slartibartfast.Textures
{
    [Flags]
    public enum TextureType
    {
        None = 0,
        Color = 1,
        Height = 2,
        Gloss = 4
    }
}