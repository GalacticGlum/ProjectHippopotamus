using System.Collections.Generic;
using System.Linq;
using Hippopotamus.Engine.UI.Hierarchy;
using Hippopotamus.Engine.UI.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.UI
{
    public abstract class Widget : IHierarchyNode<Widget>
    {
        public abstract Rectangle Area { get; protected set; }
        public abstract Rectangle AbsoluteArea { get; set; }

        public abstract Rectangle AbsoluteInputArea{ get; }
        public Rectangle SliceArea { get; }
        public Rectangle ClipArea { get; }

        public Widget Parent => hierarchyNode.Parent.Data;
        public IEnumerable<Widget> Children
        {
            get { return from node in hierarchyNode.Children select node.Data; }
            set
            {
                hierarchyNode.Children.Clear();
                AddChildren(value);
            }
        }

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set { hierarchyNode.ExecuteNodeEvent((sender, node) => node.Data.visible = value); }
        }

        private bool active;
        public bool Active
        {
            get { return active; }
            set{ hierarchyNode.ExecuteNodeEvent((sender, node) => node.Data.active = value);}
        }

        public bool IsPressed => UserInterfaceInput.CurrentlyPressedWidget == this;
        public bool IsFocused => UserInterfaceInput.CurrentlyFocusedWidget == this;
        public bool IsHover => UserInterfaceInput.CurrentlyHoverWidget == this;

        protected internal bool BlocksInput { get; set; }

        protected virtual void OnResize()
        {
            AbsoluteArea = Area;
        }

        protected virtual void OnEnterHover() { }
        protected virtual void OnExitHover() { }

        protected virtual void OnPressed() { }
        protected virtual void OnStopPressed() { }

        protected virtual void OnFocused() { }
        protected virtual void OnStopFocused() { }

        protected virtual void OnKeyDown(KeyEventArgs args) { }
        protected virtual void OnKeyUp(KeyEventArgs args) { }
        protected virtual void OnCharacterInputted(CharacterEventArgs args) { }

        protected virtual void OnMouseHover(MouseEventArgs args) { }
        protected virtual void OnMouseClick(MouseEventArgs args) { }
        protected virtual void OnMouseDoubleClick(MouseEventArgs args) { }

        protected virtual void OnMouseDown(MouseEventArgs args) { }
        protected virtual void OnMouseUp(MouseEventArgs args) { }
        protected virtual void OnMouseMove(MouseEventArgs args) { }
        protected virtual void OnMouseWheel(MouseEventArgs args) { }

        private readonly HierarchyNode<Widget> hierarchyNode;

        protected Widget()
        {
            hierarchyNode = new HierarchyNode<Widget>(this);

            Visible = true;
            Active = true;

            BlocksInput = false;
        }

        public abstract void Initialize();
        public abstract void Layout();

        public abstract void Draw();
        public abstract void DrawWithNoClipping();
        protected abstract void Update();

        public void AddChild(Widget child)
        {
            hierarchyNode.AddChild(child);
        }

        public void RemoveChild(Widget child)
        {
            hierarchyNode.RemoveChild(child);
        }

        public void AddChildren(IEnumerable<Widget> children)
        {
            hierarchyNode.AddChildren(children);
        }

        public HierarchyNode<Widget> GetHierarchyNode()
        {
            return hierarchyNode;
        }
    }

    public abstract class Widget<T> : Widget where T : RenderData
    {
        protected T RenderHint { get; set; }
    }
}
