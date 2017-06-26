using UnityEngine;

public class MouseController
{
	public void Update ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Tile tile = WorldController.Instance.GetTileFromWorldCoordinates(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (tile != null)
            {
                tile.Type = TileType.Empty;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Tile tile = WorldController.Instance.GetTileFromWorldCoordinates(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (tile != null)
            {
                tile.Type = TileType.Grass;
            }
        }
    }
}
