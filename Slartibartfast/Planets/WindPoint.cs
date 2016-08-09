using Slartibartfast.Math;

namespace Slartibartfast.Planets
{
    public struct WindPoint
    {
        #region Public Fields

        /// <summary>
        /// The distance the wind affects the world.
        /// </summary>
        public float Distance;

        /// <summary>
        /// The location of the windpoint on the surface.
        /// </summary>
        public Vector2 Location;

        /// <summary>
        /// The rotation of the windpoint. The wind on the point is always 0.
        /// </summary>
        public float Rotation;

        #endregion Public Fields
    }
}