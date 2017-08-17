using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using ExtTextureFilterAnisotropic = OpenTK.Graphics.OpenGL.ExtTextureFilterAnisotropic;
using IwVoxelGame.Utils;

namespace Minecraft.Graphics {
    public class TextureArray2D {
        public readonly int width;
        public readonly int height;

        private readonly int _id;

        public TextureArray2D(int width, int height, int count) {
            this.width = width;
            this.height = height;

            _id = GL.GenTexture();
            Bind(TextureUnit.Texture0);
            GL.TexStorage3D(TextureTarget3d.Texture2DArray, 1, SizedInternalFormat.Rgba8, width, height, count);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (float)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2DArray, (TextureParameterName)ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, 16);
        }

        public void SetTexture(int index, TextureData data) {
            GL.TexSubImage3D(TextureTarget.Texture2DArray, 0, 0, 0, index, data.Width, data.Height, 
                1, PixelFormat.Bgra, PixelType.UnsignedByte, data.DataPtr);
            data.Dispose();

            Logger.Debug($"Added texture {index} into texture array {_id}");
        }

        public void GenMinmaps() {
            Bind(TextureUnit.Texture0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2DArray);
        }

        public void Bind(TextureUnit unit) {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2DArray, _id);
        }
    }
}
