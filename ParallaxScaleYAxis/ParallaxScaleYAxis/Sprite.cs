using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;

namespace ParallaxScaleYAxis
{
    public abstract class Sprite
    {
        private Texture2D _texture;
        protected Rectangle? Location;
        protected Vector2 Origin;

        public Vector2 Position;
        public Color Color;
        public float Rotation;
        public float Scale;
        public float Depth;
        public SpriteEffects Effects;

        protected Sprite()
        {
            Color = Color.White;
            Effects = SpriteEffects.None;
        }

        public abstract void Initialize();
        protected void Initialize(Rectangle? location, Vector2 origin)
        {
            Rotation = 0;
            Scale = 1;
            Location = location;
            Origin = origin;
        }

        public void LoadConent(ContentManager content, string fileName)
        {
            _texture = content.Load<Texture2D>(fileName);
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateDepth();
            UpdateScale();
            UpdateColor();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition)
        {
            spriteBatch.Draw(_texture, GetDrawPosition(cameraPosition), Location, Color, Rotation, Origin, Scale, Effects, Depth);
        }

        private Vector2 GetDrawPosition(Vector2 cameraPosition)
        {
            var drawPosition = Position;
            drawPosition -= cameraPosition;
            drawPosition.X *= Scale;

            drawPosition.X += TestComponent.BufferWidth / 2;
            drawPosition.Y += TestComponent.BufferHeight / 2;

            return drawPosition;
        }

        private void UpdateDepth()
        {
            Depth = (Position.Y - TestComponent.Horizon) / (720 - TestComponent.Horizon);
        }

        private void UpdateScale()
        {
            Scale = TestComponent.ScaleFromOriginalSize + (Depth * TestComponent.ScaleFromDepth);

            //const float eyeLevel = 70; // runner
            //const float eyeLevel = 135; // snowman
            //Scale = Depth * ((TestComponent.BufferHeight - TestComponent.Horizon) / eyeLevel);
        }

        private void UpdateColor()
        {
            var greyValue = 0.75f + (Depth * 0.25f);
            Color = new Color(greyValue, greyValue, greyValue);
        }
    }
}