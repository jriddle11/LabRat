﻿
namespace LabRat
{
    /// <summary>
    /// A struct representing circular bounds
    /// </summary>
    public struct BoundingCircle
    {
        /// <summary>
        /// The center of the circle
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// The radius of the circle
        /// </summary>
        public float Radius;

        public float Bottom => Center.Y + Radius;

        public float Top => Center.Y - Radius;

        public float Left => Center.X - Radius;

        public float Right => Center.X + Radius;

        /// <summary>
        /// Constructs a new bounding circle
        /// </summary>
        /// <param name="center">the center</param>
        /// <param name="radius">the radius</param>
        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Tests for collision between this and another bounding circle
        /// </summary>
        /// <param name="other">the other circle</param>
        /// <returns>true if a collision is detected</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }
    }
}
