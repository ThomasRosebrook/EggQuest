using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using EggQuest.Collisions;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Reflection.Metadata;

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

        private GraphicsDevice _graphicsDevice;

        private VertexPositionColor[] _vertices;

        private int[] _indices;

        private BasicEffect _effect;

        public Vector3 EggPosition;

        public Vector3 EggVelocity = new Vector3(-200f, 200f, 0f);

        private float _rotationAngle = 20f;

        private Matrix _world;

        private Matrix _projection;
        private Matrix _view;


        private const int SpawnInterval = 2000;
        private double timeSinceLastShot;

        public List<Projectile> Projectiles = new List<Projectile>();

        public static int ScreenWidth;
        public static int ScreenHeight;

        HealthBar healthBar;

        public Egg() : base(new BoundingOval())
        public Egg(GraphicsDevice graphicsDevice, Matrix view, Matrix projection) : base(new BoundingOval())
        {
            _graphicsDevice = graphicsDevice;
            Position = new Vector2(400, 200);  // Where the egg spawns
            EggPosition = new Vector3(-Position.X, Position.Y, -550); 
            Velocity = new Vector2(-2, -2);    // Initial speed and direction of the egg
            timeSinceLastShot = 0;
            _projection = projection;
            _view = view;

            _effect = new BasicEffect(graphicsDevice)
            {
                LightingEnabled = false,  // Enable lighting
                VertexColorEnabled = true,  // Enable vertex colors to be used
                View = view,
                Projection = projection,
                TextureEnabled = false,  // Disable texture mapping
                Alpha = 1.0f  // Full opacity
            };

            _effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);  // Soft ambient light (light that is always present)
            _effect.DirectionalLight0.Enabled = true;  // Enable directional light
            _effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1f);  // White light (default)
            _effect.DirectionalLight0.Direction = new Vector3(0f, -1f, -1f);  // Light coming from above and in front
            _effect.DirectionalLight0.SpecularColor = new Vector3(0.25f);  // Specular light (shininess)





            CreateEggMesh();
           
            hp = 10;
            hp = 20; //change for whatever we want it to be in the future

            healthBar = new HealthBar(HealthType.Egg, hp, new Vector2(ScreenWidth - hp * 32, 48));
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("MissingTexture");
            PTexture = content.Load<Texture2D>("ShellFragment");
            EggDamageSound = content.Load<SoundEffect>("EggHit");
            Width = Texture.Width;
            Height = Texture.Height;
            Origin = new Vector2(Width / 2, Height / 2); // Center of the egg
            Vector2 eggCenter = new Vector2(EggPosition.X + Width / 2, EggPosition.Y + Height / 2); //center of the egg for rotating stuff to make an oval

            Hitbox = new BoundingOval(eggCenter, Width / 2, Height / 2);
            healthBar.LoadContent(content);
        }



        private void CreateEggMesh()
        {
            List<VertexPositionColor> vertices = new List<VertexPositionColor>();
            List<int> indices = new List<int>();

            int latitudeSegments = 20;
            int longitudeSegments = 20;

            // Define the gradient colors based on height
            Color lightColor = Color.White;   // Simulated light color
            Color shadowColor = Color.Gray;   // Simulated shadow color

            for (int lat = 0; lat <= latitudeSegments; lat++)
            {
                float theta = lat * MathHelper.Pi / latitudeSegments;
                float sinTheta = MathF.Sin(theta);
                float cosTheta = MathF.Cos(theta);

                for (int lon = 0; lon <= longitudeSegments; lon++)
                {
                    float phi = lon * 2 * MathHelper.Pi / longitudeSegments;
                    float sinPhi = MathF.Sin(phi);
                    float cosPhi = MathF.Cos(phi);

                    Vector3 normal = new Vector3(cosPhi * sinTheta, cosTheta, sinPhi * sinTheta);
                    Vector3 position = normal;

                    position.Y *= 1.5f;  // Scale Y for egg shape
                    position *= 0.5f;    // Scale egg size

                    // Apply gradient based on the Y value of the position (or any other axis)
                    float lightIntensity = MathF.Max(0, position.Y); // Use Y-axis for shading
                    Color vertexColor = Color.Lerp(shadowColor, lightColor, lightIntensity);  // Smooth gradient

                    vertices.Add(new VertexPositionColor(position, vertexColor));
                }
            }

            for (int lat = 0; lat < latitudeSegments; lat++)
            {
                for (int lon = 0; lon < longitudeSegments; lon++)
                {
                    int first = lat * (longitudeSegments + 1) + lon;
                    int second = first + longitudeSegments + 1;

                    indices.Add(first);
                    indices.Add(second);
                    indices.Add(first + 1);

                    indices.Add(second);
                    indices.Add(second + 1);
                    indices.Add(first + 1);
                }
            }

            _vertices = vertices.ToArray();
            _indices = indices.ToArray();
        }

        public void Update(GameTime gameTime, int screenWidth, int screenHeight)
        {

            Position += Velocity;
            EggPosition += EggVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Hitbox.SetPosition(new Vector2(Position.X + Width / 2, Position.Y + Height / 2));


            // Define the visible boundaries based on the screen width and height, assuming centered origin
            float leftBound = -screenWidth / 2 + Width / 2;  // Left edge of screen
            float rightBound = screenWidth / 2 - Width / 2;   // Right edge of screen
            float topBound = -screenHeight / 2 + Height / 2;  // Top edge of screen
            float bottomBound = screenHeight / 2 - Height / 2; // Bottom edge of screen

            // A small tolerance value to help prevent jittering or getting stuck at boundaries
            const float boundaryBuffer = 1.0f;

            // Check if the egg goes beyond the left or right boundaries
            if (EggPosition.X <= leftBound || EggPosition.X >= rightBound)
            {
                // If it's very close to the boundary, move it slightly past it to avoid being stuck
                if (EggPosition.X <= leftBound)
                {
                    EggPosition.X = leftBound + boundaryBuffer;
                }
                else if (EggPosition.X >= rightBound)
                {
                    EggPosition.X = rightBound - boundaryBuffer;
                }

                // Reverse the velocity direction to simulate the bounce
                EggVelocity.X *= -1;
            }

            // Check if the egg goes beyond the top or bottom boundaries
            if (EggPosition.Y <= topBound || EggPosition.Y >= bottomBound)
            {
                // If it's very close to the boundary, move it slightly past it to avoid being stuck
                if (EggPosition.Y <= topBound)
                {
                    EggPosition.Y = topBound + boundaryBuffer;
                }
                else if (EggPosition.Y >= bottomBound)
                {
                    EggPosition.Y = bottomBound - boundaryBuffer;
                }

                // Reverse the velocity direction to simulate the bounce
                EggVelocity.Y *= -1;
            }
            
            // makes the egg bounce like DVD logo
            if (Position.X <= Width / 2 || Position.X + Width / 2 >= ScreenWidth)
            {
                Velocity.X *= -1;
            }

            if (Position.Y <= Height / 2 || Position.Y + Height / 2>= ScreenHeight)
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
                Projectiles.Add(new Projectile(Position, direction * 5, PTexture, 1, RandomHelper.NextFloat(0,MathHelper.TwoPi))); ///number can be increased for more speed
                //the 1 at the end is the scale. since these are the boss p
            }
        }

        // Draws the egg and projectiles
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            //base.Draw(gameTime, spriteBatch);
            //DrawEgg(view, projection);

            foreach (var projectile in Projectiles)
            {
                projectile.Draw(gameTime, spriteBatch);
            }

            healthBar.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// would handel everything that happens with getting hit. 
        /// </summary>
        public void onhit()
        {
            hp -= 1;
            healthBar.Health -= 1;
            EggDamageSound.Play();
            // the egg flashes red for a second maybe.
        }

        public void DrawEgg(Matrix view, Matrix projection)
        {
            _world = Matrix.CreateScale(100.0f) * Matrix.CreateTranslation(EggPosition);

            _effect.World = _world;
            _effect.View = view;
            _effect.Projection = projection;

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _graphicsDevice.DrawUserIndexedPrimitives(
                     Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleList,
                    _vertices,
                    0,
                    _vertices.Length,
                    _indices,
                    0,
                    _indices.Length / 3
                );
            }
        }
    }
}
