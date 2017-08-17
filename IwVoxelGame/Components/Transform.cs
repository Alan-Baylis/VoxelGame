using IwVoxelGame.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwVoxelGame.Components {
    public class Transform {
        public readonly Vector3 worldUp = Vector3.UnitY;
        public readonly Vector3 worldDown = -Vector3.UnitY;
        public readonly Vector3 worldRight = Vector3.UnitX;
        public readonly Vector3 worldLeft = -Vector3.UnitX;
        public readonly Vector3 worldForward = Vector3.UnitZ;
        public readonly Vector3 worldBackwards = -Vector3.UnitZ;

        public Vector3 Up { private set; get; }
        public Vector3 Down { private set; get; }
        public Vector3 Right { private set; get; }
        public Vector3 Left { private set; get; }
        public Vector3 Forward { private set; get; }
        public Vector3 Backward { private set; get; }

        public Vector3 position;
        public Vector3 scale;
        public Vector3 rotation;

        public Transform() : this(Vector3.Zero, Vector3.Zero, Vector3.Zero) { }

        public Transform(Vector3 position, Vector3 scale, Vector3 rotation) {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
        }

        public void Move(Vector3 moveVector) {
            position += moveVector;
        }

        public void Scale(Vector3 scaleVector) {
            scale += scaleVector;
        }

        public void Rotate(Vector3 rotationVector) {
            rotation = (rotation + rotationVector).Mod(MathHelper.TwoPi);

            Forward = new Vector3(
                (float)(Math.Sin(rotation.X) * Math.Cos(rotation.Y)),
                (float)Math.Sin(rotation.Y),
                (float)(Math.Cos(rotation.X) * Math.Cos(rotation.Y)));

            Right = Vector3.Cross(worldUp, Forward);
            Right.NormalizeFast();

            Up = -Vector3.Cross(worldRight, Forward);
            Up.NormalizeFast();

            Backward = -Forward;
            Left = -Right;
            Down = -Up;
        }

        public void Rotate(float yaw, float pitch, float roll) {
            Rotate(new Vector3(yaw, pitch, roll));
        }
    }
}
