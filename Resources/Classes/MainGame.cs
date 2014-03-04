using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class MainGame : Game
    {
        public readonly GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public Color BackgroundColor;

        private readonly TestComponent _test;

        public MainGame()
        {
            Graphics = new GraphicsDeviceManager(this) { IsFullScreen = true };
            Content.RootDirectory = "Content";

            BackgroundColor = Color.CornflowerBlue;

            Joystick.LoadPlayer1();
            Joystick.LoadPlayer2();

            _test = new TestComponent(this);
        }

        protected override void Initialize()
        {
            _test.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            _test.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            Joystick.Player1.Update();
            Joystick.Player2.Update();
            _test.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);

            _test.Draw(SpriteBatch);

            base.Draw(gameTime);
        }
    }
}