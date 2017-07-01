using UnityEngine;

/// <summary>
/// Instance of an asteroid which manages it's positon and destruction.
/// </summary>
public class AsteroidInstance : MonoBehaviour
{
    private float speed = 10;
    private int radius;

    private Vector3 targetPosition;

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        TerrainUtilities.GenerateCircle(radius, World.Current.WorldData, WorldController.Instance.WorldCoordiantesToGridSpace(transform.position), TileType.Empty);

        if (transform.position == targetPosition)
        {
            // TODO: Play impact animation
            Destroy(gameObject);
        }
    }

    public static AsteroidInstance Create(int radius, float speed, Vector3 targetPosition, GameObject prefab)
    {
        float angle = Random.Range(40, -40)  * Mathf.Deg2Rad;
        int distance = Mathf.FloorToInt(Camera.main.orthographicSize * 3);

        Vector3 spawnPosition = targetPosition + new Vector3(Mathf.Sin(angle) * distance, Mathf.Cos(angle) * distance, -1);
        GameObject instance = Instantiate(prefab, spawnPosition, Quaternion.identity);
        AsteroidInstance asteroidInstance = instance.AddComponent<AsteroidInstance>();

        asteroidInstance.radius = radius;
        asteroidInstance.speed = speed;
        asteroidInstance.targetPosition = targetPosition;

        return asteroidInstance;
    }
}
