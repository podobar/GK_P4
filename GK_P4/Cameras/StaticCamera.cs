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
    class StaticCamera : Camera
    {
        public StaticCamera(Vector3 position, float pitch, float yaw, float roll) : base(position,pitch,yaw,roll)
        {
        }
        public override void Move()
        {
            //calculateDirection();
        }
        //private void calculateDirection()
        //{
        //    float dx = this.Position.X - entity.position.X;
        //    float dy = this.Position.Y - entity.position.Y;
        //    float dz = this.Position.Z - entity.position.Z;

        //    float distance = (float)Math.Sqrt(Math.Pow(dy, 2) + Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dz, 2)));
        //    Pitch = (float)Math.Asin(dx / distance);
        //    //only Yaw And Pitch may change!
        //}
    }
}
