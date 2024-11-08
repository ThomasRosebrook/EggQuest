using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EggQuest.Collisions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
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
        private Texture2D _background;
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
            _background = Content.Load<Texture2D>("background-purple");
        }

        protected override void Update(GameTime gameTime)
        {
            _inputManager.Update(gameTime);
            if (_inputManager.Exit) Exit();

            if (_player.hp > 0)
            {
                Projectile toRemove = null;
                foreach (Projectile p in _theEgg.Projectiles)
                {
                    if (p.CollidesWith(_player))
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
                _player.InputDirection = _inputManager.Direction;
                _player.Update(gameTime);
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                base.Update(gameTime);
            }
            else
            {
                if (_inputManager.SpacePressed) ResetGame();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Rectangle(0, 0, _screenWidth, _screenHeight), Color.White);
            if (_theEgg.hp <= 0)
            {/// if you beat the game it will be handled here
                GraphicsDevice.Clear(Color.Black);
                Vector2 messageSize = _font.MeasureString("You Won");
                Vector2 position = new Vector2((_screenWidth - messageSize.X) / 2, (_screenHeight - messageSize.Y) / 2);
                _spriteBatch.DrawString(_font, "You Won", position, Color.White);
            }else if(_player.hp <= 0)
            {
                GraphicsDevice.Clear(Color.Black);
                string message = "You Lose";
                Vector2 messageSize = _font.MeasureString(message);
                Vector2 position = new Vector2((_screenWidth - messageSize.X) / 2, (_screenHeight - messageSize.Y) / 2);
                _spriteBatch.DrawString(_font, message, position, Color.White);
                message = "Press Space to Retry";
                messageSize = _font.MeasureString(message);
                position = new Vector2((_screenWidth - messageSize.X) / 2, (_screenHeight - messageSize.Y) / 2 + 50);
                _spriteBatch.DrawString(_font, message, position, Color.White);
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

        private void ResetGame()
        {
            _player = new Player(new Vector2(_screenWidth / 2, _screenHeight / 2));
            _theEgg = new Egg();
            _theEgg.LoadContent(Content);
            _player.LoadContent(Content);
        }
    }
}