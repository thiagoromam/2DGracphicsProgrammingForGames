using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using ParallaxScaleYAxis;

// ReSharper disable once CheckNamespace
namespace Resources
{
    public class TestComponent
    {
        public const int BufferWidth = 1280;
        public const int BufferHeight = 720;
        public const int Horizon = 240;

        private readonly Sprite _background;
        private readonly Runner _runner;
        private readonly Runner _runner2;
        private Snowman[] _snowmen;

        public TestComponent(GameBase game)
        {
            game.Graphics.PreferredBackBufferWidth = BufferWidth;
            game.Graphics.PreferredBackBufferHeight = BufferHeight;

            _background = new Background();
            _runner = new Runner();
            _runner2 = new Runner();
        }

        public void Initialize()
        {
            _runner.Initialize();
            _runner.Position = new Vector2(400, 680);

            _runner2.Initialize();
            _runner2.Position = new Vector2(600, 400);

            _snowmen = new Snowman[36];
            var snowmenQuadrant = 6;
            var random = new Random();
            for (int i = 0; i < snowmenQuadrant; i++)
            {
                var y = Horizon + i * 80;

                for (int j = 0; j < snowmenQuadrant; j++)
                {
                    var x = 130 + j * 200;

                    var snowman = new Snowman();
                    snowman.Initialize();
                    snowman.Position = new Vector2(x, y);

                    _snowmen[snowmenQuadrant * i + j] = snowman;
                }
            }

            _background.Initialize();
        }

        public void LoadContent(ContentManager content)
        {
            _runner.LoadConent(content, "run_cycle");
            _runner2.LoadConent(content, "run_cycle");

            for (int i = 0; i < _snowmen.Length; i++)
                _snowmen[i].LoadConent(content, "snow_assets");

            _background.LoadConent(content, "background");
        }

        public void Update(GameTime gameTime)
        {
            _runner.IsRunning = false;

            if (Joystick.Player1.IsUpPressing)
            {
                _runner.IsRunning = true;
                _runner.Velocity.Y -= 10;
            }
            else if (Joystick.Player1.IsDownPressing)
            {
                _runner.IsRunning = true;
                _runner.Velocity.Y += 10;
            }

            if (Joystick.Player1.IsLeftPressing)
            {
                _runner.IsRunning = true;
                _runner.Effects = SpriteEffects.FlipHorizontally;
                _runner.Velocity.X -= 10;
            }
            else if (Joystick.Player1.IsRightPressing)
            {
                _runner.IsRunning = true;
                _runner.Effects = SpriteEffects.None;
                _runner.Velocity.X += 10;
            }

            _runner.Update(gameTime);
            _runner2.Update(gameTime);

            for (int i = 0; i < _snowmen.Length; i++)
                _snowmen[i].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var cameraPosition = _runner.Position;

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);

            _background.Draw(spriteBatch, cameraPosition);
            _runner.Draw(spriteBatch, cameraPosition);
            _runner2.Draw(spriteBatch, cameraPosition);

            for (int i = 0; i < _snowmen.Length; i++)
                _snowmen[i].Draw(spriteBatch, cameraPosition);

            spriteBatch.End();
        }
    }
}
