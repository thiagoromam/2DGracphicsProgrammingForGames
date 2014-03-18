using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// ReSharper disable ForCanBeConvertedToForeach
namespace ParticleEffects.System
{
    public class EffectPool
    {
        private readonly List<EffectManager> _effects;
        private readonly ParticlePool _particlePool;

        public EffectPool()
        {
            _effects = new List<EffectManager>();
            _particlePool = new ParticlePool();
        }

        public IEffectInitializer Add(IEffect effect)
        {
            var effectManager = new EffectManager(_particlePool, effect);
            _effects.Add(effectManager);
            return effectManager;
        }

        public void LoadContent(ContentManager content)
        {
            for (var i = 0; i < _effects.Count; ++i)
                _effects[i].LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < _effects.Count; ++i)
                _effects[i].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < _effects.Count; ++i)
                _effects[i].Draw(spriteBatch);
        }
    }
}
