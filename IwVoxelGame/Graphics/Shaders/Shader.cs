using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwVoxelGame.Graphics.Shaders {
    public class Shader {
        private readonly int _shaderId;
        private readonly string _name;

        public int ShaderId => _shaderId;
        public string Name => _name;

        public Shader(ShaderType shaderType, string filepath) {
            _name = filepath;
            _shaderId = GL.CreateShader(shaderType);

            string code = File.ReadAllText(filepath);
            GL.ShaderSource(_shaderId, code);
            GL.CompileShader(_shaderId);
        }
    }
}
