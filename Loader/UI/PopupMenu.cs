using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class PopupMenu : IGameObject
    {
        public Vector2 Position;
        public bool Enabled;
        public int RefreshCount = 1;

        public bool CanReload = true;
        public bool CanRefresh = true;

        private bool _opened = false;

        private Texture2D _popupTop;
        private Texture2D _popupBottom;
        private Texture2D _popupBtn;

        private Image _refreshImage;
        private Image _restartImage;

        private Text _refreshCountText;

        private Button _refreshBtn;
        private Button _spaceBtn;
        private Button _restartBtn;

        public event EventHandler Refresh;
        public event EventHandler Reload;

        private Vector2 _textOffset = new Vector2(250, 5);

        public void LoadContent(ContentManager content)
        {
            _popupTop = content.Load<Texture2D>("forms/popup_top");
            _popupBottom = content.Load<Texture2D>("forms/popup_bottom");
            _popupBtn = content.Load<Texture2D>("forms/popup_button");

            _refreshBtn = new(HandleRefresh, Position, false, "forms/popup_button", "forms/popup_button_press");
            _refreshBtn.LoadContent(content);
            _refreshBtn.LayerDepth = 0.01f;
            _refreshBtn.Mode = ButtonMode.Hover;

            _refreshImage = new(Position, "forms/refresh_text");
            _refreshImage.LoadContent(content);
            _refreshImage.LayerDepth = 0.001f;

            _refreshCountText = new(Position, RefreshCount.ToString(), Color.Black, true, 1);
            _refreshCountText.LoadContent(content);
            _refreshCountText.LayerDepth = 0.001f;

            _spaceBtn = new(null, Position, false, "forms/popup_button", "forms/popup_button");
            _spaceBtn.LoadContent(content);
            _spaceBtn.LayerDepth = 0.01f;
            _spaceBtn.Mode = ButtonMode.Hover;

            _restartBtn = new(HandleReload, Position, false, "forms/popup_button", "forms/popup_button_press");
            _restartBtn.LoadContent(content);
            _restartBtn.LayerDepth = 0.01f;
            _restartBtn.Mode = ButtonMode.Hover;

            _restartImage = new(Position, "forms/restart_text");
            _restartImage.LoadContent(content);
            _restartImage.LayerDepth = 0.001f;
            _restartImage.Color = Color.Black;
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
            if (RefreshCount > 0)
            {
                _refreshCountText.Set(RefreshCount.ToString(), Color.Black);
            }
            else
            {
                _refreshCountText.Set(RefreshCount.ToString(), Color.DarkGray);
            }

            _restartBtn.Update(gameTime);
            var buttonsPos = new Vector2(Position.X, Position.Y + _popupTop.Height);
            var offset = new Vector2(0, _popupBtn.Height);
            _refreshBtn.Position = buttonsPos;
            _refreshImage.Position = buttonsPos;
            _refreshCountText.Position = buttonsPos + _textOffset;

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
            spriteBatch.Draw(_popupTop, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.001f);
            _refreshBtn.Draw(spriteBatch);
            _refreshImage.Draw(spriteBatch);
            _spaceBtn.Draw(spriteBatch);
            _restartBtn.Draw(spriteBatch);
            _restartImage.Draw(spriteBatch);
            _refreshCountText.Draw(spriteBatch);
            spriteBatch.Draw(_popupBottom, new Vector2(Position.X, Position.Y + _popupTop.Height + _popupBtn.Height * 3), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.001f);
        }
    }
}
