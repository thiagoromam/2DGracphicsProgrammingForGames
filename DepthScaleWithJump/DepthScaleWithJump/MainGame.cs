using Resources;

namespace DepthScaleWithJump
{
    public class MainGame : GameBase
    {
        protected override ITestComponent GetTest
        {
            get { return new TestComponent(this); }
        }
    }
}