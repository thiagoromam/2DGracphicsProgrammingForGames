using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleEffects.System
{
    public class EffectManager : IEffectInitializer
    {
        private readonly ParticleManager _particleManager;
        private readonly EffectBehavior _effectBehavior;

        public EffectManager(ParticlePool particlePool, IEffect effect)
        {
            _particleManager = new ParticleManager(particlePool, effect);
            _effectBehavior = new EffectBehavior(effect, _particleManager);
        }

        public void Start(Vector2 position)
        {
            _effectBehavior.Start(position);
        }

        public void LoadContent(ContentManager content)
        {
            _effectBehavior.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            _effectBehavior.Update(gameTime);
            _particleManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _particleManager.Draw(spriteBatch);
        }
    }
}
