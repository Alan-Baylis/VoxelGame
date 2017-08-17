using IwVoxelGame.Utils;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Minecraft.Graphics {
    public class TextureData : IDisposable {
        private readonly Bitmap _bitmap;
        private readonly bool _ownsBmp;

        public readonly BitmapData Data;
        public int Width => Data.Width;
        public int Height => Data.Height;
        public IntPtr DataPtr => Data.Scan0;

        public TextureData(string filename) : this(new Bitmap(filename)) {
            _ownsBmp = true;

            Logger.Debug($"Loaded texture \"{filename}\"");
        }

        public TextureData(Bitmap bitmap) {
            _bitmap = bitmap;
            _ownsBmp = false;
            Data = bitmap.LockBits(new Rectangle(new Point(0), bitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        }

        public void Dispose() {
            _bitmap.UnlockBits(Data);
            if(_ownsBmp) {
                _bitmap.Dispose();
            }
        }
    }
}