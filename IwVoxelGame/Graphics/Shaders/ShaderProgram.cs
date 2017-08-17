using IwVoxelGame.Utils;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwVoxelGame.Graphics.Shaders {
    public class ShaderProgram {
        private readonly int _shaderProgramId;

        public ShaderProgram(params Shader[] shaders) {
            _shaderProgramId = GL.CreateProgram();

            foreach(Shader shader in shaders) {
                GL.AttachShader(_shaderProgramId, shader.ShaderId);

                string infoLog = GL.GetProgramInfoLog(_shaderProgramId);
                if (!string.IsNullOrEmpty(infoLog)) {
                    Logger.Error($"There was an error linking shader \"{shader.Name}\": {infoLog}");
                } else {
                    Logger.Debug($"Loaded and attached shader \"{shader.Name}\" to porgram {_shaderProgramId}");
                }
            }

            GL.LinkProgram(_shaderProgramId);
        }

        public void Bind() {
            GL.UseProgram(_shaderProgramId);
        }

    }
}
