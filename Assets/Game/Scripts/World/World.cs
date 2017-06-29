using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hippopotamus.World;
using UnityEngine;
using Random = UnityEngine.Random;

public delegate void ItemPlacedEventHandler(object sender, ItemEventArgs args);
public class World
{
    public static World Current { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public int WidthInTiles { get; private set; }
    public int HeightInTiles { get; private set; }

    public WorldData WorldData { get; private set; }
    public Player Player { get; private set; }

    public Dictionary<string, List<Item>> Items { get; private set; }

    public event TileChangedEventHandler TileChanged;
    public void OnTileChanged(TileEventArgs args)
    {
        if (TileChanged != null)
        {
            TileChanged(this, args);
        }
    }

    public event ChunkLoadedEventHandler ChunkLoaded;
    public void OnChunkLoaded(ChunkEventArgs args)
    {
        if (ChunkLoaded != null)
        {
            ChunkLoaded(this, args);
        }
    }

    public event ChunkLoadedEventHandler ChunkUnloaded;
    public void OnChunkUnloaded(ChunkEventArgs args)
    {
        if (ChunkUnloaded != null)
        {
            ChunkUnloaded(this, args);
        }
    }

    public event ItemPlacedEventHandler ItemPlaced;
    private void OnItemPlaced(ItemEventArgs args)
    {
        if (ItemPlaced == null) return;
        ItemPlaced(this, args);
    }

    private Chunk[,] chunks;
    private readonly HashSet<Chunk> loadedChunks;
    private readonly Queue<Chunk> loadChunkQueue;
    private readonly Queue<Chunk> unloadChunkQueue;

    private readonly TerrainProcessor terrainProcessor;
    private readonly List<ITerrainProcessor> terrainProcesses;

    private bool playerLoaded;

    public World(string terrainProcessorConfiguration)
    {
        Current = this;

        loadedChunks = new HashSet<Chunk>();
        loadChunkQueue = new Queue<Chunk>();
        unloadChunkQueue = new Queue<Chunk>();

        terrainProcessor = new TerrainProcessor(terrainProcessorConfiguration);
        terrainProcesses = new List<ITerrainProcessor>();
    }

    public void Initialize(int width, int height)
    {
        Width = width;
        Height = height;

        WidthInTiles = width * Chunk.Size;
        HeightInTiles = height * Chunk.Size;

        chunks = new Chunk[Width, Height];
        WorldData = new WorldData(WidthInTiles, HeightInTiles);
        Items = new Dictionary<string, List<Item>>();

        LoadPrototypes();
    }

    private static void LoadPrototypes()
    {
        string filePath = Path.Combine(Path.Combine(Application.streamingAssetsPath, "Data"), "Items.xml");
        PrototypeManager.Items.Load(File.ReadAllText(filePath));

        foreach (ItemPrototype item in PrototypeManager.Items)
        {
            Debug.Log(item.Type);
            Debug.Log(item.MaxStackSize);
            Debug.Log("------");
        }
    }

    public void Generate()
    {
        CreateChunks();
        foreach (ITerrainProcessor process in terrainProcesses)
        {
            process.Generate(WorldData);
        }

        //terrainProcessor.Execute(WorldData);
        //Table generatedData = Lua.GetVariable("worldData").Table;
        //for (int x = 0; x < WorldData.Width; ++x)
        //{
        //    for (int y = 0; y < WorldData.Height; ++y)
        //    {
        //        int key = x * WorldData.Height + y;
        //        DynValue value = generatedData.Get(key);

        //        if (value.IsNotNil())
        //        {
        //            WorldData.SetTileTypeAt(x, y, TileType.Grass/*(TileType)generatedData[key]*/);
        //        }
        //    }
        //}

        //Lua.RunSourceCode("worldData = nil; collectgarbage()");

        //CreatePlayer();
        Save("moo.data");
    }

    public void Generate(string seed)
    {
        Random.InitState(seed.GetHashCode());
        Generate();
    }

    public void CreateChunks()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                chunks[x, y] = new Chunk(new Vector2(x, y), new Vector2(x, y) * Chunk.Size);
            }
        }
    }

    public Chunk GetChunkContaining(int x, int y)
    {
        if (x < 0 || x >= WidthInTiles || y < 0 || y >= HeightInTiles) return null;

        int chunkX = (int)Math.Floor(x / (double)Chunk.Size);
        int chunkY = (int)Math.Floor(y / (double)Chunk.Size);
        if (chunkX < 0 || chunkX >= Width || chunkY < 0 || chunkY >= Height) return null;

        return chunks[chunkX, chunkY];
    }

    public Tile GetTileAt(int x, int y)
    {
        if (x < 0 || x >= WidthInTiles || y < 0 || y >= HeightInTiles) return null;
        Chunk chunk = GetChunkContaining(x, y);

        // Tile (x) position relative to the chunk
        int tileX = x % Chunk.Size;
        // Tile (y) position relative to the chunk
        int tileY = y % Chunk.Size;

        if (tileX < 0 || tileX >= Chunk.Size || tileY < 0 || tileY >= Chunk.Size) return null;
        return chunk.GetTileAt(tileX, tileY);
    }

    public Tile GetTileAt(Vector2i position)
    {
        return GetTileAt(position.X, position.Y);
    }

    public Chunk GetChunkAt(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return null;
        return chunks[x, y];
    }

    public void Save(string fileName)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(memoryStream))
            {
                writer.Write(Width);
                writer.Write(Height);

                Vector2 cameraPosition = Camera.main.transform.position;
                writer.Write(cameraPosition.x);
                writer.Write(cameraPosition.y);

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        for (int tx = 0; tx < Chunk.Size; tx++)
                        {
                            for (int ty = 0; ty < Chunk.Size; ty++)
                            {
                                writer.Write((byte)WorldData.Tiles[x * Chunk.Size + tx, y * Chunk.Size + ty]);
                            }
                        }
                    }
                }
            }

            File.WriteAllBytes(fileName, memoryStream.ToArray());
        }
    }

    public void Load(string fileName)
    {
        using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
        {
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();

                Initialize(width, height);

                float cameraX = reader.ReadSingle();
                float cameraY = reader.ReadSingle();
                Camera.main.transform.position = new Vector2(cameraX, cameraY);

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        for (int tx = 0; tx < Chunk.Size; tx++)
                        {
                            for (int ty = 0; ty < Chunk.Size; ty++)
                            {
                                WorldData.Tiles[x * Chunk.Size + tx, y * Chunk.Size + ty] = (TileType)reader.ReadByte();
                            }
                        }
                    }
                }
            }
        }
    }

    public void Update()
    {
        if (!playerLoaded)
        {
            Vector2i playerSpawnPosition = ChoosePlayerSpawnLocation();
            Camera.main.transform.position = playerSpawnPosition.ToVector3();

            Player = WorldController.Instance.CreatePlayer(playerSpawnPosition);
            playerLoaded = true;
        }

        LoadChunks(Camera.main.transform.position);
    }

    /// <summary>
    /// Place an item into the world.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="position"></param>
    public void PlaceItem(Item item, Vector2i position)
    {
        if (Items.ContainsKey(item.Type) == false)
        {
            Items[item.Type] = new List<Item>();
        }

        Items[item.Type].Add(item);
        item.SpawnPosition = position;

        OnItemPlaced(new ItemEventArgs(item));
    }

    private void LoadChunks(Vector2 loadFromPosition)
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        Chunk chunkContaining = GetChunkContaining((int)Math.Floor(loadFromPosition.x), (int)Math.Floor(loadFromPosition.y));

        if (chunkContaining == null) return;
        int viewpointX = (int)chunkContaining.Position.x;
        int viewpointY = (int)chunkContaining.Position.y;

        // find x-coordinate of the leftmost tile that belongs to this chunk
        int focusChunkLeftmostPosition = (int)(loadFromPosition.x - loadFromPosition.x % Chunk.Size);
        // find x-coordinate of the rightmost tile that belongs to this chunk
        int focusChunkRightmostPosition = focusChunkLeftmostPosition + Chunk.Size - 1;
        // find y-coordinate of the bottommost tile that belongs to this chunk
        int focusChunkBottommostPosition = (int)(loadFromPosition.y - loadFromPosition.y % Chunk.Size);
        // find y-coordinate of the topmost tile that belongs to this chunk
        int focusChunkTopmostPosition = (int)(focusChunkBottommostPosition + Chunk.Size - 1);


        float aspectRatio = (float)screenWidth / screenHeight;
        float cameraHeight = Camera.main.orthographicSize;

        int screenLeftmostPosition = (int)(loadFromPosition.x - cameraHeight * aspectRatio);
        int screenRightmostPosition = (int)(loadFromPosition.x + cameraHeight * aspectRatio);
        int screenBottommostPosition = (int)(loadFromPosition.y - cameraHeight);
        int screenTopmostPosition = (int)(loadFromPosition.y + cameraHeight);

        const int buffer = 2;
        int chunksToLeft = 0;
        while (focusChunkLeftmostPosition - chunksToLeft * Chunk.Size > screenLeftmostPosition - buffer)
        {
            ++chunksToLeft;
        }
        int chunksToRight = 0;
        while (focusChunkRightmostPosition + chunksToRight * Chunk.Size < screenRightmostPosition + buffer)
        {
            ++chunksToRight;
        }
        int chunksBelow = 0;
        while (focusChunkBottommostPosition - chunksBelow * Chunk.Size > screenBottommostPosition - buffer)
        {
            ++chunksBelow;
        }
        int chunksAbove = 0;
        while (focusChunkTopmostPosition + chunksAbove * Chunk.Size < screenTopmostPosition + buffer)
        {
            ++chunksAbove;
        }


        HashSet<Chunk> chunksToLoad = new HashSet<Chunk>();
        for (int x = -chunksToLeft; x <= chunksToRight; x++)
        {
            for (int y = -chunksBelow; y <= chunksAbove; y++)
            {
                int chunkX = viewpointX + x;
                int chunkY = viewpointY + y;
                Chunk chunkToAdd = GetChunkAt(chunkX, chunkY);
                if (chunkToAdd != null)
                {
                    chunksToLoad.Add(chunks[chunkX, chunkY]);
                }
            }
        }

        IEnumerable<Chunk> chunksToUnload = loadedChunks.Except(chunksToLoad);
        foreach (Chunk chunk in chunksToUnload.ToList())
        {
            if (!unloadChunkQueue.Contains(chunk) && chunk.Loaded)
            {
                unloadChunkQueue.Enqueue(chunk);
            }
        }

        foreach (Chunk chunk in chunksToLoad)
        {
            if (!loadChunkQueue.Contains(chunk) && !chunk.Loaded)
            {
                loadChunkQueue.Enqueue(chunk);
            }
        }

        if (unloadChunkQueue.Count > 0)
        {
            Chunk chunk = unloadChunkQueue.Dequeue();
            chunk.Unload();

            loadedChunks.Remove(chunk);
        }

        // ReSharper disable once InvertIf
        if (loadChunkQueue.Count > 0)
        {
            Chunk chunk = loadChunkQueue.Dequeue();
            chunk.Load(WorldData);

            loadedChunks.Add(chunk);
        }
    }

    public void AddGenerator(ITerrainProcessor terrainProcess)
    {
        terrainProcesses.Add(terrainProcess);
    }

    private Vector2i ChoosePlayerSpawnLocation()
    {
        int x = Mathf.RoundToInt((Width - 1) / 2.0f * Chunk.Size);
        int positionY = 0;
        for (int y = WorldData.Height; y >= 0; --y)
        {
            if (WorldData.GetTileTypeAt(x, y) == TileType.Empty) continue;

            positionY = y + 6;
            break;
        }

        return new Vector2i(x, positionY);
    }
}

