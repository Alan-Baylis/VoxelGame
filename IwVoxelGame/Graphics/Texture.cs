using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;
using GLPixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using ExtTextureFilterAnisotropic = OpenTK.Graphics.OpenGL.ExtTextureFilterAnisotropic;

namespace Minecraft.Graphics {
    public class Texture {
        private readonly int _id;

        public readonly int height;
        public readonly int width;

        public Texture(string filename) {
            TextureData textureData = new TextureData(filename);

            _id = GL.GenTexture();
            height = textureData.Height;
            width = textureData.Width;

            Bind(TextureUnit.Texture0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, 
                GLPixelFormat.Bgra, PixelType.UnsignedByte, textureData.DataPtr);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapNearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, 16);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            textureData.Dispose();
        }

        public void Bind(TextureUnit unit) {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _id);
        }
    }
}
