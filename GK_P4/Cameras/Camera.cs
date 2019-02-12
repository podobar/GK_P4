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
        public KeyboardHandler Keyboard { get; set; }

        public Camera(Vector3 position, float pitch, float yaw, float roll)
        {
            this.Position = position;
            Pitch = pitch;
            Yaw = yaw;
            Roll = roll;
        }

        public virtual void Move()
        {
            if (Keyboard.LeftPressed)
            {
                Position += new Vector3(-5f, 0, 0);
            }
            if (Keyboard.UpPressed)
            {
                Position += new Vector3(0, 0, -5f);
            }
            if (Keyboard.RightPressed)
            {
                Position += new Vector3(5f, 0, 0);
            }
            if (Keyboard.DownPressed)
            {
                Position += new Vector3(0, 0, 5f);
            }
        }
    }
}
