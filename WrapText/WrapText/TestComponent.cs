/* Challenge 7.2. Create and implement a function that will automatically
wrap lines in a text string given a specic desired maximum display width. */

/* Challenge 7.3. Expand on the implementation of Challenge 7.2 by adding
a feature that will automatically scroll text when the text string exceeds a
maximum screen height. */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using WrapText;

// ReSharper disable once CheckNamespace
namespace Core
{
    public class TestComponent
    {
        private readonly MainGame _game;
        private ClipSpriteBatch _clipSpriteBatch;

        // narrative box
        private Texture2D _narrativeBox;
        private Vector2 _narrativeBoxOrigin;
        private Vector2 _narrativeBoxPosition;

        // text
        private SpriteFont _segoe;
        private string _text;
        private Vector2 _textPosition;
        private Vector2 _textOrigin;
        private readonly Joystick _joystick;

        public TestComponent(MainGame game)
        {
            _game = game;
            _joystick = Joystick.Player1;
        }

        public void Initialize()
        {
        }

        public void LoadContent(ContentManager content)
        {
            _segoe = content.Load<SpriteFont>("Segoe");
            _text =
                "Challenge 7.2. Create and implement a function that will automatically wrap lines in a text string given a specic desired maximum display width." +
                "\n\nChallenge 7.3. Expand on the implementation of Challenge 7.2 by adding a feature that will automatically scroll text when the text string exceeds a maximum screen height.";
            _text = _segoe.WrapText(_text, 535);
            _textOrigin = _segoe.CalculateCenterOfText(_text);
            _textOrigin.X = 535 / 2;

            var viewport = _game.GraphicsDevice.Viewport;

            _narrativeBox = content.Load<Texture2D>("NarrativeBox");
            _narrativeBoxOrigin = _narrativeBox.CalculateCenter();
            _narrativeBoxPosition = new Vector2(viewport.Width / 2f, viewport.Height - 20 - _narrativeBoxOrigin.Y);

            var textArea = new Rectangle(
                (int)(_narrativeBoxPosition.X - _narrativeBoxOrigin.X) + 6,
                (int)(_narrativeBoxPosition.Y - _narrativeBoxOrigin.Y) + 6,
                _narrativeBox.Width - 12,
                _narrativeBox.Height - 12
            );

            _clipSpriteBatch = new ClipSpriteBatch(textArea, _game.SpriteBatch);

            _textPosition = _narrativeBoxPosition;
            _textPosition.Y += _textOrigin.Y - _narrativeBoxOrigin.Y;
        }

        public void Update(GameTime gameTime)
        {
            const float velocity = 0.7f;

            if (_joystick.IsUpPressing)
                _textPosition.Y += velocity;
            else if (_joystick.IsDownPressing)
                _textPosition.Y -= velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_narrativeBox, _narrativeBoxPosition, null, Color.White, 0, _narrativeBoxOrigin, 1, SpriteEffects.None, 1);
            spriteBatch.End();

            _clipSpriteBatch.Begin();
            spriteBatch.DrawString(_segoe, _text, _textPosition, Color.Black, 0, _textOrigin, 1, SpriteEffects.None, 1);
            _clipSpriteBatch.End();
        }
    }
}