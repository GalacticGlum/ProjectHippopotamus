using UnityEngine;

// TODO: Add bobbing effect
// TODO: Multiple items on the same tile???????

/// <summary>
/// Stores an instance to the item that this instance is linked to. 
/// This simply updates the tile of the item using it's world position.
/// </summary>
public class ItemInstance : MonoBehaviour
{
    public Item Item { get; private set; }
    private Vector3 transformPosition;

    private void Update()
    {
        if (transform.position == transformPosition) return;

        transformPosition = transform.position;
        Item.Tile.Item = null;
        Tile tile = WorldController.Instance.GetTileFromWorldCoordinates(transformPosition);
        if (tile == null) return;
        if (tile.Item != null && tile.Item.Type == Item.Type && tile.Item != Item)
        {
            Debug.Log("hi");
            tile.Item.StackSize += Item.StackSize;
            World.Current.RemoveItem(Item);
            return;
        }

        Item.Tile = tile;
        tile.Item = Item;
    }

    public static GameObject Create(Item item, Transform parent = null)
    {
        if (item == null) return null;
        GameObject instance = new GameObject(string.Format("{0}_Instance", item.Type), typeof(Rigidbody2D), typeof(SpriteRenderer), 
            typeof(BoxCollider2D));
        
        instance.GetComponent<Rigidbody2D>().sharedMaterial = Resources.Load<PhysicsMaterial2D>("PhysicsMaterials/Bounce");
        instance.GetComponent<BoxCollider2D>().size = Vector2.one;
        instance.transform.position = item.Tile.WorldPosition.ToVector3(1);

        SpriteRenderer spriteRenderer = instance.GetComponent<SpriteRenderer>();
        spriteRenderer.material = Resources.Load<Material>("Materials/SpriteDiffuse");
        spriteRenderer.sortingLayerName = "Items";

        instance.layer = LayerMask.NameToLayer("Items");

        ItemInstance itemInstance = instance.AddComponent<ItemInstance>();
        itemInstance.Item = item;
        //item.ItemInstance  = itemInstance;

        if (parent != null)
        {
            instance.transform.SetParent(parent, true);
        }

        return instance;
    }
}