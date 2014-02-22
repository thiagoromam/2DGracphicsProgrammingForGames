/* Challenge 6.2. The examples in this chapter were limited to objects that
rest on the ground. Now add the ability to have the player jump.

Hint: You'll need to use an \oset height" in your jump calculation,
applied in a way that will ensure the sprite scale does not shrink as the
player jumps into the air. At the same time, the jump height should be
scaled appropriately based on the current depth. */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Resources;

namespace DepthScaleWithJump
{
    public class TestComponent : ITestComponent
    {
        public const int BufferWidth = 1280;
        public const int BufferHeight = 720;
        public const int Horizon = 240;

        private readonly MainGame _game;

        private readonly Sprite _background;
        private readonly Runner _runner;
        private readonly Runner _runner2;
        private Snowman[] _snowmen;
        private RunnerControllTracker _runnerControllTracker;

        public TestComponent(MainGame game)
        {
            _game = game;
            _game.Graphics.PreferredBackBufferWidth = BufferWidth;
            _game.Graphics.PreferredBackBufferHeight = BufferHeight;

            _background = new Background();
            _runner = new Runner();
            _runner2 = new Runner();
            _runnerControllTracker = new RunnerControllTracker(_runner, Joystick.Player1);
        }

        public void Initialize()
        {
            _runner.Initialize();
            _runner.Position = new Vector2(400, 680);

            _runner2.Initialize();
            _runner2.Position = new Vector2(600, 400);

            _snowmen = new Snowman[36];
            const int snowmenQuadrant = 6;
            for (var i = 0; i < snowmenQuadrant; ++i)
            {
                var y = Horizon + i * 80;

                for (var j = 0; j < snowmenQuadrant; ++j)
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

            for (var i = 0; i < _snowmen.Length; i++)
                _snowmen[i].LoadConent(content, "snow_assets");

            _background.LoadConent(content, "background");
        }

        public void Update(GameTime gameTime)
        {
            _runnerControllTracker.Update(gameTime);
            _runner.Update(gameTime);
            _runner2.Update(gameTime);

            for (var i = 0; i < _snowmen.Length; ++i)
                _snowmen[i].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var cameraPosition = _runner.Position;

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);

            _background.Draw(spriteBatch, cameraPosition);
            _runner.Draw(spriteBatch, cameraPosition);
            _runner2.Draw(spriteBatch, cameraPosition);

            for (var i = 0; i < _snowmen.Length; ++i)
                _snowmen[i].Draw(spriteBatch, cameraPosition);

            spriteBatch.End();
        }
    }
}
