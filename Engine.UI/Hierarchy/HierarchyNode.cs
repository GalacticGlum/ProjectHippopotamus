using System.Collections.Generic;

namespace Hippopotamus.Engine.UI.Hierarchy
{
    public delegate void HierarchyNodeEventHandler<T>(object sender, HierarchyNode<T> node) where T : class;

    public class HierarchyNode<T> where T : class
    {
        public RootHierarchyNode<T> Root { get; set; }
        public bool IsRoot => Root == this;
        public bool Attached => Root != null;

        public HierarchyNode<T> Parent { get; set; }
        public List<HierarchyNode<T>> Children { get; set; }

        public T Data { get; set; }

        public HierarchyNode(T data)
        {
            Data = data;
            Children = new List<HierarchyNode<T>>();
        }

        public void AddChild(IHierarchyNode<T> child)
        {
            AddChild(child.GetHierarchyNode());

            if (IsRoot)
            {
                ExecuteNodeEventOnHierarchy((sender, node) => Root.OnChildChanged(node));
            }
            else if(Attached)
            {
                ExecuteNodeEvent((sender, node) => Root.OnChildChanged(node));
            }
        }

        public void RemoveChild(IHierarchyNode<T> child)
        {
            foreach (HierarchyNode<T> node in Children)
            {
                if (!node.Data.Equals(child)) continue;
                RemoveChild(node);
                return;
            }

            if (IsRoot)
            {
                ExecuteNodeEventOnHierarchy((sender, node) => Root.OnChildChanged(node));
            }
            else if (Attached)
            {
                ExecuteNodeEvent((sender, node) => Root.OnChildChanged(node));
            }
        }

        public void AddChild(HierarchyNode<T> child)
        {
            child.Parent = this;
            Children.Add(child);

            if (child.Root == null) return;

            child.Root = Root;
            Root.OnAttachedToRoot(this);
        }

        public void RemoveChild(HierarchyNode<T> child)
        {
            if (child.Parent != this || !Children.Contains(child)) return;

            child.Parent = null;
            Children.Remove(child);
        }

        public void AddChildren(IEnumerable<IHierarchyNode<T>> children)
        {
            foreach (IHierarchyNode<T> child in children)
            {
                AddChild(child.GetHierarchyNode());
            }

            if (IsRoot)
            {
                ExecuteNodeEventOnHierarchy((sender, node) => Root.OnChildChanged(node));
            }
            else if (Attached)
            {
                ExecuteNodeEvent((sender, node) => Root.OnChildChanged(node));
            }
        }

        public void AddChildren(params IHierarchyNode<T>[] children)
        {
            AddChildren((IEnumerable<IHierarchyNode<T>>)children);
        }

        public void RemoveChildren(IEnumerable<IHierarchyNode<T>> children)
        {
            foreach (IHierarchyNode<T> child in children)
            {
                RemoveChild(child.GetHierarchyNode());
            }

            if (IsRoot)
            {
                ExecuteNodeEventOnHierarchy((sender, node) => Root.OnChildChanged(node));
            }
            else if (Attached)
            {
                ExecuteNodeEvent((sender, node) => Root.OnChildChanged(node));
            }
        }

        public void RemoveChildren(params IHierarchyNode<T>[] children)
        {
            RemoveChildren((IEnumerable<IHierarchyNode<T>>)children);
        }

        public void ExecuteNodeEvent(HierarchyNodeEventHandler<T> eventHandler)
        {
            eventHandler?.Invoke(this, this);
            foreach (HierarchyNode<T> child in Children)
            {
                child.ExecuteNodeEvent(eventHandler);
            }
        }

        public void ExecuteNodeEventOnHierarchy(HierarchyNodeEventHandler<T> eventHandler)
        {
            foreach (HierarchyNode<T> child in Children)
            {
                child.ExecuteNodeEvent(eventHandler);
            }
        }     
    }
}
