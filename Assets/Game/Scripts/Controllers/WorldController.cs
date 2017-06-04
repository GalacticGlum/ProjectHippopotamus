using System.Collections;
using UnityEngine;
using UnityUtilities.ObjectPool;

public class WorldController : MonoBehaviour 
{
    public static WorldController Instance { get; private set; }
    public World World { get; private set; }

    public bool HasLoaded { get; private set; }
    private TileGraphicController tileGraphicController;
    private CharacterGraphicController characterGraphicController;

    private void Start()
    {
        Instance = this;

        World = new World("Data/WorldProcessor.xml");
        World.Initialize(200, 4);
        World.AddGenerator(new TerrainWorldProcessor());
        World.AddGenerator(new TerrainValleyProcessor());
        World.AddGenerator(new TerrainPrairieProcessor());
        World.AddGenerator(new TerrainCaveProcessor());
        World.AddGenerator(new TerrainCleanup());

        Camera.main.transform.position = new Vector3((World.Width - 1) / 2.0f * Chunk.Size, (World.Height - 1) / 2.0f * Chunk.Size, -1);

        // Constant seed for debugging purposes.
        World.Generate("purplehippo");
        Generate();
    }

    private void Update()
    {
        if (!HasLoaded) return;
        World.Update();
    }

    private void Generate()
    {
        tileGraphicController = new TileGraphicController();
        characterGraphicController = new CharacterGraphicController();
        HasLoaded = true;
    }
}

