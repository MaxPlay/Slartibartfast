using System;

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