using System;

public delegate void ItemChangedEventHandler(object sender, ItemEventArgs args);
public class ItemEventArgs : EventArgs
{
    public readonly Item Item;
    public ItemEventArgs(Item item)
    {
        Item = item;
    }
}

public class Item
{
    private const int DefaultMaxStackSize = 64;

    public string Type { get; set; }
    public int MaxStackSize { get; set; }

    private int stackSize = 1;
    public int StackSize
    {
        get { return stackSize; }
        set
        {
            if (stackSize == value) return;
            stackSize = value;

            OnItemChanged();
        }
    }

    public event ItemChangedEventHandler Changed;
    private void OnItemChanged()
    {
        if (Changed == null) return;
        Changed(this, new ItemEventArgs(this));
    }

    public Item()
    {
        MaxStackSize = DefaultMaxStackSize;
    }

    public Item(string type, int maxStackSize, int stackSize = 1)
    {
        Type = type;
        MaxStackSize = maxStackSize;
        StackSize = stackSize;
    }

    public Item(string type, int stackSize)
    {
        Type = type;
        MaxStackSize = PrototypeManager.Items.Contains(type) ? PrototypeManager.Items[type].MaxStackSize : DefaultMaxStackSize;
        StackSize = stackSize;
    }

    protected Item(Item item)
    {
        Type = item.Type;
        MaxStackSize = item.MaxStackSize;
        StackSize = item.StackSize;
    }

    public virtual Item Clone()
    {
        return new Item(this);
    }
}
