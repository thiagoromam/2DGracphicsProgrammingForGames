/* Challenge 5.2. Complete the tiled program example in Section 5.3, implementing
zoom controls and adding your own set of tiled graphical sprites to
represent other terrain types. As discussed at the very end of that section,
implement layered sprites so that vegetation can be mapped over terrain. */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace MapWithFogOfWar
{
    public class TestComponent : ITestComponent
    {
        private const int ScreenWidth = 1280;
        private const int ScreenHeight = 720;
        private const int SpriteWidth = 32;
        private const int SpriteHeight = 32;
        private const int MapWidth = 256;
        private const int MapHeight = 256;

        private readonly MainGame _game;

        private Sprite _player;
        private Vector2 _playerMapPosition;
        private Vector2 _playerScreenPosition;
        private float _playerForwardVelocity;
        private float _playerMaxVelocity;
        private float _playerRotation;

        private Sprite _plains;
        private Sprite _mountains;
        private Sprite _hills;
        private Sprite _water;
        private Sprite _sight;
        private Texture2D _mapTexture;
        private Color[] _gameMap;
        private int _mapXStart;
        private int _mapXEnd;
        private int _mapYStart;
        private int _mapYEnd;
        private bool[] _lineOfSight;
        private int _distanceView;

        private Vector2 _cameraPosition;
        private Vector2 _cameraOffset;
        private float _zoomLevel;
        private int _xOffset;
        private int _yOffset;

        public TestComponent(MainGame game)
        {
            _game = game;
            _game.Graphics.PreferredBackBufferWidth = ScreenWidth;
            _game.Graphics.PreferredBackBufferHeight = ScreenHeight;

            _plains = new Sprite();
            _mountains = new Sprite();
            _hills = new Sprite();
            _water = new Sprite();
            _sight = new Sprite();
            _player = new Sprite();
        }

        public void Initialize()
        {
            _gameMap = new Color[MapWidth * MapHeight];

            var viewport = _game.GraphicsDevice.Viewport;
            _cameraOffset = new Vector2(viewport.Width / 2, viewport.Height / 2);
            _playerMaxVelocity = 30;

            _distanceView = 10;
            _zoomLevel = 1;
        }

        public void LoadContent(ContentManager content)
        {
            var spritesOrigin = new Vector2(SpriteWidth / 2, SpriteHeight / 2);
            _plains.LoadContent(content, "tiledSprites", new Rectangle(32, 0, SpriteWidth, SpriteHeight), spritesOrigin);
            _mountains.LoadContent(content, "tiledSprites", new Rectangle(0, 32, SpriteWidth, SpriteHeight), spritesOrigin);
            _hills.LoadContent(content, "tiledSprites", new Rectangle(64, 0, SpriteWidth, SpriteHeight), spritesOrigin);
            _water.LoadContent(content, "tiledSprites", new Rectangle(32, 32, SpriteWidth, SpriteHeight), spritesOrigin);
            _sight.LoadContent(content, "tiledSprites", new Rectangle(96, 0, SpriteWidth, SpriteHeight), spritesOrigin);
            _player.LoadContent(content, "tiledSprites", new Rectangle(64, 32, SpriteWidth, SpriteHeight), spritesOrigin);

            _mapTexture = content.Load<Texture2D>("map01");
            _mapTexture.GetData<Color>(_gameMap);

            _lineOfSight = new bool[_gameMap.Length];
        }

        public void Update(GameTime gameTime)
        {
            if (Joystick.Player1.IsLeftPressing)
                _playerRotation -= 1.5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (Joystick.Player1.IsRightPressing)
                _playerRotation += 1.5f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Joystick.Player1.IsUpPressing)
            {
                if (_playerForwardVelocity <= _playerMaxVelocity)
                    _playerForwardVelocity += 0.3f;
            }
            else
            {
                _playerForwardVelocity *= 0.95f;
            }

            _playerMapPosition.X += (float)(_playerForwardVelocity * Math.Cos(_playerRotation) * gameTime.ElapsedGameTime.TotalSeconds);
            _playerMapPosition.Y += (float)(_playerForwardVelocity * Math.Sin(_playerRotation) * gameTime.ElapsedGameTime.TotalSeconds);

            _playerScreenPosition.X = _playerMapPosition.X * SpriteWidth;
            _playerScreenPosition.Y = _playerMapPosition.Y * SpriteHeight;

            if (Joystick.Player1.IsSumPressing)
            {
                if (_zoomLevel < 1)
                    _zoomLevel += 0.01f;
            }
            else if (Joystick.Player1.IsMinusPressing)
            {
                if (_zoomLevel > 0)
                    _zoomLevel -= 0.01f;
            }

            _cameraPosition = _playerScreenPosition;

            _xOffset = (int)(23 / _zoomLevel);
            _yOffset = (int)(13 / _zoomLevel);

            _mapXStart = Math.Max((int)(_playerMapPosition.X - _xOffset), 0);
            _mapXEnd = Math.Min((int)(_playerMapPosition.X + _xOffset), MapWidth);
            _mapYStart = Math.Max((int)(_playerMapPosition.Y - _yOffset), 0);
            _mapYEnd = Math.Min((int)(_playerMapPosition.Y + _yOffset), MapHeight);

            var viewXStart = Math.Max((int)(_playerMapPosition.X - _distanceView), 0);
            var viewXEnd = Math.Min((int)(_playerMapPosition.X + _distanceView), MapWidth);
            var viewYStart = Math.Max((int)(_playerMapPosition.Y - _distanceView), 0);
            var viewYEnd = Math.Min((int)(_playerMapPosition.Y + _distanceView), MapHeight);

            for (int i = viewXStart; i < viewXEnd; i++)
            {
                for (int j = viewYStart; j < viewYEnd; j++)
                {
                    var index = i + j * MapHeight;

                    if (!IsMountain(ref _gameMap[index]))
                        _lineOfSight[index] = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var drawLocation = (_cameraOffset / _zoomLevel) - _cameraPosition;

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                null,
                Matrix.CreateScale(_zoomLevel)
            );

            Vector2 screenLocation;
            Color mapColor;

            for (int i = _mapXStart; i < _mapXEnd; i++)
            {
                for (int j = _mapYStart; j < _mapYEnd; j++)
                {
                    var index = i + j * MapHeight;

                    screenLocation = new Vector2(i * SpriteWidth, j * SpriteWidth);
                    mapColor = _gameMap[index];

                    if (!_lineOfSight[index])
                        _sight.Draw(spriteBatch, screenLocation + drawLocation);
                    else if (IsMountain(ref mapColor))
                        _mountains.Draw(spriteBatch, screenLocation + drawLocation);
                    else if (IsHill(ref mapColor))
                        _hills.Draw(spriteBatch, screenLocation + drawLocation);
                    else if (IsPlain(ref mapColor))
                        _plains.Draw(spriteBatch, screenLocation + drawLocation);
                    else if (IsWater(ref mapColor))
                        _water.Draw(spriteBatch, screenLocation + drawLocation);
                }
            }

            _player.Draw(spriteBatch, _playerScreenPosition + drawLocation, _playerRotation);
            spriteBatch.End();
        }

        private static bool IsWater(ref Color mapColor)
        {
            return mapColor.B == 255;
        }
        private static bool IsPlain(ref Color mapColor)
        {
            return mapColor.G == 255;
        }
        private static bool IsHill(ref Color mapColor)
        {
            return mapColor.R == 128;
        }
        private static bool IsMountain(ref Color mapColor)
        {
            return mapColor.R == 255;
        }
    }

    public class Sprite
    {
        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _origin;
        private Color _color;

        public Sprite()
        {
            _color = Color.White;
        }

        public void LoadContent(ContentManager content, string fileName, Rectangle location, Vector2 origin)
        {
            _texture = content.Load<Texture2D>(fileName);
            _location = location;
            _origin = origin;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation = 0, float scale = 1)
        {
            spriteBatch.Draw(_texture, position, _location, _color, rotation, _origin, scale, SpriteEffects.None, 1);
        }
    }
}