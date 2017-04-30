using System.Collections.Generic;

namespace Engine.UI.Framework
{
    public delegate void NodeEventHandler<T>(Node<T> node) where T : class;
    public interface INode<T> where T : class
    {
        Node<T> GetNode();
    }

    public class Node<T> where T : class 
    {
        public RootNode<T> Root { get; set; }
        public bool IsRoot => Root == this;
        public bool IsAttached => Root != null;

        public Node<T> Parent { get; set; }
        public List<Node<T>> Children { get; set; }

        public T UserData { get; set; }

        public Node(T userData)
        {
            UserData = userData;
            Children = new List<Node<T>>();
        }

        public void AddChild(INode<T> node)
        {
            AddChild(node.GetNode());

            if (IsRoot)
            {
                ExecuteOnChildren(arg => Root.OnChildrenChanged(arg));
            }
            else if (IsAttached)
            {
                ExecuteOnChildren(arg => Root.OnChildrenChanged(arg));
            }
        }

        private void AddChild(Node<T> node)
        {
            node.Parent = this;
            Children.Add(node);

            if (Root == null) return;

            node.Root = Root;
            Root.OnAttachedToRoot(this);
        }

        public void AddChildren(IEnumerable<INode<T>> children)
        {
            foreach (INode<T> child in children)
            {
                AddChild(child.GetNode());
            }

            if (IsRoot)
            {
                ExecuteOnChildren(node => Root.OnChildrenChanged(node));
            }
            else if (IsAttached)
            {
                ExecuteOnChildren(node => Root.OnChildrenChanged(node));
            }
        }

        public void RemoveChild(INode<T> node)
        {
            foreach (Node<T> child in Children)
            {
                if(!child.UserData.Equals(node)) continue;
                RemoveChild(child);
                return;
            }

            if (IsRoot)
            {
                ExecuteOnChildren(arg => Root.OnChildrenChanged(arg));
            }
            else if (IsAttached)
            {
                ExecuteOnChildren(arg => Root.OnChildrenChanged(arg));
            }
        }

        private void RemoveChild(Node<T> node)
        {
            // Both of these will never happen unless something has gone terribly wrong!!!
            if (node.Parent != this || !Children.Contains(node)) return;

            node.Parent = null;
            Children.Remove(node);
        }

        /// <summary>
        /// Executes <paramref name="operation"/> on children (and it's children, and so on...).
        /// </summary>
        /// <param name="operation"></param>
        public void ExecuteOnChildren(NodeEventHandler<T> operation)
        {
            if (operation == null) return;
            foreach (Node<T> child in Children)
            {
                child.Execute(operation);
            }
        }

        /// <summary>
        /// Executes <paramref name="operation"/> on the this Node and it's children.
        /// </summary>
        /// <param name="operation"></param>
        public void Execute(NodeEventHandler<T> operation)
        {
            if (operation == null) return;
            operation(this);
            foreach (Node<T> child in Children)
            {
                child.Execute(operation);
            }
        }
    }
}
