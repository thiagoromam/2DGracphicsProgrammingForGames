using Microsoft.Xna.Framework;

namespace SnowScene
{
    public class Power
    {
        private readonly IFisicalObject _obj;
        public Vector2 Direction;

        public Power(IFisicalObject obj)
        {
            _obj = obj;
        }

        public void Apply(float velocity)
        {
            var position = _obj.Position;
            position += Direction * velocity;
            _obj.Position = position;
        }
    }
}
