using System;
using System.Collections.Generic;
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
        private readonly Joystick _joystick;
        private Effect _effect;

        public TestComponent(MainGame game)
        {
            _game = game;
            _joystick = Joystick.Player1;
            _effect = new Effect(new Vector2(200, 200), 1);
        }

        public void Initialize()
        {
        }

        public void LoadContent(ContentManager content)
        {
            _effect.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            if (_joystick.IsFirePressed)
                _effect.Create();

            _effect.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            _effect.Draw(spriteBatch);
            spriteBatch.End();
        }
    }

    public class Effect
    {
        private Texture2D _texture;

        private readonly Vector2 _origin;
        private float _originRadius;

        private int _duration;
        private int _newParticleAmmount;
        private int _burstFrequency;
        private int _burstCountdown;

        private readonly List<Particle> _particles;

        public Effect(Vector2 origin, float originRadius)
        {
            _origin = origin;
            _originRadius = originRadius;
            _particles = new List<Particle>();
        }

        public void Create()
        {
            _duration = 10000;
            _newParticleAmmount = 1;
            _burstFrequency = 16;
            _burstCountdown = _burstFrequency;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("whiteStar");
        }

        public void Update(GameTime gameTime)
        {
            var totalMilliseconds = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            _duration -= totalMilliseconds;
            _burstCountdown -= totalMilliseconds;

            if (_burstCountdown <= 0 && _duration >= 0)
            {
                for (var i = 0; i < _newParticleAmmount; ++i)
                    CreateParticle();

                _burstCountdown = _burstFrequency;
            }

            for (var i = _particles.Count - 1; i >= 0; --i)
            {
                _particles[i].Update(gameTime);

                if (_particles[i].Age <= 0)
                    _particles.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < _particles.Count; ++i)
                _particles[i].Draw(spriteBatch);
        }

        private void CreateParticle()
        {
            const int age = 3000;

            var position = _origin;
            var velocity = new Vector2((float)(100 * Math.Cos(_duration)), (float)(100 * Math.Sin(_duration)));
            var acceleration = new Vector2(0, 75);
            const float dampening = 0.75f;

            const int rotation = 0;
            const float rotationVelocity = 2f;
            const float rotationDampening = 0.99f;

            const float scale = 0.2f;
            const float scaleVelocity = 0.2f;
            const float scaleAcceleration = -0.1f;
            const float maxScale = 1f;

            var initColor = Color.White;
            var finalColor = Color.White;
            finalColor.A = 0;

            var particle = new Particle(_texture);
            particle.Create(age, position, velocity, acceleration, dampening, rotation, rotationVelocity, rotationDampening, scale, scaleVelocity, scaleAcceleration, maxScale, initColor, finalColor, age);
            _particles.Add(particle);
        }
    }

    public class Particle
    {
        public int Age;

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
            Age = age;
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
            if (Age < 0)
                return;

            Age -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
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
            if (Age < 0 || Age > _fadeAge)
                return;

            var amount = (float)Age / _fadeAge;

            _color.R = (byte)MathHelper.Lerp(_finalColor.R, _initialColor.R, amount);
            _color.G = (byte)MathHelper.Lerp(_finalColor.G, _initialColor.G, amount);
            _color.B = (byte)MathHelper.Lerp(_finalColor.B, _initialColor.B, amount);
            _color.A = (byte)MathHelper.Lerp(_finalColor.A, _initialColor.A, amount);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Age < 0)
                return;

            spriteBatch.Draw(_texture, _position, null, _color, _rotation, _origin, _scale, SpriteEffects.None, 1);
        }
    }
}