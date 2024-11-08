using EggQuest.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EggQuest
{
    public class Player : Object2D
    {
        public Player (Vector2 position) : base(new BoundingRectangle(position.X, position.Y, 33, 53))
        {
            Position = position;
            Width = 33;
            Height = 53;
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("Spatula");
        }
    }
}
