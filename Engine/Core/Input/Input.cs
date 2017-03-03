using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus.Engine.Core
{
    public enum MouseButton
    {
        LeftMouseButton = 0,
        RightMouseButton = 1,
        MiddleMouseButton = 2
    }

    public static class Input
    {
        public static Vector2 MousePosition { get; private set; }

        private static KeyboardState currentKeyState;
        private static KeyboardState lastKeyState;

        private static MouseState currentMouseState;
        private static MouseState lastMouseState;
    
        internal static void Update(GameLoopUpdateEventArgs args)
        {
            lastMouseState = currentMouseState;
            lastKeyState = currentKeyState;

            currentMouseState = Mouse.GetState();
            currentKeyState = Keyboard.GetState();

            MousePosition = currentMouseState.Position.ToVector2();
        }

        public static bool GetKey(Keys keyCode)
        {
            return currentKeyState.IsKeyDown(keyCode);
        }

        public static bool GetKeyDown(Keys keyCode)
        {
            return lastKeyState.IsKeyUp(keyCode) && currentKeyState.IsKeyDown(keyCode);
        }

        public static bool GetKeyUp(Keys keyCode)
        {
            return lastKeyState.IsKeyDown(keyCode) && currentKeyState.IsKeyUp(keyCode);
        }

        public static bool GetMouseButton(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.LeftMouseButton:
                    if (currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        return true;
                    }

                    break;
                case MouseButton.RightMouseButton:
                    if (currentMouseState.RightButton == ButtonState.Pressed)
                    {
                        return true;
                    }

                    break;
                case MouseButton.MiddleMouseButton:
                    if (currentMouseState.MiddleButton == ButtonState.Pressed)
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }

        public static bool GetMouseButtonDown(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.LeftMouseButton:
                    if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        return true;
                    }

                    break;
                case MouseButton.RightMouseButton:
                    if (lastMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
                    {
                        return true;
                    }

                    break;
                case MouseButton.MiddleMouseButton:
                    if (lastMouseState.MiddleButton == ButtonState.Released && currentMouseState.MiddleButton == ButtonState.Pressed)
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }

        public static bool GetMouseButtonUp(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.LeftMouseButton:
                    if (lastMouseState.LeftButton == ButtonState.Pressed &&
                        currentMouseState.LeftButton == ButtonState.Released)
                    {
                        return true;
                    }

                    break;
                case MouseButton.RightMouseButton:
                    if (lastMouseState.RightButton == ButtonState.Pressed &&
                        currentMouseState.RightButton == ButtonState.Released)
                    {
                        return true;
                    }

                    break;
                case MouseButton.MiddleMouseButton:
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
