using Resources;

namespace PrinciplesOfDepth
{
    public class MainGame : GameBase
    {
        protected override ITestComponent GetTest
        {
            get { return new TestComponent(this); }
        }
    }
}