/* Challenge 4.4. Implement a graphics scene that makes use of follow-through
and overlapping action. */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace FollowThroughAndOverlapping
{
    public class TestComponent : ITestComponent
    {
        private readonly MainGame _game;
        private int _connectedSpritesCount;
        private Texture2D _texture;
        private Vector2[] _positions;
        private float[] _scales;
        private Vector2 _origin;

        public TestComponent(MainGame game)
        {
            _game = game;
        }

        public void Initialize()
        {
            var viewport = _game.GraphicsDevice.Viewport;

            _connectedSpritesCount = 10;
            _positions = new Vector2[_connectedSpritesCount];
            _scales = new float[_connectedSpritesCount];

            var startPosition = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            for (var i = 0; i < _connectedSpritesCount; i++)
                _positions[i] = startPosition;

            for (var i = 0; i < _connectedSpritesCount; i++)
                _scales[i] = 1 / (float)_connectedSpritesCount * (i + 1);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Rectangle");
            _origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
        }

        public void Update(GameTime gameTime)
        {
            for (var i = _connectedSpritesCount - 2; i >= 0; i--)
            {
                var direction = _positions[i + 1] - _positions[i];

                if (direction == Vector2.Zero) continue;

                var directionLength = direction.Length();
                direction.Normalize();
                direction *= i * (float)0.5;

                if (direction.Length() > directionLength)
                    _positions[i] = _positions[i + 1];
                else
                    _positions[i] += direction;
            }

            if (Joystick.Player1.IsLeftPressing)
                _positions[_connectedSpritesCount - 1].X -= 4;
            else if (Joystick.Player1.IsRightPressing)
                _positions[_connectedSpritesCount - 1].X += 4;

            if (Joystick.Player1.IsUpPressing)
                _positions[_connectedSpritesCount - 1].Y -= 4;
            else if (Joystick.Player1.IsDownPressing)
                _positions[_connectedSpritesCount - 1].Y += 4;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            for (var i = 0; i < _connectedSpritesCount; i++)
                spriteBatch.Draw(_texture, _positions[i], null, Color.White, 0, _origin, _scales[i], SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
