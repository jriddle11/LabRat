using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LabRat
{
    public class Player : Character
    {
        public int CloneAmount = 1;
        private PopupMenu _popupMenu = new();
        private List<PlayerClone> _clones = new();
        private int _maxClones = 20;
        private Action<PlayerClone> _spawnClone;
        private Action<PlayerClone> _deleteClone;
        private float _gravity = 0.1f;
        private Vector2 _startPosition;
        private Texture2D _playerIdle;
        private Texture2D _playerRun;
        private Texture2D _playerJump;

        public Texture2D CloneIdle;
        public Texture2D CloneRun;
        public Texture2D CloneJump;

        private int _currentFrame = 0;
        private float _animationTimer = 0f;
        private float FrameTime = 0.02f;
        private const int FrameWidth = 240;
        private const int FrameHeight = 150;
        private const int TotalFrames = 20; // 4 columns * 5 rows

        public Player(Vector2 pos, Action<PlayerClone> spawnClone, Action<PlayerClone> deleteClone)
        {
            _startPosition = pos;
            Position = pos;
            _spawnClone = spawnClone;
            _deleteClone = deleteClone;
            Collider = new BoundingRectangle(Position + ColliderOffset, 60, 110);
            UpdateCollider();
            ID = int.MaxValue;
        }

        public void UpdateStartPosition(Vector2 pos)
        {
            _startPosition = pos;
            Reset();
        }

        public bool CollidesWithClones(BoundingRectangle rect)
        {
            bool col = false;
            foreach(PlayerClone clone in _clones)
            {
                if (rect.CollidesWith(clone.FloorCollider))
                {
                    col = true;
                }
            }
            return col;
        }

        public void Reset()
        {
            _popupMenu.Close();
            foreach (PlayerClone clone in _clones) _deleteClone?.Invoke(clone);
            _clones.Clear();
            if (InputManager.Recording) InputManager.StopRecording();
            Position = _startPosition;
            IsHeld = false;
            InputManager.StartRecording();
        }

        private void ResetPositions()
        {
            IsHeld = false;
            Position = _startPosition;
            RemoveOldClones();
            foreach (var clone in _clones)
            {
                clone.Start();
            }
            base.ResetEntityCollisions();
            InputManager.StartRecording();
        }

        private void RemoveOldClones()
        {
            while (_clones.Count > _maxClones)
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
            _playerIdle = content.Load<Texture2D>("player/player_idle");
            _playerRun = content.Load<Texture2D>("player/player_run");
            _playerJump = content.Load<Texture2D>("player/player_jump");

            CloneIdle = _playerIdle;
            CloneRun = _playerRun;
            CloneJump = _playerJump;

            _popupMenu.LoadContent(content);
            _popupMenu.Refresh += Refresh;
            _popupMenu.Reload += Reload;
        }

        public override void Update(GameTime gameTime)
        {
            _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_animationTimer >= FrameTime)
            {
                _animationTimer -= FrameTime;
                _currentFrame = (_currentFrame + 1) % TotalFrames;
                if(_currentFrame == TotalFrames - 1 && !IsGrounded)
                {
                    _currentFrame -= 1;
                }
            }

            _floorTimer.Update(gameTime);
            _popupMenu.CanRefresh = _clones.Count < CloneAmount;
            _popupMenu.CanReload = _clones.Count > 0;
            _popupMenu.Update(gameTime);
            UpdateVelocity(gameTime);
            ApplyVelocity(gameTime);
            if (InputManager.Mouse2Clicked && EntityCollision)
            {
                _popupMenu.Position = InputManager.GetMousePosition();
                _popupMenu.Open();
            }
            if (InputManager.Mouse1Clicked && _popupMenu.Enabled && !_popupMenu.IsHovering()) _popupMenu.Close();
            RemoveOldClones();
            base.Update(gameTime);
        }

        private void UpdateVelocity(GameTime gameTime)
        {
            Velocity = new Vector2(0, _gravity);

            if (InputManager.HoldingRight && !IsHeld)
            {
                Velocity = new Vector2(1, Velocity.Y);
                Direction = Direction.Right;
            }
            else if (InputManager.HoldingLeft && !IsHeld)
            {
                Velocity = new Vector2(-1, Velocity.Y);
                Direction = Direction.Left;
            }

            if (InputManager.PressedSpace && IsGrounded)
            {
                _gravity = -2.2f;
                UnGroundCharacter();
            }

            if (!IsGrounded && _gravity < 4f)
            {
                _gravity += 4 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _gravity = 0.5f;
            }
        }

        private void Refresh(object sender, EventArgs e)
        {
            _popupMenu.Close();
            var inputQ = InputManager.StopRecording();
            var clone = new PlayerClone(_startPosition, inputQ);

            _spawnClone?.Invoke(clone);
            _clones.Add(clone);

            ResetPositions();
        }

        private void Reload(object sender, EventArgs e)
        {
            Reset();
        }

        private Texture2D GetTexture()
        {
            if ((InputManager.HoldingLeft || InputManager.HoldingRight) && IsGrounded && !IsHeld)
            {
                FrameTime = 0.02f;
                return _playerRun;
            }
            if (!IsGrounded)
            {
                FrameTime = 0.02f;
                return _playerJump;
            }
            FrameTime = 0.05f;
            return _playerIdle;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int column = _currentFrame % 4;
            int row = _currentFrame / 4;
            Rectangle sourceRectangle = new Rectangle(column * FrameWidth, row * FrameHeight, FrameWidth, FrameHeight);

            spriteBatch.Draw(GetTexture(), Position, sourceRectangle, Color.White, 0f, Vector2.Zero, 1f, GetEffect(), 0.5f);
            _popupMenu.Draw(spriteBatch);
            //DebugManager.DrawRectangle(spriteBatch, Collider, Color.White * 0.5f);
            //DebugManager.DrawCircle(spriteBatch, FloorCollider, Color.Red * 0.5f);
        }
    }
}
