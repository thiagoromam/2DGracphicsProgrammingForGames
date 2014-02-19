using Resources;

namespace CameraOnPlayerWithYAndEnemyTracking
{
    public class MainGame : GameBase
    {
        protected override ITestComponent GetTest
        {
            get { return new TestComponent(this); }
        }
    }
}
