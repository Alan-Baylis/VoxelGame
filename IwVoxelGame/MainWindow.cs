using IwVoxelGame.Blocks.World;
using IwVoxelGame.Graphics;
using IwVoxelGame.Graphics.Shaders;
using IwVoxelGame.Utils;
using Minecraft.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwVoxelGame {
    public class MainWindow : GameWindow {
        World world;
        WorldGeneration worldGen;
        TextureArray2D textureArray;
        ShaderProgram shader;
        Matrix4 projection;
        Camera camera;
        private MouseState? lastMouseState;
        private float mouseSensitivity;

        public MainWindow() : base(1280, 720, GraphicsMode.Default, "Voxel Game") {
            Logger.Debug("Start...");

            Shader vshader = new Shader(ShaderType.VertexShader, "Content/Shaders/default.vs");
            Shader fshader = new Shader(ShaderType.FragmentShader, "Content/Shaders/default.fs");
            shader = new ShaderProgram(vshader, fshader);

            textureArray = new TextureArray2D(16, 16, 4);
            textureArray.SetTexture(0, new TextureData("Content/Textures/stone.png"));
            textureArray.SetTexture(1, new TextureData("Content/Textures/dirt.png"));
            textureArray.SetTexture(2, new TextureData("Content/Textures/grass.png"));
            textureArray.SetTexture(3, new TextureData("Content/Textures/topGrass.png"));
            textureArray.GenMinmaps();

            camera = new Camera();
            mouseSensitivity = 0.005f;

            world = new World();
            worldGen = new WorldGeneration();
            worldGen.Generate(camera.transform.position, world);
        }

        protected override void OnResize(EventArgs e) {
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(100), (float)Width / Height, .01f, 1000);

            GL.Viewport(new Point(0), ClientSize);

            Logger.Debug($"Resized window to {Width}, {Height}");
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            KeyboardState keyboardState = Keyboard.GetState();
            Vector3 cameraMove = Vector3.Zero;
            if (keyboardState.IsKeyDown(Key.W)) cameraMove.Z -= 1;
            if (keyboardState.IsKeyDown(Key.S)) cameraMove.Z += 1;
            if (keyboardState.IsKeyDown(Key.A)) cameraMove.X -= 1;
            if (keyboardState.IsKeyDown(Key.D)) cameraMove.X += 1;
            if (keyboardState.IsKeyDown(Key.ShiftLeft)) cameraMove.Y -= 1;
            if (keyboardState.IsKeyDown(Key.Space)) cameraMove.Y += 1;

            if (cameraMove.LengthSquared > 0.0001f) {
                camera.Move(cameraMove.Normalized() * 0.5f);
            }

            MouseState mouseState = Mouse.GetState();
            if (lastMouseState.HasValue) {
                float pitch = mouseState.Y - lastMouseState.Value.Y;
                float yaw = lastMouseState.Value.X - mouseState.X;
                camera.Rotate(yaw * mouseSensitivity, pitch * mouseSensitivity, 0);
            }

            if(mouseState.IsButtonDown(MouseButton.Left)) {
                worldGen.Generate(camera.transform.position, world);
            }

            lastMouseState = mouseState;

            camera.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            GL.ClearColor(Color.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            shader.Bind();

            Matrix4 worldMat = Matrix4.Identity;
            GL.UniformMatrix4(0, false, ref worldMat);
            GL.UniformMatrix4(4, false, ref camera.view);
            GL.UniformMatrix4(8, false, ref projection);

            textureArray.Bind(TextureUnit.Texture0);

            WorldRenderer.RenderWorld(world);

            SwapBuffers();
        }
    }
}
