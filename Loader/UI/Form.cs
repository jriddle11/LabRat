using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabRat
{
    public class Form : IGameObject
    {
        public Vector2 Position;
        public bool Enabled = true;

        public float LayerDepth = .98f;
        public static bool Initialized => _menuBg != null;
        public List<Button> Buttons = new();
        public List<Text> Texts = new();
        public List<Image> Images = new();

        private static Texture2D _menuBg;
        private static Texture2D _menuBgBig;
        private static Texture2D _menuBgSmall;
        private FormType _type;
        private string _title;

        public int Width => GetWidth();

        public Form(Vector2 pos, FormType type, string title)
        {
            Position = pos;
            _type = type;
            _title = title;
        }

        private void CloseForm()
        {
            Enabled = false;
        }

        public void LoadContent(ContentManager content)
        {
            _menuBg = content.Load<Texture2D>("forms/menu_bg");
            _menuBgBig = content.Load<Texture2D>("forms/menu_bg_big");
            _menuBgSmall = content.Load<Texture2D>("forms/menu_bg_small");

            Buttons.Add(new Button(CloseForm, Position + new Vector2(GetTexture().Width - 60, 0), false, "forms/menu_close_btn", "forms/menu_close_btn_press", "forms/menu_close_btn_hover"));
            Texts.Add(new Text(Position + new Vector2(13, 5), _title, Color.Black, false));

            foreach (Button button in Buttons) button.LoadContent(content);
            foreach(Text text in Texts) text.LoadContent(content);
            foreach(Image image in Images) image.LoadContent(content);
        }

        public void AddButton(Button button)
        {
            Buttons.Add(button);
        }

        public void AddImage(Image image)
        {
            Images.Add(image);
        }

        public void AddText(Text text)
        {
            Texts.Add(text);
        }

        public void Reset()
        {
            Buttons.Clear();
            Images.Clear();
            Texts.Clear();
            CloseForm();
        }

        public void Update(GameTime gameTime)
        {
            if (!Initialized || !Enabled) return;
            foreach(Button button in Buttons) button.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Initialized || !Enabled) return;
            spriteBatch.Draw(GetTexture(), Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, LayerDepth);
            foreach (Button button in Buttons) button.Draw(spriteBatch);
            foreach(Text text in Texts) text.Draw(spriteBatch);
            foreach(Image image in Images) image.Draw(spriteBatch);
        }

        private Texture2D GetTexture()
        {
            switch (_type)
            {
                case FormType.Standard:
                    return _menuBg;
                case FormType.Wide:
                    return _menuBgBig;
                case FormType.Small:
                    return _menuBgSmall;
                default:
                    return _menuBg;
            }
        }

        private int GetWidth()
        {
            switch (_type)
            {
                case FormType.Standard:
                    return 400;
                case FormType.Wide:
                    return 600;
                case FormType.Small:
                    return 400;
                default:
                    return 400;
            }
        }

    }

    public enum FormType
    {
        Standard, Wide, Small
    }
}
