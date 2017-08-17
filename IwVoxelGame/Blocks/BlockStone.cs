using IwVoxelGame.Utils;

namespace IwVoxelGame.Blocks {
    public class BlockStone : Block {
        public BlockStone() : base(1) { }

        public override int GetTextureId(BlockFace blockface) {
            return 0;
        }
    }
}
