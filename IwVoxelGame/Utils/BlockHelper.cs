using IwVoxelGame.Blocks;
using IwVoxelGame.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using IwVoxelGame.Blocks.World;

namespace IwVoxelGame.Utils {
    public static class BlockFaceHelper {
        public static readonly BlockFace[] faces = new BlockFace[] {
            BlockFace.LEFT,
            BlockFace.RIGHT,
            BlockFace.BOTTOM,
            BlockFace.TOP,
            BlockFace.BACK,
            BlockFace.FRONT
        };

        private static readonly Vector3[] facePositions = {
            //left
            new Vector3(-0.5f, +0.5f, -0.5f), new Vector3(-0.5f, +0.5f, +0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, +0.5f),
            //right
            new Vector3(+0.5f, +0.5f, +0.5f), new Vector3(+0.5f, +0.5f, -0.5f),
            new Vector3(+0.5f, -0.5f, +0.5f), new Vector3(+0.5f, -0.5f, -0.5f),
            //bottom
            new Vector3(-0.5f, -0.5f, +0.5f), new Vector3(+0.5f, -0.5f, +0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(+0.5f, -0.5f, -0.5f),
            //top
            new Vector3(-0.5f, +0.5f, -0.5f), new Vector3(+0.5f, +0.5f, -0.5f),
            new Vector3(-0.5f, +0.5f, +0.5f), new Vector3(+0.5f, +0.5f, +0.5f),
            //back
            new Vector3(+0.5f, +0.5f, -0.5f), new Vector3(-0.5f, +0.5f, -0.5f),
            new Vector3(+0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, -0.5f),
            //front
            new Vector3(-0.5f, +0.5f, +0.5f), new Vector3(+0.5f, +0.5f, +0.5f),
            new Vector3(-0.5f, -0.5f, +0.5f), new Vector3(+0.5f, -0.5f, +0.5f)
        };

        private static readonly Vector2[] faceTexCoords = {
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(0, 1), new Vector2(1, 1),
        };

        private static readonly uint[] faceIndices = { 2, 1, 0, 2, 3, 1 };

        public static Vector3i GetNormal(this BlockFace blockface) {
            switch (blockface) {
                case BlockFace.LEFT: return new Vector3i(-1, 0, 0);
                case BlockFace.RIGHT: return new Vector3i(+1, 0, 0);
                case BlockFace.BOTTOM: return new Vector3i(0, -1, 0);
                case BlockFace.TOP: return new Vector3i(0, +1, 0);
                case BlockFace.BACK: return new Vector3i(0, 0, -1);
                case BlockFace.FRONT: return new Vector3i(0, 0, +1);
                default: {
                        Exception ex = new Exception("Invalid BlockFace");
                        Logger.Exception(ex);
                        throw ex;
                    }
            }
        }

        public static void AddBlockToVbo(World world, Vector3i worldPos, Vector3i blockPos, Block block, VertexArrayObject<Vector3> vao) {
            if (block == null) return;

            foreach (BlockFace face in faces) {
                Vector3i normal = face.GetNormal();
                if (world.GetBlock(worldPos + normal) == null) {
                    AddFaceToVbo(block, blockPos, face, vao);
                }
            }
        }

        private static void AddFaceToVbo(Block block, Vector3i blockPos, BlockFace blockFace, VertexArrayObject<Vector3> vao) {
            int indicesOffset = vao.GetBuffer(0).Length;

            for (int i = 0; i < 4; i++) {
                vao.Add(0, facePositions[(int)blockFace * 4 + i] + blockPos.AsVector3());
            }

            int textureIndex = block.GetTextureId(blockFace);
            for (int i = 0; i < 4; i++) {
                vao.Add(1, new Vector3(faceTexCoords[i]) { Z = textureIndex });
            }

            uint[] newIndices = new uint[faceIndices.Length];
            for (int i = 0; i < newIndices.Length; i++) {
                newIndices[i] = (uint)(faceIndices[i] + indicesOffset);
            }

            vao.AddIndices(newIndices);
        }
    }
}
