using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus.Engine.Core
{
    public static class InputManager
    {
        private static readonly Dictionary<string, KeyboardMappedInput> keyboardInputMap;
        private static readonly Dictionary<string, MouseMappedInput> mouseInputMap;

        static InputManager()
        {
            keyboardInputMap = new Dictionary<string, KeyboardMappedInput>();
            mouseInputMap = new Dictionary<string, MouseMappedInput>();
        }

        internal static void Update(GameLoopEventArgs args)
        {
            Input.Update();

            foreach (KeyboardMappedInput input in keyboardInputMap.Values)
            {
                input.Trigger(args);
            }

            foreach (MouseMappedInput input in mouseInputMap.Values)
            {
                input.Trigger(args);
            }
        }

        public static void RegisterInputMapping(string name, MappedInputModifier modifier, params Keys[] keys)
        {
            if (keyboardInputMap.ContainsKey(name))
            {
                keyboardInputMap[name].Modifier = modifier;
                keyboardInputMap[name].RegisterInput(keys);
            }
            else
            {
                keyboardInputMap.Add(name, new KeyboardMappedInput(name, 0, null)
                {
                    InputMap = keys.ToList(),
                    Modifier = modifier
                });
            }
        }

        public static void RegisterInputMapping(string name, MappedInputModifier modifier, params MouseButton[] mouseButtons)
        {
            if (mouseInputMap.ContainsKey(name))
            {
                mouseInputMap[name].Modifier = modifier;
                mouseInputMap[name].RegisterInput(mouseButtons);
            }
            else
            {
                mouseInputMap.Add(name, new MouseMappedInput(name, 0, null)
                {
                    InputMap = mouseButtons.ToList(),
                    Modifier = modifier
                });
            }
        }

        public static void RegisterKeyInputAction(string name, MappedInputType type, InputTriggeredEventHandler action)
        {
            if (keyboardInputMap.ContainsKey(name))
            {
                keyboardInputMap[name].SetTriggeredEvent(action);
                keyboardInputMap[name].Type = type;
            }
            else
            {
                keyboardInputMap.Add(name, new KeyboardMappedInput(name, type, action));
            }
        }

        public static void RegisterMouseInputAction(string name, MappedInputType type, InputTriggeredEventHandler action)
        {
            if (mouseInputMap.ContainsKey(name))
            {
                mouseInputMap[name].SetTriggeredEvent(action);
                mouseInputMap[name].Type = type;
            }
            else
            {
                mouseInputMap.Add(name, new MouseMappedInput(name, type, action));
            }
        }       
    }
}
