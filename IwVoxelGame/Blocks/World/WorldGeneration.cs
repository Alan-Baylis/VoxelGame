using IwVoxelGame.Utils;
using OpenTK;

namespace IwVoxelGame.Blocks.World {
    public class WorldGeneration {
        OpenSimplexNoise noise = new OpenSimplexNoise();
        double scaleX = 10;
        double scaleZ = 10;

        //private PerlinNoise noise;
        private int chunkGenDistance = 1;

        public void Generate(Vector3 playerPos, World world) {
            Vector3i start = new Vector3i(
                -chunkGenDistance + (int)playerPos.X / Chunk.Size,
                -chunkGenDistance + (int)playerPos.Y / Chunk.Size,
                -chunkGenDistance + (int)playerPos.Z / Chunk.Size);

            Vector3i end = start + chunkGenDistance*2;

            for (int x = start.X; x < end.X; x++) {
                for (int y = start.Y; y < end.Y; y++) {
                    for (int z = start.Z; z < end.Z; z++) {
                        world.GenerateChunk(new Vector3i(x, y, z));
                    }
                }
            }


            //for (int x = startX; x < endX; x++) {
            //    for (int z = startZ; z < endZ; z++) {
            //        int y = (int)(noise.Evaluate(x / scaleX, z / scaleZ) * 5);
            //        world.SetBlock(new Vector3i(x, y, z), new BlockGrass());
            //        int down = 0;
            //        while (++down < 64) {
            //            if (down < 5) {
            //                world.SetBlock(new Vector3i(x, y - down, z), new BlockDirt());
            //                continue;
            //            }

            //            if (down < 10) {
            //                world.SetBlock(new Vector3i(x, y - down, z), new BlockStone());
            //                continue;
            //            }

            //            if (noise.Evaluate(x / 20.0, (y - down) / 20.0, z / 20.0) > 0) {
            //                world.SetBlock(new Vector3i(x, y - down, z), new BlockStone());
            //            }
            //        }
            //    }
            //}

            world.Update();
        }
    }
}
