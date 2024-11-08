using EggQuest.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;

namespace EggQuest
{
    public class Player : Object2D
    {
        const float SPEED = 2;
        const float BACON_SPEED = 5;

        /// <summary>
        /// color of the ship for when taking damage
        /// </summary>
        private Color _shipColor = Color.White;
        /// <summary>
        /// all below stuff for handling taking damage
        /// </summary>
        private float _flashTimer = 0f;
        private float FlashDuration = 0.5f;
        private bool _isFlashing = false;
        /// <summary>
        /// how much health the player has
        /// </summary>
        public int hp = 5;
        //private Vector2 origin;

        public Vector2 InputDirection;

        short directionIndex;
        short animationFrame;
        double animationTimer = 0;

        Texture2D projectileTexture;

        public List<Projectile> Projectiles = new List<Projectile>();

        float angle;
        Vector2 direction = new Vector2(0,-1);

        public Player (Vector2 position) : base(new BoundingRectangle(position.X, position.Y, 66, 106))
        {
            Position = position;
            Width = 66;
            Height = 106;
        }
        public override void Update(GameTime gameTime)
        {
            if (_isFlashing)
            {
                _flashTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_flashTimer <= 0)
                {
                    _isFlashing = false;
                    _shipColor = Color.White;  // Reset color back to white
                }
            }
            //base.Update(gameTime);


            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            float angularVelocity = 0;

            directionIndex = 0;

            if (InputDirection.X != 0)
            {
                if (InputDirection.X < 0)
                {
                    angularVelocity -= 2;
                    directionIndex = 2;
                }
                else
                {
                    angularVelocity += 2;
                    directionIndex = 1;
                }

                angle += angularVelocity * time;
                direction.X = (float)Math.Sin(angle);
                direction.Y = (float)-Math.Cos(angle);
            }
            
            if (InputDirection.Y != 0)
            {
                directionIndex += 3;

                Velocity = direction * 100;

                if (InputDirection.Y > 0) Velocity *= -1;
                
                Position += Velocity * SPEED * time;
            }
            Hitbox.SetPosition(Position);

            //if (Velocity.Y != 0) directionIndex = 2;
            //else if (Velocity.X < 0) directionIndex = 3;
            //else if (Velocity.X > 0) directionIndex = 1;
            //else directionIndex = 0;

            if (animationTimer >= 5)
            {
                if (animationFrame == 2) animationFrame = 0;
                else animationFrame++;
                animationTimer = 0;
            }
            else animationTimer += gameTime.ElapsedGameTime.TotalSeconds * 100;

            foreach (var projectile in Projectiles)
            {
                projectile.Update(gameTime);
            }
            Projectiles.RemoveAll(p => !p.IsActive);
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("Spatula");
            projectileTexture = content.Load<Texture2D>("Bacon");
            //origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
        }

        public void SpawnProjectile()
        {
            Projectiles.Add(new Projectile(Position + 50 * direction, direction * BACON_SPEED, projectileTexture, 1));
        }

        /// <summary>
        /// method to handle everything that happens when the ship takes damage
        /// </summary>
        public void OnHit()
        {
            if (!_isFlashing)
            {
                _isFlashing = true;
                _flashTimer = FlashDuration;
                _shipColor = Color.Red;
                hp -= 1;
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, new Rectangle(animationFrame * 128, directionIndex * 128, 128, 128), _shipColor, angle, new Vector2(64, 64), 1f, SpriteEffects.None, 0f);

            foreach (var projectile in Projectiles)
            {
                projectile.Draw(gameTime, spriteBatch);
            }
        }
    }
}
