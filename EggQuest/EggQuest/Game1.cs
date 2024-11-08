using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EggQuest.Collisions;
using System.Windows.Forms;
namespace EggQuest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Egg _theEgg;
        private Player _player;
        private InputManager _inputManager;
        private int _screenWidth = 1000;
        private int _screenHeight = 800;
        private double timer; 
        private SpriteFont _font;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = _screenWidth;
            _graphics.PreferredBackBufferHeight = _screenHeight;
            _graphics.ApplyChanges();

            _inputManager = new InputManager();
            _player = new Player(new Vector2(_screenWidth / 2, _screenHeight / 2));
            _theEgg = new Egg();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _theEgg.LoadContent(Content);
            _player.LoadContent(Content);
            _font = Content.Load<SpriteFont>("Arcade");
        }

        protected override void Update(GameTime gameTime)
        {
            _inputManager.Update(gameTime);
            if (_inputManager.Exit) Exit();
            Projectile toRemove = null;
            foreach(Projectile p in _theEgg.Projectiles)
            {
                if (p.Hitbox.CollidesWith(_player.Hitbox))
                {
                    _player.OnHit();
                    toRemove = p;
                }
            }
            if (toRemove != null)
            {
                _theEgg.Projectiles.Remove(toRemove);
            }
            /*
            foreach(Projectile p in _player.projectiles)
            {
                do something here
            }
            */
            _theEgg.Update(gameTime, _screenWidth, _screenHeight);
            _player.Velocity = _inputManager.Direction * 100;
            _player.Update(gameTime);
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            if(_theEgg.hp <= 0)
            {/// if you beat the game it will be handled here
                GraphicsDevice.Clear(Color.Black);
                Vector2 messageSize = _font.MeasureString("You Won");
                Vector2 position = new Vector2((_screenWidth - messageSize.X) / 2, (_screenHeight - messageSize.Y) / 2);
                _spriteBatch.DrawString(_font, "You Won", position, Color.White);
            }else if(_player.hp <= 0)
            {
                GraphicsDevice.Clear(Color.Black);
                Vector2 messageSize = _font.MeasureString("You Lose");
                Vector2 position = new Vector2((_screenWidth - messageSize.X) / 2, (_screenHeight - messageSize.Y) / 2);
                _spriteBatch.DrawString(_font, "You Lose", position, Color.White);
            }
            else
            {
                _theEgg.Draw(gameTime, _spriteBatch);
                _player.Draw(gameTime, _spriteBatch);
                _spriteBatch.DrawString(_font, timer.ToString("F2"), new Vector2(50, 50), Color.White);
                _spriteBatch.DrawString(_font, "HP " + _player.hp.ToString(), new Vector2(50, 20), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}