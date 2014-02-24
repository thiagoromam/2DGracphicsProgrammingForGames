using Resources;

namespace SnowScene
{
    public class MainGame : GameBase
    {
        protected override ITestComponent GetTest
        {
            get { return new TesteComponent(); }
        }
    }
}
