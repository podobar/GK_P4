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
        public bool A_Pressed { get; set; }
        public bool D_Pressed { get; set; }


        public void KeyPressed(KeyboardKeyEventArgs e, bool pressed)
        {
            if (e.Key == Key.A)
                A_Pressed = pressed;
            else if (e.Key == Key.D)
                D_Pressed = pressed;
        }
    }
}
