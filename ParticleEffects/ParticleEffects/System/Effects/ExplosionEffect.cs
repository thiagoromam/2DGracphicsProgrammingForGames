using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace ParticleEffects.System.Effects
{
    public class ExplosionEffect : IEffect
    {
        private readonly int _radius;
        private readonly Random _random;
        private Texture2D _texture;
        private Vector2 _origin;

        public int Duration { get; set; }
        public int BurstFrequency { get; private set; }
        public int BurstCountdown { get; set; }
        public int NewParticleAmount { get; private set; }
        public BlendState BlendState { get; private set; }
        public Vector2 Position { set; private get; }

        public ExplosionEffect()
        {
            Duration = 16;
            NewParticleAmount = 800;
            BurstFrequency = 16;
            BlendState = BlendState.NonPremultiplied;

            _radius = 20;
            _random = new Random();
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("whiteStar");
            _origin = _texture.CalculateCenter();
        }

        public Particle CreateParticle()
        {
            var age = 3000 + _random.Next(5000);
            var fadeAge = age / 2;

            var offset = new Vector2(
                (float)(_random.Next(_radius) * Math.Cos(_random.Next(360))),
                (float)(_random.Next(_radius) * Math.Sin(_random.Next(360)))
            );

            var position = Position + offset;
            var velocity = new Vector2(_random.Next(500) + offset.X * 30, -60 * Math.Abs(offset.Y));
            if (position.Y >= 900)
            {
                position.Y = 900;
                velocity.X *= 0.025f;
            }

            var acceleration = new Vector2(0, 400);

            var rotationVelocity = velocity.X / 50.0f;
            var scale = 0.1f + _random.Next(10) / 50.0f;
            var scaleVelocity = (_random.Next(10) - 5) / 50.0f;

            var particle = new Particle();
            particle.Initialize(
                _texture, _origin,
                age,
                position, velocity, acceleration, 1,
                0, rotationVelocity, 0.97f,
                scale, scaleVelocity, 0, 1,
                new Color((byte)(_random.Next(128) + 128), 0, 0), Color.Black, fadeAge
            );

            return particle;
        }
    }
}
