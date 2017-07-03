using UnityEngine;

/// <summary>
/// Instance of an asteroid which manages it's positon and destruction.
/// </summary>
public class AsteroidInstance : MonoBehaviour
{
    private float speed = 10;
    private int impactRadius;

    private Vector3 targetPosition;
    private GameObject impactPrefab;

    private void Update()
    {
        Vector2 movePosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        transform.position = new Vector3(movePosition.x, movePosition.y, -1);
        TerrainUtilities.GenerateCircle(1, World.Current.WorldData, WorldController.Instance.WorldCoordiantesToGridSpace(transform.position), TileType.Empty);

        if (transform.position != targetPosition) return;
        Impact();
    }

    private void Impact()
    {
        float size = impactRadius / 2f;
        Vector2i impactPosition = WorldController.Instance.WorldCoordiantesToGridSpace(transform.position + new Vector3(0, size));
        TerrainUtilities.GenerateFuzzyCircle(impactRadius, impactRadius + 2, World.Current.WorldData, impactPosition, TileType.Empty);

        GameObject impactGameObject = Instantiate(impactPrefab, transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        impactGameObject.transform.localScale = new Vector3(size, size, size);

        if (Vector3.Distance(transform.position, Player.Current.transform.position) <= impactRadius)
        {
            Player.Current.Shock(0.5f);
        }

        Destroy(impactGameObject, impactGameObject.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }

    public static AsteroidInstance Create(int impactRadius, float speed, Vector3 targetPosition, GameObject prefab, GameObject impactPrefab)
    {
        float angle = Random.Range(40, -40)  * Mathf.Deg2Rad;
        int distance = Mathf.FloorToInt(Camera.main.orthographicSize * 3);

        targetPosition.z = -1;
        Vector3 spawnPosition = targetPosition + new Vector3(Mathf.Sin(angle) * distance, Mathf.Cos(angle) * distance, -1);
        GameObject instance = Instantiate(prefab, spawnPosition, Quaternion.identity);
        AsteroidInstance asteroidInstance = instance.AddComponent<AsteroidInstance>();

        asteroidInstance.impactRadius = impactRadius;
        asteroidInstance.speed = speed;
        asteroidInstance.targetPosition = targetPosition;
        asteroidInstance.impactPrefab = impactPrefab;

        return asteroidInstance;
    }
}
