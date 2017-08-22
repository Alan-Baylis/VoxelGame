using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using IwVoxelGame.Components;

namespace IwVoxelGame.Graphics {
    public class Camera {
        public Transform transform;
        public Matrix4 view;

        public Camera() {
            transform = new Transform(new Vector3(0, 0, 0), Vector3.Zero, Vector3.Zero);
        }

        public void Move(Vector3 moveVector) {
            Vector3 delta = Vector3.Zero;

            delta += transform.Right.Normalized() * moveVector.X;
            delta += transform.worldUp * moveVector.Y;
            delta += new Vector3(transform.Forward.X, 0, transform.Forward.Z).Normalized() * moveVector.Z;

            transform.Move(delta);
        }

        public void Rotate(float yaw, float pitch, float roll) {
            if (transform.rotation.Y + pitch < MathHelper.PiOver2 && transform.rotation.Y + pitch > -MathHelper.PiOver2) {
                transform.Rotate(new Vector3(yaw, pitch, roll));
            } else {
                transform.Rotate(new Vector3(yaw, 0, roll));
            }
        }

        public void Update() {
            view = Matrix4.LookAt(transform.position, transform.position - transform.Forward, transform.worldUp)
                * Matrix4.CreateRotationZ(transform.rotation.Z);
        }
    }
}
