using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Resources
{
    public static class Helper
    {
        public static float ValueForEverySecond(this GameTime gameTime, float value)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds * value;
        }

        public static Vector2 CalculateCenterOfScreen(this Viewport viewport)
        {
            return new Vector2(viewport.Width / 2f, viewport.Height / 2f);
        }

        public static Vector2 CalculateCenterOfText(this SpriteFont font, string text)
        {
            var size = font.MeasureString(text);
            return new Vector2(size.X / 2f, size.Y / 2f);
        }
    }
}
