using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

// ReSharper disable once CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
namespace Core
{
    public class TestComponent
    {
        private Texture2D _texture;
        private float _linearY;
        private float _direction;
        private float _speed;
        private float _counter;
        private float _sinusodialY;

        public TestComponent(MainGame game)
        {
            game.Graphics.IsFullScreen = false;
            game.BackgroundColor = Color.Black;
        }

        public void Initialize()
        {
            _linearY = 1;
            _direction = 1;
            _speed = 2;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("whiteCircle");
        }

        public void Update(GameTime gameTime)
        {
            _counter += gameTime.ElapsedSeconds() * _direction * _speed;

            if (_counter > MathHelper.PiOver2)
            {
                _counter = MathHelper.PiOver2;
                _direction *= -1;
            }
            if (_counter < -MathHelper.PiOver2)
            {
                _counter = -MathHelper.PiOver2;
                _direction *= -1;
            }

            _linearY = _counter / MathHelper.PiOver2;
            _sinusodialY = (float)Math.Sin(_counter);

            _linearY = -Math.Abs(_linearY);
            _sinusodialY = -Math.Abs(_sinusodialY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var scaledLin2 = _linearY * 256 + 300;
            var scaledSinY = _sinusodialY * 256 + 300;

            spriteBatch.Begin();
            spriteBatch.Draw(_texture, new Vector2(360, scaledSinY));
            spriteBatch.Draw(_texture, new Vector2(790, scaledLin2));
            spriteBatch.End();
        }
    }
}