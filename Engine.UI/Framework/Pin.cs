using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.UI.Framework
{
    public class Pin
    {
        public bool IsPushed { get; private set; }
        public Vector2 Shift => start - Mouse.GetState().Position.ToVector2();

        private Vector2 start;

        public Pin()
        {
            start = Vector2.Zero;
        }

        public void Push()
        {
            IsPushed = true;
            start = Mouse.GetState().Position.ToVector2();
        }

        public void Pull()
        {
            IsPushed = false;
        }
    }
}
