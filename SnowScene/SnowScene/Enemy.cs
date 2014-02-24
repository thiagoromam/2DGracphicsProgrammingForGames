using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Resources;

namespace SnowScene
{
    public class Enemy
    {
        private readonly RunnerAnimation _animation;

        private Vector2 _position;
        public Vector2 Velocity;
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

        public Enemy()
        {
            _animation = new RunnerAnimation();
        }

        public void Initialize()
        {
            _animation.Initialize();
            _position = new Vector2(1100, 300);
            Velocity = new Vector2(0, 0);
            _maxVelocity = new Vector2(5, 0);
        }

        public void LoadContent(ContentManager content)
        {
            _animation.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            if (Joystick.Player2.IsLeftPressing)
            {
                if (Velocity.X > -_maxVelocity.X)
                    Velocity.X -= 0.2f;

                _animation.TurnLeft = true;
            }
            else if (Joystick.Player2.IsRightPressing)
            {
                if (Velocity.X < _maxVelocity.X)
                    Velocity.X += 0.2f;

                _animation.TurnLeft = false;
            }
            else
            {
                Velocity *= 0.97f;
            }

            _position += Velocity;

            _animation.UpdateMsUntilNextCel(gameTime);
            var relativeVelocity = Math.Abs(Velocity.X / _maxVelocity.X);
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
