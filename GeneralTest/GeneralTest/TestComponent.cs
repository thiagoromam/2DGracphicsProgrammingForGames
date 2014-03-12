using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly MainGame _game;
        private readonly Joystick _joystick;
        private Effect _effect;
        private readonly EffectType[] _effectTypes;
        private Vector2 _effectNamePosition;
        private string _effectName;
        private int _currentEffectType;
        private SpriteFont _segoe;
        public static float ScreenHeight;

        public TestComponent(MainGame game)
        {
            _game = game;
            _game.BackgroundColor = Color.Black;

            _effectTypes = Enum.GetValues(typeof(EffectType)).Cast<EffectType>().ToArray();
            _joystick = Joystick.Player1;
        }

        public void Initialize()
        {
            var viewport = _game.GraphicsDevice.Viewport;
            ScreenHeight = viewport.Height;
            _effect = new Effect(viewport.CalculateCenterOfScreen());
            _effectNamePosition = new Vector2(50, 50);
        }

        public void LoadContent(ContentManager content)
        {
            _effect.LoadContent(content);
            _segoe = content.Load<SpriteFont>("Segoe");
            _effectName = _effectTypes[_currentEffectType].ToString();
        }

        public void Update(GameTime gameTime)
        {
            if (_joystick.IsLeftPressed)
            {
                _currentEffectType--;

                if (_currentEffectType < 0)
                    _currentEffectType = _effectTypes.Length - 1;

                _effectName = _effectTypes[_currentEffectType].ToString();

                _effect.Invalidate();
            }
            else if (_joystick.IsRightPressed)
            {
                _currentEffectType++;

                if (_currentEffectType == _effectTypes.Length)
                    _currentEffectType = 0;

                _effectName = _effectTypes[_currentEffectType].ToString();

                _effect.Invalidate();
            }

            if (_joystick.IsFirePressed)
            {
                _effect.Create(_effectTypes[_currentEffectType]);
            }
            else if (_joystick.IsFire2Pressed)
            {
                _effect.Invalidate();
            }

            _effect.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(_segoe, _effectName, _effectNamePosition, Color.White);
            spriteBatch.End();

            _effect.Draw(spriteBatch);
        }
    }

    public enum EffectType
    {
        Spiral,
        Fire,
        FireWall,
        MovingFlame,
        Smoke,
        Explosion,
        Snow
    }

    public class Effect
    {
        private Texture2D _starTexture;
        private Texture2D _circleTexture;
        private Texture2D _snowFlakeTexture;

        private readonly Vector2 _origin;
        private int _radius;

        private int _duration;
        private int _newParticleAmount;
        private int _burstFrequency;
        private int _burstCountdown;

        private readonly List<Particle> _particles;
        private EffectType _effectType;
        private BlendState _blendState;

        private readonly Random _random;

        public Effect(Vector2 origin)
        {
            _origin = origin;
            _particles = new List<Particle>();
            _random = new Random();
        }

        public void LoadContent(ContentManager content)
        {
            _starTexture = content.Load<Texture2D>("whiteStar");
            _circleTexture = content.Load<Texture2D>("whiteCircle");
            _snowFlakeTexture = content.Load<Texture2D>("snowFlake");
        }

        public void Update(GameTime gameTime)
        {
            var totalMilliseconds = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            _duration -= totalMilliseconds;
            _burstCountdown -= totalMilliseconds;

            if (_burstCountdown <= 0 && _duration >= 0)
            {
                for (var i = 0; i < _newParticleAmount; ++i)
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
            spriteBatch.Begin(SpriteSortMode.FrontToBack, _blendState);
            for (var i = 0; i < _particles.Count; ++i)
                _particles[i].Draw(spriteBatch);
            spriteBatch.End();
        }

        public void Invalidate()
        {
            _duration = 0;
        }

        public bool IsAlive()
        {
            return _duration > 0 || _particles.Count > 0;
        }

        public void Create(EffectType effectType)
        {
            _effectType = effectType;
            switch (effectType)
            {
                case EffectType.Spiral: CreateSpiral(); break;
                case EffectType.Fire: CreateFire(); break;
                case EffectType.FireWall: CreateFireWall(); break;
                case EffectType.MovingFlame: CreateMovingFlame(); break;
                case EffectType.Smoke: CreateSmoke(); break;
                case EffectType.Explosion: CreateExplosion(); break;
                case EffectType.Snow: CreateSnow(); break;
                default: throw new ArgumentOutOfRangeException("effectType");
            }
        }
        private void CreateSpiral()
        {
            _duration = 10000;
            _newParticleAmount = 1;
            _burstFrequency = 16;
            _burstCountdown = _burstFrequency;
            _blendState = BlendState.NonPremultiplied;
        }
        private void CreateFire()
        {
            _duration = 60000;
            _newParticleAmount = 10;
            _burstFrequency = 16;
            _burstCountdown = _burstFrequency;

            _radius = 50;
            _blendState = BlendState.Additive;
        }
        private void CreateFireWall()
        {
            _duration = 60000;
            _newParticleAmount = 50;
            _burstFrequency = 16;
            _burstCountdown = _burstFrequency;

            _radius = 50;
            _blendState = BlendState.Additive;
        }
        private void CreateMovingFlame()
        {
            _duration = 60000;
            _newParticleAmount = 15;
            _burstFrequency = 16;
            _burstCountdown = _burstFrequency;

            _radius = 15;
            _blendState = BlendState.Additive;
        }
        private void CreateSmoke()
        {
            _duration = 60000;
            _newParticleAmount = 4;
            _burstFrequency = 16;
            _burstCountdown = _burstFrequency;

            _radius = 50;
            _blendState = BlendState.Additive;
        }
        private void CreateExplosion()
        {
            _duration = 16;
            _newParticleAmount = 800;
            _burstFrequency = 16;
            _burstCountdown = _burstFrequency;

            _radius = 20;
            _blendState = BlendState.NonPremultiplied;
        }
        private void CreateSnow()
        {
            _duration = 60000;
            _newParticleAmount = 1;
            _burstFrequency = 64;
            _burstCountdown = _burstFrequency;

            _radius = 50;
            _blendState = BlendState.NonPremultiplied;
        }

        private void CreateParticle()
        {
            switch (_effectType)
            {
                case EffectType.Spiral: CreateSpiralParticle(); break;
                case EffectType.Fire: CreateFireParticle(); break;
                case EffectType.FireWall: CreateFireWallParticle(); break;
                case EffectType.MovingFlame: CreateMovingFlameParticle(); break;
                case EffectType.Smoke: CreateSmokeParticle(); break;
                case EffectType.Explosion: CreateExplosionParticle(); break;
                case EffectType.Snow: CreateSnowParticle(); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        private void CreateSpiralParticle()
        {
            const int age = 3000;

            var velocity = new Vector2((float)(100.0f * Math.Cos(_duration)), (float)(100.0f * Math.Sin(_duration)));
            var acceleration = new Vector2(0, 75);
            const float dampening = 1f;

            const float rotation = 0.0f;
            const float rotationVelocity = 2.0f;
            const float rotationDampening = 0.99f;

            const float scale = 0.2f;
            const float scaleVelocity = 0.2f;
            const float scaleAcceleration = -0.1f;
            const float maxScale = 1.0f;

            var initialColor = Color.DarkRed;
            var finalColor = Color.DarkRed;
            finalColor *= 0;

            AddNewParticle(_starTexture, age, _origin, velocity, acceleration, dampening, rotation, rotationVelocity, rotationDampening,
                scale, scaleVelocity, scaleAcceleration, maxScale, initialColor, finalColor, age);
        }
        private void CreateFireParticle()
        {
            const int age = 3000;
            const int fadeAge = 2750;

            var offset = new Vector2(
                (float)(_random.Next(_radius) * Math.Cos(_random.Next(360))),
                (float)(_random.Next(_radius) * Math.Sin(_random.Next(360)))
            );
            var position = _origin + offset;
            var velocity = new Vector2(-(offset.X * 0.5f), 0);
            var acceleration = new Vector2(0, -_random.Next(200));


            const float dampening = 0.96f;

            const float rotation = 0;
            const float rotationVelocity = 0;
            const float rotationDampening = 1;

            const float scale = 0.5f;
            const float scaleVelocity = -0.1f;
            const float scaleAcceleration = 0;
            const float maxScale = 1;

            var initialColor = Color.Red;
            var finalColor = Color.Yellow;
            finalColor.A = 0;

            AddNewParticle(_circleTexture, age, position, velocity, acceleration, dampening, rotation, rotationVelocity, rotationDampening,
                scale, scaleVelocity, scaleAcceleration, maxScale, initialColor, finalColor, fadeAge);
        }
        private void CreateFireWallParticle()
        {
            const int age = 3000;
            const int fadeAge = 2750;

            var offset = new Vector2(
                (float)(_random.Next(_radius) * Math.Cos(_random.Next(360))),
                (float)(_random.Next(_radius) * Math.Sin(_random.Next(360)))
            );
            var offset2 = new Vector2((float)(400 * Math.Cos(_duration)), 0);
            var position = _origin + offset + offset2;
            var velocity = new Vector2(-(offset.X * 0.5f), 0);
            var acceleration = new Vector2(0, -_random.Next(200));


            const float dampening = 0.96f;

            const float rotation = 0;
            const float rotationVelocity = 0;
            const float rotationDampening = 1;

            const float scale = 0.5f;
            const float scaleVelocity = -0.1f;
            const float scaleAcceleration = 0;
            const float maxScale = 1;

            var initialColor = Color.Red;
            var finalColor = Color.Yellow;
            finalColor.A = 0;

            AddNewParticle(_circleTexture, age, position, velocity, acceleration, dampening, rotation, rotationVelocity, rotationDampening,
                scale, scaleVelocity, scaleAcceleration, maxScale, initialColor, finalColor, fadeAge);
        }
        private void CreateMovingFlameParticle()
        {
            var age = 500 + _random.Next(500);
            var fadeAge = age - _random.Next(100);

            var offset = new Vector2(
                (float)(_random.Next(_radius) * Math.Cos(_random.Next(360))),
                (float)(_random.Next(_radius) * Math.Sin(_random.Next(360)))
            );
            offset.X += (float)(200 * Math.Cos(_duration / 500f));

            var position = _origin + offset;
            var velocity = new Vector2(-(offset.X * 0.5f), -500);
            var acceleration = new Vector2(0, -_random.Next(300));

            const float dampening = 0.96f;

            const float rotation = 0;
            const float rotationVelocity = 2;
            const float rotationDampening = 0.99f;

            const float scale = 0.5f;
            const float scaleVelocity = -0.1f;
            const float scaleAcceleration = 0;
            const float maxScale = 1;

            var initialColor = Color.DarkBlue;
            var finalColor = Color.DarkOrange;
            finalColor.A = 0;

            AddNewParticle(_circleTexture, age, position, velocity, acceleration, dampening, rotation, rotationVelocity, rotationDampening,
                scale, scaleVelocity, scaleAcceleration, maxScale, initialColor, finalColor, fadeAge);
        }
        private void CreateSmokeParticle()
        {
            var age = 5000 + _random.Next(5000);
            var fadeAge = age - _random.Next(100);

            var offset = new Vector2(
                (float)(_random.Next(_radius) * Math.Cos(_random.Next(360))),
                (float)(_random.Next(_radius) * Math.Sin(_random.Next(360)))
            );
            var offset2 = new Vector2((float)(400 * Math.Cos(_duration / 500f)), 0);
            var position = _origin + offset + offset2;
            var velocity = new Vector2(0, -30 - _random.Next(30));
            var acceleration = new Vector2(10 + _random.Next(10), 0);

            const float dampening = 1f;

            const float rotation = 0;
            const float rotationVelocity = 0;
            const float rotationDampening = 1;

            const float scale = 0.6f;
            var scaleVelocity = _random.Next(10) / 50f;
            const float scaleAcceleration = 0;
            const float maxScale = 3;

            var initialColor = Color.Black;
            initialColor.A = 128;
            var finalColor = new Color(32, 32, 32, 0);

            AddNewParticle(_circleTexture, age, position, velocity, acceleration, dampening, rotation, rotationVelocity, rotationDampening,
                scale, scaleVelocity, scaleAcceleration, maxScale, initialColor, finalColor, fadeAge);
        }
        private void CreateExplosionParticle()
        {
            var age = 3000 + _random.Next(5000);
            var fadeAge = age / 2;

            var position = new Vector2(200, TestComponent.ScreenHeight);

            var offset = new Vector2(
                (float)(_random.Next(_radius) * Math.Cos(_random.Next(360))),
                (float)(_random.Next(_radius) * Math.Sin(_random.Next(360)))
            );
            position += offset;
            var velocity = new Vector2(_random.Next(500) + offset.X * 30, -60 * Math.Abs(offset.Y));
            var acceleration = new Vector2(0, 400);

            const float dampening = 1.0f;

            const float rotation = 0.0f;
            var rotationVelocity = velocity.X / 50.0f;
            const float rotationDampening = 0.97f;

            var scale = 0.1f + _random.Next(10) / 50.0f;
            var scaleVelocity = (_random.Next(10) - 5) / 50.0f;
            const float scaleAcceleration = 0.0f;
            const float maxScale = 1.0f;

            var initialColor = new Color((byte)(_random.Next(128) + 128), 0, 0);
            var finalColor = Color.Black;

            AddNewParticle(_starTexture, age, position, velocity, acceleration, dampening, rotation, rotationVelocity, rotationDampening,
                scale, scaleVelocity, scaleAcceleration, maxScale, initialColor, finalColor, fadeAge);
        }
        private void CreateSnowParticle()
        {
            var scale = 0.1f + _random.Next(10) / 20f;
            const float scaleVelocity = 0;
            const float scaleAcceleration = 0;
            const float maxScale = 1;

            var age = (int)(10000 / scale);

            var offset = new Vector2(
                (float)(_random.Next(_radius) * Math.Cos(_random.Next(360))),
                (float)(_random.Next(_radius) * Math.Sin(_random.Next(360)))
            );
            var offset2 = new Vector2((float) (_origin.X * Math.Cos(_duration / 500f)) ,0);
            var position = new Vector2(_origin.X, -50) + offset + offset2;

            var velocity = new Vector2(_random.Next(10) - 5, 100 * scale);

            const float dampening = 1;

            const float rotation = 0;
            var rotationVelocity = velocity.X / 5;
            const float rotationDampening = 1;

            var initialColor = Color.White;
            var finalColor = Color.White;
            finalColor.A = 0;


            AddNewParticle(_snowFlakeTexture, age, position, velocity, Vector2.Zero, dampening, rotation, rotationVelocity, rotationDampening,
                scale, scaleVelocity, scaleAcceleration, maxScale, initialColor, finalColor, age);
        }

        private void AddNewParticle(
            Texture2D texture,
            int age, Vector2 position, Vector2 velocity, Vector2 acceleration, float dampening,
            float rotation, float rotationVelocity, float rotationDampening,
            float scale, float scaleVelocity, float scaleAcceleration, float maxScale,
            Color initialColor, Color finalColor, int fadeAge)
        {
            var particle = new Particle(texture);
            particle.Create(age, position, velocity, acceleration, dampening, rotation, rotationVelocity, rotationDampening,
                scale, scaleVelocity, scaleAcceleration, maxScale, initialColor, finalColor, fadeAge);
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

            Age -= gameTime.ElapsedGameTime.Milliseconds;

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

            if (_position.Y >= TestComponent.ScreenHeight)
            {
                _position.Y = TestComponent.ScreenHeight;
                _velocity.X = 0;
            }
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