using Slartibartfast.Extensions;
using Slartibartfast.Math;
using System;
using System.Drawing;

namespace Slartibartfast.Planets
{
    internal class TectonicPlate
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private Color debugColor;

        public Color DebugColor
        {
            get { return debugColor; }
            set { debugColor = value; }
        }

        private Vector2 moveDirection;

        public Vector2 MoveDirection
        {
            get { return moveDirection; }
            set { moveDirection = value; }
        }

        public TectonicPlate(int id)
        {
            Random rand = new Random(DateTime.Now.Millisecond*id);
            moveDirection = rand.Vector2();
            debugColor = rand.Color();
        }
    }
}