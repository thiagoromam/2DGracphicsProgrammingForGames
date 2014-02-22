using System;
using Microsoft.Xna.Framework;

namespace DepthScaleWithJump
{
    public class Runner : Sprite
    {
        private readonly float _maxVelocity;

        private int _currentCel;
        private int _numberOfCels;
        private float _msUntilNextCel;
        private int _msPerCel;

        public Vector2 Velocity;

        public bool IsRunning;
        public bool IsJumping;
        public Vector2 PositionFromGroundWhileJumping;

        public Runner()
        {
            _maxVelocity = TestComponent.BufferWidth / 6;
        }

        public override void Initialize()
        {
            Initialize(new Rectangle(0, 0, 128, 128), new Vector2(57, 105));

            _numberOfCels = 12;
            _currentCel = 0;
            _msPerCel = 50;
            _msUntilNextCel = _msPerCel;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateAnimation(gameTime);
            UpdatePosition(gameTime);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            _msUntilNextCel -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            var relativeVelocity = Math.Abs(Velocity.X / _maxVelocity);
            if (relativeVelocity > 0.05f)
            {
                var mustUpdate = _msUntilNextCel <= 0;

                if (mustUpdate)
                    _currentCel++;

                if (_currentCel >= _numberOfCels)
                    _currentCel = 0;

                var location = Location.Value;
                location.X = location.Width * _currentCel;
                Location = location;

                if (mustUpdate)
                    _msUntilNextCel = (int)(_msPerCel * (2f - relativeVelocity));
            }
        }

        private void UpdatePosition(GameTime gameTime)
        {
            var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Velocity.X = MathHelper.Clamp(Velocity.X, -_maxVelocity, _maxVelocity);
            Position.X += Scale * Velocity.X * elapsedSeconds;

            if (IsJumping)
            {
                Position.Y += Scale * Velocity.Y;
            }
            else
            {
                Velocity.Y = MathHelper.Clamp(Velocity.Y, -_maxVelocity, _maxVelocity);
                Position.Y += Scale * Velocity.Y * elapsedSeconds;
                Position.Y = MathHelper.Clamp(Position.Y, TestComponent.Horizon, TestComponent.BufferHeight);
            }
        }

        protected override float GetYGround()
        {
            if (IsJumping)
                return PositionFromGroundWhileJumping.Y;

            return base.GetYGround();
        }
    }
}