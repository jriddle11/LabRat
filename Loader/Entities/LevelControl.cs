using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework;

namespace LabRat
{
    public class LevelControl : IGameObject
    {
        public int Level { get; private set; }
        public Tilemap Tilemap;

        private Exit _exit;
        private LevelDesigner _handler = new();

        private string _rootDir;
        private Action<bool> _backToMenu;
        private Action _save;

        private VideoPlayer _videoPlayer = new();
        private Video _video;
        private bool _playing = false;

        private Texture2D _bgtexture;
        private Texture2D _border;
        private Texture2D _currentVideoFrame;

        private int _maxLevels = 3;
        public LevelControl(int maxLevels, Action<bool> backToMenu, Action saveGame)
        {
            Tilemap = new("tileload.txt");
            _maxLevels = maxLevels;
            _backToMenu = backToMenu;
            _save = saveGame;
            _exit = new Exit(Vector2.Zero, HandleExit);
        }

        private void HandleExit()
        {
            if (InputManager.Recording) InputManager.StopRecording();
            Context.Player.Frozen = true;
            Context.Camera.Cover.SetNextAction(LeaveLevel);
            Context.Camera.Cover.FadeIn();
        }

        private void LeaveLevel()
        {
            if (Level < _maxLevels)
            {
                Level += 1;
                _save?.Invoke();
                LoadLevel(Level);
            }
            else
            {
                _backToMenu?.Invoke(true);
            }
        }

        public void LoadContent(ContentManager content)
        {
            _handler.LoadContent(content);
            _exit.LoadContent(content);
            Tilemap.LoadContent(content);
            _rootDir = content.RootDirectory;
            Tilemap.Offset = new Vector2(0, 0);
            _video = content.Load<Video>("lab");
            _bgtexture = content.Load<Texture2D>("tiles_bg");
            _border = content.Load<Texture2D>("border");
            _playing = true;
        }

        public void LoadLevel(int level)
        {
            Context.Camera.Cover.FadeOut();
            Level = level;
            Tilemap.LoadMap("levels/level" + level + ".txt", _rootDir);
            _handler.SetupFor(level);
            InputManager.StartRecording();
            _exit.UpdatePosition(Tilemap.EndPosition);
            Context.Player.UpdateStartPosition(Tilemap.StartPosition);
            _exit.Exited = false;
            Context.Player.Frozen = false;
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.PressedEscape && !Context.Camera.Cover.FadedIn)
            {
                _backToMenu?.Invoke(false);
            }
            if (_playing)
            {
                if (_videoPlayer.State == MediaState.Stopped)
                {
                    _videoPlayer.Play(_video);
                }
            }
            _handler.Update(gameTime);
            _exit.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_bgtexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(_border, new Vector2(1280, 600), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, .999f);
            if (_videoPlayer.State == MediaState.Playing)
            {
                _currentVideoFrame = _videoPlayer.GetTexture();

            }
            if(_currentVideoFrame != null)
            {
                spriteBatch.Draw(_currentVideoFrame, new Vector2(1280,600), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
            _handler.Draw(spriteBatch);
            Tilemap.Draw(spriteBatch);
            _exit.Draw(spriteBatch);
        }
    }
}
