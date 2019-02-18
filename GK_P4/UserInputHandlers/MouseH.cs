using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.UserInputHandlers
{
    public class MouseH
    {
        public float WheelDelta { get; set; }
        public void Update(MouseWheelEventArgs e)
        {
            WheelDelta += e.DeltaPrecise;
        }
        public void Reset()
        {
            WheelDelta = 0;
        }
    }
}
