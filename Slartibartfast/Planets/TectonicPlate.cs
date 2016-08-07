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
            //Note: This is for generating debug-colors only. That is the reason, because this is not streamlined with the other random-objects.
            Random rand = new Random(DateTime.Now.Millisecond * id);
            moveDirection = rand.Vector2();
            debugColor = rand.Color();
            heightModifier = (float)rand.NextDouble() * 2 - 1;
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

        private float heightModifier;

        public float HeightModifier
        {
            get { return heightModifier; }
            set { heightModifier = value; }
        }


        #endregion Public Properties
    }
}