using EggQuest.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EggQuest
{
    public class Player : Object2D
    {
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
        private Vector2 origin;
        public Player (Vector2 position) : base(new BoundingRectangle(position.X, position.Y, 33, 53))
        {
            Position = position;
            Width = 33;
            Height = 53;
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
            base.Update(gameTime);
        }
        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("Spatula");
            origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
        }
        /// <summary>
        /// method to handel everythign that happens when the ship takes damage
        /// </summary>
        public void OnHit()
        {
            _isFlashing = true;
            _flashTimer = FlashDuration;
            _shipColor = Color.Red;
            hp -= 1;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, _shipColor, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
