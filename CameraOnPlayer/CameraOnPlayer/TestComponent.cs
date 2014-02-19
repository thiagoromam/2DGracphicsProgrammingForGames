using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using System;

namespace CameraOnPlayer
{
    public class TestComponent : ITestComponent
    {
        private MainGame _game;

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

        // Snowman
        private Texture2D _snowmanTexture;
        private Rectangle _snowmanCelLocation;
        private Vector2 _snowmanOrigin;
        private Vector2[] _snowmenPositions = new Vector2[10];

        // Camera
        private Vector2 _cameraPosition;
        private Vector2 _cameraOffset;
        
        // Scale
        private float _zoomLevel;

        public TestComponent(MainGame mainGame)
        {
            _game = mainGame;
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
            for (int i = 0; i < 10; i++)
                _snowmenPositions[i] = new Vector2(200 * i, 200);

            // Camera
            var viewport = _game.GraphicsDevice.Viewport;
            _cameraOffset = new Vector2(viewport.Width / 2, viewport.Height / 2); // half the screen size
            _cameraPosition = _position;

            // Scale
            _zoomLevel = 1;
        }

        public void LoadContent(ContentManager content)
        {
            // Runner
            _runCycleTexture = content.Load<Texture2D>("run_cycle");

            // Snowman
            _snowmanTexture = content.Load<Texture2D>("snow_assets");
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
                _velocity *= 0.97f;
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

            // Camera Delay
            const float MULTIPLIER = 0.05f;
            if (_cameraPosition.X < _position.X)
            {
                _cameraPosition.X -=
                ((_cameraPosition.X - _position.X) * MULTIPLIER);
            }
            else if (_cameraPosition.X > _position.X)
            {
                _cameraPosition.X
                += ((_cameraPosition.X - _position.X) * -MULTIPLIER);
            }

            if (Joystick.Player1.IsUpPressing)
            {
                _zoomLevel += 0.01f;
            }
            else if (Joystick.Player1.IsDownPressing)
            {
                _zoomLevel -= 0.01f;
            }

            if (_cameraPosition.X > 1300)
                _cameraPosition.X = 1300;
            if (_cameraPosition.X < 0)
                _cameraPosition.X = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var drawLocation = _cameraOffset / _zoomLevel - _cameraPosition;

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                null,
                Matrix.CreateScale(_zoomLevel)
            );

            // Runner
            spriteBatch.Draw(_runCycleTexture, drawLocation + _position, _currentCelLocation, Color.White, 0, _origin, 1, _effects, 1);

            // Snowman
            for (int i = 0; i < 10; i++)
            {
                spriteBatch.Draw(_snowmanTexture, drawLocation + _snowmenPositions[i], _snowmanCelLocation, Color.White, 0, _snowmanOrigin, 1, SpriteEffects.None, 1);
            }

            spriteBatch.End();
        }
    }
}
