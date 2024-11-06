using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class PlayerClone : Character
    {
        public bool IsActive;

        private float _gravity = 0.1f;

        private Queue<InputTimeStamp> _inputs;

        private Queue<InputTimeStamp> _activeInputs;

        private InputTimeStamp _currentInput;

        private float _currentTime = 0;

        private Vector2 _startPosition;

        private int _currentFrame = 0;
        private float _animationTimer = 0f;
        private float FrameTime = 0.02f;
        private const int FrameWidth = 240;
        private const int FrameHeight = 150;
        private const int TotalFrames = 20; // 4 columns * 5 rows

        public PlayerClone(Vector2 pos, Queue<InputTimeStamp> inputs)
        {
            _startPosition = pos;
            _inputs = inputs;
            Position = pos;
            Collider = new BoundingRectangle(Position + ColliderOffset, 60, 110);
            UpdateCollider();
        }

        public void Start()
        {
            IsHeld = false;
            _activeInputs = new Queue<InputTimeStamp>(_inputs);
            Position = _startPosition;
            _currentTime = 0;
            IsActive = true;
            base.ResetEntityCollisions();
        }

        public override void LoadContent(ContentManager content)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_animationTimer >= FrameTime)
            {
                _animationTimer -= FrameTime;
                _currentFrame = (_currentFrame + 1) % TotalFrames;
                if (_currentFrame == TotalFrames - 1 && !IsGrounded)
                {
                    _currentFrame -= 1;
                }
            }

            _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _floorTimer.Update(gameTime);
            UpdateVelocity(gameTime);
            ApplyVelocity(gameTime);
            base.Update(gameTime);
        }

        private void UpdateVelocity(GameTime gameTime)
        {
            Velocity = new Vector2(0, _gravity);

            if (_activeInputs.Count > 0 && _activeInputs.Peek().Time <= _currentTime)
            {
                _currentInput = _activeInputs.Dequeue();
            }

            if (_currentInput.Time <= _currentTime && _activeInputs.Count == 0)
            {
                _currentInput = InputTimeStamp.NoInput;
                //IsActive = false;
            }

            if (_currentInput.HoldRight && !IsHeld)
            {
                Velocity = new Vector2(1, Velocity.Y);
                Direction = Direction.Right;
            }
            if (_currentInput.HoldLeft && !IsHeld)
            {
                Velocity = new Vector2(-1, Velocity.Y);
                Direction = Direction.Left;
            }
            if (_currentInput.PressSpace && IsGrounded)
            {
                _gravity = -2.2f;
                UnGroundCharacter();
            }

            // Gravity physics handling
            if (!IsGrounded && _gravity < 4f)
            {
                _gravity += 4 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _gravity = 0.5f;
            }
        }

        private Texture2D GetTexture()
        {
            if ((_currentInput.HoldLeft || _currentInput.HoldRight) && IsGrounded && !IsHeld)
            {
                FrameTime = 0.02f;
                return Context.Player.CloneRun;
            }
            if (!IsGrounded && EntityCollision)
            {
                FrameTime = 0.02f;
                return Context.Player.CloneJump;
            }
            FrameTime = 0.05f;
            return Context.Player.CloneIdle; ;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int column = _currentFrame % 4;
            int row = _currentFrame / 4;
            Rectangle sourceRectangle = new Rectangle(column * FrameWidth, row * FrameHeight, FrameWidth, FrameHeight);

            spriteBatch.Draw(GetTexture(), Position, sourceRectangle, Color.Black  * 10f, 0f, Vector2.Zero, 1f, GetEffect(), .6f);
            //DebugManager.DrawRectangle(spriteBatch, Collider, Color.White * 0.5f);
            //DebugManager.DrawCircle(spriteBatch, FloorCollider, Color.Red * 0.5f);
        }
    }

}
