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
            World.Current.PlaceItem(new Item("Ice", 1), WorldController.Instance.GetTileFromMousePosition().WorldPosition);
        }

        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftControl))
        {
            Tile tile = WorldController.Instance.GetTileFromMousePosition();
            if (tile != null)
            {
                tile.Type = TileType.Parse("Quartz");
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
