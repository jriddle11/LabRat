using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LabRat
{
    public interface IGameObject
    {
        public void LoadContent(ContentManager content);

        public void Update(GameTime gameTime);

        public void Draw(SpriteBatch spriteBatch);
    }
}
