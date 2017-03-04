using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Rendering;
using Microsoft.Xna.Framework;
using Ninject;

namespace Hippopotamus.World
{
    public class World
    {
        public static World Current { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int WidthInTiles { get; private set; }
        public int HeightInTiles { get; private set; }

        public event ChunkLoadedEventHandler ChunkLoaded;
        public event ChunkLoadedEventHandler ChunkUnloaded;
        public event TileChangedEventHandler TileChanged;

        private Chunk[,] chunks;
        private readonly HashSet<Chunk> loadedChunks;

        private readonly List<IWorldGenerator> worldGeneratorPasses;
        private WorldData worldData;

        public World()
        {
            Current = this;

            loadedChunks = new HashSet<Chunk>();
            worldGeneratorPasses = new List<IWorldGenerator>();
        }

        public void Initialize(int width, int height)
        {
            Width = width;
            Height = height;

            WidthInTiles = width * Chunk.Size;
            HeightInTiles = height * Chunk.Size;

            chunks = new Chunk[Width, Height];
            worldData = new WorldData(WidthInTiles, HeightInTiles);
        }

        public void Generate()
        {
            CreateChunks();
            foreach (IWorldGenerator pass in worldGeneratorPasses)
            {
                pass.Reseed();
                pass.Generate(worldData);
            }

            Save("moo.data");
        }

        public void Generate(int seed)
        {
            CreateChunks();
            foreach (IWorldGenerator pass in worldGeneratorPasses)
            {
                pass.Reseed(seed);
                pass.Generate(worldData);
            }

            Save("moo.data");
        }

        public void CreateChunks()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    chunks[x, y] = new Chunk(new Vector2(x, y));
                    chunks[x, y].ChunkLoaded += OnChunkLoaded;
                    chunks[x, y].ChunkUnloaded += OnChunkUnloaded;
                    chunks[x, y].TileChanged += OnTileChanged;
                }
            }
        }

        private void OnChunkLoaded(object sender, ChunkEventArgs args)
        {
            ChunkLoaded?.Invoke(this, new ChunkEventArgs(args.Chunk));
        }

        private void OnChunkUnloaded(object sender, ChunkEventArgs args)
        {
            ChunkUnloaded?.Invoke(this, new ChunkEventArgs(args.Chunk));
        }

        private void OnTileChanged(object sender, TileEventArgs args)
        {
            TileChanged?.Invoke(this, args);
        }

        public void AddGenerator(TerrainWorldGenerator worldGenerator)
        {
            worldGeneratorPasses.Add(worldGenerator);
        }

        public Chunk GetChunkContaining(int x, int y)
        {
            if (x < 0 || x >= WidthInTiles || y < 0 || y >= HeightInTiles) return null;

            int chunkX = (int) Math.Floor(x / (double) Chunk.Size);
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
                                    writer.Write((byte)worldData.Tiles[x * Chunk.Size + tx, y * Chunk.Size + ty]);
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
                                    worldData.Tiles[x * Chunk.Size + tx, y * Chunk.Size + ty] = (TileType) reader.ReadByte();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Update()
        {
            int screenWidth = DependencyInjector.Kernel.Get<GameEngine>().GraphicsDevice.Viewport.Width;
            int screenHeight = DependencyInjector.Kernel.Get<GameEngine>().GraphicsDevice.Viewport.Height;

            float zoom = Camera.Main.OrthographicSize;
            int viewportWidth = (int) Math.Ceiling((double)screenWidth / (Chunk.Size * Tile.Size * 2 * zoom));
            int viewportHeight = (int)Math.Ceiling((double)screenHeight / (Chunk.Size * Tile.Size * 2 * zoom));

            Vector2 cameraPosition = Camera.Main.Transform.Position;
            Vector2 tileAtCameraPosition = new Vector2(cameraPosition.X / Tile.Size, cameraPosition.Y / Tile.Size);
            Chunk chunkContaining = GetChunkContaining((int)Math.Round(tileAtCameraPosition.X), (int)Math.Round(tileAtCameraPosition.Y));

            if (chunkContaining == null) return;
            int viewpointX = (int) chunkContaining.Position.X;
            int viewpointY = (int) chunkContaining.Position.Y;

            HashSet<Chunk> chunksToLoad = new HashSet<Chunk>();

            for (int x = -viewportWidth; x <= viewportWidth; x++)
            {
                for (int y = -viewportHeight; y <= viewportHeight; y++)
                {
                    int chunkX = viewpointX + x;
                    int chunkY = viewpointY + y;
                    if (chunkX >= 0 && chunkX < Width && chunkY >= 0 && chunkY < Height)
                    {
                        chunksToLoad.Add(chunks[chunkX, chunkY]);
                    }
                }
            }

            IEnumerable<Chunk> chunksToUnload = loadedChunks.Except(chunksToLoad);
            foreach (Chunk chunk in chunksToUnload.ToList())
            {
                chunk.Unload();
                loadedChunks.Remove(chunk);
            }

            foreach (Chunk chunk in chunksToLoad)
            {
                chunk.Load(worldData);
                loadedChunks.Add(chunk);
            }
        }
    }
}
