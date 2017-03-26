using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus.Engine.Core
{
    internal abstract class MappedInput<T>
    {
        public string Name { get; set; }
        public MappedInputType Type { get; set; }

        public List<T> InputMap { get; set; }
        public MappedInputModifier Modifier { get; set; }
        protected event InputTriggeredEventHandler Triggered;

        protected abstract bool GetHeld();
        protected abstract bool GetDown();
        protected abstract bool GetUp();

        public MappedInput(string name, MappedInputType type, InputTriggeredEventHandler triggeredEventHandler)
        {
            InputMap = new List<T>();

            Name = name;
            Type = type;

            Triggered = triggeredEventHandler;
        }

        public void SetTriggeredEvent(InputTriggeredEventHandler triggeredEvent)
        {
            Triggered = triggeredEvent;
        }

        public void Trigger(GameLoopEventArgs args)
        {
            if (HasInput())
            {
                Triggered?.Invoke(this, args);
            }
        }

        public void RegisterInput(params T[] inputs)
        {
            foreach (T input in inputs)
            {
                if (!InputMap.Contains(input))
                {
                    InputMap.Add(input);
                }
            }
        }

        protected virtual bool HasInput()
        {
            if (!HasInputModifier()) return false;

            switch (Type)
            {
                case MappedInputType.Held:
                    return GetHeld();
                case MappedInputType.Down:
                    return GetDown();
                case MappedInputType.Up:
                    return GetUp();
            }

            return false;
        }

        protected bool HasInputModifier()
        {
            MappedInputModifier currentActive = MappedInputModifier.None;
            if (GetInputModifier(MappedInputModifier.Shift))
            {
                currentActive = MappedInputModifier.Shift;
            }

            if (GetInputModifier(MappedInputModifier.Control))
            {
                currentActive = MappedInputModifier.Control;
            }

            if (GetInputModifier(MappedInputModifier.Alt))
            {
                currentActive = MappedInputModifier.Alt;
            }

            return currentActive == Modifier;
        }

        protected static bool GetInputModifier(MappedInputModifier modifier)
        {
            switch (modifier)
            {
                case MappedInputModifier.None:
                    return GetNoneInputModifier();
                case MappedInputModifier.Shift:
                    return Input.GetKey(Keys.LeftShift) || Input.GetKey(Keys.RightShift);
                case MappedInputModifier.Control:
                    return Input.GetKey(Keys.LeftControl) || Input.GetKey(Keys.RightControl);
                case MappedInputModifier.Alt:
                    return Input.GetKey(Keys.LeftAlt) || Input.GetKey(Keys.RightAlt);
            }

            return false;
        }

        private static bool GetNoneInputModifier()
        {
            return !(GetInputModifier(MappedInputModifier.Shift) || 
                GetInputModifier(MappedInputModifier.Control) || 
                GetInputModifier(MappedInputModifier.Alt));
        }
    }
}
