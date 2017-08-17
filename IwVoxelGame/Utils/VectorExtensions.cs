using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwVoxelGame.Utils {
    public static class VectorExtensions {
        public static Vector3 Mod(this Vector3 left, float right) {
            return new Vector3(
                left.X % right,
                left.Y % right,
                left.Z % right);
        }

        public static Vector3i Mod(this Vector3i left, int right) {
            return new Vector3i(
                left.X % right,
                left.Y % right,
                left.Z % right);
        }
    }
}
