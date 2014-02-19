using Microsoft.Xna.Framework;

namespace PrinciplesOfDepth
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

            if (IsRunning && _msUntilNextCel <= 0)
            {
                _currentCel++;
                _msUntilNextCel = _msPerCel;
            }

            if (_currentCel >= _numberOfCels)
                _currentCel = 0;

            var location = Location.Value;
            location.X = location.Width * _currentCel;
            Location = location;
        }

        private void UpdatePosition(GameTime gameTime)
        {
            Velocity *= 0.95f;
            Velocity.X = MathHelper.Clamp(Velocity.X, -_maxVelocity, _maxVelocity);
            Velocity.Y = MathHelper.Clamp(Velocity.Y, -_maxVelocity, _maxVelocity);

            var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position.X += Scale * Velocity.X * elapsedSeconds;
            Position.Y += Scale * Velocity.Y * elapsedSeconds;
            Position.Y = MathHelper.Clamp(Position.Y, TestComponent.Horizon, TestComponent.BufferHeight);
        }
    }
}