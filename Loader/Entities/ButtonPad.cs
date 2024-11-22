using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabRat
{
    public class ButtonPad : IGameObject, ILevelComponent
    {
        public Vector2 Position = Vector2.Zero;
        private static Texture2D _padTexture;
        private static Texture2D _offTexture;
        private static Texture2D _onTexture;
        private Vector2 _colOffset = new Vector2(30, 100);
        private Vector2 _tileOffset = new Vector2(0, 128);
        private bool _isPressed = false;
        private BoundingRectangle _collider;
        private List<IActivatable> _activatables = new();
        private bool _enabled = false;
        public bool Enabled { get => _enabled; set => _enabled = value; }

        private void UpdateCollider()
        {
            _collider = new BoundingRectangle(Position + _colOffset, _padTexture.Width - 60, _padTexture.Height - 100);
        }
        
        public void Reset()
        {
            _activatables.Clear();
            _enabled = false;
        }

        public void LoadContent(ContentManager content)
        {
            _padTexture ??= content.Load<Texture2D>("button_pad");
            _offTexture ??= content.Load<Texture2D>("button_off");
            _onTexture ??= content.Load<Texture2D>("button_on");
            foreach (var activatable in _activatables) activatable.LoadContent(content);
            _collider = new BoundingRectangle(Position + _colOffset, _padTexture.Width - 60, _padTexture.Height - 100);
        }

        public void Update(GameTime gameTime)
        {
            if (!_enabled) return;
            UpdateCollider();
            _isPressed = _collider.CollidesWith(Context.Player.FloorCollider) || Context.Player.CollidesWithClones(_collider);
            foreach (var activatable in _activatables) activatable.IsActivated = _isPressed;
            foreach (var activatable in _activatables) activatable.Update(gameTime);
        }

        public void AddConnection(IActivatable activatable)
        {
            _activatables.Add(activatable);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_enabled) return;
            if(!_isPressed)spriteBatch.Draw(_padTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, .1f);
            spriteBatch.Draw(_isPressed ? _onTexture : _offTexture, Position + _tileOffset, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
            foreach (var activatable in _activatables) activatable.Draw(spriteBatch);
        }
    }
}
