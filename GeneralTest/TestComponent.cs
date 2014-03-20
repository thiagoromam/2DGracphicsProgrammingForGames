using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

// ReSharper disable once CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
namespace Core
{
    public class TestComponent
    {
        private readonly MainGame _game;
        private Texture2D _texture;
        private Texture2D _snowman;
        private Texture2D _snowmanInverted;

        private Vector2 _position;
        private Vector2 _velocity;

        public TestComponent(MainGame game)
        {
            _game = game;
            _game.Graphics.IsFullScreen = false;
        }

        public void Initialize()
        {
            _position = Vector2.Zero;
            _velocity = new Vector2(1, 1.5f);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("snow_assets");

            var source = new Rectangle(0, 128, 256, 256);
            var colors = new Color[source.Width * source.Height];
            _texture.GetData(0, source, colors, 0, (source.Width * source.Height));

            _snowman = new Texture2D(_game.GraphicsDevice, source.Width, source.Height);
            _snowmanInverted = new Texture2D(_game.GraphicsDevice, source.Width, source.Height);

            _snowman.SetData(colors);
            _snowmanInverted.SetData(colors);
        }

        public void Update(GameTime gameTime)
        {
            UpdatePosition();
            UpdatePixel();
        }

        private void UpdatePosition()
        {
            _position += _velocity;

            if (_position.X < 0 || _position.X > 255)
                _velocity.X *= -1;
            if (_position.Y < 0 || _position.Y > 255)
                _velocity.Y *= -1;

            _position.X = MathHelper.Clamp(_position.X, 0, 255);
            _position.Y = MathHelper.Clamp(_position.Y, 0, 255);
        }

        private void UpdatePixel()
        {
            const int width = 256;
            const int height = 256;

            var colors = new Color[width * height];
            _snowman.GetData(colors);

            for (var i = 0; i < width; ++i)
            {
                for (var j = 0; j < height; ++j)
                {
                    var index = j + (width * i);
                    var distance = Math.Sqrt(Math.Pow(i - _position.X, 2) + Math.Pow(j - _position.Y, 2));
                    const double radius = 50;

                    if (distance < radius)
                    {
                        colors[index].R = (byte)(255 -colors[index].R);
                        colors[index].G = (byte)(255 -colors[index].G);
                        colors[index].B = (byte)(255 -colors[index].B);
                        colors[index].A = 255;
                    }
                }
            }

            _snowmanInverted.SetData(colors);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            spriteBatch.Draw(_snowman, Vector2.Zero, Color.White);
            spriteBatch.Draw(_snowmanInverted, new Vector2(256, 0), Color.White);
            spriteBatch.End();
        }
    }
}