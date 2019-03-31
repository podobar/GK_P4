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
    class CCTVCamera : Camera
    {
        public CCTVCamera(Vector3 position, float pitch, float yaw, float roll) : base(position,pitch,yaw,roll)
        {
        }
        public override void Move()
        {
        }
    }
}
