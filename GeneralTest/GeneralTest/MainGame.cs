using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources.Classes;

namespace GeneralTest
{
    public class MainGame : GameBase
    {
        protected override Resources.ITestComponent GetTest
        {
            get { return new TestComponent(this); }
        }
    }
}
