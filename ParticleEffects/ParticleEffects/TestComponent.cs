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
        private readonly EffectManager _effectManager;
        private readonly IEffectInitializer _effect;

        public TestComponent(MainGame game)
		{
			_game = game;
            _game.BackgroundColor = Color.Black;
            
            _effectManager = new EffectManager();
            _effect = _effectManager.Add(new FireEffect());
            _joystick = Joystick.Player1;
		}
	
        public void Initialize()
        {
            _center = _game.GraphicsDevice.Viewport.CalculateCenterOfScreen();
        }
		
        public void LoadContent(ContentManager content)
		{
            _effectManager.LoadContent(content);
		}
		
        public void Update(GameTime gameTime)
		{
            if (_joystick.IsFirePressed)
                _effect.Start(_center);

            _effectManager.Update(gameTime);
		}
		
        public void Draw(SpriteBatch spriteBatch)
		{
            _effectManager.Draw(spriteBatch);
		}
    }
}