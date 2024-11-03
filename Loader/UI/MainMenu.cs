using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class MainMenu : IGameObject
    {
        public int LevelSelected = 0;
        public Vector2 Position;
        private Vector2 _creditPos => Position - new Vector2(550, 100);
        private Vector2 _optionsPos => Position - new Vector2(-550, -200);
        private Vector2 _levelsPos => Position - new Vector2(100, 0);

        private Form _mainForm;
        private Form _optionsForm;
        private Form _creditsForm;
        private Form _levelsForm;

        private Color _headerFontColor = Color.White;
        private Color _bodyFontColor = Color.Black;

        private Action _quit;
        private Action _startLevel;

        private int _levels;
        private int _highestLevel;

        private bool _levelsOpen;

        public MainMenu(Vector2 pos, int levels, int highestLevel, Action quit, Action startLevel)
        {
            Position = pos;
            _quit = quit;
            _levels = levels;
            _highestLevel = highestLevel;
            _startLevel = startLevel;
        }

        public void UpdateLevels(int level)
        {
            _highestLevel = level;
            UpdateLevelButtons();
        }

        public void LoadContent(ContentManager content)
        {
            LoadMainForm(content);
            LoadCreditForm(content);
            LoadOptionsForm(content);
            LoadLevelForm(content);
        }

        private void LoadMainForm(ContentManager content)
        {
            _mainForm = new(Position, FormType.Standard, "LabRat.exe");
            _mainForm.AddButton(new Button(HandleStartPress, Position + new Vector2(_mainForm.Width / 2, 100), true, "menu_btn", "menu_btn_press"));
            _mainForm.AddButton(new Button(HandleOptionsPress, Position + new Vector2(_mainForm.Width / 2, 200), true, "menu_btn", "menu_btn_press"));
            _mainForm.AddButton(new Button(HandleCreditsPress, Position + new Vector2(_mainForm.Width / 2, 300), true, "menu_btn", "menu_btn_press"));
            _mainForm.AddText(new Text(Position + new Vector2(_mainForm.Width / 2, 110), "START", _headerFontColor, true));
            _mainForm.AddText(new Text(Position + new Vector2(_mainForm.Width / 2, 210), "OPTIONS", _headerFontColor, true));
            _mainForm.AddText(new Text(Position + new Vector2(_mainForm.Width / 2, 310), "CREDITS", _headerFontColor, true));
            _mainForm.LoadContent(content);
        }

        private void LoadOptionsForm(ContentManager content)
        {
            _optionsForm = new(_optionsPos, FormType.Standard, "Options");
            _optionsForm.AddText(new Text(_optionsPos + new Vector2(_optionsForm.Width / 2, 210), "Under  Construction", _bodyFontColor, true));
            _optionsForm.LoadContent(content);
            _optionsForm.Enabled = false;
        }

        private void LoadLevelForm(ContentManager content)
        {
            _levelsForm = new(_levelsPos, FormType.Wide, "Levels");

            for(int i = 0; i < _levels; i++)
            {
                int level = i;
                var pos = _levelsPos + new Vector2(54 + ((i * 55) % 550), 70 + 70 * ((i / 10)));
                var button = new Button(() => HandleLevelPress(level + 1), pos, true, "menu_level_btn", "menu_level_btn_press");
                var text = new Text(pos - (level > 8 ? new Vector2(12, 0) : new Vector2(5, 0)), "" + (level + 1), _headerFontColor, false);
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

        private void UpdateLevelButtons()
        {
            for (int i = 0; i < _levelsForm.Buttons.Count; i++)
            {
                if (i - 1 > _highestLevel)
                {
                    _levelsForm.Buttons[i].Enabled = false;
                    _levelsForm.Texts[i].Enabled = false;
                }
                else
                {
                    _levelsForm.Buttons[i].Enabled = true;
                    _levelsForm.Texts[i].Enabled = true;
                }
            }
        }

        private void LoadCreditForm(ContentManager content)
        {
            _creditsForm = new(_creditPos, FormType.Standard, "Credits");
            _creditsForm.AddText(new Text(_creditPos + new Vector2(_creditsForm.Width / 2, 110), "Created   by", _bodyFontColor, true));
            _creditsForm.AddText(new Text(_creditPos + new Vector2(_creditsForm.Width / 2, 210), "Joshua   Riddle", _bodyFontColor, true));

            _creditsForm.LoadContent(content);
            _creditsForm.Enabled = false;
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
            _mainForm.Enabled = true;
            _levelsForm.Enabled = false;
            _optionsForm.Enabled = false;
            _creditsForm.Enabled = false;
            _levelsOpen = false;
        }

        private void HandleOptionsPress()
        {
            _optionsForm.Enabled = true;
        }

        private void HandleCreditsPress()
        {
            _creditsForm.Enabled = true;
        }

        public void Update(GameTime gameTime)
        {
            _levelsForm.Update(gameTime);
            _optionsForm.Update(gameTime);
            _creditsForm.Update(gameTime);
            _mainForm.Update(gameTime);
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
            _optionsForm.Draw(spriteBatch);
            _creditsForm.Draw(spriteBatch);
        }
    }
}
