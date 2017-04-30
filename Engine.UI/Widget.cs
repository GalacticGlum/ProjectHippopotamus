using System;
using System.Collections.Generic;
using System.Linq;
using Engine.UI.Framework;
using Engine.UI.Framework.Rendering;
using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using InputManager = Engine.UI.Framework.Input.InputManager;

namespace Engine.UI
{
    public abstract class Widget : INode<Widget>
    {
        public Widget Parent => node.Parent.UserData;
        public IEnumerable<Widget> Children
        {
            get { return from child in node.Children select child.UserData; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                node.Children.Clear();
                AddWidgets(value);
            }
        }

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set
            {
                node.Execute(n => n.UserData.visible = value);
            }
        }

        private bool active;
        public bool Active
        {
            get { return active; }
            set
            {
                node.Execute(n => n.UserData.active = value);
            }
        }

        public bool IsPressed => InputManager.PressedWidget == this;
        public bool IsFocused => InputManager.FocusedWidget == this;
        public bool IsHovered => InputManager.HoveringWidget == this;

        public abstract Rectangle Area { get; protected set; }
        public abstract Rectangle AbsoluteArea { get; set; }

        internal abstract Rectangle InputArea { get; }
        internal Rectangle ScissorArea { get; set; }
        internal Rectangle ClipArea { get; set; }
        internal bool BlocksInput { get; set; }

        internal InputManager InputManager { get; set; }
        protected UIManager Owner { get; set; }

        protected virtual void Resize()
        {
            AbsoluteArea = Area;
        }

        protected internal virtual void EnterHoverState() {}
        protected internal virtual void ExitHoverState() {}
        protected internal virtual void EnterPressedState() {}
        protected internal virtual void ExitPressedState() {}
        protected internal virtual void EnterFocusState() {}
        protected internal virtual void ExitFocusState() {}

        protected internal virtual void KeyDown(KeyEventArgs args) {}
        protected internal virtual void KeyUp(KeyEventArgs args) {}
        protected internal virtual void CharacterTyped(CharacterEventArgs args) { }

        protected internal virtual void MouseHover(MouseEventArgs args) { }
        protected internal virtual void MouseClicked(MouseEventArgs args) { }
        protected internal virtual void MouseDoubleClicked(MouseEventArgs args) { }
        protected internal virtual void MouseDown(MouseEventArgs args) { }
        protected internal virtual void MouseUp(MouseEventArgs args) { }
        protected internal virtual void MouseMove(MouseEventArgs args) { }
        protected internal virtual void MouseWheel(MouseEventArgs args) { }

        private readonly Node<Widget> node;


        internal abstract void Prepare(UIManager uiManager);
        internal abstract void CreateLayout();
        internal abstract void Draw();
        internal abstract void DrawWithoutClipping();

        protected internal abstract void Update();

        protected Widget()
        {
            node = new Node<Widget>(this);
            Visible = true;
            Active = true;
            BlocksInput = false;
        }

        public Node<Widget> GetNode()
        {
            return node;
        }

        public void AddWidget(Widget widget)
        {
            node.AddChild(widget);
        }

        public void AddWidgets(IEnumerable<Widget> widgets)
        {
            node.AddChildren(widgets);
        }

        public void RemoveWidget(Widget widget)
        {
            node.RemoveChild(widget);
        }
    }

    public abstract class Widget<T> : Widget where T : RenderHint
    {
        public override Rectangle Area { get; protected set; }
        public override Rectangle AbsoluteArea
        {
            get { return RenderHint.Area; }
            set { RenderHint.Area = value; }
        }

        public virtual string Skin { set { RenderHint.Skin = value; } }
        public virtual string Text { set { RenderHint.Text = value; } }

        internal override Rectangle InputArea => RenderHint.SafeArea;
        protected internal T RenderHint { get; set; }
        protected abstract T CreateRenderHint();
        protected abstract void Attach();

        protected Widget()
        {
            RenderHint = CreateRenderHint();
        }

        internal override void Prepare(UIManager uiManager)
        {
            Owner = uiManager;

            ScissorArea = GameEngine.Context.Viewport.Bounds;
            ClipArea = GameEngine.Context.Viewport.Bounds;

            InputManager = uiManager.InputManager;
            RenderHint.RenderManager = uiManager.RenderManager;
            RenderHint.Load();

            Attach();
            Resize();
        }

        internal override void CreateLayout()
        {
            ClipArea = Rectangle.Intersect(RenderHint.ClippingArea, GameEngine.Context.Viewport.Bounds);
        }

        internal override void Draw()
        {
            RenderHint.Draw();
        }

        internal override void DrawWithoutClipping()
        {
            RenderHint.DrawWithoutClipping();
        }
    }
}
