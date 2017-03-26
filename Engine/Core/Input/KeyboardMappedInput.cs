using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus.Engine.Core
{
    internal class KeyboardMappedInput : MappedInput<Keys>
    {
        public KeyboardMappedInput(string name, MappedInputType type, InputTriggeredEventHandler triggeredEventHandler) : base(name, type, triggeredEventHandler)
        {
        }
        
        protected override bool GetHeld()
        {
            return InputMap.Any(Input.GetKey);
        }

        protected override bool GetDown()
        {
            return InputMap.Any(Input.GetKeyDown);
        }

        protected override bool GetUp()
        {
            return InputMap.Any(Input.GetKeyUp);
        }
    }
}
