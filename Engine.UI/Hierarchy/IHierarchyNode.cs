namespace Hippopotamus.Engine.UI.Hierarchy
{
    public interface IHierarchyNode<T> where T : class
    {
        HierarchyNode<T> GetHierarchyNode();
    }
}
