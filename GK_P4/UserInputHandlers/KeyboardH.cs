using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace GK_P4.UserInputHandlers
{
    public class KeyboardH
    {
        public bool UpPressed { get; set; }
        public bool DownPressed { get; set; }
        public bool LeftPressed { get; set; }
        public bool RightPressed { get; set; }
        public bool A_Pressed { get; set; }
        public bool D_Pressed { get; set; }


        public void KeyPressed(KeyboardKeyEventArgs e, bool pressed)
        {
            if (e.Key == Key.Up)
                UpPressed = pressed;
            else if (e.Key == Key.Down)
                DownPressed = pressed;
            else if (e.Key == Key.Left)
                LeftPressed = pressed;
            else if (e.Key == Key.Right)
                RightPressed = pressed;
            else if (e.Key == Key.A)
                A_Pressed = pressed;
            else if (e.Key == Key.D)
                D_Pressed = pressed;
        }
    }
}
