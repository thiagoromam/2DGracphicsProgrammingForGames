using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleEffects.System
{
    public class EffectGenerator : IEffectInitializer
    {
        private readonly IEffect _effect;
        private readonly int _initialDuration;
        private readonly Particle[] _particles;
        private int _particlesCount;

        public EffectGenerator(IEffect effect)
        {
            _effect = effect;
            _initialDuration = _effect.Duration;
            _effect.Duration = 0;
            _effect.BurstCountdown = 0;
            _particles = new Particle[2000];
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
                    _effect.InitializeParticle(GetParticle());

                _effect.BurstCountdown = _effect.BurstFrequency;
            }

            if (_effect.Duration > 0)
                _effect.Duration -= gameTime.ElapsedGameTime.Milliseconds;
            
            for (var i = 0; i < _particlesCount; ++i)
                _particles[i].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, _effect.BlendState);
            for (var i = 0; i < _particlesCount; ++i)
                _particles[i].Draw(spriteBatch);

            spriteBatch.End();
        }

        public Particle GetParticle()
        {
            Particle particle;
            for (var i = 0; i < _particlesCount; ++i)
            {
                particle = _particles[i];
                if (!particle.IsAlive)
                    return particle;
            }

            particle = new Particle();
            _particles[_particlesCount++] = particle;

            return particle;
        }
    }
}