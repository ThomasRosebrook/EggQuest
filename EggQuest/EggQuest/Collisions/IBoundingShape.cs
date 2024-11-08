using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggQuest.Collisions
{
    public interface IBoundingShape
    {
        /// <summary>
        /// Checks if the bounding shape collides with another bounding shape
        /// </summary>
        /// <param name="other">The Other Bounding Shape to check this one against</param>
        /// <returns>true if the shapes collide</returns>
        public bool CollidesWith(IBoundingShape other);

        /// <summary>
        /// Sets the position of the bounding shape
        /// </summary>
        /// <param name="position">Position or Center of Bounding Shape</param>
        public void SetPosition(Vector2 position);

        /// <summary>
        /// Sets the width of the bounding shape
        /// </summary>
        /// <param name="width">Width for Rectangle, Radius for Center, Horizontal Radius for Oval</param>
        public void SetWidth(float width);

        /// <summary>
        /// Sets the height of the bounding shape
        /// </summary>
        /// <param name="height">Height for Rectangle, Radius for Center, Vertical Radius for Oval</param>
        public void SetHeight(float height);

        /// <summary>
        /// Gets the width of the bounding shape
        /// </summary>
        /// <returns>Width for Rectangle, Radius for Center, Horizontal Radius for Oval</returns>
        public float GetWidth();

        /// <summary>
        /// Gets the height of the bounding shape
        /// </summary>
        /// <returns>Height for Rectangle, Radius for Center, Vertical Radius for Oval</returns>
        public float GetHeight();

        /// <summary>
        /// Gets the position of the bounding shape
        /// </summary>
        /// <returns>Position or Center of Bounding Shape</returns>
        public Vector2 GetPosition();
    }
}
