using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidbody2D;
    private bool stopForce;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (stopForce) return;
        rigidbody2D.velocity = new Vector2(10, -10);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        stopForce = true;
        Destroy(gameObject, 1);

        // TODO: Save to WorldData 
        Tile tile = WorldController.Instance.GetTileFromWorldCoordinates(transform.position);
        TerrainUtilities.StampCircle(Random.Range(1, 6), World.Current, tile, TileType.Empty);
    }
}
