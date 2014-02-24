using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace SnowScene
{
    public class TesteComponent : ITestComponent
    {
        private Texture2D _background;
        private readonly Runner _player;
        private readonly Enemy _enemy;
        private readonly Snowball _ball;
        private Vector2 _backgroundPosition;
        private bool _ballThrowed;
        private float _enemyDelay;

        public TesteComponent()
        {
            _player = new Runner();
            _enemy = new Enemy();
            _ball = new Snowball();
        }

        public void Initialize()
        {
            _backgroundPosition = Vector2.Zero;
            _player.Initialize();
            _enemy.Initialize();
            _ball.Initialize();
        }

        public void LoadContent(ContentManager content)
        {
            _background = content.Load<Texture2D>("snow_bg");
            _player.LoadContent(content);
            _enemy.LoadContent(content);
            _ball.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);

            if (Joystick.Player1.IsFirePressed)
            {
                var ballPosition = _player.Position;

                if (!_player.GoingLeft)
                    ballPosition.X += 70;

                _ball.ThrowToLeft = _player.GoingLeft;
                ballPosition.Y += (_player.Height - _ball.Height) / 2;

                _ball.Position = ballPosition;
                _ballThrowed = true;
            }

            if (_ballThrowed)
            {
                _ball.Update(gameTime);

                bool intersects;
                if (_ball.ThrowToLeft)
                {
                    intersects = _ball.Position.X <= _enemy.Position.X + _enemy.Width - (_ball.Width / 4);
                }
                else
                {
                    intersects = _ball.Position.X + (_ball.Width / 4) >= _enemy.Position.X;
                }

                if (intersects)
                {
                    _ballThrowed = false;
                    _enemyDelay = 3;
                    _ball.Reset();
                }
            }

            if (_enemyDelay > 0)
            {
                _enemyDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                _enemy.Velocity /= 2;
            }

            _enemy.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_background, _backgroundPosition, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
            _player.Draw(spriteBatch);
            _enemy.Draw(spriteBatch);

            if (_ballThrowed)
            {
                _ball.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}