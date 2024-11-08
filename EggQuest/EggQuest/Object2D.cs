using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using EggQuest.Collisions;

namespace EggQuest
{
    /// <summary>
    /// A class to represent a generic 2D object
    /// </summary>
    public class Object2D
    {
        /// <summary>
        /// Position of the object
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Texture of the object
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// Velocity of the object
        /// </summary>
        public Vector2 Velocity = new Vector2(0,0);

        /// <summary>
        /// Acceleration of the object
        /// </summary>
        public Vector2 Acceleration = new Vector2(0,0);

        /// <summary>
        /// Width of the object
        /// </summary>
        public int Width;

        /// <summary>
        /// Height of the object
        /// </summary>
        public int Height;

        /// <summary>
        /// The BoundingRectangle, BoundingOval, or BoundingCircle of the object
        /// </summary>
        IBoundingShape Hitbox;

        /// <summary>
        /// Constructor for a 2D object
        /// </summary>
        /// <param name="hitbox">A BoundingRectangle, BoundingOval, or BoundingCircle object</param>
        public Object2D(IBoundingShape hitbox)
        {
            Hitbox = hitbox;
        }

        /// <summary>
        /// Generic content loading for an object
        /// </summary>
        /// <param name="content">ContentManager</param>
        public virtual void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("MissingTexture");
            Width = Texture.Width;
            Height = Texture.Height;
        }

        /// <summary>
        /// Updates the objects position based on its velocity and acceleration in linear physics
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            Velocity += Acceleration;
            Position += Velocity;
        }

        /// <summary>
        /// Checks if the object collides with another object
        /// </summary>
        /// <param name="other">The other object to check if this collides with</param>
        /// <returns>true if the two objects collide</returns>
        public bool CollidesWith(Object2D other) => Hitbox.CollidesWith(other.Hitbox);

        /// <summary>
        /// Draws the object
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        /// <param name="spriteBatch">The SpriteBatch</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
        }
    }
}
