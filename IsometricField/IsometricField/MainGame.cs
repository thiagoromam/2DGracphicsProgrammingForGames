using Resources;

namespace IsometricField
{
    public class MainGame : GameBase
    {
        protected override ITestComponent GetTest
        {
            get { return new TestComponent(this); }
        }
    }
}
