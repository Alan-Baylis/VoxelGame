using IwVoxelGame.Graphics;
using IwVoxelGame.Utils;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace IwVoxelGame.Blocks.World {
    /**
     * 
     * World
     * 
     * Queue chunks by position for loading
     * 'Load' chunk on thread by generating blocks
     * pass back information to main thread to be assigned to a new chunk
     * render all chunks
     * 
     * unload chunks if they get too far away
     **/

    public class World {
        private readonly Dictionary<Vector3i, Chunk> _chunks;
        private readonly Queue<Chunk> m_chunksToBeLoaded;
        private readonly Queue<Chunk> m_chunksToBeGenerated;

        public Dictionary<Vector3i, Chunk> Chunks => _chunks;

        public World() {
            _chunks = new Dictionary<Vector3i, Chunk>();
            m_chunksToBeLoaded = new Queue<Chunk>();
            m_chunksToBeGenerated = new Queue<Chunk>();
        }

        public void LoadChunk(Vector3i position) {
            if(!_chunks.TryGetValue(position, out Chunk chunk)) {
                chunk = new Chunk(this, position);
                _chunks.Add(position, chunk);
                //Task.Factory.StartNew(()=> { GenerateChunk(chunk); });
                GenerateChunk(chunk);
            }
        }

        private void GenerateChunk(Chunk chunk) {
            chunk.SetBlock(new Vector3i(0, 0, 0), new BlockStone());
            chunk.AddBlocksToVao();
            m_chunksToBeLoaded.Enqueue(chunk);
        }

        public void Update() {
            lock(m_chunksToBeLoaded) {
                while(m_chunksToBeLoaded.Count > 0) {
                    m_chunksToBeLoaded.Dequeue().Update();
                }
            }
        }

        public void SetBlock(Vector3i worldPos, Block block) {
            var position = FormatPosition(worldPos);
            if(_chunks.TryGetValue(position.chunkPos, out Chunk chunk)) {
                chunk.SetBlock(position.blockPos, block);
            } else {
                LoadChunk(position.chunkPos);
                SetBlock(worldPos, block);
            }
        }

        public Block GetBlock(Vector3i worldPos) {
            var position = FormatPosition(worldPos);

            if(_chunks.TryGetValue(worldPos, out Chunk chunk)) {
                return chunk.GetBlock(position.blockPos);
            } else {
                return null;
            }
        }

        private (Vector3i chunkPos, Vector3i blockPos) FormatPosition(Vector3i position) {
            Vector3i chunkPos = new Vector3i(
                position.X < 0 ? (position.X + 1) / Chunk.Size - 1 : position.X / Chunk.Size,
                position.Y < 0 ? (position.Y + 1) / Chunk.Size - 1 : position.Y / Chunk.Size,
                position.Z < 0 ? (position.Z + 1) / Chunk.Size - 1 : position.Z / Chunk.Size);

            Vector3i blockPos = new Vector3i(
                position.X < 0 ? (position.X + 1) % Chunk.Size + Chunk.Size - 1 : position.X % Chunk.Size,
                position.Y < 0 ? (position.Y + 1) % Chunk.Size + Chunk.Size - 1 : position.Y % Chunk.Size,
                position.Z < 0 ? (position.Z + 1) % Chunk.Size + Chunk.Size - 1 : position.Z % Chunk.Size);

            return (chunkPos, blockPos);
        }
    }
}
