using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework;

namespace Engine.UI.Framework
{
    public class Pin
    {
        public bool IsPushed { get; private set; }
        public Vector2 Shift => start - Input.MousePosition;

        private Vector2 start;

        public Pin()
        {
            start = Vector2.Zero;
        }

        public void Push()
        {
            IsPushed = true;
            start = Input.MousePosition;
        }

        public void Pull()
        {
            IsPushed = false;
        }
    }
}
