using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwVoxelGame.Graphics {
    public class VertexArrayObject<TVector> : IDisposable where TVector : struct {
        //Buffering
        private readonly int _vaoId;
        private readonly int[] _bufferIds;
        private readonly int _indicesId;

        //Drawing
        private readonly VertexBufferObject<TVector>[] _vbos;
        private readonly List<uint> _indices;

        private bool _boolHasBeenUploaded;

        public VertexArrayObject(int bufferCount) {
            _vaoId = GL.GenVertexArray();
            _indicesId = GL.GenBuffer();

            _bufferIds = new int[bufferCount];
            GL.GenBuffers(_bufferIds.Length, _bufferIds);

            _vbos = new VertexBufferObject<TVector>[bufferCount];
            _indices = new List<uint>();
        }

        public void SetVbo(int bufferIndex, VertexBufferObject<TVector> vbo) {
            _vbos[bufferIndex] = vbo;
        }

        public void Add(int buffer, TVector data) {
            _vbos[buffer].Add(data);
        }

        public void AddIndex(uint index) {
            _indices.Add(index);
        }

        public void AddIndices(params uint[] indices) {
            _indices.AddRange(indices);
        }

        public void Upload() {
            GL.BindVertexArray(_vaoId);

            for (int i = 0; i < _vbos.Length; i++) {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[i]);
                GL.BufferData(BufferTarget.ArrayBuffer, _vbos[i].Length * _vbos[i].DataTypeSizeInBytes, _vbos[i].Data.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(i, _vbos[i].InternalDataPointCount, _vbos[i].InternalDataType, false, 0, 0);
                GL.EnableVertexArrayAttrib(_vaoId, i);
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indicesId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Count * sizeof(uint), _indices.ToArray(), BufferUsageHint.StaticDraw);

            _boolHasBeenUploaded = true;
        }

        public void Draw(BeginMode mode, DrawElementsType type) {
            if (_boolHasBeenUploaded) {
                GL.BindVertexArray(_vaoId);
                GL.DrawElements(mode, _indices.Count, type, 0);
            }
        }

        public void Clear() {
            foreach(VertexBufferObject<TVector> vbo in _vbos) {
                vbo.Clear();
            }

            _indices.Clear();
        }

        public VertexBufferObject<TVector> GetBuffer(int buffer) {
            if (_vbos.Length <= buffer) return null;
            return _vbos[buffer];
        }

        public void Dispose() {
            GL.DeleteBuffer(_indicesId);
            GL.DeleteBuffers(_bufferIds.Length, _bufferIds);
            GL.DeleteVertexArray(_vaoId);
        }
    }
}
