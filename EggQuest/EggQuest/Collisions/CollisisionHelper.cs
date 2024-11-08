using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EggQuest.Collisions;

namespace EggQuest.Collisions
{
    public static class CollisionHelper
    {
        /// <summary>
        /// Detects collision between two BoundingCircles.
        /// </summary>
        /// <param name="a">First bounding circle.</param>
        /// <param name="b">Second bounding circle.</param>
        /// <returns>True if collision occurs, false otherwise.</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >= Math.Pow(a.Center.X - b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects collision between two BoundingRectangles.
        /// </summary>
        /// <param name="a">First bounding rectangle.</param>
        /// <param name="b">Second bounding rectangle.</param>
        /// <returns>True if collision occurs, false otherwise.</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right || a.Top > b.Bottom || a.Bottom < b.Top);
        }

        /// <summary>
        /// Detects collision between a BoundingCircle and a BoundingRectangle.
        /// </summary>
        /// <param name="c">The bounding circle.</param>
        /// <param name="r">The bounding rectangle.</param>
        /// <returns>True if collision occurs, false otherwise.</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);
            return Math.Pow(c.Radius, 2) >= Math.Pow(c.Center.X - nearestX, 2) + Math.Pow(c.Center.Y - nearestY, 2);
        }

        /// <summary>
        /// Detects collision between two BoundingOvals.
        /// </summary>
        /// <param name="a">First bounding oval.</param>
        /// <param name="b">Second bounding oval.</param>
        /// <returns>True if collision occurs, false otherwise.</returns>
        public static bool Collides(BoundingOval a, BoundingOval b)
        {
            // Calculate the distance between the centers of the ovals
            float dx = a.Center.X - b.Center.X;
            float dy = a.Center.Y - b.Center.Y;

            // Scaling factors for each radius
            float scaleX = a.HorizontalRadius + b.HorizontalRadius;
            float scaleY = a.VerticalRadius + b.VerticalRadius;

            // Normalized distance check (considering ovals as ellipses)
            return (dx * dx) / (scaleX * scaleX) + (dy * dy) / (scaleY * scaleY) <= 1;
        }

        /// <summary>
        /// Detects collision between a BoundingOval and a BoundingRectangle.
        /// </summary>
        /// <param name="o">The bounding oval.</param>
        /// <param name="r">The bounding rectangle.</param>
        /// <returns>True if collision occurs, false otherwise.</returns>
        public static bool Collides(BoundingOval o, BoundingRectangle r)
        {
            // Transform the rectangle into the oval's coordinate space and check for overlap
            float nearestX = MathHelper.Clamp(o.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(o.Center.Y, r.Top, r.Bottom);

            float dx = (nearestX - o.Center.X) / o.HorizontalRadius;
            float dy = (nearestY - o.Center.Y) / o.VerticalRadius;

            return dx * dx + dy * dy <= 1;
        }

        /// <summary>
        /// Detects collision between a BoundingCircle and a BoundingOval.
        /// </summary>
        /// <param name="c">The bounding circle.</param>
        /// <param name="o">The bounding oval.</param>
        /// <returns>True if collision occurs, false otherwise.</returns>
        public static bool Collides(BoundingCircle c, BoundingOval o)
        {
            // Transform circle's center into the oval's coordinate space and check for overlap
            float dx = (c.Center.X - o.Center.X) / o.HorizontalRadius;
            float dy = (c.Center.Y - o.Center.Y) / o.VerticalRadius;

            return dx * dx + dy * dy <= (c.Radius * c.Radius) / (o.HorizontalRadius * o.VerticalRadius);
        }

    }
}
