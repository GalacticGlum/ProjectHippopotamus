using System.Collections;
using UnityEngine;
using UnityUtilities.ObjectPool;

public class WorldController : MonoBehaviour 
{
    public static WorldController Instance { get; private set; }
    public World World { get; private set; }

    public bool HasLoaded { get; private set; }

    private MouseController mouseController;
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
    }

    private void Update()
    {
        if (!HasLoaded) return;
        World.Update();
        mouseController.Update();
    }

    private void Generate()
    {
        tileGraphicController = new TileGraphicController();
        mouseController = new MouseController();

        HasLoaded = true;
    }
}

