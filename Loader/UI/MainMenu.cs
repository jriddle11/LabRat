using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class MainMenu : IGameObject
    {
        public int LevelSelected = 0;
        public Vector2 Position;
        private Vector2 _aboutPos => Position - new Vector2(550, 100);
        private Vector2 _creditsPos => Position - new Vector2(-550, -200);
        private Vector2 _levelsPos => Position - new Vector2(100, 0);

        private Form _mainForm;
        private Form _creditsForm;
        private Form _aboutForm;
        private Form _levelsForm;
        private Form _winForm;

        private Color _white = Color.White;
        private Color _black = new Color(60,60,60);

        private Action _quit;
        private Action _startLevel;

        private int _levels;
        private int _highestLevel;

        private bool _levelsOpen;

        private ContentManager _content;


        public MainMenu(Vector2 pos, int levels, int highestLevel, Action quit, Action startLevel)
        {
            Position = pos;
            _quit = quit;
            _levels = levels;
            _highestLevel = highestLevel;
            _startLevel = startLevel;
        }

        public void LoadContent(ContentManager content)
        {
            _content ??= content;
            LoadMainForm(content);
            LoadAboutForm(content);
            LoadCreditsForm(content);
            LoadLevelForm(content);
            LoadWinForm(content);
            SoundManager.PlaySongMuffled();
        }

        public void BackToMenu(int level)
        {
            _highestLevel = level;
            LoadContent(_content);
        }

        private void LoadWinForm(ContentManager content)
        {
            _winForm = new(Position, FormType.Standard, "Winner");
            _winForm.LayerDepth = 0.9f;
            _winForm.AddText(new Text(Position + new Vector2(_winForm.Width / 2, 110), "YOU   WIN", _black, true));
            _winForm.LoadContent(content);
            _winForm.Enabled = false;
        }

        private void LoadMainForm(ContentManager content)
        {
            _mainForm = new(Position, FormType.Standard, "Ultimate   Mouse");
            _mainForm.AddButton(new Button(HandleStartPress, Position + new Vector2(_mainForm.Width / 2, 100), true, "forms/menu_btn", "forms/menu_btn_press", "forms/menu_btn_hover"));
            _mainForm.AddButton(new Button(HandleAboutPress, Position + new Vector2(_mainForm.Width / 2, 200), true, "forms/menu_btn", "forms/menu_btn_press", "forms/menu_btn_hover"));
            _mainForm.AddButton(new Button(HandleCreditsPress, Position + new Vector2(_mainForm.Width / 2, 300), true, "forms/menu_btn", "forms/menu_btn_press", "forms/menu_btn_hover"));
            _mainForm.AddText(new Text(Position + new Vector2(_mainForm.Width / 2, 110), "START", _black, true));
            _mainForm.AddText(new Text(Position + new Vector2(_mainForm.Width / 2, 210), "ABOUT", _black, true));
            _mainForm.AddText(new Text(Position + new Vector2(_mainForm.Width / 2, 310), "CREDITS", _black, true));
            _mainForm.LoadContent(content);
        }

        private void LoadCreditsForm(ContentManager content)
        {
            _creditsForm = new(_creditsPos, FormType.Standard, "Credits");
            _creditsForm.AddText(new Text(_creditsPos + new Vector2(20, 60), "'Zipper slider metalic' - vmmaniac", _black, false, .75f));
            _creditsForm.AddText(new Text(_creditsPos + new Vector2(20, 90), "'Mouse' - charliecatling", _black, false, .75f));
            _creditsForm.AddText(new Text(_creditsPos + new Vector2(20, 120), "'Square buttons cartoon menu' - chardingtont", _black, false, .75f));
            _creditsForm.AddText(new Text(_creditsPos + new Vector2(20, 150), "'Laboratory assets' - Anouk Paardekam", _black, false, .75f));
            _creditsForm.AddText(new Text(_creditsPos + new Vector2(20, 180), "'Chibi Base Mesh' - DuNguyn", _black, false, .75f));
            _creditsForm.AddText(new Text(_creditsPos + new Vector2(20, 210), "'Mouse Click' - Jurij", _black, false, .75f));
            _creditsForm.AddText(new Text(_creditsPos + new Vector2(20, 240), "'Upbeat Mission' - Cyberwave-Orchestra", _black, false, .75f));
            _creditsForm.AddText(new Text(_creditsPos + new Vector2(20, 270), "'Toggle Button On' - Milan Wulf", _black, false, .75f));
            _creditsForm.AddText(new Text(_creditsPos + new Vector2(20, 300), "'Jump Sound Effect' - freesound_community", _black, false, .75f));
            _creditsForm.LoadContent(content);
            _creditsForm.Enabled = false;
        }

        private void LoadLevelForm(ContentManager content)
        {
            _levelsForm = new(_levelsPos, FormType.Wide, "Levels");

            for(int i = 0; i < _levels; i++)
            {
                int level = i;
                var pos = _levelsPos + new Vector2(54 + ((i * 55) % 550), 70 + 70 * ((i / 10)));
                var button = new Button(() => HandleLevelPress(level + 1), pos, true, "forms/menu_level_btn", "forms/menu_level_btn_press", "forms/menu_level_btn_hover");
                var text = new Text(pos - (level > 8 ? new Vector2(12, 0) : new Vector2(5, 0)), "" + (level + 1), _black, false);
                if (level > _highestLevel)
                {
                    button.Enabled = false;
                    text.Enabled = false;
                }

                _levelsForm.AddButton(button);
                _levelsForm.AddText(text);
            
            }

            _levelsForm.LoadContent(content);
            _levelsForm.Enabled = false;
        }

        private void LoadAboutForm(ContentManager content)
        {
            _aboutForm = new(_aboutPos, FormType.Standard, "About");
            _aboutForm.AddText(new Text(_aboutPos + new Vector2(_aboutForm.Width / 2, 110), "Created   by", _black, true));
            _aboutForm.AddText(new Text(_aboutPos + new Vector2(_aboutForm.Width / 2, 210), "Josh   Riddle", _black, true));

            _aboutForm.LoadContent(content);
            _aboutForm.Enabled = false;
        }

        private void HandleStartPress()
        {
            _levelsOpen = true;
            _mainForm.Enabled = false;
            _levelsForm.Enabled = true;
        }

        private void HandleLevelPress(int level)
        {

            Reset();
            LevelSelected = level;
            _startLevel?.Invoke();
        }

        public void Reset()
        {
            _winForm.Enabled = false;
            _mainForm.Enabled = true;
            _levelsForm.Enabled = false;
            _creditsForm.Enabled = false;
            _aboutForm.Enabled = false;
            _levelsOpen = false;
        }

        private void HandleCreditsPress()
        {
            _creditsForm.Enabled = true;
        }

        private void HandleAboutPress()
        {
            _aboutForm.Enabled = true;
        }

        public void Winner()
        {
            _winForm.Enabled = true;
        }

        public void Update(GameTime gameTime)
        {
            _levelsForm.Update(gameTime);
            _creditsForm.Update(gameTime);
            _aboutForm.Update(gameTime);
            if(!_winForm.Enabled)_mainForm.Update(gameTime);
            _winForm?.Update(gameTime);
            if(!_levelsForm.Enabled && _levelsOpen)
            {
                _levelsOpen = false;
                _mainForm.Enabled = true;
            }
            if (!_mainForm.Enabled && !_levelsForm.Enabled) _quit?.Invoke();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _levelsForm.Draw(spriteBatch);
            _mainForm.Draw(spriteBatch);
            _creditsForm.Draw(spriteBatch);
            _aboutForm.Draw(spriteBatch);
            _winForm.Draw(spriteBatch);
        }
    }
}
