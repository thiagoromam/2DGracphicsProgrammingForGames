using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// ReSharper disable once CheckNamespace
namespace Resources
{
    public class TestComponent
    {
		private readonly GameBase _game;
	
		public TestComponent(GameBase game)
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