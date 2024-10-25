using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class Button : IGameObject
    {
        public bool Enabled = true;
        public bool IsHovering { get; private set; }
        public bool IsHeldDown { get; private set; }

        private Action _onButtonClick;
        private bool _centered = false;
        private bool _clicked = false;
        private Texture2D _texture;
        private Texture2D _hoverTexture;
        private string _texturePath;
        private string _hoverTexturePath;
        private Vector2 _position;

        private BoundingRectangle _collider;

        public Button(Action onButtonClick, Vector2 pos, bool centered, string texturePath, string hoverTexurePath = "")
        {
            _onButtonClick = onButtonClick;
            _centered = centered;
            _position = pos;
            _texturePath = texturePath;
            _hoverTexturePath = hoverTexurePath;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>(_texturePath);
            if (_centered) _position -= new Vector2(_texture.Width / 2, 0);
            _collider = new BoundingRectangle(_position.X, _position.Y, _texture.Width, _texture.Height);
            if (_hoverTexturePath != "") _hoverTexture = content.Load<Texture2D>(_hoverTexturePath);
            else _hoverTexture = _texture;
        }

        public void Update(GameTime gameTime)
        {
            if(!Enabled) return;
            if (IsHovering && InputManager.Mouse1JustPressed)
            {
                _clicked = true;
            }
            if (IsHovering && InputManager.Mouse1Up && IsHeldDown) _onButtonClick?.Invoke();
            if (!IsHovering || InputManager.Mouse1Up)
            {
                _clicked = false;
            }
            IsHovering = InputManager.IsMouseInRectangle(_collider);
            IsHeldDown = IsHovering && _clicked;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(IsHeldDown ? _hoverTexture : _texture, _position, Enabled ? Color.White : Color.Black);
        }
    }
}
