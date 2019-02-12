using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK_P4.Cameras;
using OpenTK;

namespace GK_P4.Utilities
{
    public class Matrices
    {
        public static Matrix4 CreateTransformationMatrix(Vector3 translation, Vector3 rotation, float scale)
        {
            Matrix4 matrix = Matrix4.Identity;
            matrix *= Matrix4.CreateScale(scale, scale, scale);

            matrix *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X));
            matrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));
            matrix *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));

            matrix *= Matrix4.CreateTranslation(translation);

            return matrix;
        }
        public static Matrix4 CreateViewMatrix(Camera camera)
        {
            Matrix4 viewMatrix = Matrix4.Identity;
            Vector3 negativeCameraPosition = -camera.Position;

            viewMatrix *= Matrix4.CreateTranslation(negativeCameraPosition);

            viewMatrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(camera.Yaw));
            viewMatrix *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(camera.Pitch));

            return viewMatrix;
        }
    }
}
