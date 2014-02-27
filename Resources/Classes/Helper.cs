using Microsoft.Xna.Framework;

namespace Resources
{
    public static class Helper
    {
        public static float ValueForEverySecond(this GameTime gameTime, float value)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds * value;
        }
    }
}
