using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EggQuest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Egg _theEgg;
        private Player _player;
        private InputManager _inputManager;
        private int _screenWidth = 1500;
        private int _screenHeight = 900;
        private double timer; 
        private SpriteFont _font;
        private Texture2D _background;
        private Song _spaceStation;
        private Texture2D _scrambeled;
        private Matrix _view;
        private Matrix _projection;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = _screenWidth;
            _graphics.PreferredBackBufferHeight = _screenHeight;
        }

        protected override void Initialize()
        {
            MediaPlayer.Volume = 0.5f;
            _graphics.PreferredBackBufferWidth = _screenWidth;
            _graphics.PreferredBackBufferHeight = _screenHeight;
            _graphics.ApplyChanges();

            Projectile.ScreenHeight = _screenHeight;
            Projectile.ScreenWidth = _screenWidth;
            Egg.ScreenHeight = _screenHeight;
            Egg.ScreenWidth = _screenWidth;

            _inputManager = new InputManager();
            _player = new Player(new Vector2(_screenWidth / 2, _screenHeight / 2));
            _view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);  // Adjust the Z value for camera distance
            //_projection = Matrix.CreatePerspectiveFieldOfView(_screenWidth, _screenHeight, 0.1f, 1000f);
            _projection = Matrix.CreatePerspectiveFieldOfView(
    MathHelper.PiOver2,  // Field of view angle (about 45 degrees)
    _screenWidth / (float)_screenHeight,  // Aspect ratio
    0.1f,  // Near plane distance (how close objects can be to the camera)
    1000f  // Far plane distance (how far objects can be from the camera)
);
            _theEgg = new Egg(GraphicsDevice, _view, _projection);

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _theEgg.LoadContent(Content);
            _player.LoadContent(Content);
            _font = Content.Load<SpriteFont>("Arcade");
            _background = Content.Load<Texture2D>("background-purple");
            _spaceStation = Content.Load<Song>("space_station");
            _scrambeled = Content.Load<Texture2D>("ScrambledEggs");
        }

        protected override void Update(GameTime gameTime)
        {
            _inputManager.Update(gameTime);
            if (_inputManager.Exit) Exit();

            if (_player.hp > 0 && _theEgg.hp > 0)
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
                
                foreach(Projectile p in _player.Projectiles)
                {
                    if (p.CollidesWith(_theEgg))
                    {
                        _theEgg.onhit();
                        p.IsActive = false;
                    }
                }
                _player.Projectiles.RemoveAll(p => !p.IsActive);
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(_spaceStation);
                }
                if (_inputManager.SpacePressed) _player.SpawnProjectile();
                _theEgg.Update(gameTime);
                _player.InputDirection = _inputManager.Direction;
                _player.Update(gameTime);
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                base.Update(gameTime);
            }
            else
            {
                if (_inputManager.Retry) ResetGame();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Rectangle(0, 0, _screenWidth, _screenHeight), Color.White);
            _spriteBatch.End();
            _spriteBatch.Begin();
            if (_theEgg.hp <= 0)
            {/// if you beat the game it will be handled here
                GraphicsDevice.Clear(Color.Black);
                MediaPlayer.Stop();
                _spriteBatch.Draw(_scrambeled, new Rectangle(0, 0, _screenWidth, _screenHeight), Color.White);
                Vector2 messageSize = _font.MeasureString("You Won");
                Vector2 position = new Vector2((_screenWidth - messageSize.X) / 2, 10 + messageSize.Y);
                _spriteBatch.DrawString(_font, "You Won", position, Color.Black);
                messageSize = _font.MeasureString("Press R to Retry");
                position = new Vector2((_screenWidth - messageSize.X) / 2, _screenHeight - messageSize.Y);
                _spriteBatch.DrawString(_font, "Press R to Retry", position, Color.Black);
            }
            else if(_player.hp <= 0)
            { /// if you die it happens here
                MediaPlayer.Stop();
                GraphicsDevice.Clear(Color.Black);
                string message = "You Lose";
                Vector2 messageSize = _font.MeasureString(message);
                Vector2 position = new Vector2((_screenWidth - messageSize.X) / 2, (_screenHeight - messageSize.Y) / 2);
                _spriteBatch.DrawString(_font, message, position, Color.White);
                message = "Press R to Retry";
                messageSize = _font.MeasureString(message);
                position = new Vector2((_screenWidth - messageSize.X) / 2, (_screenHeight - messageSize.Y) / 2 + 50);
                _spriteBatch.DrawString(_font, message, position, Color.White);
            }
            else
            {
                GraphicsDevice.DepthStencilState = DepthStencilState.None;
                _theEgg.DrawEgg(_view, _projection);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                _theEgg.Draw(gameTime, _spriteBatch);
                _player.Draw(gameTime, _spriteBatch);
                _spriteBatch.DrawString(_font, timer.ToString("F0"), new Vector2(50, 100), Color.White);
                //_spriteBatch.DrawString(_font, "HP " + _player.hp.ToString(), new Vector2(50, 20), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame()
        {
            _player = new Player(new Vector2(_screenWidth / 2, _screenHeight / 2));
            _theEgg = new Egg(GraphicsDevice, _view, _projection);
            _theEgg.LoadContent(Content);
            _player.LoadContent(Content);
            timer = 0;
        }
    }
}