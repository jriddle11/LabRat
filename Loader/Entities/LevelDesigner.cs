using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabRat
{
    public class LevelDesigner : IGameObject
    {
        private ContentManager _content;
        private List<ButtonPad> _buttons = new();
        private List<Form> _tutorialForms = new();
        private Vector2[] _positions = new Vector2[10];

        public LevelDesigner()
        {
            for(int i = 0; i < 10; i++)
            {
                _buttons.Add(new ButtonPad());
            }
            for (int i = 0; i < 10; i++)
            {
                _tutorialForms.Add(new Form(Vector2.Zero, FormType.Small, "Tutorial.exe"));
            }
        }

        private void Reset()
        {
            Context.Player.CloneAmount = 1;
            foreach(var button in _buttons) button.Reset();
            foreach(var form in _tutorialForms) form.Reset();
        }

        public void SetupFor(int level)
        {
            Reset();
            switch (level)
            {
                case 1:
                    _positions[0] = new Vector2(600, 1200);
                    _positions[1] = new Vector2(2500, 1200);
                    _tutorialForms[0].Position = _positions[0];
                    _tutorialForms[0].AddImage(new Image(_positions[0], "forms/sign_move"));
                    _tutorialForms[0].LoadContent(_content);
                    _tutorialForms[0].Enabled = true;

                    _tutorialForms[1].Position = _positions[1];
                    _tutorialForms[1].AddImage(new Image(_positions[1], "forms/sign_jump"));
                    _tutorialForms[1].LoadContent(_content);
                    _tutorialForms[1].Enabled = true;
                    break;
                case 2:
                    _positions[0] = new Vector2(2200, 800);
                    _positions[1] = new Vector2(2200, 1100);
                    _tutorialForms[0].Position = _positions[0];
                    _tutorialForms[0].AddImage(new Image(_positions[0], "forms/sign_time"));
                    _tutorialForms[0].LoadContent(_content);
                    _tutorialForms[0].Enabled = true;

                    _tutorialForms[1].Position = _positions[1];
                    _tutorialForms[1].AddImage(new Image(_positions[1], "forms/sign_clone"));
                    _tutorialForms[1].LoadContent(_content);
                    _tutorialForms[1].Enabled = true;
                    break;
                case 3:
                    _positions[0] = new Vector2(600, 1200);
                    _tutorialForms[0].Position = _positions[0];
                    _tutorialForms[0].AddImage(new Image(_positions[0], "forms/sign_ride"));
                    _tutorialForms[0].LoadContent(_content);
                    _tutorialForms[0].Enabled = true;
                    break;
                case 4:
                    _positions[0] = new Vector2(17 * 128, 12 * 128);
                    _positions[1] = new Vector2(22 * 128, 10 * 128);

                    _buttons[0].Position = _positions[0];
                    _buttons[0].AddConnection(new Laser(_positions[1], 3));
                    _buttons[0].LoadContent(_content);
                    _buttons[0].Enabled = true;
                    break;
                case 5:
                    _positions[0] = new Vector2(8 * 128, 12 * 128);
                    _positions[1] = new Vector2(8 * 128, 6 * 128);

                    for(int i = 0; i < 4; i++)
                    {
                        _buttons[i].Position = _positions[0] + new Vector2(i * 3 * 128, 0);
                        _buttons[i].AddConnection(new Laser(_positions[1] + new Vector2(i * 3 * 128, 0), 5));
                        _buttons[i].LoadContent(_content);
                        _buttons[i].Enabled = true;
                    }
                    break;
                case 6:
                    Context.Player.CloneAmount = 2;
                    _positions[0] = new Vector2(8 * 128, 12 * 128);
                    _positions[1] = new Vector2(16 * 128, 3 * 128);
                    _positions[2] = new Vector2(17 * 128, 3 * 128);
                    _positions[3] = new Vector2(15 * 128, 5 * 128);

                    _buttons[0].Position = _positions[0];
                    _buttons[0].AddConnection(new Laser(_positions[1], 10));
                    _buttons[0].LoadContent(_content);
                    _buttons[0].Enabled = true;

                    _buttons[1].Position = _positions[3];
                    _buttons[1].AddConnection(new Laser(_positions[2], 10));
                    _buttons[1].LoadContent(_content);
                    _buttons[1].Enabled = true;
                    break;
                case 7:
                    Context.Player.CloneAmount = 5;
                    break;
                case 8:
                    Context.Player.CloneAmount = 5;
                    _positions[0] = new Vector2(300, 1000);
                    _tutorialForms[0].Position = _positions[0];
                    _tutorialForms[0].AddImage(new Image(_positions[0], "forms/sign_patience"));
                    _tutorialForms[0].LoadContent(_content);
                    _tutorialForms[0].Enabled = true;
                    break;
            }
        }

        public void LoadContent(ContentManager content)
        {
            _content = content;
            foreach(var button in _buttons) button.LoadContent(content);
            foreach(var form in _tutorialForms) form.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            foreach(ButtonPad button in _buttons) button.Update(gameTime);
            foreach(var form in _tutorialForms) form.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var button in _buttons) button.Draw(spriteBatch);
            foreach(var form in _tutorialForms) form.Draw(spriteBatch);
        }
    }
}
