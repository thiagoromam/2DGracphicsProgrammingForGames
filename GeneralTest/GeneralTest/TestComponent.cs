using Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GeneralTest
{
    public class TestComponent : ITestComponent
    {
        private MainGame _game;

        public TestComponent(MainGame game)
        {
            _game = game;
        }

        public void Initialize()
        {
        }

        public void LoadContent(ContentManager content)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
