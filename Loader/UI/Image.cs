using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class Image : IGameObject
    {
        public Vector2 Position;

        public Color Color = Color.White;

        public float LayerDepth = 0.95f;

        private Texture2D _texture;

        private string _path;

        public Image(Vector2 pos, string path)
        {
            Position = pos;
            _path = path;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>(_path);
        }

        public void Update(GameTime gameTime)
        {
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, LayerDepth);
        }
    }
}
