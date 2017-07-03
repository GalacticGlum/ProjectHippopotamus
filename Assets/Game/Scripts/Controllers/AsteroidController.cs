using System.IO;
using MoonSharp.Interpreter;
using UnityEngine;

[LuaExposeType]
[MoonSharpUserData]
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

    public void SpawnAtPlayer(int impactRadius, float speed)
    {
        Tile playerTile = Player.Current.GetTile();
        Spawn(impactRadius, speed, World.Current.GetTileAt(playerTile.WorldPosition.X, playerTile.WorldPosition.Y - 5));
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