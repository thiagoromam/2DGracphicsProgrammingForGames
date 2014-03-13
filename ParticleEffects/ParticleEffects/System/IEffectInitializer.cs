using Microsoft.Xna.Framework;

namespace ParticleEffects.System
{
    public interface IEffectInitializer
    {
        Vector2 Position { set; }
        void Initialize();
    }
}