using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// ReSharper disable once CheckNamespace
namespace Core
{
    public class TestComponent
    {
		private readonly MainGame _game;
        private Texture2D _surge;
        private Effect _effect;

        public TestComponent(MainGame game)
		{
			_game = game;
		}
	
        public void Initialize()
		{
		}
		
        public void LoadContent(ContentManager content)
        {
            _surge = content.Load<Texture2D>("surge");
            _effect = content.Load<Effect>("RedChannelEffect");
        }
		
        public void Update(GameTime gameTime)
		{
		}
		
        public void Draw(SpriteBatch spriteBatch)
		{
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            
            _effect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(_surge, Vector2.Zero, Color.White);
            
            spriteBatch.End();
		}
    }
}