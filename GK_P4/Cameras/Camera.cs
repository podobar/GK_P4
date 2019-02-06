using GK_P4.UserInputHandlers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.Cameras
{
    public class Camera
    {
        public Vector3 Position { get; set; }
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Roll { get; set; }
        public KeyboardHandler Keyboard {get;}

        public Camera(Vector3 position, float pitch, float yaw, float roll, KeyboardHandler keyboard)
        {
            this.Position = position;
            Pitch = pitch;
            Yaw = yaw;
            Roll = roll;
            Keyboard = keyboard;
        }

        public void Move()
        {
            if (Keyboard.LeftPressed)
            {
                Position += new Vector3(-0.3f, 0, 0);
            }
            if (Keyboard.UpPressed)
            {
                Position += new Vector3(0, 0, -0.3f);
            }
            if (Keyboard.RightPressed)
            {
                Position += new Vector3(0.3f, 0, 0);
            }
            if (Keyboard.DownPressed)
            {
                Position += new Vector3(0, 0, 0.3f);
            }
        }
    }
}
