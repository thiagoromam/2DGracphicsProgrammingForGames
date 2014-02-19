using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PrinciplesOfDepth
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
            var drawPosition = Position;
            drawPosition.X -= cameraPosition.X;
            drawPosition.X *= Scale;
            drawPosition.X += TestComponent.BufferWidth / 2;

            spriteBatch.Draw(
                _texture,
                drawPosition,
                Location,
                Color,
                Rotation,
                Origin,
                Scale,
                Effects,
                Depth
                );
        }

        private void UpdateDepth()
        {
            Depth = (Position.Y - TestComponent.Horizon) / (720 - TestComponent.Horizon);
        }

        private void UpdateScale()
        {
            Scale = 0.25f + (Depth * 0.75f);

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