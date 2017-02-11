using System;
using System.Collections.Generic;

namespace Hippopotamus.Engine.Core
{
    public delegate void GameObjectAddedEventHandler(object sender, GameObjectEventArgs args);
    public class GameObjectEventArgs : EventArgs
    {
        public GameObject GameObject { get; }
        public GameObjectEventArgs(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }

    public static class GameObjectManager
    {
        public static readonly HashSet<GameObject> SubscribedObjects = new HashSet<GameObject>();
        public static GameObjectAddedEventHandler GameObjectSubscribed;
        public static void OnGameObjectSubscribed(GameObjectEventArgs args)
        {
            GameObjectSubscribed?.Invoke(null, args);
        }

        public static void Update()
        {
            foreach (GameObject subscribedObject in SubscribedObjects)
            {
                SystemManager.Update(subscribedObject);
            }
        }

        public static void Subscribe(GameObject gameObject)
        {
            if (gameObject == null) return;
            if (SubscribedObjects.Contains(gameObject)) return;

            SubscribedObjects.Add(gameObject);
            OnGameObjectSubscribed(new GameObjectEventArgs(gameObject));
        }

        public static void Unsubscribe(GameObject gameObject)
        {
            if (gameObject == null) return;
            if (SubscribedObjects.Contains(gameObject))
            {
                SubscribedObjects.Remove(gameObject);
            }
        }
    }
}
