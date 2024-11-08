using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using EggQuest.Collisions;

namespace EggQuest.Collisions
{
    /// <summary>
    /// a struct representing circular bounds
    /// </summary>
    public struct BoundingCircle : IBoundingShape
    {
        /// <summary>
        /// the center of the bounding circle
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// the radius of the boundingcircle
        /// </summary>
        public float Radius;

        /// <summary>
        /// construct a new bounding  circle
        /// </summary>
        /// <param name="center">the center</param>
        /// <param name="radius">the radius</param>
        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool CollidesWith(IBoundingShape other)
        {
            if (other is BoundingRectangle rect) return CollisionHelper.Collides(this, rect);
            if (other is BoundingCircle circle) return CollisionHelper.Collides(circle, this);
            if (other is BoundingOval oval) return CollisionHelper.Collides(this, oval);

            return false;
        }

        /// <summary>
        /// teses a collsion between this and anohter bounding circle
        /// </summary>
        /// <param name="other">the other bounding circle</param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// teses a collsion between this and a bounding rectangle
        /// </summary>
        /// <param name="other">the other rectangle/param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }
        /// <summary>
        /// teses a collsion between this and a bounding Oval
        /// </summary>
        /// <param name="other">the other rectangle/param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingOval other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public float GetHeight() => Radius;

        public Vector2 GetPosition() => Center;

        public float GetWidth() => Radius;

        public void SetHeight(float height)
        {
            Radius = height;
        }

        public void SetPosition(Vector2 position)
        {
            Center = position;
        }

        public void SetWidth(float width)
        {
            Radius = width;
        }
    }
}
