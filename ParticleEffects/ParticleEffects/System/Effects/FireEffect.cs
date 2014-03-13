using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace ParticleEffects.System.Effects
{
    public class FireEffect : IEffect
    {
        private Texture2D _texture;
        private readonly int _radius;
        private Vector2 _origin;
        private readonly Random _random;

        public int Duration { get; set; }
        public int BurstFrequency { get; private set; }
        public int BurstCountdown { get; set; }
        public int NewParticleAmount { get; private set; }
        public BlendState BlendState { get; private set; }
        public Vector2 Position { set; private get; }

        public FireEffect()
        {
            Duration = 60000;
            NewParticleAmount = 10;
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
            var particle = new Particle();
            
            var offset = new Vector2(
                (float)(_random.Next(_radius) * Math.Cos(_random.Next(360))),
                (float)(_random.Next(_radius) * Math.Sin(_random.Next(360)))
            );
            var position = Position + offset;
            var velocity = new Vector2(-(offset.X * 0.5f), 0);
            var acceleration = new Vector2(0, -_random.Next(200));

            var finalColor = Color.Yellow;
            finalColor.A = 0;

            particle.Initialize(
                _texture, _origin,
                3000,
                position, velocity, acceleration, 0.96f,
                0, 0, 1,
                0.5f, -0.1f, 0, 1,
                Color.Red, finalColor, 2750
            );

            return particle;
        }
    }
}
