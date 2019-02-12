﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.Lights
{
    public class Light
    {
        public Vector3 Position { get; set; }
        public Vector3 Colour { get; set; }

        public Light(Vector3 position, Vector3 colour)
        {
            Position = position;
            Colour = colour;
        }
    }
}