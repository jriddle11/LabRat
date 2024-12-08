using Microsoft.Xna.Framework.Input;

namespace LabRat
{
    public static class InputManager
    {
        //Mouse
        public static bool Mouse1Down => _currentMouseState.LeftButton == ButtonState.Pressed;
        public static bool Mouse1Up => _currentMouseState.LeftButton == ButtonState.Released;
        public static bool Mouse1Clicked => _currentMouseState.LeftButton == ButtonState.Pressed && _priorMouseState.LeftButton == ButtonState.Released;

        public static bool Mouse2Down => _currentMouseState.RightButton == ButtonState.Pressed;
        public static bool Mouse2Up => _currentMouseState.RightButton == ButtonState.Released;
        public static bool Mouse2Clicked => _currentMouseState.RightButton == ButtonState.Pressed && _priorMouseState.RightButton == ButtonState.Released;

        //Keyboard
        public static bool HoldingDown => _currentKeyboardState.IsKeyDown(Keys.S) || _currentKeyboardState.IsKeyDown(Keys.Down);
        public static bool HoldingUp => _currentKeyboardState.IsKeyDown(Keys.W) || _currentKeyboardState.IsKeyDown(Keys.Up);
        public static bool HoldingLeft => _currentKeyboardState.IsKeyDown(Keys.A) || _currentKeyboardState.IsKeyDown(Keys.Left);
        public static bool HoldingRight => _currentKeyboardState.IsKeyDown(Keys.D) || _currentKeyboardState.IsKeyDown(Keys.Right);

        public static bool HoldingSpace => _currentKeyboardState.IsKeyDown(Keys.Space);
        public static bool HoldingShift => _currentKeyboardState.IsKeyDown(Keys.LeftShift) || _currentKeyboardState.IsKeyDown(Keys.RightShift);
        public static bool HoldingEscape => _currentKeyboardState.IsKeyDown(Keys.Escape);
        public static bool HoldingZ => _currentKeyboardState.IsKeyDown(Keys.Z);
        public static bool HoldingX => _currentKeyboardState.IsKeyDown(Keys.X);

        public static bool IsHolding => HoldingDown || HoldingLeft || HoldingUp || HoldingRight;

        public static bool PressedDown => HoldingDown && _priorKeyboardState.IsKeyUp(Keys.S) && _priorKeyboardState.IsKeyUp(Keys.Down);
        public static bool PressedUp => HoldingUp && _priorKeyboardState.IsKeyUp(Keys.W) && _priorKeyboardState.IsKeyUp(Keys.Up);
        public static bool PressedRight => HoldingRight && _priorKeyboardState.IsKeyUp(Keys.Right) && _priorKeyboardState.IsKeyUp(Keys.D);
        public static bool PressedLeft => HoldingLeft && _priorKeyboardState.IsKeyUp(Keys.Left) && _priorKeyboardState.IsKeyUp(Keys.A);

        public static bool PressedSpace => HoldingSpace && _priorKeyboardState.IsKeyUp(Keys.Space);
        public static bool PressedEscape => HoldingEscape && _priorKeyboardState.IsKeyUp(Keys.Escape);
        public static bool PressedZ => HoldingZ && _priorKeyboardState.IsKeyUp(Keys.Z);
        public static bool PressedX => HoldingX && _priorKeyboardState.IsKeyUp(Keys.X);

        //States
        private static KeyboardState _priorKeyboardState;
        private static KeyboardState _currentKeyboardState;
        private static MouseState _currentMouseState;
        private static MouseState _priorMouseState;

        //Recording
        private static Queue<InputTimeStamp> _inputHistory = new();
        public static bool Recording;
        private static float _recordCurrentTime;
        private static bool _recordingPressJump = false;
        private static bool _recordingHoldLeft = false;
        private static bool _recordingHoldRight = false;

        public static void StartRecording()
        {
            _inputHistory = new();
            _recordCurrentTime = 0;
            _inputHistory.Enqueue(new InputTimeStamp(_recordCurrentTime));
            Recording = true;
        }

        public static Queue<InputTimeStamp> StopRecording()
        {
            Recording = false;
            _inputHistory.Enqueue(new InputTimeStamp(_recordCurrentTime));
            return _inputHistory;
        }

        public static bool Holding(string keyString)
        {
            Enum.TryParse(keyString, true, out Keys key);
            return _currentKeyboardState.IsKeyDown(key);
        }

        public static bool Holding(string[] keyStrings)
        {
            bool holding = false;
            foreach(string keyString in keyStrings)
            {
                Enum.TryParse(keyString, true, out Keys key);
                holding = holding || _currentKeyboardState.IsKeyDown(key);
            }
            return holding;
        }

        public static bool Pressed(string keyString)
        {
            Enum.TryParse(keyString, true, out Keys key);
            return _currentKeyboardState.IsKeyDown(key) && _priorKeyboardState.IsKeyDown(key);
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

            if (_priorMouseState.RightButton == ButtonState.Released && _currentMouseState.RightButton == ButtonState.Pressed)
            {
                SoundManager.PlayMouseClickFull();
            }
            if (_priorMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed)
            {
                SoundManager.PlayMouseClickFull();
            }

            /*
            if(_priorMouseState.LeftButton == ButtonState.Pressed && _currentMouseState.LeftButton == ButtonState.Released)
            {
                SoundManager.PlayMouseClickUp();
            }
            if (_priorMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed)
            {
                SoundManager.PlayMouseClickDown();
            }
            if (_priorMouseState.RightButton == ButtonState.Pressed && _currentMouseState.RightButton == ButtonState.Released)
            {
                SoundManager.PlayMouseClickUp();
            }
            if (_priorMouseState.RightButton == ButtonState.Released && _currentMouseState.RightButton == ButtonState.Pressed)
            {
                SoundManager.PlayMouseClickDown();
            }*/

            #endregion

            #region Keyboard

            _priorKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            if (Recording)
            {
                _recordCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(!RecordingInputIsTheSame())
                {
                    _inputHistory.Enqueue(new InputTimeStamp(_recordCurrentTime));
                    _recordingHoldLeft = HoldingLeft;
                    _recordingHoldRight = HoldingRight;
                    _recordingPressJump = PressedSpace;
                }
            }

            #endregion
        }

        public static bool IsMouseInRectangle(BoundingRectangle rectangle)
        {
            Vector2 transformedMousePosition = GetMousePosition();

            return rectangle.Contains(transformedMousePosition);
        }

        public static Vector2 GetMousePosition()
        {
            Vector2 mousePosition = new Vector2(_currentMouseState.X, _currentMouseState.Y);

            // Transform the mouse position to world space using the inverse of the camera transform
            return Vector2.Transform(mousePosition, Matrix.Invert(Context.Camera.Transform));
        }

        private static bool RecordingInputIsTheSame()
        {
            var a = HoldingLeft == _recordingHoldLeft;
            var b = HoldingRight == _recordingHoldRight;
            var c = PressedSpace == _recordingPressJump;
            return a && b && c;
        }
    }

    public struct InputTimeStamp
    {
        public float Time;
        public bool HoldRight;
        public bool HoldLeft;
        public bool PressSpace;

        private static readonly InputTimeStamp noInput = new InputTimeStamp();
        public static InputTimeStamp NoInput => noInput;

        public InputTimeStamp(float time)
        {
            Time = time;
            HoldRight = InputManager.HoldingRight;
            HoldLeft = InputManager.HoldingLeft;
            PressSpace = InputManager.PressedSpace;
        }
    }
}
