using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class PopupMenu : IGameObject
    {
        public Vector2 Position;
        public bool Enabled;

        public bool CanReload = true;
        public bool CanRefresh = true;

        private bool _opened = false;

        private Texture2D _popupTop;
        private Texture2D _popupBottom;
        private Texture2D _popupBtn;

        private Image _refreshImage;
        private Image _restartImage;

        private Button _refreshBtn;
        private Button _spaceBtn;
        private Button _restartBtn;

        public event EventHandler Refresh;
        public event EventHandler Reload;

        public void LoadContent(ContentManager content)
        {
            _popupTop = content.Load<Texture2D>("popup_top");
            _popupBottom = content.Load<Texture2D>("popup_bottom");
            _popupBtn = content.Load<Texture2D>("popup_button");

            _refreshBtn = new(HandleRefresh, Position, false, "popup_button", "popup_button_press");
            _refreshBtn.LoadContent(content);
            _refreshBtn.LayerDepth = 0.01f;
            _refreshBtn.Mode = ButtonMode.Hover;

            _refreshImage = new(Position, "refresh_text");
            _refreshImage.LoadContent(content);
            _refreshImage.LayerDepth = 0f;

            _spaceBtn = new(null, Position, false, "popup_button", "popup_button");
            _spaceBtn.LoadContent(content);
            _spaceBtn.LayerDepth = 0.01f;
            _spaceBtn.Mode = ButtonMode.Hover;

            _restartBtn = new(HandleReload, Position, false, "popup_button", "popup_button_press");
            _restartBtn.LoadContent(content);
            _restartBtn.LayerDepth = 0.01f;
            _restartBtn.Mode = ButtonMode.Hover;

            _restartImage = new(Position, "restart_text");
            _restartImage.LoadContent(content);
            _restartImage.LayerDepth = 0f;
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

        private void HandleReload()
        {
            Reload?.Invoke(this, EventArgs.Empty);
        }

        public bool IsHovering()
        {
            return InputManager.IsMouseInRectangle(new BoundingRectangle(Position.X, Position.Y, _popupTop.Width, _popupTop.Height * 3));
        }

        public void Update(GameTime gameTime)
        {
            if (!Enabled) return;
            if (CanRefresh)
            {
                _refreshImage.Color = Color.Black;
                _refreshBtn.Update(gameTime);
            }
            else
            {
                _refreshBtn.Reset();
                _refreshImage.Color = Color.DarkGray;
            }

            if(CanReload)
            {
                _restartImage.Color = Color.Black;
                _restartBtn.Update(gameTime);
            }
            else
            {
                _restartBtn.Reset();
                _restartImage.Color = Color.DarkGray;
            }
            var buttonsPos = new Vector2(Position.X, Position.Y + _popupTop.Height);
            var offset = new Vector2(0, _popupBtn.Height);
            _refreshBtn.Position = buttonsPos;
            _refreshImage.Position = buttonsPos;

            _spaceBtn.Position = _refreshBtn.Position + offset;
            _restartBtn.Position = _spaceBtn.Position + offset;
            _restartImage.Position = _restartBtn.Position;
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
            _spaceBtn.Draw(spriteBatch);
            _restartBtn.Draw(spriteBatch);
            _restartImage.Draw(spriteBatch);
            spriteBatch.Draw(_popupBottom, new Vector2(Position.X, Position.Y + _popupTop.Height + _popupBtn.Height * 3), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
