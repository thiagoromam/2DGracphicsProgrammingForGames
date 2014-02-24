using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace BouncingBall
{
    public class TestComponent : ITestComponent
    {
        private readonly MainGame _game;

        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _origin;
        private float _velocity;
        private float _floorY;

        public TestComponent(MainGame game)
        {
            _game = game;
        }

        public void Initialize()
        {
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Ball");
            var viewport = _game.GraphicsDevice.Viewport;
            _floorY = viewport.Height - _origin.Y;
            _origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
            _position = new Vector2(viewport.Width / 2f, _floorY);
        }

        public void Update(GameTime gameTime)
        {
            if (_position.Y + _origin.Y >= _floorY)
            {
                _velocity = -25;
            }

            _velocity += (float)gameTime.ElapsedGameTime.TotalSeconds * 32;
            _position.Y += _velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, _position, null, Color.White, 0, _origin, 1, SpriteEffects.None, 1);
            spriteBatch.End();
        }
    }
}