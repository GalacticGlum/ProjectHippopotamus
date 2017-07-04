using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ItemGraphicController
{
    private readonly Transform itemParent;
    private readonly Dictionary<Item, GameObject> itemGameObjects;

    public ItemGraphicController()
    {
        int itemLayer = LayerMask.NameToLayer("Items");
        Physics2D.IgnoreLayerCollision(itemLayer, itemLayer);

        // Setup parent
        itemParent = new GameObject("Items").transform;
        itemGameObjects = new Dictionary<Item, GameObject>();

        World.Current.ItemPlaced += OnItemPlaced;
        World.Current.ItemRemoved += OnItemRemoved;

        Dictionary<string, List<Item>> items = World.Current.GetItems();
        foreach (string type in items.Keys)
        {
            foreach (Item item in items[type])
            {
                OnItemPlaced(this, new ItemEventArgs(item));
            }
        }
    }

    private void OnItemPlaced(object sender, ItemEventArgs args)
    {
        GameObject itemGameObject = ItemInstance.Create(args.Item, itemParent);
        itemGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        itemGameObjects.Add(args.Item, itemGameObject);

        SpriteRenderer spriteRenderer = itemGameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = SpriteManager.Get("Item", args.Item.Type);
        spriteRenderer.sortingLayerName = "Item";

        args.Item.Changed += OnItemChanged;
    }

    private void OnItemChanged(object sender, ItemEventArgs args)
    {
    }

    private void OnItemRemoved(object sender, ItemEventArgs args)
    {
        if (!itemGameObjects.ContainsKey(args.Item)) return;
        Object.Destroy(itemGameObjects[args.Item]);
    }
}
