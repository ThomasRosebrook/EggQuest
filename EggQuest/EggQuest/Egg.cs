using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using EggQuest.Collisions;
using SharpDX.Direct3D9;
using System.Windows.Forms;

namespace EggQuest
{
    public class Egg
    {
        /// <summary>
        /// Position of the egg
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Image for the egg
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// image for projectile
        /// </summary>
        public Texture2D PTexture;

        /// <summary>
        /// Velocity of the egg
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// Center of the egg for drawing
        /// </summary>
        public Vector2 Origin;

        private const int SpawnInterval = 1000;
        private double timeSinceLastShot;
        private int eggWidth;
        private int eggHeight;

        public List<Projectile> Projectiles = new List<Projectile>();

        public BoundingOval Hitbox;

        public Egg()
        {
            Position = new Vector2(100, 100); // Where the egg spawns
            Velocity = new Vector2(2, 2); // Initial speed and direction of the egg
            timeSinceLastShot = 0;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("MissingTexture");
            PTexture = content.Load<Texture2D>("MissingTexture");
            eggWidth = Texture.Width;
            eggHeight = Texture.Height;
            Origin = new Vector2(eggWidth / 2, eggHeight / 2); // Center of the egg
            Vector2 eggCenter = new Vector2(Position.X + eggWidth / 2, Position.Y + eggHeight / 2); //center of the egg for rotating stuff to make an oval

            Hitbox = new BoundingOval(eggCenter, eggWidth / 2, eggHeight / 2);
        }

        public void Update(GameTime gameTime, int screenWidth, int screenHeight)
        {
            Position += Velocity;
            Hitbox.Center = new Vector2(Position.X + eggWidth / 2, Position.Y + eggHeight / 2);

            // makes the egg bounce like DVD logo
            if (Position.X <= 0 || Position.X + eggWidth >= screenWidth)
            {
                Velocity.X *= -1;
            }

            if (Position.Y <= 0 || Position.Y + eggHeight >= screenHeight)
            {
                Velocity.Y *= -1;
            }

            timeSinceLastShot += gameTime.ElapsedGameTime.TotalMilliseconds; ///make the projectiles exist. 
            if (timeSinceLastShot >= SpawnInterval)
            {
                SpawnProjectiles();
                timeSinceLastShot = 0;
            }

            foreach (var projectile in Projectiles)
            {
                projectile.Update(gameTime);
            }
            Projectiles.RemoveAll(p => !p.IsActive);
        }

        /// <summary>
        /// spawns in 8 projectiles in the cardnel directions
        /// </summary>
        private void SpawnProjectiles()
        {
            Vector2[] directions = new Vector2[]
            {
                //projectiles orderd like a clock ticking clockwise. 
                new Vector2(0, -1),
                new Vector2(1, -1),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1),
                new Vector2(-1, 1),
                new Vector2(-1, 0),
                new Vector2(-1, -1),

            };

            foreach (var direction in directions)
            {
                Projectiles.Add(new Projectile(Position, direction * 5, PTexture)); ///number can be increased for more speed
            }
        }

        // Draws the egg and projectiles
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);

            foreach (var projectile in Projectiles)
            {
                projectile.Draw(spriteBatch);
            }
        }
    }
}
