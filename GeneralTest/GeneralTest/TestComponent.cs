using System;
using Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GeneralTest
{
    public class TestComponent : ITestComponent
    {
        private MainGame _game;

        public const int BufferWidth = 1280;
        public const int BufferHeight = 720;
        public const int Horizon = 240;

        private Sprite _background;
        private Runner _runner;
        private Runner _runner2;
        private Snowman[] _snowmen;

        private Vector2 _cameraOffset;
        private Vector2 _cameraPosition;

        public TestComponent(MainGame game)
        {
            _game = game;
            _game.Graphics.PreferredBackBufferWidth = BufferWidth;
            _game.Graphics.PreferredBackBufferHeight = BufferHeight;

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

            var viewport = _game.GraphicsDevice.Viewport;
            _cameraOffset = new Vector2(viewport.Width / 2, viewport.Height / 2);
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

            _cameraPosition = _runner.Position;
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

    public class Runner : Sprite
    {
        private readonly float MaxVelocity;

        private int _currentCel;
        private int _numberOfCels;
        private float _msUntilNextCel;
        private int _msPerCel;

        public Vector2 Velocity;

        public bool IsRunning;

        public Runner()
        {
            MaxVelocity = TestComponent.BufferWidth / 6;
        }

        public override void Initialize()
        {
            Initialize(new Rectangle(0, 0, 128, 128), new Vector2(57, 105));

            _numberOfCels = 12;
            _currentCel = 0;
            _msPerCel = 50;
            _msUntilNextCel = _msPerCel;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateAnimation(gameTime);
            UpdatePosition(gameTime);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            _msUntilNextCel -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (IsRunning && _msUntilNextCel <= 0)
            {
                _currentCel++;
                _msUntilNextCel = _msPerCel;
            }

            if (_currentCel >= _numberOfCels)
                _currentCel = 0;

            var location = Location.Value;
            location.X = location.Width * _currentCel;
            Location = location;
        }

        private void UpdatePosition(GameTime gameTime)
        {
            Velocity *= 0.95f;
            Velocity.X = MathHelper.Clamp(Velocity.X, -MaxVelocity, MaxVelocity);
            Velocity.Y = MathHelper.Clamp(Velocity.Y, -MaxVelocity, MaxVelocity);

            var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position.X += Scale * Velocity.X * elapsedSeconds;
            Position.Y += Scale * Velocity.Y * elapsedSeconds;
            Position.Y = MathHelper.Clamp(Position.Y, TestComponent.Horizon, TestComponent.BufferHeight);
        }
    }

    public class Snowman : Sprite
    {
        public override void Initialize()
        {
            Initialize(new Rectangle(0, 128, 256, 256), new Vector2(119, 185));
        }
    }

    public class Background : Sprite
    {
        public override void Initialize()
        {
            Initialize(null, Vector2.Zero);
        }
    }

    public abstract class Sprite
    {
        private Texture2D _texture;
        protected Rectangle? Location;
        protected Vector2 Origin;

        public Vector2 Position;
        public Color Color;
        public float Rotation;
        public float Scale;
        public float Depth;
        public SpriteEffects Effects;

        public Sprite()
        {
            Color = Color.White;
            Effects = SpriteEffects.None;
        }

        public abstract void Initialize();
        protected void Initialize(Rectangle? location, Vector2 origin)
        {
            Rotation = 0;
            Scale = 1;
            Location = location;
            Origin = origin;
        }

        public void LoadConent(ContentManager content, string fileName)
        {
            _texture = content.Load<Texture2D>(fileName);
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateDepth();
            UpdateScale();
            UpdateColor();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition)
        {
            var drawPosition = Position;
            drawPosition.X -= cameraPosition.X;
            drawPosition.X *= Scale;
            drawPosition.X += TestComponent.BufferWidth / 2;

            spriteBatch.Draw(
                _texture,
                drawPosition,
                Location,
                Color,
                Rotation,
                Origin,
                Scale,
                Effects,
                Depth
            );
        }

        private void UpdateDepth()
        {
            Depth = (Position.Y - TestComponent.Horizon) / (720 - TestComponent.Horizon);
        }

        private void UpdateScale()
        {
            Scale = 0.25f + (Depth * 0.75f);
        }

        private void UpdateColor()
        {
            var greyValue = 0.75f + (Depth * 0.25f);
            Color = new Color(greyValue, greyValue, greyValue);
        }
    }
}
