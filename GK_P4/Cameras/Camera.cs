using GK_P4.UserInputHandlers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.Cameras
{
    public abstract class Camera
    {
        public Vector3 Position { get; set; }
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Roll { get; set; }
        public KeyboardH Keyboard { get; set; }

        public Camera(Vector3 position, float pitch, float yaw, float roll)
        {
            this.Position = position;
            Pitch = pitch;
            Yaw = yaw;
            Roll = roll;
        }

        public abstract void Move();
    }
}
