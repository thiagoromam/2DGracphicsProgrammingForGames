/* Challenge 5.1. Add a jumping feature to the camera example in Section
5.1. Implement the two options for y-axis camera movement (track the
player and don't track the player), allowing the user to toggle the y-axis
camera movement during runtime. Add an arrow to track players when
they are off-screen. */

using Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CameraOnPlayerWithYAndEnemyTracking
{
    public class TestComponent : ITestComponent
    {
        private readonly MainGame _game;

        // Runner
        private Texture2D _runCycleTexture;
        private Rectangle _currentCelLocation;
        private int _numberOfCels;
        private int _currentCel;
        private int _msPerCel;
        public int _msUntilNextCel;
        private SpriteEffects _effects;
        private Vector2 _origin;
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _maxVelocity;
        private bool _isJumping;

        // Snowman
        private Texture2D _snowmanTexture;
        private Rectangle _snowmanCelLocation;
        private Vector2 _snowmanOrigin;
        private Vector2[] _snowmenPositions;

        private Texture2D _enemyTexture;
        private Rectangle _enemyCelLocation;
        private Vector2 _enemyOrigin;
        private float _halfEnemyOriginLength;
        private Vector2 _enemyPosition;

        // Arrow
        private Texture2D _arrowTexture;
        private Vector2 _arrowOrigin;
        private Vector2 _arrowPosition;
        private float _arrowRotation;
        private bool _showArrow;
        private bool _alwaysShowArrow;

        // Camera
        private Vector2 _cameraPosition;
        private Vector2 _cameraOffset;
        private float _cameraOffsetLength;
        private bool _trackYAxis;

        public TestComponent(MainGame game)
        {
            _game = game;
        }

        public void Initialize()
        {
            // Runner
            _numberOfCels = 12;
            _currentCel = 0;
            _msPerCel = 50;
            _msUntilNextCel = _msPerCel;
            _currentCelLocation = new Rectangle(0, 0, 128, 128);
            _effects = SpriteEffects.None;
            _position = new Vector2(100, 100);
            _velocity = new Vector2(0, 0);
            _maxVelocity = new Vector2(5, 0);
            _origin = new Vector2(64, 64);

            // Snowman
            _snowmanCelLocation = new Rectangle(0, 128, 256, 256);
            _snowmanOrigin = new Vector2(128, 128);
            _snowmenPositions = new Vector2[10];
            for (int i = 0; i < 10; i++)
                _snowmenPositions[i] = new Vector2(200 * i, 250);

            // Enemy
            _enemyCelLocation = _snowmanCelLocation;
            _enemyOrigin = _snowmanOrigin;
            _halfEnemyOriginLength = _enemyOrigin.Length() / 2;
            _enemyPosition = new Vector2(2000, 100);

            // Arrow
            _arrowOrigin = new Vector2(16, 16);
            _arrowRotation = 0;

            // Camera
            var viewport = _game.GraphicsDevice.Viewport;
            _cameraOffset = new Vector2(viewport.Width / 2, viewport.Height / 2); // half the screen size
            _cameraOffsetLength = _cameraOffset.Length();
            _trackYAxis = true;
            _cameraPosition = _position;
        }

        public void LoadContent(ContentManager content)
        {
            // Runner
            _runCycleTexture = content.Load<Texture2D>("run_cycle");

            // Snowman
            _snowmanTexture = content.Load<Texture2D>("snow_assets");

            // Enemy
            _enemyTexture = _snowmanTexture;

            // Arrow
            _arrowTexture = content.Load<Texture2D>("Arrow");
        }

        public void Update(GameTime gameTime)
        {
            // Runner
            if (Joystick.Player1.IsLeftPressing)
            {
                if (_velocity.X > -_maxVelocity.X)
                    _velocity.X -= 0.2f;

                _effects = SpriteEffects.FlipHorizontally;
            }
            else if (Joystick.Player1.IsRightPressing)
            {
                if (_velocity.X < _maxVelocity.X)
                    _velocity.X += 0.2f;

                _effects = SpriteEffects.None;
            }
            else
            {
                _velocity.X *= 0.97f;
            }


            if (_isJumping)
            {
                if (_position.Y == 100)
                {
                    _velocity.Y = 0;
                    _isJumping = false;
                }
                else
                {
                    _velocity.Y += 28 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    var excess = _position.Y + _velocity.Y - 100;
                    if (excess > 0)
                        _velocity.Y -= excess;
                }
            }

            if (!_isJumping && Joystick.Player1.IsJumpPressing)
            {
                _isJumping = true;
                _velocity.Y = -13;
            }

            _position += _velocity;

            _msUntilNextCel -= gameTime.ElapsedGameTime.Milliseconds;
            var relativeVelocity = Math.Abs(_velocity.X / _maxVelocity.X);
            if (relativeVelocity > 0.05f)
            {
                var mustUpdate = _msUntilNextCel <= 0;

                if (mustUpdate)
                    _currentCel++;

                if (_currentCel >= _numberOfCels)
                    _currentCel = 0;

                _currentCelLocation.X = _currentCelLocation.Width * _currentCel;

                if (mustUpdate)
                    _msUntilNextCel = (int)(_msPerCel * (2f - relativeVelocity));
            }

            if (Joystick.Player1.IsFire2Pressed)
                _alwaysShowArrow = !_alwaysShowArrow;

            // Enemy
            var enemyDirection = _enemyPosition - _position;
            if (_alwaysShowArrow || enemyDirection.Length() + _halfEnemyOriginLength > _cameraOffsetLength)
            {
                _showArrow = true;
                enemyDirection.Normalize();
                _arrowRotation = (float)Math.Atan2(enemyDirection.Y, enemyDirection.X);
                _arrowPosition = _position + enemyDirection * 70;
            }
            else
            {
                _showArrow = false;
            }

            if (Joystick.Player1.IsFirePressed)
                _trackYAxis = !_trackYAxis;

            // Camera
            const float cameraVelocity = 0.05f;
            var playerPositionFromCamera = _position;

            if (!_trackYAxis)
            {
                if (_cameraPosition.Y < 100)
                {
                    _cameraPosition.Y += 5;
                }
                else
                {
                    _cameraPosition.Y = 100;
                }

                playerPositionFromCamera.Y = _cameraPosition.Y;
            }
            var cameraDirection = playerPositionFromCamera - _cameraPosition;

            if (cameraDirection != Vector2.Zero)
            {
                var directionLength = cameraDirection.Length();
                cameraDirection.Normalize();
                cameraDirection *= directionLength * cameraVelocity;

                if (cameraDirection.Length() > directionLength)
                    _cameraPosition = playerPositionFromCamera;
                else
                    _cameraPosition += cameraDirection;
            }

            if (_cameraPosition.X > 1800)
                _cameraPosition.X = 1800;
            if (_cameraPosition.X < 0)
                _cameraPosition.X = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var drawLocation = _cameraOffset - _cameraPosition;

            spriteBatch.Begin();

            // Runner
            spriteBatch.Draw(_runCycleTexture, drawLocation + _position, _currentCelLocation, Color.White, 0, _origin, 1, _effects, 1);

            // Enemy
            spriteBatch.Draw(_enemyTexture, drawLocation + _enemyPosition, _enemyCelLocation, Color.Gray, 0, _enemyOrigin, 1, SpriteEffects.None, 1);

            // Arrow
            if (_showArrow)
                spriteBatch.Draw(_arrowTexture, drawLocation + _arrowPosition, null, Color.White, _arrowRotation, _arrowOrigin, 1, SpriteEffects.None, 1);

            // Snowman
            for (int i = 0; i < 10; i++)
                spriteBatch.Draw(_snowmanTexture, drawLocation + _snowmenPositions[i], _snowmanCelLocation, Color.White, 0, _snowmanOrigin, 1, SpriteEffects.None, 1);

            spriteBatch.End();
        }
    }
}