using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Resources
{
	public abstract class GameBase : Game
	{
		public readonly GraphicsDeviceManager Graphics;
		private SpriteBatch _spriteBatch;
        public Color BackgroundColor;
		
		private readonly TestComponent _test;

        protected GameBase()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.IsFullScreen = true;
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);
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

            _test.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
	}
}