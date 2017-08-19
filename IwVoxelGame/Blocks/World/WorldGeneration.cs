using IwVoxelGame.Utils;
using System.Collections.Concurrent;
using System.Threading;

namespace IwVoxelGame.Blocks.World {
    public static class WorldGeneration {
        private static OpenSimplexNoise noise = new OpenSimplexNoise(0);
        private static BlockingCollection<Chunk> m_chunks = new BlockingCollection<Chunk>();

        static WorldGeneration() {
            Thread th = new Thread(() => {
                while(true) {
                    if(m_chunks.Count > 0) {
                        Chunk chunk = m_chunks.Take();
                        chunk.Generate(noise);
                        chunk.Update();
                    } else {
                        Thread.Sleep(10);
                    }
                }
            });

            th.IsBackground = true;
            th.Start();
        }

        public static void QueueChunk(Chunk chunk) {
            m_chunks.Add(chunk);
        }
    }
}
