using IwVoxelGame.Utils;

namespace IwVoxelGame.Blocks {
    public abstract class Block {
        protected readonly ushort _id;

        public ushort Id => _id;
        public BlockType Type => (BlockType)_id;

        public Block(ushort id) {
            _id = id;
        }

        public override string ToString() {
            return $"{Type}";
        }

        public abstract int GetTextureId(BlockFace blockface);
    }
}
