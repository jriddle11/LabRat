using Microsoft.Xna.Framework.Input;

namespace LabRat
{
    public static class InputManager
    {
        //Mouse
        public static bool Mouse1Down => _currentMouseState.LeftButton == ButtonState.Pressed;
        public static bool Mouse1Up => _currentMouseState.LeftButton == ButtonState.Released;
        public static bool Mouse1JustPressed => _currentMouseState.LeftButton == ButtonState.Pressed && _priorMouseState.LeftButton == ButtonState.Released;
        public static bool Mouse1Clicked;

        //Keyboard
        public static bool HoldingUp;
        public static bool HoldingDown;
        public static bool HoldingLeft;
        public static bool HoldingRight;

        public static bool HoldingSpace;
        public static bool HoldingEscape;
        public static bool HoldingZ;
        public static bool HoldingX;
        public static bool HoldingShift;

        public static bool IsHolding => HoldingDown || HoldingLeft || HoldingUp || HoldingRight;

        public static bool PressedDown;
        public static bool PressedUp;
        public static bool PressedLeft;
        public static bool PressedRight;

        public static bool PressedSpace;
        public static bool PressedEscape;
        public static bool PressedZ;
        public static bool PressedX;

        private static KeyboardState _priorKeyboardState;
        private static KeyboardState _currentKeyboardState;
        private static MouseState _currentMouseState;
        private static MouseState _priorMouseState;

        private static Queue<InputTimeStamp> _inputHistory;
        public static bool Recording;
        private static float _recordCurrentTime;
        private static bool _recordingHoldingSpace = false;
        private static bool _recordingHoldingLeft = false;
        private static bool _recordingHoldingRight = false;

        public static void StartRecording()
        {
            _inputHistory = new();
            _recordCurrentTime = 0;
            _inputHistory.Enqueue(new InputTimeStamp(_currentKeyboardState ,_recordCurrentTime));
            Recording = true;
        }

        public static Queue<InputTimeStamp> StopRecording()
        {
            Recording = false;
            _inputHistory.Enqueue(new InputTimeStamp(_currentKeyboardState, _recordCurrentTime));
            return _inputHistory;
        }

        public static Direction GetKeyboardDirection()
        {
            Direction direction = Direction.None;
            if (HoldingLeft) direction = Direction.Left;
            if (HoldingRight) direction = Direction.Right;
            if (HoldingUp) direction = Direction.Up;
            if (HoldingDown) direction = Direction.Down;
            if (HoldingLeft && HoldingUp) direction = Direction.UpLeft;
            if (HoldingRight && HoldingUp) direction = Direction.UpRight;
            if (HoldingLeft && HoldingDown) direction = Direction.DownLeft;
            if (HoldingRight && HoldingDown) direction = Direction.DownRight;
            return direction;
        }

        public static void Update(GameTime gameTime)
        {
            #region Mouse

            _priorMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            Mouse1Clicked = false;
            if (_currentMouseState.LeftButton == ButtonState.Pressed && _priorMouseState.LeftButton == ButtonState.Released)
            {
                Mouse1Clicked = true;
            }
            else
            {
                Mouse1Clicked = false;
            }

            #endregion

            #region Keyboard

            _priorKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            //Holding button
            HoldingDown = _currentKeyboardState.IsKeyDown(Keys.S) || _currentKeyboardState.IsKeyDown(Keys.Down);
            HoldingUp = _currentKeyboardState.IsKeyDown(Keys.W) || _currentKeyboardState.IsKeyDown(Keys.Up);
            HoldingLeft = _currentKeyboardState.IsKeyDown(Keys.A) || _currentKeyboardState.IsKeyDown(Keys.Left);
            HoldingRight = _currentKeyboardState.IsKeyDown(Keys.D) || _currentKeyboardState.IsKeyDown(Keys.Right);

            HoldingSpace = _currentKeyboardState.IsKeyDown(Keys.Space);
            HoldingShift = _currentKeyboardState.IsKeyDown(Keys.LeftShift) || _currentKeyboardState.IsKeyDown(Keys.RightShift);
            HoldingEscape = _currentKeyboardState.IsKeyDown(Keys.Escape);
            HoldingZ = _currentKeyboardState.IsKeyDown(Keys.Z);
            HoldingX = _currentKeyboardState.IsKeyDown(Keys.X);

            //Pressing button
            PressedDown = HoldingDown && _priorKeyboardState.IsKeyUp(Keys.S) && _priorKeyboardState.IsKeyUp(Keys.Down);
            PressedUp = HoldingUp && _priorKeyboardState.IsKeyUp(Keys.W) && _priorKeyboardState.IsKeyUp(Keys.Up);
            PressedRight = HoldingRight && _priorKeyboardState.IsKeyUp(Keys.Right) && _priorKeyboardState.IsKeyUp(Keys.D);
            PressedLeft = HoldingLeft && _priorKeyboardState.IsKeyUp(Keys.Left) && _priorKeyboardState.IsKeyUp(Keys.A);

            PressedSpace = HoldingSpace && _priorKeyboardState.IsKeyUp(Keys.Space);
            PressedEscape = HoldingEscape && _priorKeyboardState.IsKeyUp(Keys.Escape);
            PressedZ = HoldingZ && _priorKeyboardState.IsKeyUp(Keys.Z);
            PressedX = HoldingX && _priorKeyboardState.IsKeyUp(Keys.X);

            if (Recording)
            {
                _recordCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(!RecordingInputIsTheSame())
                {
                    _inputHistory.Enqueue(new InputTimeStamp(_currentKeyboardState, _recordCurrentTime));
                    _recordingHoldingLeft = _currentKeyboardState.IsKeyDown(Keys.Left);
                    _recordingHoldingRight = _currentKeyboardState.IsKeyDown(Keys.Right);
                    _recordingHoldingSpace = _currentKeyboardState.IsKeyDown(Keys.Z);
                }
            }

            #endregion
        }

        public static bool IsMouseInRectangle(BoundingRectangle rectangle)
        {
            var mouse = _currentMouseState;
            return rectangle.Contains(new Vector2(mouse.X, mouse.Y));
        }

        private static bool RecordingInputIsTheSame()
        {
            var a = _currentKeyboardState.IsKeyDown(Keys.Left) == _recordingHoldingLeft;
            var b = _currentKeyboardState.IsKeyDown(Keys.Right) == _recordingHoldingRight;
            var c = _currentKeyboardState.IsKeyDown(Keys.Z) == _recordingHoldingSpace;
            return a && b && c;
        }
    }

    public struct InputTimeStamp
    {
        public float Time;
        public bool HoldingRight;
        public bool HoldingLeft;
        public bool PressedSpace;

        private static readonly InputTimeStamp noInput = new InputTimeStamp();
        public static InputTimeStamp NoInput => noInput;

        public InputTimeStamp(KeyboardState state, float time)
        {
            Time = time;
            HoldingRight = state.IsKeyDown(Keys.Right);
            HoldingLeft = state.IsKeyDown(Keys.Left);
            PressedSpace = state.IsKeyDown(Keys.Z);
        }

        public InputTimeStamp(float time)
        {
            Time = time;
            HoldingRight = false;
            HoldingLeft = false;
            PressedSpace = false;
        }
    }
}
