using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class Button : IGameObject
    {
        public bool Enabled = true;
        public Vector2 Position;
        public bool IsHovering { get; private set; }
        public bool IsHeldDown { get; private set; }
        public float LayerDepth = 0.95f;
        public ButtonMode Mode = ButtonMode.Press;

        private Action _onButtonClick;
        private bool _centered = false;
        private bool _clicked = false;
        private Texture2D _texture;
        private Texture2D _hoverTexture;
        private string _texturePath;
        private string _hoverTexturePath;

        private BoundingRectangle _collider;

        public Button(Action onButtonClick, Vector2 pos, bool centered, string texturePath, string hoverTexurePath = "")
        {
            _onButtonClick = onButtonClick;
            _centered = centered;
            Position = pos;
            _texturePath = texturePath;
            _hoverTexturePath = hoverTexurePath;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>(_texturePath);
            if (_centered) Position -= new Vector2(_texture.Width / 2, 0);
            if (_hoverTexturePath != "") _hoverTexture = content.Load<Texture2D>(_hoverTexturePath);
            else _hoverTexture = _texture;
        }

        public void Update(GameTime gameTime)
        {
            if(!Enabled) return;
            _collider = new BoundingRectangle(Position.X, Position.Y, _texture.Width, _texture.Height);

            if(Mode == ButtonMode.Press)
            {
                if (IsHovering && InputManager.Mouse1Clicked)
                {
                    _clicked = true;
                }
                if (IsHovering && InputManager.Mouse1Up && IsHeldDown) _onButtonClick?.Invoke();
                if (!IsHovering || InputManager.Mouse1Up)
                {
                    _clicked = false;
                }
            }
            else
            {
                if (IsHovering)
                {
                    _clicked = true;
                }
                if (IsHovering && InputManager.Mouse1Clicked) _onButtonClick?.Invoke();
                if (!IsHovering)
                {
                    _clicked = false;
                }
            }
            IsHovering = InputManager.IsMouseInRectangle(_collider);
            IsHeldDown = IsHovering && _clicked;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(IsHeldDown ? _hoverTexture : _texture, Position, null, Enabled ? Color.White : Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, LayerDepth);
        }
    }

    public enum ButtonMode { 
        Press, Hover
    }
}
