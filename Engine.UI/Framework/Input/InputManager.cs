using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.UI.Framework.Input
{
    // This is really just a recreation of our Input class in the Engine. 
    // TODO: Merge with the engine's Input system.
    internal class InputManager
    {
        public Point MousePosition{ get; set; }
        private Widget hoveringWidget;
        public Widget HoveringWidget
        {
            get { return hoveringWidget; }
            private set
            {
                if (value == hoveringWidget) return;
                if (value != null) value.EnterHoverState();
                if (hoveringWidget != null) hoveringWidget.ExitHoverState();

                hoveringWidget = value;
            }
        }

        private Widget pressedWidget;
        public Widget PressedWidget
        {
            get{ return pressedWidget; }
            private set
            {
                if (value == pressedWidget) return;
                if (value != null) value.EnterPressedState();
                if (pressedWidget != null) hoveringWidget.ExitPressedState();

                pressedWidget = value;
            }
        }

        private Widget focusedWidget;
        public Widget FocusedWidget
        {
            get { return focusedWidget; }
            private set
            {
                if (value == focusedWidget) return;
                if (value != null) value.EnterFocusState();
                if (focusedWidget != null) hoveringWidget.ExitFocusState();

                focusedWidget = value;
            }
        }

        private readonly MouseEvents mouseEvents;
        private readonly KeyboardEvents keyboardEvents;
        private readonly RootNode<Widget> rootWidget;

        internal InputManager(RootNode<Widget> rootWidget)
        {
            this.rootWidget = rootWidget;
            mouseEvents = new MouseEvents();
            keyboardEvents = new KeyboardEvents();

            MousePosition = Mouse.GetState().Position;
            RegisterInputEvents();
        }

        private void RegisterInputEvents()
        {
            MouseEvents.MouseMoved += (sender, args) =>
            {
                if (FocusedWidget != null && FocusedWidget.InputArea.Contains(MousePosition) && FocusedWidget.BlocksInput) return;
                HoveringWidget = GetHoverWidget();

                if (PressedWidget == null) return;
                if (!PressedWidget.InputArea.Contains(MousePosition))
                {
                    PressedWidget = null;
                }
            };

            MouseEvents.ButtonReleased += (sender, args) =>
            {
                if(args.Button != MouseButton.Left) return;
                PressedWidget?.MouseClicked(args);
                PressedWidget = null;
            };

            MouseEvents.ButtonPressed += (sender, args) =>
            {
                if (args.Button != MouseButton.Left) return;
                if (HoveringWidget == null && FocusedWidget != null && !FocusedWidget.BlocksInput) return;

                FocusedWidget = HoveringWidget;
                PressedWidget = HoveringWidget;
            };

            MouseEvents.ButtonDoubleClicked += (sender, args) =>
            {
                if (args.Button != MouseButton.Left) return;
                if (HoveringWidget == null) return;

                FocusedWidget = HoveringWidget;
                PressedWidget = HoveringWidget;
            };

            KeyboardEvents.KeyTyped += (sender, args) => FocusedWidget?.CharacterTyped(args); 
            KeyboardEvents.KeyPressed += (sender, args) => FocusedWidget?.KeyDown(args);
            KeyboardEvents.KeyReleased += (sender, args) => FocusedWidget?.KeyUp(args);
            MouseEvents.ButtonDoubleClicked += (sender, args) => FocusedWidget?.MouseDoubleClicked(args);
            MouseEvents.ButtonPressed += (sender, args) =>FocusedWidget?.MouseDown(args);
            MouseEvents.MouseMoved += (sender, args) => FocusedWidget?.MouseMove(args);
            MouseEvents.ButtonReleased += (sender, args) => FocusedWidget?.MouseUp(args);
            MouseEvents.MouseWheelMoved += (sender, args) => FocusedWidget?.MouseWheel(args);
        }

        internal void Update(GameTime gameTime, MouseState mouseState)
        {
            MousePosition = mouseState.Position;
            mouseEvents.Update(gameTime);
            keyboardEvents.Update(gameTime);
        }

        private Widget GetHoverWidget()
        {
            Widget hover = null;
            foreach (Node<Widget> child in rootWidget.Children)
            {
                FindHover(ref hover, child);
            }

            return hover;
        }

        private void FindHover(ref Widget hover, Node<Widget> node)
        {
            if (!node.UserData.AbsoluteArea.Contains(MousePosition)) return;
            if (node.Parent.UserData != null && !node.UserData.InputArea.Contains(MousePosition)) return;
            if (!node.UserData.Active || !node.UserData.Visible) return;

            hover = node.UserData;
            foreach (Node<Widget> child in node.Children)
            {
                FindHover(ref hover, child);
            }
        }
    }
}
