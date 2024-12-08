using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LabRat
{
    public class Slider : IGameObject
    {
        public Vector2 Position;
        public Vector2 HandlePosition;

        public float Value = 1f;

        private static Texture2D _backTexture;
        private static Texture2D _handleTexture;

        private BoundingRectangle _handleCollider;
        private const float MinValue = 0f;
        private const float MaxValue = 1f;
        private const float Step = 0.1f;

        public void LoadContent(ContentManager content) 
        {
            _backTexture ??= content.Load<Texture2D>("forms/slider_back");
            _handleTexture ??= content.Load<Texture2D>("forms/slider_handle");
            
            HandlePosition = Position + new Vector2(_backTexture.Width * (Value - MinValue) / (MaxValue - MinValue), 0);
            _handleCollider = new BoundingRectangle(HandlePosition, _handleTexture.Width, _handleTexture.Height);
        }

        public void Update(GameTime gameTime)
        {
            // Handle mouse input for dragging the slider handle
            MouseState mouseState = Mouse.GetState();
            bool mouseOverHandle = InputManager.IsMouseInRectangle(_handleCollider);

            if (mouseOverHandle && mouseState.LeftButton == ButtonState.Pressed)
            {
                // Calculate the new value based on the mouse position
                float newValue = (mouseState.X - Position.X) / _backTexture.Width;
                newValue = MathHelper.Clamp(newValue, MinValue, MaxValue); // Ensure value is between Min and Max

                // Snap to the nearest step (0.1f)
                newValue = MathHelper.Clamp(MathF.Round(newValue / Step) * Step, MinValue, MaxValue);

                // Update the handle position and the value
                Value = newValue;
                HandlePosition = Position + new Vector2(_backTexture.Width * (Value - MinValue) / (MaxValue - MinValue), 0);
                _handleCollider = new BoundingRectangle(HandlePosition, _handleTexture.Width, _handleTexture.Height);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the background slider
            spriteBatch.Draw(_backTexture, Position, Color.White);

            // Draw the handle at the correct position
            spriteBatch.Draw(_handleTexture, HandlePosition, Color.White);
        }
    }
}
