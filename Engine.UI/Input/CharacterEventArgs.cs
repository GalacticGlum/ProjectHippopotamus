using System;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus.Engine.UI.Input
{
    public delegate void CharacterEventHandler(object sender, CharacterEventArgs args);
    public class CharacterEventArgs : InputEventArgs
    {
        public char Character { get; }

        public KeyboardState KeyboardState { get; }
        public Modifiers Modifiers { get; }

        public CharacterEventArgs(char character, KeyboardState keyboardState, Modifiers modifiers, TimeSpan time) : base(time)
        {
            Character = character;
            KeyboardState = keyboardState;
            Modifiers = modifiers;
        }
    }
}
