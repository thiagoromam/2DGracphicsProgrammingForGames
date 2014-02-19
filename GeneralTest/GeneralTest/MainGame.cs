using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace GeneralTest
{
    public class MainGame : GameBase
    {
        protected override ITestComponent GetTest
        {
            get { return new TestComponent(this); }
        }
    }
}
