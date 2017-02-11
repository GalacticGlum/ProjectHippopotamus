using Microsoft.Xna.Framework;

namespace Hippopotamus.Engine.Core
{
    public class Transform : Component
    {
        public Vector2 Position { get; set; }

        private float rotation;
        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                MathHelper.Clamp(rotation, 0, 360);
            }
        }

        public Vector2 Size { get; set; }

        public Transform()
        {
            Position = Vector2.Zero;
            Size = Vector2.One;
            Rotation = 0.0f;
        }

        public void Translate(float x, float y) { Translate(new Vector2(x, y)); }
        public void Translate(Vector2 size)
        {
            Position += size;
        }

        public void Rotate(float rotationFactor)
        {
            Rotation += rotationFactor;
        }

        public void Scale(float x, float y) { Scale(new Vector2(x, y)); }
        public void Scale(Vector2 size)
        {
            Size += size;
        }
    }
}
