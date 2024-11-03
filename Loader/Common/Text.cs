using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LabRat
{
    public class Text : IGameObject
    {
        public bool Enabled = true;
        public Vector2 Position;
        public Color Color;

        private string _text;
        private bool _centered;

        private static SpriteFont _font;

        public Text(Vector2 pos, string text, Color color, bool centered = false)
        {
            Position = pos;
            Color = color;
            _text = text;
            _centered = centered;
        }

        public void LoadContent(ContentManager content)
        {
            if(_font == null) _font = content.Load<SpriteFont>("Arial");
            if (_centered)
            {
                Position.X -= _font.MeasureString(_text).X / 2;
            }
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Enabled) spriteBatch.DrawString(_font, _text, Position, Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.85f);
        }
    }
}
