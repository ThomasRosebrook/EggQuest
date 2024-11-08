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
    public struct BoundingCircle
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
        /// <summary>
        /// teses a collsion between this and anohter bounding circle
        /// </summary>
        /// <param name="other">the other bounding circle</param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisisionHelper.Collides(this, other);
        }

        /// <summary>
        /// teses a collsion between this and a bounding rectangle
        /// </summary>
        /// <param name="other">the other rectangle/param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisisionHelper.Collides(this, other);
        }
        /// <summary>
        /// teses a collsion between this and a bounding Oval
        /// </summary>
        /// <param name="other">the other rectangle/param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingOval other)
        {
            return CollisisionHelper.Collides(this, other);
        }
    }
}
