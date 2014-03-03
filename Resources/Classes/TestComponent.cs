using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// ReSharper disable once CheckNamespace
namespace Core
{
    public class TestComponent
    {
		private readonly Game _game;
	
		public TestComponent(Game game)
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