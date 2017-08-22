using IwVoxelGame.Blocks.World;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace IwVoxelGame.Graphics {
    public static class WorldRenderer {
        public static void RenderWorld(World world) {
            foreach (var chunk in world.LoadedChunks) {
                Matrix4 worldMat = Matrix4.CreateTranslation(
                    chunk.Key.X * Chunk.Size,
                    chunk.Key.Y * Chunk.Size,
                    chunk.Key.Z * Chunk.Size);

                GL.UniformMatrix4(0, false, ref worldMat);
                chunk.Value.Draw();
            }
        }
    }
}
