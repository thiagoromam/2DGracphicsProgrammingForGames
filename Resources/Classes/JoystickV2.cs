using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class Joystick
    {
        public static Joystick Player1 { get; private set; }
        public static Joystick Player2 { get; private set; }

        private KeyboardState _lastState;
        private readonly Keys _left;
        private readonly Keys _up;
        private readonly Keys _right;
        private readonly Keys _down;
        private readonly Keys _jump;
        private readonly Keys _fire;
        private readonly Keys _fire2;

        public bool IsLeftPressing { get; private set; }
        public bool IsLeftPressed { get; private set; }
        public bool IsRightPressing { get; private set; }
        public bool IsRightPressed { get; private set; }
        public bool IsDownPressing { get; private set; }
        public bool IsUpPressing { get; private set; }
        public bool IsJumpPressing { get; private set; }
        public bool IsFirePressed { get; private set; }
        public bool IsFire2Pressed { get; private set; }
        public bool IsSumPressing { get; private set; }
        public bool IsMinusPressing { get; private set; }

        private Joystick(Keys left, Keys up, Keys right, Keys down, Keys jump, Keys fire, Keys fire2)
        {
            _left = left;
            _up = up;
            _right = right;
            _down = down;
            _jump = jump;
            _fire = fire;
            _fire2 = fire2;
        }

        public void Update()
        {
            IsLeftPressing = false;
            IsLeftPressed = false;
            IsRightPressing = false;
            IsRightPressed = false;
            IsUpPressing = false;
            IsDownPressing = false;

            var state = Keyboard.GetState();

            if (state.IsKeyDown(_left))
                IsLeftPressing = true;
            else if (state.IsKeyDown(_right))
                IsRightPressing = true;

            if (state.IsKeyDown(_up))
                IsUpPressing = true;
            else if (state.IsKeyDown(_down))
                IsDownPressing = true;

            if (state.IsKeyUp(_left) && _lastState.IsKeyDown(_left))
                IsLeftPressed = true;
            else if (state.IsKeyUp(_right) && _lastState.IsKeyDown(_right))
                IsRightPressed = true;

            IsJumpPressing = state.IsKeyDown(_jump);
            IsFirePressed = state.IsKeyUp(_fire) && _lastState.IsKeyDown(_fire);
            IsFire2Pressed = state.IsKeyUp(_fire2) && _lastState.IsKeyDown(_fire2);

            IsSumPressing = state.IsKeyDown(Keys.Add);
            IsMinusPressing = state.IsKeyDown(Keys.Subtract);

            _lastState = state;
        }

        public static void LoadPlayer1()
        {
            Player1 = new Joystick(Keys.A, Keys.W, Keys.D, Keys.S, Keys.Space, Keys.J, Keys.K);
        }

        public static void LoadPlayer2()
        {
            Player2 = new Joystick(Keys.Left, Keys.Up, Keys.Right, Keys.Down, Keys.NumPad0, Keys.NumPad1, Keys.NumPad2);
        }
    }
}
