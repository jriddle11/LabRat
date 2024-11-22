using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace LabRat
{
    public class LabRatGame : Game
    {
        public static LabRatGame Instance;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private MainMenu _mainMenu;
        private readonly Vector2 _camOffset = new Vector2(0, -200);
        private LevelControl _levelControl;
        private List<Character> _characters = new();
        private List<Character> _newCharacters = new();
        private List<Character> _deleteCharacters = new();
        private bool _playing = false;

        private readonly Color _bgColor = new Color(30,30,30);

        private int _levelsCompleted = 0;

        private const int MaxLevels = 4;

        public LabRatGame()
        {
            Debug.WriteLine("We have begun");
            Instance = this;
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public void HandleSpawnClone(PlayerClone c)
        {
            _newCharacters.Add(c);
        }

        public void HandleDespawnClone(PlayerClone c)
        {
            _deleteCharacters.Add(c);
        }

        public void HandleBackToMenu()
        {
            SoundManager.PlaySongMuffled();
            Context.Player.Reset();
            _playing = false;
            Context.Camera.Reset();
            Context.Camera.ForceZoom(1f);
            _newCharacters.Clear();
            foreach(Character c in _characters)
            {
                if(c.ID != int.MaxValue)
                {
                    _deleteCharacters.Add(c);
                }
            }
        }

        public void HandleLevelStart()
        {
            if (_mainMenu.LevelSelected == 0) return;
            _playing = true;
            _levelControl.LoadLevel(_mainMenu.LevelSelected);
            Context.Camera.Boundaries = new Vector2(_levelControl.Tilemap.MapPixelWidth, _levelControl.Tilemap.MapPixelHeight);
            Context.Camera.ForceZoom(0.7f);
            SoundManager.PlaySong();
        }

        protected override void Initialize()
        {
            //Window.IsBorderless = true;
            base.Initialize();
        }

        private void LoadGame()
        {
            _levelsCompleted = SaveManager.LoadGame();
        }

        private void SaveGame()
        {
            if (_levelControl != null)
            {
                if (_levelControl.Level - 1 > _levelsCompleted)
                {
                    _levelsCompleted = _levelControl.Level - 1;
                }
                _mainMenu.UpdateLevels(_levelsCompleted);
            }
            SaveManager.SaveGame(_levelsCompleted);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //DebugManager.LoadContent(this);
            LoadGame();
            SoundManager.LoadContent(Content);

            _mainMenu = new MainMenu(new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 - 200, 200), MaxLevels, _levelsCompleted, Exit, HandleLevelStart);
            _mainMenu.LoadContent(Content);
            Context.Camera = new Camera(_graphics);
            _levelControl = new LevelControl(MaxLevels, HandleBackToMenu, SaveGame);
            _levelControl.LoadContent(Content);
            Context.Player = new Player(new Vector2(500,1500), HandleSpawnClone, HandleDespawnClone);
            Context.Player.LoadContent(Content);

            _characters.Add(Context.Player);
        }


        protected override void Update(GameTime gameTime)
        {
            InputManager.Update(gameTime);
            if (InputManager.Mouse1Clicked || InputManager.Mouse2Clicked)
            {
                SoundManager.PlayMouseClick();
            }
            if (!_playing)
            {
                _mainMenu.Update(gameTime);
                return;
            }

            _levelControl.Update(gameTime);

            AddNewCharacters();
            DeleteCharacters();
            foreach (var character in _characters) 
            {
                character.Update(gameTime);
                if(_newCharacters.Contains(character)) _newCharacters.Remove(character);
            }

            PhysicsHelper.ResolveClonePhysics(_characters);
            CollisionHelper.ResolveCollisions(_characters, _levelControl.Tilemap.Colliders, 3);

            Context.Camera.Follow(Context.Player.Position + _camOffset);

            base.Update(gameTime);

        }

        private void AddNewCharacters()
        {
            foreach (var newCharacter in _newCharacters)
            {
                if (!_characters.Contains(newCharacter)) _characters.Add(newCharacter);
            }
            _newCharacters.Clear();
        }

        private void DeleteCharacters()
        {
            foreach (var character in _deleteCharacters)
            {
                if (_characters.Contains(character)) _characters.Remove(character);
            }
            _deleteCharacters.Clear();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_bgColor);
            if (_playing)
            {
                _spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront, transformMatrix: Context.Camera.Transform);
                _levelControl.Draw(_spriteBatch);
                foreach (Character character in _characters) character.Draw(_spriteBatch);
            }
            else
            {
                _spriteBatch.Begin(transformMatrix: Context.Camera.Transform);
                _mainMenu.Draw(_spriteBatch);

            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
