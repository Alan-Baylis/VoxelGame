using IwVoxelGame.Utils;
using System.Collections.Generic;

namespace IwVoxelGame.Blocks.World {
    public class World {
        private readonly Dictionary<Vector3i, Chunk> _chunks;

        public Dictionary<Vector3i, Chunk> Chunks => _chunks;

        public World() {
            _chunks = new Dictionary<Vector3i, Chunk>();
        }

        public void SetBlock(Vector3i position, Block block) {
            var pos = FormatPosition(position);

            if (_chunks.TryGetValue(pos.chunkPos, out Chunk chunk)) {
                chunk.SetBlock(pos.blockPos, block);
            } else {
                Chunk newChunk = new Chunk(this, pos.chunkPos);
                newChunk.SetBlock(pos.blockPos, block);
                _chunks.Add(pos.chunkPos, newChunk);
            }
        }

        public Block GetBlock(Vector3i position) {
            var pos = FormatPosition(position);

            Block block = null;
            if (_chunks.TryGetValue(pos.chunkPos, out Chunk chunk)) {
                block = chunk.GetBlock(pos.blockPos);
            }

            return block;
        }

        public void Update() {
            foreach (Chunk chunk in _chunks.Values) {
                chunk.Update();
            }
        }

        public void GenerateChunk(Vector3i chunkPos) {
            Chunk chunk = new Chunk(this, chunkPos);
            chunk.Generate();
            _chunks.Add(chunkPos, chunk);
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
