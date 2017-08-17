using IwVoxelGame.Graphics;
using IwVoxelGame.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwVoxelGame.Blocks.World {
    public class Chunk {
        public const int Size = 16;
        private readonly Block[,,] _blocks;
        private readonly World _world;
        private readonly Vector3i _chunkPos;

        private VertexArrayObject<Vector3> vao;
        private bool needsRenderUpdate;
        
        public Chunk(World world, Vector3i chunkPos) {
            _blocks = new Block[Size, Size, Size];
            _world = world;
            _chunkPos = chunkPos;

            vao = new VertexArrayObject<Vector3>(2);
            vao.SetVbo(0, new VertexBufferObject<Vector3>(Vector3.SizeInBytes, 3, VertexAttribPointerType.Float));
            vao.SetVbo(1, new VertexBufferObject<Vector3>(Vector3.SizeInBytes, 3, VertexAttribPointerType.Float));

            Logger.Debug($"New chunk created at {chunkPos}");
        }

        public void SetBlock(Vector3i position, Block block) {
            _blocks[position.X, position.Y, position.Z] = block;
            needsRenderUpdate = true;
        }

        public Block GetBlock(Vector3i position) {
            if (!IndexIsValid(position)) return null;
            Block block = _blocks[position.X, position.Y, position.Z];

            return block;
        }

        public void Update() {
            if (needsRenderUpdate) {
                vao.Clear();

                AddBlocksToVao();

                vao.Upload();
            }
        }

        public void Generate() {
            _blocks[1, 0, 0] = new BlockStone();
        }

        public void Draw() {
            vao.Draw(BeginMode.Triangles, DrawElementsType.UnsignedInt);
        }

        private void AddBlocksToVao() {
            for (int x = 0; x < _blocks.GetLength(0); x++) {
                for (int y = 0; y < _blocks.GetLength(1); y++) {
                    for (int z = 0; z < _blocks.GetLength(2); z++) {
                        Vector3i blockPos = new Vector3i(x, y, z);
                        BlockFaceHelper.AddBlockToVbo(_world, _chunkPos * Size + blockPos, blockPos, _blocks[x, y, z], vao);
                    }
                }
            }
        }

        private bool IndexIsValid(Vector3i position) {
            return position.X >= 0 && position.X < Size
                && position.Y >= 0 && position.Y < Size
                && position.Z >= 0 && position.Z < Size;
        }
    }
}
