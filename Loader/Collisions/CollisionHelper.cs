
namespace LabRat
{
    public static class CollisionHelper
    {
        /// <summary>
        /// Detects collision between 2 bounding circles
        /// </summary>
        /// <param name="a">The first circle</param>
        /// <param name="b">The second circle</param>
        /// <returns>returns true for collision</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >= Math.Pow(a.Center.X - b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects collision between 2 bounding rectangles
        /// </summary>
        /// <param name="a">The first rectangele</param>
        /// <param name="b">The second rectangle</param>
        /// <returns>returns true for collision</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right || a.Top > b.Bottom|| a.Bottom < b.Top);
        }

        /// <summary>
        /// Detects collision between circle and rectangles
        /// </summary>
        /// <param name="c">The circle</param>
        /// <param name="r">The rectangle</param>
        /// <returns>returns true if collision detected</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);
            return Math.Pow(c.Radius, 2) >= Math.Pow(c.Center.X - nearestX, 2) + Math.Pow(c.Center.Y - nearestY, 2);
        }

        /// <summary>
        /// Detects collision between circle and rectangles
        /// </summary>
        /// <param name="c">The circle</param>
        /// <param name="r">The rectangle</param>
        /// <returns>returns true if collision detected</returns>
        public static bool Collides(BoundingRectangle r, BoundingCircle c)
        {
            return Collides(c, r);
        }


        /// <summary>
        /// Resolves all collisions between static objects and dynamic characters
        /// </summary>
        /// <param name="characters"></param>
        /// <param name="staticColliders"></param>
        /// <param name="maxIterations">The number of times to resolve collisions per update per character</param>
        public static void ResolveCollisions(List<Character> characters, List<BoundingRectangle> staticColliders, int maxIterations)
        {

            foreach (var character in characters)
            {
                bool touchingFloor = false;
                for (int i = 0; i < maxIterations; i++)
                {
                    bool collisionResolved = true;


                    foreach (var staticRect in staticColliders)
                    {
                        if (character.FloorCollider.CollidesWith(staticRect))
                        {
                            touchingFloor = true;
                        }
                        if (character.Collider.CollidesWith(staticRect))
                        {
                            SeparateFromStaticRect(character, staticRect);
                            collisionResolved = false;
                        }

                    }

                    foreach (var otherCharacter in characters)
                    {
                        if (character.ID <= otherCharacter.ID) continue;
                        if (character.Velocity.Y < 0) continue;
                        if (!character.EntityCollision) continue;
                        if (character.FloorCollider.CollidesWith(otherCharacter.Collider))
                        {
                            touchingFloor = true;
                        }
                        if (character.FloorCollider.CollidesWith(otherCharacter.Collider))
                        {
                            SeparateFromOthers(character, otherCharacter);
                            collisionResolved = false;
                        }
                    }


                    if (collisionResolved) break;
                }
                if (touchingFloor)
                {
                    character.GroundCharacter();
                }
                else
                {
                    character.IsGrounded = false;
                }
            }
        }

        /// <summary>
        /// Seperates a dynamic character from a static object
        /// </summary>
        /// <param name="character"></param>
        /// <param name="staticRect"></param>
        private static void SeparateFromStaticRect(Character character, BoundingRectangle staticRect)
        {
            var right = character.Collider.Right - staticRect.Left;
            var left = staticRect.Right - character.Collider.Left;
            var top = character.Collider.Bottom - staticRect.Top;
            var bottom = staticRect.Bottom - character.Collider.Top;

            float overlapX = Math.Min(right, left);
            float overlapY = Math.Min(top, bottom);

            int xSign = (overlapX == right) ? 1 : -1;
            int ySign = (overlapY == bottom) ? -1 : 1;

            float adjustmentFactor = 0.5f; 

            if (Math.Abs(overlapX) < Math.Abs(overlapY))
            {
                character.Position = new Vector2(character.Position.X - overlapX * adjustmentFactor * xSign, character.Position.Y);
                character.UpdateCollider();
            }

            else
            {
                character.Position = new Vector2(character.Position.X, character.Position.Y - overlapY * adjustmentFactor * ySign);
                character.UpdateCollider();
            }

            int iterations = 0;
            while (character.Collider.CollidesWith(staticRect) && iterations < 10)
            {
                iterations++;

                if (Math.Abs(overlapX) < Math.Abs(overlapY))
                {
                    character.Position = new Vector2(character.Position.X - xSign * 0.1f, character.Position.Y);
                    character.UpdateCollider();
                }
                else
                {
                    character.Position = new Vector2(character.Position.X, character.Position.Y - ySign * 0.1f);
                    character.UpdateCollider();
                }
            }
        }

        /// <summary>
        /// Seperates dynamic characters from eachother
        /// </summary>
        /// <param name="character"></param>
        /// <param name="otherCharacter"></param>
        private static void SeparateFromOtherCharacter(Character character, Character otherCharacter)
        {
            // Calculate the overlap in each direction
            float right = character.Collider.Right - otherCharacter.Collider.Left;  // Right side of character and left side of otherCharacter
            float left = otherCharacter.Collider.Right - character.Collider.Left;   // Left side of character and right side of otherCharacter
            float top = character.Collider.Bottom - otherCharacter.Collider.Top;    // Bottom side of character and top side of otherCharacter
            float bottom = otherCharacter.Collider.Bottom - character.Collider.Top; // Bottom side of otherCharacter and top side of character

            // Find the minimum overlap in both X and Y directions
            float overlapX = Math.Min(right, left);
            float overlapY = Math.Min(top, bottom);

            // Determine direction of separation (sign of overlap)
            int xSign = (overlapX == right) ? 1 : -1;
            int ySign = (overlapY == bottom) ? -1 : 1;

            // Now we need to decide which axis to prioritize for separation
            if (Math.Abs(overlapX) < Math.Abs(overlapY))
            {
                // Horizontal overlap: Push characters apart horizontally
                character.Position = new Vector2(character.Position.X - xSign * Math.Abs(overlapX) / 2, character.Position.Y);
                otherCharacter.Position = new Vector2(otherCharacter.Position.X + xSign * Math.Abs(overlapX) / 2, otherCharacter.Position.Y);
            }
            else
            {
                // Vertical overlap: This is where we allow standing on heads
                // Only adjust vertically when overlap is vertical
                if (character.Collider.Bottom <= otherCharacter.Collider.Top) // If the player is on top
                {
                    // Move the character down to just stand on the other character's head
                    character.Position = new Vector2(character.Position.X, character.Position.Y - ySign * Math.Abs(overlapY) / 2);
                    otherCharacter.Position = new Vector2(otherCharacter.Position.X, otherCharacter.Position.Y + ySign * Math.Abs(overlapY) / 2);
                }
                else
                {
                    // Handle bottom overlap (if needed)
                    character.Position = new Vector2(character.Position.X, character.Position.Y - ySign * Math.Abs(overlapY) / 2);
                    otherCharacter.Position = new Vector2(otherCharacter.Position.X, otherCharacter.Position.Y + ySign * Math.Abs(overlapY) / 2);
                }
            }

            // Update the collider for both characters after adjusting positions
            character.UpdateCollider();
            otherCharacter.UpdateCollider();
        }

        public static void SeparateFromOthers(Character character, Character otherCharacter)
        {
            if (character.FloorCollider.CollidesWith(otherCharacter.Collider))
            {
                var top = character.FloorCollider.Bottom - otherCharacter.Collider.Top - character.FloorCollider.Radius - 1;
                var bottom = otherCharacter.Collider.Bottom - character.FloorCollider.Top;

                float overlapY = Math.Min(top, bottom);

                int ySign = (overlapY == bottom) ? -1 : 1;

                float adjustmentFactor = 0.5f;

                if (top < bottom)
                {
                    character.Position = new Vector2(character.Position.X, character.Position.Y - overlapY * adjustmentFactor * ySign);
                    character.UpdateCollider();
                }

                int iterations = 0;
                while (character.Collider.CollidesWith(otherCharacter.Collider) && iterations < 10)
                {
                    iterations++;

                    if (top < bottom)
                    {
                        character.Position = new Vector2(character.Position.X, character.Position.Y - ySign * 0.1f);
                        character.UpdateCollider();
                    }
                }
            }


        }

    }
}
