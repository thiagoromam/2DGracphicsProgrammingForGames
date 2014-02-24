using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnowScene
{
    public class Snowball : IGameElement, IFisicalObject
    {
        private const float Velocity = 10;

        private readonly Gravity _gravity;
        private readonly Power _power;
        private Texture2D _texture;
        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Vector2 _direction;
        private bool _throwToLeft = true;

        public float Width
        {
            get { return _texture.Width; }
        }
        public float Height
        {
            get { return _texture.Height; }
        }
        public bool ThrowToLeft
        {
            get { return _throwToLeft; }
            set
            {
                if (value == _throwToLeft) return;

                _throwToLeft = value;
                var direction = _direction;
                direction.X *= value ? -1 : 1;
                _power.Direction = direction;
            }
        }

        public Snowball()
        {
            _gravity = new Gravity(this);
            _power = new Power(this);
        }

        public void Initialize()
        {
            _direction = new Vector2(3, -1);
            _direction.Normalize();
            ThrowToLeft = false;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("snowball");
        }

        public void Update(GameTime gameTime)
        {
            _power.Apply(Velocity);
            _gravity.Apply(Velocity, gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }

        public void Reset()
        {
            _gravity.Reset();
        }
    }
}
