namespace Hippopotamus.Engine.UI.Hierarchy
{
    public class RootHierarchyNode<T> : HierarchyNode<T> where T : class
    {
        public event HierarchyNodeEventHandler<T> AttachedToRoot;
        internal void OnAttachedToRoot(HierarchyNode<T> node) { AttachedToRoot?.Invoke(this, node); }

        public event HierarchyNodeEventHandler<T> ChildChanged;
        internal void OnChildChanged(HierarchyNode<T> node) { ChildChanged?.Invoke(this, node); }

        public RootHierarchyNode() : base(null)
        {
            Root = this;
        }
    }
}
