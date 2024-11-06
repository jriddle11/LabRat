using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class LevelControl : IGameObject
    {
        public int Level { get; private set; }
        public Tilemap Tilemap;

        private LabRatGame _game;

        private Exit _exit;

        private string _rootDir;
        private Action _backToMenu;
        private Action _save;

        private Form _moveForm;
        private Form _jumpForm;
        private Form _refreshForm;
        private Form _cloneForm;

        private const int MaxLevels = 3;
        public LevelControl(LabRatGame game, Action backToMenu, Action saveGame)
        {
            Tilemap = new("tileload.txt");
            _backToMenu = backToMenu;
            _save = saveGame;
            _exit = new Exit(Vector2.Zero, HandleExit);
            _game = game;
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
            LoadTutorialForms(content);
            _exit.LoadContent(content);
            Tilemap.LoadContent(content);
            _rootDir = content.RootDirectory;
            Tilemap.Offset = new Vector2(0, 0);
        }

        private void LoadTutorialForms(ContentManager content)
        {
            var _moveFormPos = new Vector2(600, 1200);
            var _jumpFormPos = new Vector2(2500, 1200);
            var _refreshFormPos = new Vector2(2200, 800);
            var _cloneFormPos = new Vector2(2200, 1100);

            _moveForm = new(_moveFormPos, FormType.Small, "Tutorial.exe");
            _moveForm.AddImage(new Image(_moveFormPos, "sign_move"));
            _moveForm.LoadContent(content);

            _jumpForm = new(_jumpFormPos, FormType.Small, "Tutorial.exe");
            _jumpForm.AddImage(new Image(_jumpFormPos, "sign_jump"));
            _jumpForm.LoadContent(content);

            _refreshForm = new(_refreshFormPos, FormType.Small, "Tutorial.exe");
            _refreshForm.AddImage(new Image(_refreshFormPos, "sign_time"));
            _refreshForm.LoadContent(content);

            _cloneForm = new(_cloneFormPos, FormType.Small, "Tutorial.exe");
            _cloneForm.AddImage(new Image(_cloneFormPos, "sign_clone"));
            _cloneForm.LoadContent(content);
        }

        private void ResetForms()
        {
            _moveForm.Enabled = true;
            _jumpForm.Enabled = true;
            _refreshForm.Enabled = true;
            _cloneForm.Enabled = true;
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
            ResetForms();
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.PressedEscape)
            {
                _backToMenu?.Invoke();
            }
            UpdateTutorialForms(gameTime);
            _exit.Update(gameTime);
        }

        private void UpdateTutorialForms(GameTime gameTime)
        {
            _moveForm.Update(gameTime);
            _jumpForm.Update(gameTime);
            _refreshForm.Update(gameTime);
            _cloneForm.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawTutorialForms(spriteBatch);
            Tilemap.Draw(spriteBatch);
            _exit.Draw(spriteBatch);
        }
        
        private void DrawTutorialForms(SpriteBatch spriteBatch)
        {
            if (Level == 1)
            {
                _moveForm.Draw(spriteBatch);
                _jumpForm.Draw(spriteBatch);
            }
            if(Level == 2)
            {
                _refreshForm.Draw(spriteBatch);
                _cloneForm.Draw(spriteBatch);
            }
        }
    }
}
