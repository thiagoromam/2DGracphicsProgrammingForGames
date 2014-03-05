using System.Text;
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

        public static float ElapsedSeconds(this GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
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

        public static Vector2 CalculateCenter(this Texture2D texture)
        {
            return new Vector2(texture.Width / 2f, texture.Height / 2f);
        }

        public static string WrapText(this SpriteFont font, string text, float maximumWidth)
        {
            var words = text.Split(' ');

            var newText = new StringBuilder();

            foreach (var word in words)
            {
                if (font.MeasureString(newText + word).X > maximumWidth)
                    newText.AppendLine();

                newText.Append(word);
                newText.Append(' ');
            }

            return newText.ToString().Trim();
        }
    }
}
