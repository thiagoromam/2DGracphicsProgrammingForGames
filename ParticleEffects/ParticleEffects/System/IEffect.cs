using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleEffects.System
{
    public interface IEffect
    {
        int Duration { get; set; }
        int BurstFrequency { get; }
        int BurstCountdown { get; set; }
        int NewParticleAmount { get; }
        BlendState BlendState { get; }
        Vector2 Position { set; }

        void LoadContent(ContentManager content);
        Particle CreateParticle();
    }
}