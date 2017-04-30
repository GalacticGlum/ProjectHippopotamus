namespace Engine.UI.Framework
{
    public class RootNode<T> : Node<T> where T : class
    {
        public RootNode() : base(null)
        {
            Root = this;
        }

        public event NodeEventHandler<T> AttachedToRootEventHandler;
        public void OnAttachedToRoot(Node<T> node)
        {
            AttachedToRootEventHandler?.Invoke(node);
        }

        public event NodeEventHandler<T> ChildrenChangedEventHandler;
        public void OnChildrenChanged(Node<T> node)
        {
            ChildrenChangedEventHandler?.Invoke(node);
        }
    }
}
