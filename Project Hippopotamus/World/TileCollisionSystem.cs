using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hippopotamus.World
{
    public class TileCollisionSystem : EntitySystem
    {
        public TileCollisionSystem()
        {
            World.Current.ChunkLoaded += OnChunkLoaded;
            World.Current.ChunkUnloaded += OnChunkUnloaded;
        }

        private void OnChunkLoaded(object sender, ChunkEventArgs args)
        {
            //Logger.Log("CollisionSystem", $"Loaded chunk {args.Chunk.ToString()}");
            //CreateCollisionForChunk(args.Chunk);
        }

        private void OnChunkUnloaded(object sender, ChunkEventArgs args)
        {
            //Logger.Log("CollisionSystem", $"Unloaded chunk {args.Chunk.ToString()}");
        }

        private void CreateCollisionForChunk(Chunk chunk)
        {
            /*for (int x = 0; x < Chunk.Size; ++x)
            {
                for (int y = 0; y < Chunk.Size; ++y)
                {
                    if (HasCollision(chunk[x, y]))
                    {

                    }
                }
            }*/
        }

        private bool HasCollision(Tile tile)
        {
            return tile.Type != TileType.Empty;
        }

    }
}
