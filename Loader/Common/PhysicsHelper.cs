
namespace LabRat
{
    public static class PhysicsHelper
    {
        public static void ResolveClonePhysics(List<Character> characters)
        {
            foreach (var character in characters)
            {
                foreach (var otherCharacter in characters)
                {
                    if (character.ID <= otherCharacter.ID) continue;
                    if (character.Velocity.Y < 0.5f) continue;
                    if (!character.EntityCollision) continue;
                    character.IsHeld = false;
                    if (character.FloorCollider.CollidesWith(otherCharacter.Collider) && character.IsGrounded)
                    {
                        ApplyCloneConnection(character, otherCharacter);
                    }
                }
            }
        }

        public static void ApplyCloneConnection(Character character, Character otherCharacter)
        {
            var top = character.FloorCollider.Bottom - otherCharacter.Collider.Top - character.FloorCollider.Radius;
            var bottom = otherCharacter.Collider.Bottom - character.FloorCollider.Top;

            if (top <= bottom)
            {
                //Sticking
                //character.Position = new Vector2(otherCharacter.Position.X, otherCharacter.Position.Y - 100);
                //character.IsHeld = true;
                //character.Direction = otherCharacter.Direction;
                //Riding
                character.Position += otherCharacter.Velocity;
                character.UpdateCollider();
            }

        }
    }
}
