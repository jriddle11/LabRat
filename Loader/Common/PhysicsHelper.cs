
namespace LabRat
{
    public static class PhysicsHelper
    {
        public static void ResolveClonePhysics(List<Character> characters, GameTime gameTime)
        {
            foreach (var character in characters)
            {
                foreach (var otherCharacter in characters)
                {
                    if (character.ID <= otherCharacter.ID) continue;
                    if (!character.EntityCollision) continue;
                    if (character.FloorCollider.CollidesWith(otherCharacter.Collider) && character.IsGrounded)
                    {
                        ApplyCloneConnection(character, otherCharacter, gameTime);
                    }
                }
            }
        }

        public static void ApplyCloneConnection(Character character, Character otherCharacter, GameTime gameTime)
        {
            var top = character.FloorCollider.Bottom - otherCharacter.Collider.Top - character.FloorCollider.Radius - 1;
            var bottom = otherCharacter.Collider.Bottom - character.FloorCollider.Top;

            if (top < bottom)
            {
                character.Position = new Vector2(otherCharacter.Position.X, otherCharacter.Position.Y - 64);
            }

        }
    }
}
