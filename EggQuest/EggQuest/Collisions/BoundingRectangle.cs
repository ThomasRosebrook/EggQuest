using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EggQuest.Collisions;

namespace EggQuest.Collisions
{
    /// <summary>
    /// A bounding rectangle for collision detection
    /// </summary>
    public struct  BoundingRectangle : IBoundingShape
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public float Left => X - Width / 2;
        public float Right => X + Width / 2;
        public float Top => Y - Height / 2;
        public float Bottom => Y + Height / 2;

        public BoundingRectangle(float x, float y, float width, float height)
        {
            X= x;
            Y = y;
            Width = width;
            Height = height;
        }
        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        public bool CollidesWith(IBoundingShape other)
        {
            if (other is BoundingRectangle rect) return CollisionHelper.Collides(this, rect);
            if (other is BoundingCircle circle) return CollisionHelper.Collides(circle, this);
            if (other is BoundingOval oval) return CollisionHelper.Collides(oval, this);

            return false;
        }

        /// <summary>
        /// tests a collision between this and another bounding circle
        /// </summary>
        /// <param name="other">the other bounding circle</param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }
        /// <summary>
        /// teses a collsion between this and anohter bounding circle
        /// </summary>
        /// <param name="other">the other bounding circle</param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }
        /// <summary>
        /// teses a collsion between this and a bounding Oval
        /// </summary>
        /// <param name="other">the other rectangle/param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingOval other)
        {
            return CollisionHelper.Collides(other, this);
        }

        public float GetHeight() => Height;

        public Vector2 GetPosition() => new Vector2(X,Y);

        public float GetWidth() => Width;

        public void SetHeight(float height)
        {
            Height = height;
        }

        public void SetPosition(Vector2 position)
        {
            X = position.X;
            Y = position.Y;
        }

        public void SetWidth(float width)
        {
            Width = width;
        }
    }
}
