using Slartibartfast.Extensions;
using Slartibartfast.Math;
using System;
using System.Drawing;

namespace Slartibartfast.Planets
{
    internal class TectonicPlate
    {
        #region Private Fields

        private Color debugColor;
        private int id;

        private Vector2 moveDirection;

        #endregion Private Fields

        #region Public Constructors

        public TectonicPlate(int id)
        {
            Random rand = new Random(MathHelper.RandomSeed != null ? (int)MathHelper.RandomSeed : 0);
            moveDirection = rand.Vector2();
            debugColor = rand.Color();
        }

        #endregion Public Constructors

        #region Public Properties

        public Color DebugColor
        {
            get { return debugColor; }
            set { debugColor = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public Vector2 MoveDirection
        {
            get { return moveDirection; }
            set { moveDirection = value; }
        }

        #endregion Public Properties
    }
}