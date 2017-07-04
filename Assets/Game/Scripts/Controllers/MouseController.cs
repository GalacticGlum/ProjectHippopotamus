using UnityEngine;

public class MouseController
{
	public void Update ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Tile tile = WorldController.Instance.GetTileFromMousePosition();
            if (tile != null)
            {
                tile.Type = TileType.Empty;
            }
        }

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            //WorldController.Instance.GetTileFromMousePosition().PlaceItem("Ice", 1);
            World.Current.PlaceItem("Ice", 1, WorldController.Instance.GetTileFromMousePosition());
        }

        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftControl))
        {
            Tile tile = WorldController.Instance.GetTileFromMousePosition();
            if (tile != null)
            {
                //tile.Type = TileType.Parse("Ice");
                Debug.Log(tile.Item == null ? "Item null" : string.Format("Item {0}, {1}", tile.Item.Type, tile.Item.StackSize));
            }
        }

        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftControl))
        {
            Tile tile = WorldController.Instance.GetTileFromMousePosition();
            if (tile != null)
            {
                WorldController.Instance.AsteroidController.Spawn(5, 10, tile);
            }
        }
    }
}
