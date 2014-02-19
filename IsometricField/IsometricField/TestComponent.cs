/* Challenge 5.5. Implement an isometric tiled graphics program. */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace IsometricField
{
    public class TestComponent : ITestComponent
    {
        private const int MapWidth = 20;
        private const int MapHeight = 20;
        private readonly MainGame _game;

        private Sprite _grass1;
        private Sprite _grass2;
        private MapPoint[,] _map;

        private Vector2 _cameraOffset;

        public TestComponent(MainGame game)
        {
            _game = game;
            _grass1 = new Sprite();
            _grass2 = new Sprite();
        }

        public void Initialize()
        {
            _map = new MapPoint[MapWidth, MapHeight];

            var viewport = _game.GraphicsDevice.Viewport;
            _cameraOffset = new Vector2(viewport.Width / 2, viewport.Height / 2);
        }

        public void LoadContent(ContentManager content)
        {
            var fileName = "grassland_tiles";
            _grass1.LoadContent(content, fileName, new Rectangle(0, 0, 64, 32));
            _grass2.LoadContent(content, fileName, new Rectangle(0, 32, 64, 32));

            for (int i = 0; i < MapHeight; i++)
            {
                var initial = _cameraOffset.X - i * 32;
                var grass = i % 2 == 0 ? _grass1 : _grass2;

                for (int j = 0; j < MapWidth; j++)
                {
                    var x = initial + j * 32;
                    var y = 100 + (i + j) * 16;

                    _map[j, i] = new MapPoint(grass, new Vector2(x, y));
                }
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapHeight; j++)
                {
                    _map[i, j].Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }
    }

    public struct MapPoint
    {
        private Sprite _sprite;
        private Vector2 _position;

        public MapPoint(Sprite sprite, Vector2 position)
        {
            _sprite = sprite;
            _position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, _position);
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

        public void LoadContent(ContentManager content, string fileName, Rectangle location, Vector2? origin = null)
        {
            _texture = content.Load<Texture2D>(fileName);
            _location = location;
            _origin = origin ?? new Vector2(location.Width / 2, location.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation = 0, float scale = 1)
        {
            spriteBatch.Draw(_texture, position, _location, _color, rotation, _origin, scale, SpriteEffects.None, 1);
        }
    }
}
