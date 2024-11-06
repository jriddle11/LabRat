using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public class Exit : IGameObject
    {
        public Vector2 Position;

        public bool Exited { get; set; } = true;

        private Action _exitLevel;

        private Texture2D _texture;

        private BoundingRectangle _collider;


        public Exit(Vector2 pos, Action exitLevel)
        {
            Position = pos;
            _exitLevel = exitLevel;
        }

        public void UpdatePosition(Vector2 pos)
        {
            Position = pos;
            _collider = new(Position, _texture.Width, _texture.Height);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("cheese");
            _collider = new(Position, _texture.Width, _texture.Height);
        }

        public void Update(GameTime gameTime)
        {
            if (_collider.CollidesWith(Context.Player.Collider) && !Exited)
            {
                Exited = true;
                _exitLevel?.Invoke();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.Yellow);
        }
    }
}
