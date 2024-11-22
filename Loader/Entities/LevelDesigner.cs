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
            foreach(var button in _buttons) button.Reset();
            foreach(var form in _tutorialForms) form.Reset();
        }

        public void SetupFor(int level)
        {
            Reset();
            switch (level)
            {
                case 1:
                    var moveFormPos = new Vector2(600, 1200);
                    var jumpFormPos = new Vector2(2500, 1200);
                    _tutorialForms[0].Position = moveFormPos;
                    _tutorialForms[0].AddImage(new Image(moveFormPos, "forms/sign_move"));
                    _tutorialForms[0].LoadContent(_content);
                    _tutorialForms[0].Enabled = true;

                    _tutorialForms[1].Position = jumpFormPos;
                    _tutorialForms[1].AddImage(new Image(jumpFormPos, "forms/sign_jump"));
                    _tutorialForms[1].LoadContent(_content);
                    _tutorialForms[1].Enabled = true;
                    break;
                case 2:
                    var refreshFormPos = new Vector2(2200, 800);
                    var cloneFormPos = new Vector2(2200, 1100);
                    _tutorialForms[0].Position = refreshFormPos;
                    _tutorialForms[0].AddImage(new Image(refreshFormPos, "forms/sign_time"));
                    _tutorialForms[0].LoadContent(_content);
                    _tutorialForms[0].Enabled = true;

                    _tutorialForms[1].Position = cloneFormPos;
                    _tutorialForms[1].AddImage(new Image(cloneFormPos, "forms/sign_clone"));
                    _tutorialForms[1].LoadContent(_content);
                    _tutorialForms[1].Enabled = true;
                    break;
                case 3:
                    var rideFormPos = new Vector2(600, 1200);
                    _tutorialForms[0].Position = rideFormPos;
                    _tutorialForms[0].AddImage(new Image(rideFormPos, "forms/sign_ride"));
                    _tutorialForms[0].LoadContent(_content);
                    _tutorialForms[0].Enabled = true;
                    break;
                case 4:
                    var laserPos = new Vector2(22 * 128, 10 * 128);
                    var buttonPos = new Vector2(17 * 128, 12 * 128);

                    _buttons[0].Position = buttonPos;
                    _buttons[0].AddConnection(new Laser(laserPos, 3));
                    _buttons[0].LoadContent(_content);
                    _buttons[0].Enabled = true;
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
