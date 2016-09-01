namespace Slartibartfast.Textures
{
    public class TextureSet
    {
        #region Private Fields

        private Texture colorTex;

        private Texture glossTex;

        private Texture heightTex;

        #endregion Private Fields

        #region Public Constructors

        public TextureSet(Texture color, Texture height, Texture gloss)
        {
            this.colorTex = color;
            this.heightTex = height;
            this.glossTex = gloss;
        }

        #endregion Public Constructors

        #region Public Properties

        public Texture ColorTexture
        {
            get { return colorTex; }
            set { colorTex = value; }
        }

        public Texture GlossTexture
        {
            get { return glossTex; }
            set { glossTex = value; }
        }

        public Texture HeightTexture
        {
            get { return heightTex; }
            set { heightTex = value; }
        }

        #endregion Public Properties
    }
}