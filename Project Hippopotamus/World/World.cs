using System;
using System.Collections.Generic;
using System.IO;
using Hippopotamus.Engine.Rendering;
using Microsoft.Xna.Framework;

namespace Hippopotamus.World
{
    public class World
    {
        public static World Current { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int HorizontalChunks { get; private set; }
        public int VerticalChunks { get; private set; }

        public event ChunkLoadedEventHandler ChunkLoaded;
        public event TileChangedEventHandler TileChanged;

        private readonly Chunk[,] chunks;
        private readonly List<IWorldGenerator> worldGeneratorPasses;

        public World(int horizontalChunks, int verticalChunks)
        {
            Current = this;

            HorizontalChunks = horizontalChunks;
            VerticalChunks = verticalChunks;

            Width = horizontalChunks * Chunk.Size;
            Height = verticalChunks * Chunk.Size;

            chunks = new Chunk[HorizontalChunks, VerticalChunks];
            worldGeneratorPasses = new List<IWorldGenerator>();
        }

        public void Generate()
        {
            Clear();
            foreach (IWorldGenerator pass in worldGeneratorPasses)
            {
                pass.Reseed();
                pass.Generate(this);
            }

            Save("moo.data");
        }

        public void Generate(int seed)
        {
            Clear();
            foreach (IWorldGenerator pass in worldGeneratorPasses)
            {
                pass.Reseed(seed);
                pass.Generate(this);
            }

            Save("moo.data");
        }

        public void Clear()
        {
            for (int x = 0; x < HorizontalChunks; x++)
            {
                for (int y = 0; y < VerticalChunks; y++)
                {
                    chunks[x, y] = new Chunk(new Vector2(x, y));
                    chunks[x, y].ChunkLoaded += OnChunkLoaded;
                    chunks[x, y].TileChanged += OnTileChanged;
                }
            }
        }

        private void OnChunkLoaded(object sender, ChunkEventArgs args)
        {
            ChunkLoaded?.Invoke(this, new ChunkEventArgs(args.Chunk));
        }

        public void OnTileChanged(object sender, TileEventArgs args)
        {
            TileChanged?.Invoke(this, args);
        }


        public void AddGenerator(TerrainWorldGenerator worldGenerator)
        {
            worldGeneratorPasses.Add(worldGenerator);
        }

        public Chunk GetChunkContaining(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return null;

            int chunkX = (int) Math.Floor(x / (double) Chunk.Size);
            int chunkY = (int)Math.Floor(y / (double)Chunk.Size);
            if (chunkX < 0 || chunkX >= HorizontalChunks || chunkY < 0 || chunkY >= VerticalChunks) return null;

            return chunks[chunkX, chunkY];
        }

        public Tile GetTileAt(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return null;
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
                    writer.Write(Chunk.Size);

                    Vector2 cameraPosition = Camera.Main.Transform.Position;
                    writer.Write(cameraPosition.X);
                    writer.Write(cameraPosition.Y);

                    for (int x = 0; x < HorizontalChunks; x++)
                    {
                        for (int y = 0; y < VerticalChunks; y++)
                        {
                            chunks[x, y].Save(writer);
                        }
                    }
                }

                File.WriteAllBytes(fileName, memoryStream.ToArray());
            }
        }

        public void Load(string fileName, Chunk chunk = null)
        {
            if (chunk != null && chunk.Loaded) return;

            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    Width = reader.ReadInt32();
                    Height = reader.ReadInt32();
                    int chunkSize = reader.ReadInt32();

                    float cameraX = reader.ReadSingle();
                    float cameraY = reader.ReadSingle();

                    HorizontalChunks = (int) Math.Ceiling(Width / (double) chunkSize);
                    VerticalChunks = (int) Math.Ceiling(Height / (double) chunkSize);

                    if (chunk == null)
                    {
                        Camera.Main.Transform.Position = new Vector2(cameraX, cameraY);
                        Clear();

                        chunk = GetChunkContaining((int)Math.Round(cameraX / Tile.Size), (int)Math.Round(cameraY / Tile.Size));
                    }

                    // Skip to the chunk we want to actually load in
                    long chunkPosition = (long) chunk.Position.X * VerticalChunks + (long) chunk.Position.Y;
                    long skipLength = 1 * Chunk.Size * Chunk.Size * chunkPosition;
                    reader.BaseStream.Seek(skipLength, SeekOrigin.Current);

                    chunk.Load(reader);
                }
            }
        }
    }
}
