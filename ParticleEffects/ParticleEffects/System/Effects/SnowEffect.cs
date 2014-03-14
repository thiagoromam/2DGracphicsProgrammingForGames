using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace ParticleEffects.System.Effects
{
    public class SnowEffect : IEffect
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

        public SnowEffect()
        {
            Duration = 60000;
            NewParticleAmount = 1;
            BurstFrequency = 64;
            BlendState = BlendState.NonPremultiplied;
            
            _radius = 50;
            _random = new Random();
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("snowFlake");
            _origin = _texture.CalculateCenter();
        }

        public Particle CreateParticle()
        {
            var scale = 0.1f + _random.Next(10) / 20f;

            var age = (int)(10000 / scale);

            var offset = new Vector2(
                (float)(_random.Next(_radius) * Math.Cos(_random.Next(360))),
                (float)(_random.Next(_radius) * Math.Sin(_random.Next(360)))
            );
            var offset2 = new Vector2((float) (Position.X * Math.Cos(Duration / 500f)) ,0);
            var position = Position + offset + offset2;
            var velocity = new Vector2(_random.Next(10) - 5, 100 * scale);

            var rotationVelocity = velocity.X / 5;

            var finalColor = Color.White;
            finalColor.A = 0;

            var particle = new Particle();
            particle.Initialize(
                _texture, _origin,
                age,
                position, velocity, Vector2.Zero, 1,
                0, rotationVelocity, 1,
                scale, 0, 0, 1,
                Color.White, finalColor, age
            );

            return particle;
        }
    }
}
