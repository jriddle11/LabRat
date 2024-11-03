using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    public abstract class Character : IGameObject
    {
        public static float Speed => 500f;
        public int ID { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public Direction Direction { get; set; } = Direction.Right;

        public bool IsHeld = false;

        public bool EntityCollision = false;

        public bool IsGrounded;

        protected Timer _floorTimer;

        protected Timer _collisionTimer;

        private static int _id = 0;

        public BoundingRectangle Collider
        {
            get
            {
                return _collider;
            }
            set
            {
                _collider = value;
            }
        }

        public Vector2 ColliderOffset = new Vector2(90, 38);

        private BoundingRectangle _collider;

        public BoundingCircle FloorCollider
        {
            get
            {
                return _floorCollider;
            }
            set
            {
                _floorCollider = value;
            }
        }

        private BoundingCircle _floorCollider = new(new Vector2(140,140), 15);

        public Character()
        {
            ID = _id++;
            _floorTimer = new(.3f);
            _collisionTimer = new(1f);
        }

        public void GroundCharacter()
        {
            if (_floorTimer.TimesUp)
            {
                IsGrounded = true;
            }
        }

        public void ApplyVelocity(GameTime gameTime)
        {
            Velocity *= Speed;
            Velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Velocity;
            UpdateCollider();
        }

        public void UnGroundCharacter()
        {
            IsGrounded = false;
            IsHeld = false;
            _floorTimer.Reset();
        }

        protected SpriteEffects GetEffect()
        {
            if (Direction == Direction.Left) return SpriteEffects.FlipHorizontally;
            return SpriteEffects.None;
        }

        protected virtual void ResetEntityCollisions()
        {
            _collisionTimer.Reset();
            EntityCollision = false;
        }

        public void UpdateCollider()
        {
            _collider.X = Position.X + ColliderOffset.X;
            _collider.Y = Position.Y + ColliderOffset.Y;
            _floorCollider.Center = Position + new Vector2(120, 140);
        }

        public abstract void LoadContent(ContentManager content);

        public virtual void Update(GameTime gameTime)
        {
            if (_collisionTimer.TimeIsUp(gameTime))
            {
                EntityCollision = true;
            }
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
