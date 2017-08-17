using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace IwVoxelGame.Graphics {
    public class VertexBufferObject<TVector> where TVector : struct {
        private readonly List<TVector> _data;
        private readonly int _dataTypeSizeInBytes;
        private readonly int _internalDataPointCount;
        private readonly VertexAttribPointerType _internalDataType;

        public TVector[] Data => _data.ToArray();
        public int Length => _data.Count;
        public int InternalDataPointCount => _internalDataPointCount;
        public int DataTypeSizeInBytes => _dataTypeSizeInBytes;
        public VertexAttribPointerType InternalDataType => _internalDataType;

        public VertexBufferObject(int dataTypeSizeInBytes, int internalDataPointCount, VertexAttribPointerType internalDataType) {
            _data = new List<TVector>();
            _dataTypeSizeInBytes = dataTypeSizeInBytes;
            _internalDataPointCount = internalDataPointCount;
            _internalDataType = internalDataType;
        }

        public void Add(TVector data) {
            _data.Add(data);
        }

        public void Clear() {
            _data.Clear();
        }
    }
}