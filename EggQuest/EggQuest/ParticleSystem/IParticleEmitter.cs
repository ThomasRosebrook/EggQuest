using Microsoft.Xna.Framework;

namespace EggQuest.ParticleSystem
{
    public interface IParticleEmitter
    {
        public Vector2 Position { get; }
        public Vector2 Velocity { get; }
    }
}
