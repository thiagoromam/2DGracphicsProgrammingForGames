using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnowScene
{
    public class RunnerAnimation
    {
        private Texture2D _runCycleTexture;
        private Rectangle _currentCelLocation;
        private int _numberOfCels;
        private int _currentCel;
        private int _msPerCel;
        public int MsUntilNextCel;
        private SpriteEffects _effects;
        private bool _turnLeft;
        public Rectangle CurrentCelLocation
        {
            get { return _currentCelLocation; }
        }
        public int MsPerCel
        {
            get { return _msPerCel; }
        }
        public Texture2D Texture
        {
            get { return _runCycleTexture; }
        }
        public bool TurnLeft
        {
            get { return _turnLeft; }
            set
            {
                if (value == _turnLeft) return;

                _turnLeft = value;
                _effects = value ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            }
        }
        public SpriteEffects Effects
        {
            get { return _effects; }
        }

        public void Initialize()
        {
            _numberOfCels = 12;
            _currentCel = 0;
            _msPerCel = 50;

            MsUntilNextCel = _msPerCel;
            _currentCelLocation.X = 0;
            _currentCelLocation.Y = 0;
            _currentCelLocation.Width = 128;
            _currentCelLocation.Height = 128;
            _effects = SpriteEffects.None;
        }

        public void LoadContent(ContentManager content)
        {
            _runCycleTexture = content.Load<Texture2D>("run_cycle");
        }

        public void UpdateMsUntilNextCel(GameTime gameTime)
        {
            MsUntilNextCel -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public bool UpdateCurrentCel()
        {
            var mustUpdate = MsUntilNextCel <= 0;

            if (mustUpdate)
                _currentCel++;

            if (_currentCel >= _numberOfCels)
                _currentCel = 0;

            _currentCelLocation.X = _currentCelLocation.Width * _currentCel;

            return mustUpdate;
        }
    }
}
