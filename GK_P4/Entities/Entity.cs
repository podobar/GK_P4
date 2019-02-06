using GK_P4.Models;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.Entities
{
    public class Entity
    {
        public TexturedModel model;
        public Vector3 position;
        public Vector3 rotation;
        public float scale;

        public Entity(TexturedModel model, Vector3 position, Vector3 rotation, float scale)
        {
            this.model = model;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public void IncreasePosition(float dx,float dy, float dz)
        {
            position.X += dx;
            position.Y += dy;
            position.Z += dz;
        }
        public void IncreaseRotation(float rx, float ry, float rz)
        {
            rotation.X += rx;
            rotation.Y+= ry;
            rotation.Z += rz;
        }
    }
}
