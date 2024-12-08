using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabRat
{
    public class Cover : IGameObject
    {
        public Vector2 Position;
        public float FadeSpeed = .4f;
        public bool FadedIn { get; private set; } = true;
        public bool FadedOut { get; private set; } = false;
        private Texture2D _texture;
        private float _opacity = 1f;
        private bool _covering = false;
        private Action _next;
        private bool _called = false;
        private bool _start = false;
        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("cover");
        }

        public void SetNextAction(Action action)
        {
            _called = false;
            _next = action;
            _start = false;
        }

        public void FadeOut()
        {
            FadeSpeed = 1f;
            _opacity = .99f;
            _covering = false;
            FadedIn = FadedOut = false;
            _start = true;
        }

        public void FadeIn()
        {
            FadeSpeed = 1f;
            _opacity = 0.1f;
            _covering = true;
            FadedIn = FadedOut = false;
            _start = true;
        }

        public void QuickOut()
        {
            _opacity = .99f;
            _covering = true;
            FadedIn = FadedOut = false;
            _start = true;
        }

        public void Update(GameTime gameTime)
        {
            if(_covering && _opacity < 1f)
            {
                _opacity += (float)gameTime.ElapsedGameTime.TotalSeconds * FadeSpeed;
            }
            else if(_covering && _opacity > 1f)
            {
                _opacity = 1f;
            }
            

            if(!_covering && _opacity > 0f)
            {
                _opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds * FadeSpeed;
            }
            else if (!_covering && _opacity < 0f)
            {
                _opacity = 0f;
            }

            if(_opacity == 1f)
            {
                FadedIn = true;
                FadedOut = false;
            }
            else if(_opacity == 0f)
            {
                FadedIn = false;
                FadedOut = true;
            }
            else
            {
                FadedIn = false;
                FadedOut= false;
            }

            if(_start && !_called && (_opacity == 0f || _opacity == 1f))
            {
                _called = true;
                _next?.Invoke();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White * _opacity, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
        }
    }
}
