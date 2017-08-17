using IwVoxelGame.Utils;

namespace IwVoxelGame.Blocks {
    public class BlockAir : Block {
        public BlockAir() : base(0) { }

        public override int GetTextureId(BlockFace blockface) {
            return -1;
        }
    }
}
