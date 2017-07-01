using System.IO;
using UnityEngine;

public class AsteroidController
{
    private readonly GameObject[] asteroidPrefabs;
    public AsteroidController()
    {
        FileInfo[] files = new DirectoryInfo(Path.Combine(Application.dataPath, "Game/Resources/Prefabs/Asteroids")).GetFiles("*.prefab", SearchOption.TopDirectoryOnly);
        asteroidPrefabs = new GameObject[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            string filePath = string.Format("Prefabs/Asteroids/{0}", files[i].Name);
            asteroidPrefabs[i] = Resources.Load<GameObject>(filePath.Replace(".prefab", string.Empty));
        }
    }

    public void Spawn(int impactRadius, float speed, Tile targetTile)
    {
        GameObject asteroidPrefab = GetAsteroidPrefab();
        AsteroidInstance.Create(impactRadius, speed, targetTile.WorldPosition.ToVector2(), asteroidPrefab, GetImpactPrefab(asteroidPrefab));
    }

    private GameObject GetAsteroidPrefab()
    {
        return asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
    }

    private static GameObject GetImpactPrefab(Object asteroidPrefab)
    {
        return Resources.Load<GameObject>(string.Format("Prefabs/Asteroids/ImpactEffects/{0}_Impact", asteroidPrefab.name));
    }
}