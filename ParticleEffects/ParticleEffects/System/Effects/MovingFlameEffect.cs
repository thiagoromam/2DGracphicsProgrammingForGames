﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace ParticleEffects.System.Effects
{
    public class MovingFlameEffect : IEffect
    {
        private readonly Random _random;
        private readonly int _radius;
        private readonly Color _finalColor;
        private Texture2D _texture;
        private Vector2 _origin;

        public int Duration { get; set; }
        public int BurstFrequency { get; private set; }
        public int BurstCountdown { get; set; }
        public int NewParticleAmount { get; private set; }
        public BlendState BlendState { get; private set; }
        public Vector2 Position { set; private get; }

        public MovingFlameEffect()
        {
            Duration = 60000;
            NewParticleAmount = 15;
            BurstFrequency = 16;
            BlendState = BlendState.Additive;

            _random = new Random();
            _radius = 15;
            _finalColor = Color.DarkOrange;
            _finalColor.A = 0;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("whiteCircle");
            _origin = _texture.CalculateCenter();
        }

        public void InitializeParticle(Particle particle)
        {
            var age = 500 + _random.Next(500);
            var fadeAge = age - _random.Next(100);

            var offset = new Vector2(
                (float)(_random.Next(_radius) * Math.Cos(_random.Next(360))),
                (float)(_random.Next(_radius) * Math.Sin(_random.Next(360)))
            );
            offset.X += (float)(200 * Math.Cos(Duration / 500f));

            var position = Position + offset;
            var velocity = new Vector2(-(offset.X * 0.5f), -500);
            var acceleration = new Vector2(0, -_random.Next(300));

            particle.Initialize(
                _texture, _origin,
                age, position, velocity, acceleration, 0.96f,
                0, 2, 0.99f,
                0.5f, -0.1f, 0, 1,
                Color.DarkBlue, _finalColor, fadeAge
            );
        }
    }
}