using IwVoxelGame.Graphics;
using IwVoxelGame.Utils;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace IwVoxelGame.Blocks.World {
    public class World {
        public const int MaxChunkUpdate = 6;

        private readonly Dictionary<Vector3i, Chunk> _loadedChunks;
        private readonly Dictionary<Vector3i, Chunk> _chunksReadyToAdd;
        private readonly List<Vector3i> _chunksReadyToRemove;

        private readonly Queue<Chunk> _chunksReadyToUpload;
        private readonly Queue<Chunk> _populatedChunks;
        private readonly Queue<Chunk> _chunkUpdates;

        private readonly Thread _unloadThread;
        private readonly Thread _loadThread;
        private readonly Thread _updateThread;

        public World() {
            _loadedChunks = new Dictionary<Vector3i, Chunk>();
            _chunksReadyToAdd = new Dictionary<Vector3i, Chunk>();
            _chunksReadyToRemove = new List<Vector3i>();
            _chunksReadyToUpload = new Queue<Chunk>();
            _populatedChunks = new Queue<Chunk>();
            _chunkUpdates = new Queue<Chunk>();
            
            _unloadThread = new Thread(UnloadThread) { Name = "Unload Thread" };
            _loadThread = new Thread(LoadThread) { Name = "Load Thread" };
            _updateThread = new Thread(UpdateThread) { Name = "Update Thread" };

            _unloadThread.Start();
            _loadThread.Start();
            _updateThread.Start();
        }


        
        public void SetBlock(int x, int y, int z, Block block, bool update) { SetBlock(new Vector3i(x, y, z), block, update); }
        public void SetBlock(Vector3i pos, Block block, bool update) {
            var position = FormatPosition(pos);
            Vector3i chunkPos = position.chunkPos;
            Vector3i blockPos = position.blockPos;

            if(_loadedChunks.TryGetValue(chunkPos, out Chunk chunk)) {
                if (chunk.GetBlock(blockPos).Id == block.Id) return;
                chunk.SetBlock(blockPos, block);
            } else {
                chunk = new Chunk(this, chunkPos);
                chunk.SetBlock(blockPos, block);
            }

            //Do not update chunks
            if (!update) return;

            //Update chunks
            if (blockPos.X == 0)                   QueueChunkUpdate(chunkPos + new Vector3i(-1, 0, 0));
            else if (blockPos.X == Chunk.Size - 1) QueueChunkUpdate(chunkPos + new Vector3i(+1, 0, 0));

            if (blockPos.Y == 0)                   QueueChunkUpdate(chunkPos + new Vector3i(0, -1, 0));
            else if (blockPos.Y == Chunk.Size - 1) QueueChunkUpdate(chunkPos + new Vector3i(0, +1, 0));

            if (blockPos.Z == 0)                   QueueChunkUpdate(chunkPos + new Vector3i(0, 0, -1));
            else if (blockPos.Z == Chunk.Size - 1) QueueChunkUpdate(chunkPos + new Vector3i(0, 0, +1));

            QueueChunkUpdate(chunk);
        }

        public void QueueChunkUpdate(Vector3i chunkPos) {
            if(_loadedChunks.TryGetValue(chunkPos, out Chunk chunk)) {
                QueueChunkUpdate(chunk);
            }
        }

        public void QueueChunkUpdate(Chunk chunk) {
            lock(_chunkUpdates) {
                _chunkUpdates.Enqueue(chunk);
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
