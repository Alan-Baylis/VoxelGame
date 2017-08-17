using IwVoxelGame.Utils;

namespace IwVoxelGame.Blocks {
    public class BlockGrass : Block {
        public BlockGrass() : base(3) { }

        public override int GetTextureId(BlockFace blockface) {
            if (blockface == BlockFace.TOP) return 3;
            if (blockface == BlockFace.BOTTOM) return 1;
            return 2;
        }
    }
}
