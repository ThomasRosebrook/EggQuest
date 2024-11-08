using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using EggQuest.Collisions;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Audio;
namespace EggQuest
{
    public class Egg : Object2D
    {
        /// <summary>
        /// how much health the egg has. i specifically think we shouldnt use a health bar. 
        /// </summary>
        public int hp;

        /// <summary>
        /// image for projectile
        /// </summary>
        public Texture2D PTexture;

        /// <summary>
        /// Center of the egg for drawing
        /// </summary>
        public Vector2 Origin;
        /// <summary>
        /// sound for when egg gets hit
        /// </summary>
        public SoundEffect EggDamageSound;

        private const int SpawnInterval = 2000;
        private double timeSinceLastShot;

        public List<Projectile> Projectiles = new List<Projectile>();

        public Egg() : base(new BoundingOval())
        {
            Position = new Vector2(400, 200); // Where the egg spawns
            Velocity = new Vector2(-2, -2); // Initial speed and direction of the egg
            timeSinceLastShot = 0;
            hp = 10; //change for whatever we want it to be in the future
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("MissingTexture");
            PTexture = content.Load<Texture2D>("MissingTexture");
            EggDamageSound = content.Load<SoundEffect>("EggHit");
            Width = Texture.Width;
            Height = Texture.Height;
            Origin = new Vector2(Width / 2, Height / 2); // Center of the egg
            Vector2 eggCenter = new Vector2(Position.X + Width / 2, Position.Y + Height / 2); //center of the egg for rotating stuff to make an oval

            Hitbox = new BoundingOval(eggCenter, Width / 2, Height / 2);
        }

        public void Update(GameTime gameTime, int screenWidth, int screenHeight)
        {
            Position += Velocity;
            Hitbox.SetPosition(new Vector2(Position.X + Width / 2, Position.Y + Height / 2));

            // makes the egg bounce like DVD logo
            if (Position.X <= Width / 2 || Position.X + Width / 2 >= screenWidth)
            {
                Velocity.X *= -1;
            }

            if (Position.Y <= Height / 2 || Position.Y + Height / 2>= screenHeight)
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
                Projectiles.Add(new Projectile(Position, direction * 5, PTexture, 1)); ///number can be increased for more speed
                //the 1 at the end is the scale. since these are the boss p
            }
        }

        // Draws the egg and projectiles
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            foreach (var projectile in Projectiles)
            {
                projectile.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// would handel everything that happens with getting hit. 
        /// </summary>
        public void onhit()
        {
            hp -= 1;
            EggDamageSound.Play();
            // the egg flashes red for a second maybe.
        }
    }
}
