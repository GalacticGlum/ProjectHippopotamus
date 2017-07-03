using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemGraphicController
{
    private readonly Transform itemParent;
    private readonly Dictionary<Item, GameObject> itemGameObjects;

    public ItemGraphicController()
    {
        // Setup parent
        itemParent = new GameObject("Items").transform;
        itemGameObjects = new Dictionary<Item, GameObject>();

        World.Current.ItemPlaced += OnItemPlaced;
        foreach (string type in World.Current.Items.Keys)
        {
            foreach (Item item in World.Current.Items[type])
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
        throw new NotImplementedException();
    }
}
