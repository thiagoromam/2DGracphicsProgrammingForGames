using Microsoft.Xna.Framework;

namespace SnowScene
{
    public class Gravity
    {
        private float _gravity;

        private readonly IFisicalObject _obj;

        public Gravity(IFisicalObject obj)
        {
            _obj = obj;
        }

        public void Apply(float velocity, GameTime gameTime)
        {
            _gravity += (float)gameTime.ElapsedGameTime.TotalSeconds * 32 / velocity;
            var position = _obj.Position;
            position.Y += _gravity;
            _obj.Position = position;
        }

        public void Reset()
        {
            _gravity = 0;
        }
    }
}
