using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ParticleEffects.System;
using ParticleEffects.System.Effects;
using Resources;

// ReSharper disable once CheckNamespace
namespace Core
{
    public class TestComponent
    {
		private readonly MainGame _game;
        private readonly Joystick _joystick;
        private Vector2 _center;
        private readonly EffectPool _effectPool;
        private readonly IEffectInitializer _effect;

        public TestComponent(MainGame game)
		{
			_game = game;
            _game.BackgroundColor = Color.Black;
            
            _effectPool = new EffectPool();
            _effect = _effectPool.Add(new MovingFlameEffect());
            _joystick = Joystick.Player1;
		}
	
        public void Initialize()
        {
            _center = _game.GraphicsDevice.Viewport.CalculateCenterOfScreen();
        }
		
        public void LoadContent(ContentManager content)
		{
            _effectPool.LoadContent(content);
		}
		
        public void Update(GameTime gameTime)
		{
            if (_joystick.IsFirePressed)
                _effect.Start(_center);

            _effectPool.Update(gameTime);
		}
		
        public void Draw(SpriteBatch spriteBatch)
		{
            _effectPool.Draw(spriteBatch);
		}
    }
}