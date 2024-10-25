using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class PlayerClone : Character
    {
        public bool IsActive;

        private float _gravity = 0.1f;

        private Texture2D _cloneTexture;

        private Queue<InputTimeStamp> _inputs;

        private Queue<InputTimeStamp> _activeInputs;

        private InputTimeStamp _currentInput;

        private float _currentTime = 0;

        private Vector2 _startPosition;

        public PlayerClone(Vector2 pos, Texture2D texture, Queue<InputTimeStamp> inputs)
        {
            _startPosition = pos;
            _cloneTexture = texture;
            _inputs = inputs;
            Position = pos;
            Collider = new BoundingRectangle(Position, 64, 64);
            UpdateCollider();
        }

        public void Start(GameTime gameTime)
        {
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

            if (_currentInput.HoldingRight)
            {
                Velocity = new Vector2(1, Velocity.Y);
            }
            if (_currentInput.HoldingLeft)
            {
                Velocity = new Vector2(-1, Velocity.Y);
            }
            if (_currentInput.PressedSpace && IsGrounded)
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_cloneTexture, Position, null, Color.LightGoldenrodYellow, 0f, Vector2.Zero, .5f, SpriteEffects.None, .5f);
        }
    }

}
