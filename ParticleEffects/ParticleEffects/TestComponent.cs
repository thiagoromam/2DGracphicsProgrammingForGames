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
        private readonly EffectGenerator _effect;
        private readonly Joystick _joystick;
        private Vector2 _center;

        public TestComponent(MainGame game)
		{
			_game = game;
		    _effect = new EffectGenerator(new SpiralEffect());
            _joystick = Joystick.Player1;
		}
	
        public void Initialize()
        {
            _center = _game.GraphicsDevice.Viewport.CalculateCenterOfScreen();
        }
		
        public void LoadContent(ContentManager content)
		{
            _effect.LoadContent(content);
		}
		
        public void Update(GameTime gameTime)
		{
            if (_joystick.IsFirePressed)
            {
                _effect.Initialize();
                _effect.Position = _center;
            }

            _effect.Update(gameTime);
		}
		
        public void Draw(SpriteBatch spriteBatch)
		{
            _effect.Draw(spriteBatch);
		}
    }
}