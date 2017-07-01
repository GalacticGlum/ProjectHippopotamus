using UnityEngine;

// Do items want to be able to move around??
// Maybe they should be linked to a object in the game (such as a tile); though this will look weird .
// Items could be linked to a tile and they will move to the tile from their spawn position?
// TODO: Player should not collide with items. 

/// <summary>
/// Stores an instance to the item that this instance is linked to.
/// </summary>
public class ItemInstance : MonoBehaviour
{
    public Item Item { get; private set; }

    public static GameObject Create(Item item, Transform parent = null)
    {
        if (item == null) return null;
        GameObject instance = new GameObject(string.Format("{0}_Instance", item.Type), typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D));
        instance.GetComponent<BoxCollider2D>().size = Vector2.one;
        instance.transform.position = item.SpawnPosition.ToVector3(1);
        instance.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/SpriteDiffuse");

        ItemInstance itemInstance = instance.AddComponent<ItemInstance>();
        itemInstance.Item = item;
        item.ItemInstance  = itemInstance;

        if (parent != null)
        {
            instance.transform.SetParent(parent, true);
        }

        return instance;
    }
}