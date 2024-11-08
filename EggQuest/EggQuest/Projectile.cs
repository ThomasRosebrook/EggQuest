using EggQuest.Collisions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EggQuest
{
    public class Projectile
    {
        /// <summary>
        /// position of the projectile
        /// </summary>
        public Vector2 Position { get; private set; }
        /// <summary>
        /// the orgin of the projectile
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// velcoity of the projectile
        /// </summary>
        public Vector2 Velocity { get; private set; }

        /// <summary>
        /// whatever teh texture is
        /// </summary>
        private Texture2D texture;
        public BoundingRectangle Hitbox;

        public bool IsActive;

        public Projectile(Vector2 startPosition, Vector2 velocity, Texture2D image)
        {
            Position = startPosition;
            Velocity = velocity;
            IsActive = true;
            texture = image;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Hitbox = new BoundingRectangle(Position.X - Origin.X, Position.Y - Origin.Y, texture.Width, texture.Height);

        }
        public void Update(GameTime gameTime)
        {
            Position += Velocity;
            Hitbox.Y = Position.Y;
            Hitbox.X = Position.X;
            if (Position.X < 0 - texture.Width || Position.X > 1000 + texture.Width || Position.Y < 0 - texture.Height || Position.Y > 800 + texture.Height)
            {
                IsActive = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                spriteBatch.Draw(texture, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
