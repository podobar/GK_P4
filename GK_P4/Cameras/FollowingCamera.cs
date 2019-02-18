using GK_P4.Entities;
using GK_P4.UserInputHandlers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.Cameras
{
    public class FollowingCamera : Camera
    {
        private Entity entity { get; }
        public FollowingCamera(Vector3 position, float pitch, float yaw, float roll, Entity entity) : base(position, pitch, yaw, roll)
        {
            this.entity = entity;
        }
        public override void Move()
        {
            calculatePitch();
            calculateYaw();
        }
        private void calculatePitch()
        {
            //(y,z)
            float dy = this.Position.Y - entity.position.Y;
            float dz = this.Position.Z - entity.position.Z;
            float d = (float)Math.Sqrt(dy * dy + dz * dz);
            //Pitch = (float)MathHelper.RadiansToDegrees(Math.Asin(Math.Abs(dy) / d));
        }
        private void calculateYaw()
        {
            
            float dx = this.Position.X - entity.position.X;
            float dz = this.Position.Z - entity.position.Z;
            float d = (float)Math.Sqrt(dx * dx + dz * dz);
            Yaw = -(float)MathHelper.RadiansToDegrees(Math.Asin(dx / d));
        }
    }
}
