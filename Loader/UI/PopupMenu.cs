using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class PopupMenu : IGameObject
    {
        public Vector2 Position;
        public bool Enabled;

        public bool CanRefresh = true;

        private bool _opened = false;

        private Texture2D _popupTop;
        private Texture2D _popupBottom;
        private Texture2D _popupBtn;

        private Image _refreshImage;

        Button _refreshBtn;

        public event EventHandler Refresh;

        public void LoadContent(ContentManager content)
        {
            _popupTop = content.Load<Texture2D>("popup_top");
            _popupBottom = content.Load<Texture2D>("popup_bottom");
            _popupBtn = content.Load<Texture2D>("popup_button");

            _refreshBtn = new(HandleRefresh, Position, false, "popup_button", "popup_button_press");
            _refreshBtn.LoadContent(content);
            _refreshBtn.LayerDepth = 0f;
            _refreshBtn.Mode = ButtonMode.Hover;

            _refreshImage = new(Position, "refresh_text");
            _refreshImage.LoadContent(content);
            _refreshImage.LayerDepth = 0f;
        }

        public void Open()
        {
            _opened = true;
        }

        public void Close()
        {
            _opened = false;
            Enabled = false;
        }

        private void HandleRefresh()
        {
            Refresh?.Invoke(this, EventArgs.Empty);
        }

        public bool IsHovering()
        {
            return InputManager.IsMouseInRectangle(new BoundingRectangle(Position.X, Position.Y, _popupTop.Width, _popupTop.Height * 3));
        }

        public void Update(GameTime gameTime)
        {
            if (!Enabled) return;
            _refreshBtn.Update(gameTime);
            var refreshPos = new Vector2(Position.X, Position.Y + _popupTop.Height);
            _refreshBtn.Position = refreshPos;
            _refreshImage.Position = refreshPos;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(_opened && !Enabled)
            {
                Enabled = true;
                return;
            }
            if (!Enabled) return;
            spriteBatch.Draw(_popupTop, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _refreshBtn.Draw(spriteBatch);
            _refreshImage.Draw(spriteBatch);
            spriteBatch.Draw(_popupBottom, new Vector2(Position.X, Position.Y + _popupTop.Height + _popupBtn.Height), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
