using EggQuest.Collisions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;

namespace EggQuest
{
    public class Projectile : Object2D
    {
        /// <summary>
        /// the orgin of the projectile
        /// </summary>
        public Vector2 Origin;
        public bool IsActive;

        public Projectile(Vector2 startPosition, Vector2 velocity, Texture2D image) : base(new BoundingRectangle(startPosition.X - image.Width / 2, startPosition.Y - image.Height / 2, image.Width, image.Height))
        {
            Position = startPosition;
            Velocity = velocity;
            IsActive = true;
            Texture = image;
            Width = image.Width;
            Height = image.Height;

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

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
                base.Draw(gameTime, spriteBatch);
            }
        }
    }
}
