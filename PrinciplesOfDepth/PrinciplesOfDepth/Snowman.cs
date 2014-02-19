using Microsoft.Xna.Framework;

namespace PrinciplesOfDepth
{
    public class Snowman : Sprite
    {
        public override void Initialize()
        {
            Initialize(new Rectangle(0, 128, 256, 256), new Vector2(119, 185));
        }
    }
}