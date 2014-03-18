using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// ReSharper disable ForCanBeConvertedToForeach
namespace ParticleEffects.System
{
    public class ParticleManager : IParticleInitializer
    {
        private readonly ParticlePool _particlePool;
        private readonly IEffect _effect;
        private readonly int[] _range;
        private int _particlesCount;

        public ParticleManager(ParticlePool particlePool, IEffect effect)
        {
            _particlePool = particlePool;
            _effect = effect;
            _range = _particlePool.GetRange(2000);
        }

        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < _particlesCount; ++i)
                _particlePool[_range[i]].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, _effect.BlendState);
            
            for (var i = 0; i < _particlesCount; ++i)
                _particlePool[_range[i]].Draw(spriteBatch);
            
            spriteBatch.End();
        }

        public void InitializeParticle()
        {
            for (var i = 0; i < _particlesCount; ++i)
            {
                var particle = _particlePool[_range[i]];
                if (particle.IsAlive) continue;

                _effect.InitializeParticle(particle);
                return;
            }

            if (_particlesCount < _range.Length)
            {
                var particle = new Particle();
                _particlePool[_particlesCount++] = particle;
                _effect.InitializeParticle(particle);
            }
        }
    }
}
