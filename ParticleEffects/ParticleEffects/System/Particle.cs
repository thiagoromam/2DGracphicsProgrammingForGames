using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace ParticleEffects.System
{
    public class Particle
    {
        private Texture2D _texture;
        private Vector2 _origin;

        private int _age;

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

        public bool IsAlive
        {
            get { return _age > 0; }
        }

        public void Initialize(
            Texture2D texture, Vector2 origin,
            int age,
            Vector2 position, Vector2 velocity, Vector2 acceleration, float dampening,
            float rotation, float rotationVelocity, float rotationDampening,
            float scale, float scaleVelocity, float scaleAcceleration, float maxScale,
            Color initialColor, Color finalColor, int fadeAge)
        {
            _origin = origin;
            _texture = texture;
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
            if (!IsAlive) return;

            _age -= gameTime.ElapsedGameTime.Milliseconds;

            UpdatePosition(gameTime);
            UpdateRotation(gameTime);
            UpdateScale(gameTime);
            UpdateColor();
        }

        private void UpdatePosition(GameTime gameTime)
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
            if (_age > _fadeAge) return;

            var amount = (float)_age / _fadeAge;

            _color.R = (byte)MathHelper.Lerp(_finalColor.R, _initialColor.R, amount);
            _color.G = (byte)MathHelper.Lerp(_finalColor.G, _initialColor.G, amount);
            _color.B = (byte)MathHelper.Lerp(_finalColor.B, _initialColor.B, amount);
            _color.A = (byte)MathHelper.Lerp(_finalColor.A, _initialColor.A, amount);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsAlive) return;

            spriteBatch.Draw(_texture, _position, null, _color, _rotation, _origin, _scale, SpriteEffects.None, 1);
        }
    }
}