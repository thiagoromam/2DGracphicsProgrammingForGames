using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using Color = Microsoft.Xna.Framework.Color;

// ReSharper disable once CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
namespace Core
{
    public class TestComponent
    {
        private readonly MainGame _game;
        private int _screenWidth;
        private int _screenHeight;
        private Texture2D _snowAssets;
        private Rectangle _snowmanSource;
        private Vector2 _snowmanOrigin;
        private Vector2[] _positions;
        private Vector2 _centerOfScreen;

        public TestComponent(MainGame game)
        {
            _game = game;
            _game.Graphics.IsFullScreen = false;
        }

        public void Initialize()
        {
            var viewport = _game.GraphicsDevice.Viewport;
            _screenWidth = viewport.Width;
            _screenHeight = viewport.Height;
            _centerOfScreen = viewport.CalculateCenterOfScreen();
        }

        public void LoadContent(ContentManager content)
        {
            _snowAssets = content.Load<Texture2D>("snow_assets");
            _snowmanSource = new Rectangle(0, 128, 256, 256);
            _snowmanOrigin = new Vector2(_snowmanSource.Width / 2f, _snowmanSource.Height / 2f);

            _positions = new Vector2[9];
            var offset = new Vector2(_centerOfScreen.X - 500, _centerOfScreen.Y - 250);
            for (var i = 0; i < 3; i++)
            {
                var x = offset.X + i * 500;
                for (var j = 0; j < 3; j++)
                {
                    var y = offset.Y + j * 250;

                    _positions[i + (3 * j)] = new Vector2(x, y);
                }
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var tempBinding = _game.GraphicsDevice.GetRenderTargets();
            var tempRenderTarget = new RenderTarget2D(_game.GraphicsDevice, _screenWidth, _screenHeight);

            _game.GraphicsDevice.SetRenderTarget(tempRenderTarget);

            _game.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            for (var i = 0; i < _positions.Length; ++i)
            {
                spriteBatch.Draw(_snowAssets, _positions[i], _snowmanSource, Color.White, 0, _snowmanOrigin, 1, SpriteEffects.None, 1);
            }
            spriteBatch.End();

            _game.GraphicsDevice.SetRenderTargets(tempBinding);

            var colors = new Color[_screenWidth * _screenHeight];
            var maxColorsIndex = colors.Length - 1;
            tempRenderTarget.GetData(colors);

            for (var i = 0; i < _screenWidth; i++)
            {
                for (var j = 0; j < _screenHeight; j++)
                {
                    const int blurAmount = 5;

                    var index = i + (_screenWidth * j);
                    var previousIndex = index - blurAmount;
                    var nextIndex = index + blurAmount;

                    if (previousIndex < 0 || nextIndex > maxColorsIndex) continue;

                    var currentElement = colors[index];
                    var previousElement = colors[previousIndex];
                    var nextElement = colors[nextIndex];

                    colors[index].R = (byte)((previousElement.R + currentElement.R + nextElement.R) / 3.0f);
                    colors[index].G = (byte)((previousElement.G + currentElement.G + nextElement.G) / 3.0f);
                    colors[index].B = (byte)((previousElement.B + currentElement.B + nextElement.B) / 3.0f);
                }
            }

            var texture = new Texture2D(_game.GraphicsDevice, _screenWidth, _screenHeight);
            texture.SetData(colors);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}