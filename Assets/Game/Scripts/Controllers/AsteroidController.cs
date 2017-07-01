using UnityEngine;

public class AsteroidController
{
    private readonly GameObject[] asteroidPrefabs;
    public AsteroidController()
    {
        asteroidPrefabs = Resources.LoadAll<GameObject>("Prefabs/Asteroids");
    }

    public void Spawn(int impactRadius, float speed, Tile targetTile)
    {
        GameObject asteroidPrefab = GetAsteroidPrefab();
        AsteroidInstance.Create(impactRadius, speed, targetTile.WorldPosition.ToVector2(), asteroidPrefab, GetImpactPrefab(asteroidPrefab));
    }

    private GameObject GetAsteroidPrefab()
    {
        return asteroidPrefabs[(int)Random.value * asteroidPrefabs.Length];
    }

    private static GameObject GetImpactPrefab(Object asteroidPrefab)
    {
        return asteroidPrefab == null ? null : Resources.Load<GameObject>(string.Format("Prefabs/Asteroids/{0}_Impact", asteroidPrefab.name));
    }
}