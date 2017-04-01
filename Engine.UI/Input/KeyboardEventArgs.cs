using System;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus.Engine.UI.Input
{
    public delegate void KeyEventHandler(object sender, KeyEventArgs args);
    public class KeyEventArgs : InputEventArgs
    {
        public Keys Key { get; }
        public Modifiers Modifiers { get; }

        public KeyEventArgs(Keys key, Modifiers modifiers, TimeSpan time) : base(time)
        {
            Modifiers = modifiers;
            Key = key;
        }
    }
}
