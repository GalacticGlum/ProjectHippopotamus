using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus.Engine.Core
{
    public enum MouseButton
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    public delegate void InputTriggeredEventHandler(object sender, GameLoopEventArgs args);

    public static class Input
    {
        public static Vector2 MousePosition { get; private set; }

        private static KeyboardState currentKeyState;
        private static KeyboardState lastKeyState;

        private static MouseState currentMouseState;
        private static MouseState lastMouseState;
    
        internal static void Update()
        {
            lastMouseState = currentMouseState;
            lastKeyState = currentKeyState;

            currentMouseState = Mouse.GetState();
            currentKeyState = Keyboard.GetState();

            MousePosition = currentMouseState.Position.ToVector2();
        }

        internal static bool GetKey(Keys keyCode)
        {
            return currentKeyState.IsKeyDown(keyCode);
        }

        internal static bool GetKeyDown(Keys keyCode)
        {
            return lastKeyState.IsKeyUp(keyCode) && currentKeyState.IsKeyDown(keyCode);
        }

        internal static bool GetKeyUp(Keys keyCode)
        {
            return lastKeyState.IsKeyDown(keyCode) && currentKeyState.IsKeyUp(keyCode);
        }

        internal static bool GetMouseButton(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    if (currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        return true;
                    }

                    break;
                case MouseButton.Right:
                    if (currentMouseState.RightButton == ButtonState.Pressed)
                    {
                        return true;
                    }

                    break;
                case MouseButton.Middle:
                    if (currentMouseState.MiddleButton == ButtonState.Pressed)
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }

        internal static bool GetMouseButtonDown(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        return true;
                    }

                    break;
                case MouseButton.Right:
                    if (lastMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
                    {
                        return true;
                    }

                    break;
                case MouseButton.Middle:
                    if (lastMouseState.MiddleButton == ButtonState.Released && currentMouseState.MiddleButton == ButtonState.Pressed)
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }

        internal static bool GetMouseButtonUp(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    if (lastMouseState.LeftButton == ButtonState.Pressed &&
                        currentMouseState.LeftButton == ButtonState.Released)
                    {
                        return true;
                    }

                    break;
                case MouseButton.Right:
                    if (lastMouseState.RightButton == ButtonState.Pressed &&
                        currentMouseState.RightButton == ButtonState.Released)
                    {
                        return true;
                    }

                    break;
                case MouseButton.Middle:
                    if (lastMouseState.MiddleButton == ButtonState.Pressed &&
                        currentMouseState.MiddleButton == ButtonState.Released)
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }
    }
}
