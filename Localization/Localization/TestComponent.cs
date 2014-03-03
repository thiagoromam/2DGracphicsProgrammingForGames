/* Challenge 7.1. Implement a second language. Load the languages from a
file and allow the user to swap between languages. (If you're not bilingual,
use Google Translate or a similar tool to test localization within your game
system. Of course you'll want to nd a better solution before you ship your
game.) */

using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

// ReSharper disable once CheckNamespace
namespace Core
{
    public class TestComponent
    {
        private ResourceLoader _resource;
		private readonly Game _game;
        private readonly Joystick _joystick;
        private SpriteFont _segoe;
        private string _text;
        private Vector2 _origin;
        private Vector2 _center;
        private string[] _cultures;
        private int _currentCultureIndex;

        public TestComponent(Game game)
		{
			_game = game;
            _joystick = Joystick.Player1;
		}
	
        public void Initialize()
        {
            _cultures = new [] { "en-US", "pt-BR" };
        }

		public void LoadContent(ContentManager content)
        {
            _segoe = content.Load<SpriteFont>("Segoe");
            _resource = new ResourceLoader("Messages");

            _center = _game.GraphicsDevice.Viewport.CalculateCenterOfScreen();

            UpdateLocaleResources();
        }

        public void Update(GameTime gameTime)
		{
            if (_joystick.IsFirePressed)
            {
                _currentCultureIndex++;

                if (_currentCultureIndex == _cultures.Length)
                    _currentCultureIndex = 0;

                UpdateLocaleResources();
            }
		}

		public void Draw(SpriteBatch spriteBatch)
		{
		    spriteBatch.Begin();
            spriteBatch.DrawString(_segoe, _text, _center, Color.White, 0, _origin, 1, SpriteEffects.None, 1);
            spriteBatch.End();
		}


        private void UpdateLocaleResources()
        {
            ApplicationLanguages.PrimaryLanguageOverride = _cultures[_currentCultureIndex];
            _text = _resource.GetString("HelloWorld");
            _origin = _segoe.CalculateCenterOfText(_text);
        }
    }
}