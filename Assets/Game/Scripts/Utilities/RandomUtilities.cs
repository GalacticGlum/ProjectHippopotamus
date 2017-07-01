using System.Linq;
using UnityEngine;

public static class RandomUtilities
{
    public static string GenerateSeed()
    {
        // Reseed before generating a random seed...
        Random.InitState((int)System.DateTime.Now.Ticks);

        string animal = WordDictionary.Get("Animal");
        return string.Format("{0}{1}", WordDictionary.Get("Adjective"), char.ToUpper(animal.First()) + animal.Substring(1));
    }
}