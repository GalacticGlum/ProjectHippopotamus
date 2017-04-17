using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Rendering;
using Microsoft.Xna.Framework;

namespace Hippopotamus.World
{
    public class World
    {
        public static World Current { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int WidthInTiles { get; private set; }
        public int HeightInTiles { get; private set; }

        public WorldData WorldData { get; private set; }

        public event TileChangedEventHandler TileChanged;
        public void OnTileChanged(TileEventArgs args) { TileChanged?.Invoke(this, args); }

        public event ChunkLoadedEventHandler ChunkLoaded;
        public void OnChunkLoaded(ChunkEventArgs args) { ChunkLoaded?.Invoke(this, args); }

        public event ChunkLoadedEventHandler ChunkUnloaded;
        public void OnChunkUnloaded(ChunkEventArgs args) { ChunkUnloaded?.Invoke(this, args); }

        private Chunk[,] chunks;
        private readonly HashSet<Chunk> loadedChunks;
        private readonly Queue<Chunk> loadChunkQueue;
        private readonly Queue<Chunk> unloadChunkQueue;

        private readonly List<ITerrainGenerator> worldGeneratorPasses;

        public World()
        {
            Current = this;

            loadedChunks = new HashSet<Chunk>();
            loadChunkQueue = new Queue<Chunk>();
            unloadChunkQueue = new Queue<Chunk>();

            worldGeneratorPasses = new List<ITerrainGenerator>();
        }

        public void Initialize(int width, int height)
        {
            Width = width;
            Height = height;

            WidthInTiles = width * Chunk.Size;
            HeightInTiles = height * Chunk.Size;

            chunks = new Chunk[Width, Height];
            WorldData = new WorldData(WidthInTiles, HeightInTiles);
        }

        public void Generate()
        {
            CreateChunks();
            foreach (ITerrainGenerator pass in worldGeneratorPasses)
            {
                pass.Reseed();
                pass.Generate(WorldData);
            }

            Save("moo.data");
        }

        public void Generate(int seed)
        {
            CreateChunks();
            foreach (ITerrainGenerator pass in worldGeneratorPasses)
            {
                pass.Reseed(seed);
                pass.Generate(WorldData);
            }

            Save("moo.data");
        }

        public void Generate(string seed)
        {
            Generate(seed.GetHashCode());
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

        public void AddGenerator(ITerrainGenerator terrainGenerator)
        {
            worldGeneratorPasses.Add(terrainGenerator);
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

                    Vector2 cameraPosition = Camera.Main.Transform.Position;
                    writer.Write(cameraPosition.X);
                    writer.Write(cameraPosition.Y);

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
                    Camera.Main.Transform.Position = new Vector2(cameraX, cameraY);

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
            int screenWidth = GameEngine.Context.GraphicsDevice.Viewport.Width;
            int screenHeight = GameEngine.Context.GraphicsDevice.Viewport.Height;

            float zoom = Camera.Main.OrthographicSize;
            int viewportWidth = (int)Math.Ceiling((double)screenWidth / (Chunk.Size * Tile.Size * 2 * zoom)) + 2;
            int viewportHeight = (int)Math.Ceiling((double)screenHeight / (Chunk.Size * Tile.Size * 2 * zoom)) + 2;

            // this appears to be the coordinate of the TOP-LEFT pixel, is that correct?
            Vector2 cameraPosition = Camera.Main.Transform.Position;
            Vector2 tileAtCameraPosition = new Vector2(cameraPosition.X / Tile.Size, cameraPosition.Y / Tile.Size);
            Chunk chunkContaining = GetChunkContaining((int)Math.Round(tileAtCameraPosition.X), (int)Math.Round(tileAtCameraPosition.Y));

            if (chunkContaining == null) return;
            int viewpointX = (int)chunkContaining.Position.X;
            int viewpointY = (int)chunkContaining.Position.Y;

            // find x-coordinate of the leftmost pixel that belongs to this chunk
            int focusChunkLeftmostPosition = (int)(cameraPosition.X - cameraPosition.X % (Chunk.Size * Tile.Size));
            // find x-coordinate of the rightmost pixel that belongs to this chunk
            int focusChunkRightmostPosition = focusChunkLeftmostPosition + Chunk.Size * Tile.Size - 1;
            // find y-coordinate of the topmost pixel that belongs to this chunk
            int focusChunkTopmostPosition = (int)(cameraPosition.Y - cameraPosition.Y % (Chunk.Size * Tile.Size));
            //find y-coordinate of the bottommost pixel that belongs to this chunk
            int focusChunkBottommostPosition = focusChunkTopmostPosition + Chunk.Size * Tile.Size - 1;

            int screenLeftmostPosition = (int)cameraPosition.X;
            int screenRightmostPosition = (int)cameraPosition.X + screenWidth;
            int screenTopmostPosition = (int)cameraPosition.Y;
            int screenBottommostPosition = (int)cameraPosition.Y + screenHeight;

            int chunksToLeft = 0;
            while (focusChunkLeftmostPosition - chunksToLeft * Chunk.Size * Tile.Size + Tile.Size > screenLeftmostPosition)
            {
                ++chunksToLeft;
            }

            int chunksToRight = 0;
            while (focusChunkRightmostPosition + chunksToRight * Chunk.Size * Tile.Size - Tile.Size < screenRightmostPosition)
            {
                ++chunksToRight;
            }
            int chunksAbove = 0;
            while (focusChunkTopmostPosition - chunksAbove * Chunk.Size * Tile.Size + Tile.Size > screenTopmostPosition)
            {
                ++chunksAbove;
            }
            int chunksBelow = 0;
            while (focusChunkBottommostPosition + chunksBelow * Chunk.Size * Tile.Size - Tile.Size < screenBottommostPosition)
            {
                ++chunksBelow;
            }

            HashSet<Chunk> chunksToLoad = new HashSet<Chunk>();
            for (int x = -chunksToLeft; x <= chunksToRight; x++)
            {
                for (int y = -chunksAbove; y <= chunksBelow; y++)
                {
                    int chunkX = viewpointX + x;
                    int chunkY = viewpointY + y;
                    chunksToLoad.Add(chunks[chunkX, chunkY]);
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
    }
}
