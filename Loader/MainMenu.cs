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

        private bool _creditsOpen;
        private bool _optionsOpen;
        private bool _levelsOpen;

        private Texture2D _menuBg;
        private Texture2D _menuBgBig;
        private List<Button> _mainButtons = new();
        private List<Button> _optionButtons = new();
        private List<Button> _levelButtons = new();
        private List<Button> _creditButtons = new();
        private List<Text> _mainTexts = new();
        private List<Text> _creditTexts = new();
        private List<Text> _optionTexts = new();
        private List<Text> _levelTexts = new();

        private Color _headerFontColor = Color.White;
        private Color _bodyFontColor = Color.Black;

        private Action _quit;
        private Action _startLevel;

        private int _levels;
        private int _highestLevel;

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
            _menuBg = content.Load<Texture2D>("menu_bg");
            _menuBgBig = content.Load<Texture2D>("menu_bg_big");

            LoadMainFormContent(content);
            LoadCreditFormContent(content);
            LoadOptionsFormContent(content);
            LoadLevelFormContent(content);
        }

        private void LoadMainFormContent(ContentManager content)
        {
            _mainButtons.Add(new Button(_quit, Position + new Vector2(_menuBg.Width - 23, 5), true, "menu_close_btn", "menu_close_btn_press"));
            _mainButtons.Add(new Button(HandleStartPress, Position + new Vector2(_menuBg.Width / 2, 100), true, "menu_btn", "menu_btn_press"));
            _mainButtons.Add(new Button(HandleOptionsPress, Position + new Vector2(_menuBg.Width / 2, 200), true, "menu_btn", "menu_btn_press"));
            _mainButtons.Add(new Button(HandleCreditsPress, Position + new Vector2(_menuBg.Width / 2, 300), true, "menu_btn", "menu_btn_press"));

            _mainTexts.Add(new Text(Position + new Vector2(80, 5), "LabRat.exe", _headerFontColor, true));
            _mainTexts.Add(new Text(Position + new Vector2(_menuBg.Width / 2, 110), "START", _headerFontColor, true));
            _mainTexts.Add(new Text(Position + new Vector2(_menuBg.Width / 2, 210), "OPTIONS", _headerFontColor, true));
            _mainTexts.Add(new Text(Position + new Vector2(_menuBg.Width / 2, 310), "CREDITS", _headerFontColor, true));

            foreach (Button button in _mainButtons) button.LoadContent(content);
            foreach (Text text in _mainTexts) text.LoadContent(content);
        }

        private void LoadOptionsFormContent(ContentManager content)
        {
            _optionButtons.Add(new Button(HandleOptionsPress, _optionsPos + new Vector2(_menuBg.Width - 23, 5), true, "menu_close_btn", "menu_close_btn_press"));
            _optionTexts.Add(new Text(_optionsPos + new Vector2(13, 5), "Options", _headerFontColor, false));
            _optionTexts.Add(new Text(_optionsPos + new Vector2(_menuBg.Width / 2, 210), "Under  Construction", _bodyFontColor, true));

            foreach (Button button in _optionButtons) button.LoadContent(content);
            foreach (Text text in _optionTexts) text.LoadContent(content);
        }

        private void LoadLevelFormContent(ContentManager content)
        {
            _levelButtons.Add(new Button(HandleStartPress, _levelsPos + new Vector2(_menuBgBig.Width - 23, 5), true, "menu_close_btn", "menu_close_btn_press"));
            _levelTexts.Add(new Text(_levelsPos + new Vector2(13, 5), "Levels", _headerFontColor, false));

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

                _levelTexts.Add(text);
                _levelButtons.Add(button);
                
            
            }

            foreach (Button button in _levelButtons) button.LoadContent(content);
            foreach (Text text in _levelTexts) text.LoadContent(content);
        }

        private void UpdateLevelButtons()
        {
            for (int i = 0; i <_levelButtons.Count; i++)
            {
                if (i - 1 > _highestLevel)
                {
                    _levelButtons[i].Enabled = false;
                    _levelTexts[i].Enabled = false;
                }
                else
                {
                    _levelButtons[i].Enabled = true;
                    _levelTexts[i].Enabled = true;
                }
            }
        }

        private void LoadCreditFormContent(ContentManager content)
        {
            _creditButtons.Add(new Button(HandleCreditsPress, _creditPos + new Vector2(_menuBg.Width - 23, 5), true, "menu_close_btn", "menu_close_btn_press"));
            _creditTexts.Add(new Text(_creditPos + new Vector2(13, 5), "Credits", _headerFontColor, false));
            _creditTexts.Add(new Text(_creditPos + new Vector2(_menuBg.Width / 2, 110), "Created   by", _bodyFontColor, true));
            _creditTexts.Add(new Text(_creditPos + new Vector2(_menuBg.Width / 2, 210), "Joshua   Riddle", _bodyFontColor, true));

            foreach (Button button in _creditButtons) button.LoadContent(content);
            foreach(Text text in _creditTexts) text.LoadContent(content);
        }

        private void HandleStartPress()
        {
            _levelsOpen = !_levelsOpen;
        }

        private void HandleLevelPress(int level)
        {
            LevelSelected = level;
            _startLevel?.Invoke();
            _levelsOpen = false;
        }

        private void HandleOptionsPress()
        {
            _optionsOpen = !_optionsOpen;
        }

        private void HandleCreditsPress()
        {
            _creditsOpen = !_creditsOpen;
        }

        public void Update(GameTime gameTime)
        {
            if (_levelsOpen)
            {
                foreach (Button button in _levelButtons) button.Update(gameTime);
            }
            else
            {
                foreach (Button button in _mainButtons) button.Update(gameTime);
            }

            if (_creditsOpen)
            {
                foreach(Button btn in _creditButtons) btn.Update(gameTime);
            }

            if (_optionsOpen)
            {
                foreach (Button btn in _optionButtons) btn.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_levelsOpen)
            {
                //Level window
                spriteBatch.Draw(_menuBgBig, _levelsPos, Color.White);
                foreach (Button button in _levelButtons) button.Draw(spriteBatch);
                foreach (Text text in _levelTexts) text.Draw(spriteBatch);
            }
            else
            {
                //Main window
                spriteBatch.Draw(_menuBg, Position, Color.White);
                foreach (Button button in _mainButtons) button.Draw(spriteBatch);
                foreach (Text text in _mainTexts) text.Draw(spriteBatch);
            }

            //Credits window
            if (_creditsOpen)
            {
                spriteBatch.Draw(_menuBg, _creditPos, Color.White);
                foreach (Button btn in _creditButtons) btn.Draw(spriteBatch);
                foreach(Text text in _creditTexts) text.Draw(spriteBatch);
            }

            //Options window
            if (_optionsOpen)
            {
                spriteBatch.Draw(_menuBg, _optionsPos, Color.White);
                foreach (Button btn in _optionButtons) btn.Draw(spriteBatch);
                foreach (Text text in _optionTexts) text.Draw(spriteBatch);
            }
        }
    }
}
