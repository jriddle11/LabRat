using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabRat
{
    public class Laser : IActivatable
    {
        public Vector2 Position = Vector2.Zero;
        public bool IsActivated { get => _active; set => _active = value; }
        private bool _active = false;

        private static Texture2D _onTexture;
        private static Texture2D _offTexture;
        private static Texture2D _laserTexture;
        private int _length = 1;
        private List<BoundingRectangle> _colliders = new();

        public Laser(Vector2 pos, int length)
        {
            Position = pos;
            _length = length;
            for(int i = 0; i < _length; i++)
            {
                _colliders.Add(new BoundingRectangle(Position + new Vector2(54, i * 128), 20, 128));
            }
        }

        public void LoadContent(ContentManager content)
        {
            _onTexture ??= content.Load<Texture2D>("laser_red");
            _offTexture ??= content.Load<Texture2D>("laser_green");
            _laserTexture ??= content.Load<Texture2D>("laser");
        }

        public void Update(GameTime gameTime)
        {
            if (_active) return;
            foreach(var collider in _colliders)
            {
                if (collider.CollidesWith(Context.Player.Collider))
                {
                    Context.Player.Reset();
                    break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_active ? _offTexture : _onTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
            if (_active) return;
            for(int i = 0; i < _length; i++)
            {
                spriteBatch.Draw(_laserTexture, Position + new Vector2(0, i * 128), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.95f);
            }
        }
    }
}
