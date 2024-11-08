using EggQuest.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EggQuest
{
    public class HealthBar : Object2D
    {
        public int Health;

        public int MaxHealth { get; private set; }

        HealthType HealthBarType;

        public HealthBar (HealthType type, int maxHealth, Vector2 position) : base(new BoundingRectangle())
        {
            HealthBarType = type;
            MaxHealth = maxHealth;
            Health = maxHealth;
            Position = position;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void LoadContent(ContentManager content)
        {
            if (HealthBarType == HealthType.Heart) Texture = content.Load<Texture2D>("Heart");
            else Texture = content.Load<Texture2D>("ShellFragment");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < MaxHealth; i++)
            {
                int xPos = (i < Health) ? 0 : 64;
                spriteBatch.Draw(Texture, Position + new Vector2(i * 64,0), new Rectangle(xPos, 0, 64, 64), Color.White, 0f, new Vector2(32, 32), 1f, SpriteEffects.None, 0f);
            }
        }
    }

    public enum HealthType
    {
        Egg,
        Heart
    }
}
