using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

// ReSharper disable once CheckNamespace
namespace Core
{
    public class TestComponent
    {
        private readonly MainGame _game;
        private Particle _particle;
        private readonly Joystick _joystick;

        public TestComponent(MainGame game)
        {
            _game = game;
            _joystick = Joystick.Player1;
        }

        public void Initialize()
        {
        }

        public void LoadContent(ContentManager content)
        {
            _particle = new Particle(content.Load<Texture2D>("whiteStar"));
        }

        public void Update(GameTime gameTime)
        {
            if (_joystick.IsDownPressing)
            {
                _particle.Create(
                    3000, new Vector2(400, 400), new Vector2(70, -100), new Vector2(0, 75), 1,
                    0, 2, 0.99f,
                    0.2f, 0.2f, -0.1f, 1,
                    Color.White, Color.Gray, 1000
                );
            }

            _particle.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            _particle.Draw(spriteBatch);
            spriteBatch.End();
        }
    }

    public class Particle
    {
        private int _age;

        private readonly Texture2D _texture;
        private readonly Vector2 _origin;

        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _acceleration;
        private float _dampening;

        private float _rotation;
        private float _rotationVelocity;
        private float _rotationDampening;
        private float _scale;
        private float _scaleVelocity;
        private float _scaleAcceleration;
        private float _maxScale;

        private Color _color;
        private Color _initialColor;
        private Color _finalColor;
        private int _fadeAge;

        public Particle(Texture2D texture)
        {
            _texture = texture;
            _origin = texture.CalculateCenter();
        }

        public void Create(
            int age, Vector2 position, Vector2 velocity, Vector2 acceleration, float dampening, // position
            float rotation, float rotationVelocity, float rotationDampening, // rotation
            float scale, float scaleVelocity, float scaleAcceleration, float maxScale, // scale
            Color initialColor, Color finalColor, int fadeAge
        )
        {
            _age = age;
            _position = position;
            _velocity = velocity;
            _acceleration = acceleration;
            _dampening = dampening;
            _rotation = rotation;
            _rotationVelocity = rotationVelocity;
            _rotationDampening = rotationDampening;
            _scale = scale;
            _scaleVelocity = scaleVelocity;
            _scaleAcceleration = scaleAcceleration;
            _maxScale = maxScale;
            _color = initialColor;
            _initialColor = initialColor;
            _finalColor = finalColor;
            _fadeAge = fadeAge;
        }

        public void Update(GameTime gameTime)
        {
            if (_age < 0)
                return;

            _age -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            UpdatePosition(gameTime);
            UpdateRotation(gameTime);
            UpdateScale(gameTime);
            UpdateColor();
        }

        public void UpdatePosition(GameTime gameTime)
        {
            var elapsedSeconds = gameTime.ElapsedSeconds();

            _velocity *= _dampening;
            _velocity += _acceleration * elapsedSeconds;
            _position += _velocity * elapsedSeconds;
        }

        private void UpdateRotation(GameTime gameTime)
        {
            _rotation *= _rotationDampening;
            _rotation += _rotationVelocity * gameTime.ElapsedSeconds();
        }

        private void UpdateScale(GameTime gameTime)
        {
            var elapsedSeconds = gameTime.ElapsedSeconds();
            _scaleVelocity += _scaleAcceleration * elapsedSeconds;
            _scale += _scaleVelocity * elapsedSeconds;
            _scale = MathHelper.Clamp(_scale, 0, _maxScale);
        }

        private void UpdateColor()
        {
            if (_age < 0 || _age > _fadeAge)
                return;

            var amount = (float)_age / _fadeAge;

            _color.R = (byte)MathHelper.Lerp(_finalColor.R, _initialColor.R, amount);
            _color.G = (byte)MathHelper.Lerp(_finalColor.G, _initialColor.G, amount);
            _color.B = (byte)MathHelper.Lerp(_finalColor.B, _initialColor.B, amount);
            _color.A = (byte)MathHelper.Lerp(_finalColor.A, _initialColor.A, amount);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_age < 0)
                return;

            spriteBatch.Draw(_texture, _position, null, _color, _rotation, _origin, _scale, SpriteEffects.None, 1);
        }
    }
}