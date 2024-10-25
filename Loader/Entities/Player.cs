using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class Player : Character
    {
        private List<PlayerClone> _clones = new();

        private int _maxClones = 1;

        private Action<PlayerClone> _spawnClone;

        private Action<PlayerClone> _deleteClone;

        private float _gravity = 0.1f;

        private Vector2 _startPosition;

        private Texture2D _playerTexture;

        public Player(Vector2 pos, Action<PlayerClone> spawnClone, Action<PlayerClone> deleteClone)
        {
            _startPosition = pos;
            Position = pos;
            Collider = new BoundingRectangle(Position, 64, 64);
            UpdateCollider();
            ID = int.MaxValue;
            _spawnClone = spawnClone;
            _deleteClone = deleteClone;
        }

        public void UpdateStartPosition(Vector2 pos)
        {
            _startPosition = pos;
            Reset();
        }

        public void Reset()
        {
            foreach(PlayerClone clone in _clones) _deleteClone?.Invoke(clone);
            _clones.Clear();
            if(InputManager.Recording) InputManager.StopRecording();
            InputManager.StartRecording();
            Position = _startPosition;
        }

        private void NewTimeLine(GameTime gameTime)
        {
            Position = _startPosition;
            RemoveOldClones();
            foreach (var clone in _clones)
            {
                clone.Start(gameTime);
            }
            base.ResetEntityCollisions();
        }

        private void RemoveOldClones()
        {
            while(_clones.Count > _maxClones)
            {
                PlayerClone temp = _clones[0];
                foreach (var clone in _clones)
                {
                    if (clone.ID < temp.ID)
                    {
                        temp = clone;
                    }
                }
                _clones.Remove(temp);
                _deleteClone?.Invoke(temp);
            }

        }

        public override void LoadContent(ContentManager content)
        {
            _playerTexture = content.Load<Texture2D>("player");
        }

        public override void Update(GameTime gameTime)
        {
            _floorTimer.Update(gameTime);
            UpdateVelocity(gameTime);
            ApplyVelocity(gameTime);
            if (InputManager.PressedX && EntityCollision)
            {
                SpawnClone(gameTime);
            }
            RemoveOldClones();
            base.Update(gameTime);
        }

        private void UpdateVelocity(GameTime gameTime)
        {
            Velocity = new Vector2(0, _gravity);
            if (InputManager.HoldingRight)
            {
                Velocity = new Vector2(1, Velocity.Y);
            }
            if (InputManager.HoldingLeft)
            {
                Velocity = new Vector2(-1, Velocity.Y);
            }
            if (InputManager.PressedZ && IsGrounded)
            {
                _gravity = -2.2f;
                UnGroundCharacter();
            }

            if(!IsGrounded && _gravity < 4f)
            {
                _gravity += 4 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _gravity = 0.5f;
            }

        }

        private void SpawnClone(GameTime gameTime)
        {
            var inputQ = InputManager.StopRecording();
            InputManager.StartRecording();
            var clone = new PlayerClone(_startPosition, _playerTexture, inputQ);

            _spawnClone?.Invoke(clone);
            _clones.Add(clone);

            NewTimeLine(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_playerTexture, Position, null, Color.Orange, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);
        }
    }
}
