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
        AsteroidInstance.Create(impactRadius, speed, targetTile.WorldPosition.ToVector2(), GetAsteroidPrefab());
    }

    private GameObject GetAsteroidPrefab()
    {
        return asteroidPrefabs[(int)Random.value * asteroidPrefabs.Length];
    }
}