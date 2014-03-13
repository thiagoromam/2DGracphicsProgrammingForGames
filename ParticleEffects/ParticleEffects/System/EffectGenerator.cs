using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleEffects.System
{
    public class EffectGenerator : IEffectInitializer
    {
        private readonly IEffect _effect;
        private readonly List<Particle> _particles;
        private readonly int _initialDuration;

        public Vector2 Position
        {
            set { _effect.Position = value; }
        }

        public EffectGenerator(IEffect effect)
        {
            _effect = effect;
            _initialDuration = _effect.Duration;
            _effect.Duration = 0;
            _effect.BurstCountdown = 0;
            _particles = new List<Particle>();
        }

        public void Initialize()
        {
            _effect.Duration = _initialDuration;
            _effect.BurstCountdown = _effect.BurstFrequency;
        }

        public void LoadContent(ContentManager content)
        {
            _effect.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            if (_effect.Duration > 0)
                _effect.Duration -= gameTime.ElapsedGameTime.Milliseconds;

            if (_effect.BurstCountdown > 0)
                _effect.BurstCountdown -= gameTime.ElapsedGameTime.Milliseconds;

            if (_effect.BurstCountdown <= 0 && _effect.Duration > 0)
            {
                for (var i = 0; i < _effect.NewParticleAmount; ++i)
                    _particles.Add(_effect.CreateParticle());

                _effect.BurstCountdown = _effect.BurstFrequency;
            }

            for (var i = _particles.Count - 1; i >= 0; --i)
            {
                _particles[i].Update(gameTime);

                if (!_particles[i].IsAlive)
                    _particles.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, _effect.BlendState);
            for (var i = 0; i < _particles.Count; ++i)
                _particles[i].Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}