using System;

namespace Hippopotamus.Engine.UI.Input
{
    public class InputEventArgs : EventArgs
    {
        public TimeSpan Time { get; }
        public InputEventArgs(TimeSpan time)
        {
            Time = time;
        }
    }
}
