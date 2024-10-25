using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class LevelControl : IGameObject
    {
        public int Level { get; private set; }
        public Tilemap Tilemap;

        private Exit _exit;

        private string _rootDir;
        private Action _backToMenu;
        private Action _save;

        private Texture2D _signMove;
        private Texture2D _signJump;
        private Texture2D _signRefresh;
        private Texture2D _signClone;

        private const int MaxLevels = 3;
        public LevelControl(Action backToMenu, Action saveGame)
        {
            Tilemap = new("tileload.txt");
            _backToMenu = backToMenu;
            _save = saveGame;
            _exit = new Exit(Vector2.Zero, HandleExit);
        }

        private void HandleExit()
        {
            if(Level < MaxLevels)
            {
                Level += 1;
                _save?.Invoke();
                LoadLevel(Level);
            }
            else
            {
                _backToMenu?.Invoke();
            }
        }

        public void LoadContent(ContentManager content)
        {
            _signMove = content.Load<Texture2D>("sign_move");
            _signJump = content.Load<Texture2D>("sign_jump");
            _signRefresh = content.Load<Texture2D>("sign_time");
            _signClone = content.Load<Texture2D>("sign_clone");

            _exit.LoadContent(content);
            Tilemap.LoadContent(content);
            _rootDir = content.RootDirectory;
            Tilemap.Offset = new Vector2(0, 0);
        }

        public void LoadLevel(int level)
        {
            Level = level;
            Tilemap.LoadMap("level" + level + ".txt", _rootDir);
            if(InputManager.Recording)InputManager.StopRecording();
            InputManager.StartRecording();
            _exit.UpdatePosition(Tilemap.EndPosition);
            Context.Player.UpdateStartPosition(Tilemap.StartPosition);
            _exit.Exited = false;
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.PressedEscape)
            {
                _backToMenu?.Invoke();
            }
            _exit.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawTutorialSigns(spriteBatch);
            Tilemap.Draw(spriteBatch);
            _exit.Draw(spriteBatch);
        }
        
        private void DrawTutorialSigns(SpriteBatch spriteBatch)
        {
            if (Level == 1)
            {
                spriteBatch.Draw(_signMove, new Vector2(600, 1200), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.Draw(_signJump, new Vector2(2500, 1200), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
            if(Level == 2)
            {
                spriteBatch.Draw(_signRefresh, new Vector2(2200, 800), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.Draw(_signClone, new Vector2(2200, 1100), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
        }
    }
}
