using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace DepthScaleWithJump
{
    public class RunnerControllTracker
    {
        private readonly Runner _runner;
        private readonly Joystick _joystick;
        private const float VelocityDelay = 0.97f;

        public RunnerControllTracker(Runner runner, Joystick joystick)
        {
            _runner = runner;
            _joystick = joystick;
        }

        public void Update(GameTime gameTime)
        {
            _runner.IsRunning = false;

            TrackUpAndDown();
            TrackLeftAndRight();
            TrackJump(gameTime);
        }

        private void TrackJump(GameTime gameTime)
        {
            if (_runner.IsJumping)
            {
                const int gravity = 32;
                _runner.Velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_runner.Position.Y >= _runner.PositionFromGroundWhileJumping.Y)
                {
                    _runner.IsJumping = false;
                    _runner.Velocity.Y = 0;
                }
            }
            else if (_joystick.IsJumpPressing)
            {
                _runner.Velocity.Y = -13;
                _runner.IsJumping = true;
                _runner.PositionFromGroundWhileJumping = _runner.Position;
            }
        }

        private void TrackLeftAndRight()
        {
            if (_joystick.IsLeftPressing)
            {
                _runner.IsRunning = true;
                _runner.Effects = SpriteEffects.FlipHorizontally;
                _runner.Velocity.X -= 10;
            }
            else if (_joystick.IsRightPressing)
            {
                _runner.IsRunning = true;
                _runner.Effects = SpriteEffects.None;
                _runner.Velocity.X += 10;
            }
            else
            {
                _runner.Velocity.X *= VelocityDelay;
            }
        }

        private void TrackUpAndDown()
        {
            if (_runner.IsJumping) return;

            if (_joystick.IsUpPressing)
            {
                _runner.IsRunning = true;
                _runner.Velocity.Y -= 10;
            }
            else if (_joystick.IsDownPressing)
            {
                _runner.IsRunning = true;
                _runner.Velocity.Y += 10;
            }
            else
            {
                _runner.Velocity.Y *= VelocityDelay;
            }
        }
    }
}
