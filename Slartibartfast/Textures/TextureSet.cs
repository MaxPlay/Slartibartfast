using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slartibartfast.Textures
{
    public class TextureSet
    {
        private Texture colorTex;

        public Texture ColorTexture
        {
            get { return colorTex; }
            set { colorTex = value; }
        }

        private Texture glossTex;

        public Texture GlossTexture
        {
            get { return glossTex; }
            set { glossTex = value; }
        }

        private Texture heightTex;

        public TextureSet(Texture color, Texture height, Texture gloss)
        {
            this.colorTex = color;
            this.heightTex = height;
            this.glossTex = gloss;
        }

        public Texture HeightTexture
        {
            get { return heightTex; }
            set { heightTex = value; }
        }
    }
}
