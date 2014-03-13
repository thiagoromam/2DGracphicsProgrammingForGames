using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleEffects.System
{
    public class EffectManager
    {
        private readonly List<EffectGenerator> _effects;

        public EffectManager()
        {
            _effects = new List<EffectGenerator>();
        }

        public IEffectInitializer Add(IEffect effect)
        {
            var effectGenerator = new EffectGenerator(effect);
            _effects.Add(effectGenerator);
            return effectGenerator;
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
