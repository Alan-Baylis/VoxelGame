using OpenTK;

namespace IwVoxelGame.Utils {
    public struct Vector3i {
        public int X;
        public int Y;
        public int Z;

        public static Vector3i Zero => new Vector3i(0);

        public Vector3i(int x, int y, int z) {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3i(int xyz) {
            X = Y = Z = xyz;
        }

        public override string ToString() {
            return $"({X}, {Y}, {Z})";
        }

        public Vector3 AsVector3() {
            return new Vector3(X, Y, Z);
        }

        public static Vector3i operator +(Vector3i left, Vector3i right) {
            return new Vector3i(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }
        public static Vector3i operator -(Vector3i left, Vector3i right) {
            return new Vector3i(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Vector3i operator *(Vector3i left, Vector3i right) {
            return new Vector3i(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        public static Vector3i operator /(Vector3i left, Vector3i right) {
            return new Vector3i(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        }

        public static Vector3i operator +(Vector3i left, int right) {
            return new Vector3i(left.X + right, left.Y + right, left.Z + right);
        }
        public static Vector3i operator -(Vector3i left, int right) {
            return new Vector3i(left.X - right, left.Y - right, left.Z - right);
        }

        public static Vector3i operator *(Vector3i left, int right) {
            return new Vector3i(left.X * right, left.Y * right, left.Z * right);
        }

        public static Vector3i operator /(Vector3i left, int right) {
            return new Vector3i(left.X / right, left.Y / right, left.Z / right);
        }
    }
}