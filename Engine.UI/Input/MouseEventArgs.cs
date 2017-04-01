using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus.Engine.UI.Input
{
    public delegate void MouseEventHandler(object sender, MouseEventArgs args);
    public class MouseEventArgs : InputEventArgs
    {
        public MouseButton MouseButton { get; }
        public Point Location { get; }
        public int ScrollWheelDelta { get; }

        public MouseState Current { get; }
        public MouseState Previous { get; }

        public MouseEventArgs(MouseButton mouseButton, MouseState current, MouseState previous, TimeSpan time) : base(time)
        {
            MouseButton = mouseButton;
            Current = current;
            Previous = previous;

            Location = current.Position;
            ScrollWheelDelta = current.ScrollWheelValue - previous.ScrollWheelValue;
        }
    }
}
