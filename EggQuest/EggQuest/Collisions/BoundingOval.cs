using EggQuest.Collisions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggQuest.Collisions
{
    public struct BoundingOval : IBoundingShape
    {
        /// <summary>
        /// The center of the bounding oval.
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// The horizontal radius (half-width) of the oval.
        /// </summary>
        public float HorizontalRadius;

        /// <summary>
        /// The vertical radius (half-height) of the oval.
        /// </summary>
        public float VerticalRadius;

        /// <summary>
        /// Constructs a new bounding oval.
        /// </summary>
        /// <param name="center">The center of the oval.</param>
        /// <param name="horizontalRadius">The horizontal radius (half-width) of the oval.</param>
        /// <param name="verticalRadius">The vertical radius (half-height) of the oval.</param>
        public BoundingOval(Vector2 center, float horizontalRadius, float verticalRadius)
        {
            Center = center;
            HorizontalRadius = horizontalRadius;
            VerticalRadius = verticalRadius;
        }

        public bool CollidesWith(IBoundingShape other)
        {
            if (other is BoundingRectangle rect) return CollisionHelper.Collides(this, rect);
            if (other is BoundingCircle circle) return CollisionHelper.Collides(circle, this);
            if (other is BoundingOval oval) return CollisionHelper.Collides(oval, this);

            return false;
        }

        /// <summary>
        /// Tests a collision between this and another bounding oval.
        /// </summary>
        /// <param name="other">The other bounding oval.</param>
        /// <returns>True for collision, false otherwise.</returns>
        public bool CollidesWith(BoundingOval other)
        {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// Tests a collision between this and a bounding rectangle.
        /// </summary>
        /// <param name="other">The other bounding rectangle.</param>
        /// <returns>True for collision, false otherwise.</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// Tests a collision between this and a bounding circle.
        /// </summary>
        /// <param name="other">The other bounding circle.</param>
        /// <returns>True for collision, false otherwise.</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }

        public float GetHeight() => VerticalRadius;

        public Vector2 GetPosition() => Center;

        public float GetWidth() => HorizontalRadius;

        public void SetHeight(float height)
        {
            VerticalRadius = height;
        }

        public void SetPosition(Vector2 position)
        {
            Center = position;
        }

        public void SetWidth(float width)
        {
            HorizontalRadius = width;
        }
    }
}
