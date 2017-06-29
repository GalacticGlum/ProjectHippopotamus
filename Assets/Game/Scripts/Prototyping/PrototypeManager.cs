public static class PrototypeManager
{
    public static PrototypeContainer<ItemPrototype> Items { get; private set; }

    static PrototypeManager()
    {
        Items = new PrototypeContainer<ItemPrototype>("Items", "Item");
    }
}
