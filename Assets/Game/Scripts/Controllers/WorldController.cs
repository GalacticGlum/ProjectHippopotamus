using System;
using UnityEngine;

public delegate void WorldControllerLoadedEventHandler(object sender, WorldControllerEventArgs args);
public class WorldControllerEventArgs : EventArgs
{
    public readonly WorldController WorldController;
    public WorldControllerEventArgs(WorldController worldController)
    {
        WorldController = worldController;
    }
}

public class WorldController : MonoBehaviour 
{
    public static WorldController Instance { get; private set; }
    public World World { get; private set; }

    public bool HasLoaded { get; private set; }

    public event WorldControllerLoadedEventHandler Loaded;
    private void OnLoaded()
    {
        if (Loaded == null) return;
        Loaded(this, new WorldControllerEventArgs(this));
    }

    [SerializeField]
    private GameObject playerObject;

    private MouseController mouseController;
    private WorldGraphicController worldGraphicController;
    private TileGraphicController tileGraphicController;

    private void OnEnable()
    {
        Instance = this;

        World = new World("Data/WorldProcessor.xml");
        World.Initialize(200, 4);
        World.AddGenerator(new TerrainWorldProcessor());
        World.AddGenerator(new TerrainValleyProcessor());
        World.AddGenerator(new TerrainPrairieProcessor());
        World.AddGenerator(new TerrainCaveProcessor());
        World.AddGenerator(new TerrainCleanup());

        // Constant seed for debugging purposes.
        World.Generate();
        Generate();

        OnLoaded();
    }

    private void Update()
    {
        if (!HasLoaded) return;
        World.Update();
        mouseController.Update();
    }

    private void Generate()
    {
        worldGraphicController = new WorldGraphicController();
        tileGraphicController = new TileGraphicController();
        mouseController = new MouseController();

        HasLoaded = true;
    }

    public Player CreatePlayer(Vector2i position)
    {
        if (playerObject == null) return null;
        playerObject.transform.position = position.ToVector3();
        return playerObject.GetComponent<Player>();
    }

    public Tile GetTileFromWorldCoordinates(Vector2 worldCoordinates)
    {
        int x = Mathf.FloorToInt(worldCoordinates.x + 0.5f);
        int y = Mathf.FloorToInt(worldCoordinates.y + 0.5f);
        return World.GetTileAt(x, y);
    }

    public Tile GetTileFromMousePosition()
    {
        return GetTileFromWorldCoordinates(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}

