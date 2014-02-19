using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Resources
{
    public interface ITestComponent
    {
        void Initialize();
        void LoadContent(ContentManager content);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}