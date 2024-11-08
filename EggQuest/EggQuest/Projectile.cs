using EggQuest.Collisions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;

namespace EggQuest
{
    public class Projectile : Object2D
    {
        public int Scale;
        /// <summary>
        /// The origin of the projectile.
        /// </summary>
        public Vector2 Origin;
        public bool IsActive;
        /// <summary>
        /// True means player projectile; false means enemy projectile.
        /// </summary>
        public bool projectileType;

        public Projectile(Vector2 startPosition, Vector2 velocity, Texture2D image, int scale) : base(new BoundingRectangle( startPosition.X - (image.Width * scale) / 2, startPosition.Y - (image.Height * scale) / 2, image.Width * scale, image.Height * scale))
        {
            Scale = scale;
            Position = startPosition;
            Velocity = velocity;
            IsActive = true;
            Texture = image;
            Width = image.Width * scale;
            Height = image.Height * scale;

            Origin = new Vector2(Texture.Width / 2 * Scale, Texture.Height / 2 * Scale);
        }

        public override void Update(GameTime gameTime)
        {
            Position += Velocity;
            Hitbox.SetPosition(Position);
            if (Position.X < 0 - Width || Position.X > 1000 + Width || Position.Y < 0 - Height || Position.Y > 800 + Height)
            {
                IsActive = false;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Origin, Scale, SpriteEffects.None, 0f);
            }
        }
    }
}
