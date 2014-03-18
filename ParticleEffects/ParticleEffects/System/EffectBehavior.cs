using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ParticleEffects.System
{
    public class EffectBehavior : IEffectInitializer
    {
        private readonly IEffect _effect;
        private readonly int _initialDuration;
        private readonly IParticleInitializer _particleManager;

        public EffectBehavior(IEffect effect, IParticleInitializer particleManager)
        {
            _effect = effect;
            _particleManager = particleManager;
            _initialDuration = _effect.Duration;
            _effect.Duration = 0;
            _effect.BurstCountdown = 0;
        }

        public void Start(Vector2 position)
        {
            _effect.Position = position;
            _effect.Duration = _initialDuration;
            _effect.BurstCountdown = _effect.BurstFrequency;
        }

        public void LoadContent(ContentManager content)
        {
            _effect.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            if (_effect.BurstCountdown > 0)
                _effect.BurstCountdown -= gameTime.ElapsedGameTime.Milliseconds;

            if (_effect.BurstCountdown <= 0 && _effect.Duration > 0)
            {
                for (var i = 0; i < _effect.NewParticleAmount; ++i)
                    _particleManager.InitializeParticle();

                _effect.BurstCountdown = _effect.BurstFrequency;
            }

            if (_effect.Duration > 0)
                _effect.Duration -= gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}