using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileCursor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Tile tile = WorldController.Instance.GetTileFromMousePosition();
        if (tile == null || tile.Type == TileType.Empty)
        {
            spriteRenderer.enabled = false;
            return;
        }

        spriteRenderer.enabled = true;
        transform.position = tile.WorldPosition.ToVector3(-1);
    }
}
