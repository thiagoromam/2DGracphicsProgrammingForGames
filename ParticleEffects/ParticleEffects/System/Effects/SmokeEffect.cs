using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace ParticleEffects.System.Effects
{
    public class SmokeEffect : IEffect
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

        public SmokeEffect()
        {
            Duration = 60000;
            NewParticleAmount = 4;
            BurstFrequency = 16;
            BlendState = BlendState.Additive;

            _radius = 50;
            _random = new Random();
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("whiteCircle");
            _origin = _texture.CalculateCenter();
        }

        public Particle CreateParticle()
        {
            var age = 5000 + _random.Next(5000);
            var fadeAge = age - _random.Next(100);

            var offset = new Vector2(
                (float)(_random.Next(_radius) * Math.Cos(_random.Next(360))),
                (float)(_random.Next(_radius) * Math.Sin(_random.Next(360)))
            );
            var offset2 = new Vector2((float)(400 * Math.Cos(Duration / 500f)), 0);
            var position = Position + offset + offset2;
            var velocity = new Vector2(0, -30 - _random.Next(30));
            var acceleration = new Vector2(10 + _random.Next(10), 0);

            var scaleVelocity = _random.Next(10) / 50f;

            var initialColor = Color.Black;
            initialColor.A = 128;

            var particle = new Particle();
            particle.Initialize(
                _texture, _origin,
                age,
                position, velocity, acceleration, 1,
                0, 0, 1,
                0.6f, scaleVelocity, 0, 3,
                initialColor, new Color(32, 32, 32, 0), fadeAge
            );

            return particle;
        }
    }
}