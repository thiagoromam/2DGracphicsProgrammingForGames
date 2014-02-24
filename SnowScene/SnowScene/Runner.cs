using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Resources;

namespace SnowScene
{
    public class Runner : IGameElement, IFisicalObject
    {
        private readonly RunnerAnimation _animation;

        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _maxVelocity;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public float Width
        {
            get { return _animation.CurrentCelLocation.Width; }
        }
        public float Height
        {
            get { return _animation.CurrentCelLocation.Height; }
        }
        public bool GoingLeft
        {
            get { return _animation.TurnLeft; }
        }

        public Runner()
        {
            _animation = new RunnerAnimation();
        }

        public void Initialize()
        {
            _animation.Initialize();
            _position = new Vector2(100, 300);
            _velocity = new Vector2(0, 0);
            _maxVelocity = new Vector2(5, 0);
            _animation.TurnLeft = false;
        }

        public void LoadContent(ContentManager content)
        {
            _animation.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            if (Joystick.Player1.IsLeftPressing)
            {
                if (_velocity.X > -_maxVelocity.X)
                    _velocity.X -= 0.2f;

                _animation.TurnLeft = true;
            }
            else if (Joystick.Player1.IsRightPressing)
            {
                if (_velocity.X < _maxVelocity.X)
                    _velocity.X += 0.2f;

                _animation.TurnLeft = false;
            }
            else
            {
                _velocity *= 0.97f;
            }

            _position += _velocity;

            _animation.UpdateMsUntilNextCel(gameTime);
            var relativeVelocity = Math.Abs(_velocity.X / _maxVelocity.X);
            if (relativeVelocity > 0.05f && _animation.UpdateCurrentCel())
                _animation.MsUntilNextCel = (int)(_animation.MsPerCel * (2f - relativeVelocity));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _animation.Texture,
                _position,
                _animation.CurrentCelLocation,
                Color.White,
                0,
                Vector2.Zero,
                1,
                _animation.Effects,
                1
            );
        }
    }
}
