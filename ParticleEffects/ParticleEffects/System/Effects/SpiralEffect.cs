﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace ParticleEffects.System.Effects
{
    public class SpiralEffect : IEffect
    {
        private Texture2D _texture;
        private Vector2 _origin;

        public int Duration { get; set; }
        public int BurstFrequency { get; private set; }
        public int BurstCountdown { get; set; }
        public int NewParticleAmount { get; private set; }
        public BlendState BlendState { get; private set; }
        public Vector2 Position { private get; set; }

        public SpiralEffect()
        {
            Duration = 10000;
            NewParticleAmount = 1;
            BurstFrequency = 16;
            BlendState = BlendState.NonPremultiplied;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("whiteStar");
            _origin = _texture.CalculateCenter();
        }

        public Particle CreateParticle()
        {
            var particle = new Particle();
            var velocity = new Vector2((float)(100.0f * Math.Cos(Duration)), (float)(100.0f * Math.Sin(Duration)));
            var acceleration = new Vector2(0, 75);

            particle.Initialize(
                _texture, _origin,
                3000,
                Position, velocity, acceleration, 1,
                0, 2, 0.99f,
                0.2f, 0.2f, -0.1f, 1,
                Color.DarkRed, new Color(), 3000
            );

            return particle;
        }
    }
}
