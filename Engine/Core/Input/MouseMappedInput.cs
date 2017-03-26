using System.Linq;

namespace Hippopotamus.Engine.Core
{
    internal class MouseMappedInput : MappedInput<MouseButton>
    {
        public MouseMappedInput(string name, MappedInputType type, InputTriggeredEventHandler triggeredEventHandler) : base(name, type, triggeredEventHandler)
        {
        }

        protected override bool GetHeld()
        {
            return InputMap.Any(Input.GetMouseButton);
        }

        protected override bool GetDown()
        {
            return InputMap.Any(Input.GetMouseButtonDown);
        }

        protected override bool GetUp()
        {
            return InputMap.Any(Input.GetMouseButtonUp);
        }
    }
}
