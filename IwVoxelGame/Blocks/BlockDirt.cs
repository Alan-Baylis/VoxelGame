using IwVoxelGame.Utils;

namespace IwVoxelGame.Blocks {
    public class BlockDirt : Block {
        public BlockDirt() : base(2) { }

        public override int GetTextureId(BlockFace blockface) {
            return 1;
        }
    }
}
