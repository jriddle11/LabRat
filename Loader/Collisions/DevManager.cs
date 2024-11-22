using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace LabRat
{
    public static class DevManager
    {

        static Texture2D _singlePixelTexture;

        public static void LoadContent(Game game)
        {
            _singlePixelTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            Color[] colorData = new Color[1];
            colorData[0] = Color.White;
            _singlePixelTexture.SetData(colorData);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            spriteBatch.Draw(_singlePixelTexture, rect, color);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Vector2 position, Rectangle rect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect)
        {

            spriteBatch.Draw(_singlePixelTexture, position, rect, color, rotation, origin, scale, effect, .01f);

        }

        public static void DrawRectangle(SpriteBatch spriteBatch, BoundingRectangle rectCol, Color color)
        {
            spriteBatch.Draw(_singlePixelTexture, new Rectangle((int)rectCol.X, (int)rectCol.Y, (int)rectCol.Width, (int)rectCol.Height), color);
        }

        public static void DrawCircle(SpriteBatch spriteBatch, BoundingCircle circle, Color color, int segments = 100)
        {
            float increment = MathHelper.TwoPi / segments;
            float angle = 0f;

            for (int i = 0; i < segments; i++)
            {
                // Calculate start and end points of the line segment from the center
                Vector2 start = circle.Center + circle.Radius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                Vector2 end = circle.Center + circle.Radius * new Vector2((float)Math.Cos(angle + increment), (float)Math.Sin(angle + increment));

                // Draw line segment
                DrawLine(spriteBatch, start, end, color);

                angle += increment;
            }
        }

        private static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(_singlePixelTexture, start, null, color,
                             angle, Vector2.Zero, new Vector2(edge.Length(), 1), SpriteEffects.None, 0);
        }
    }
}
