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
            
        }
    }
}
