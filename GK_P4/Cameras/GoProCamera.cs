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
    public class GoProCamera : Camera
    {
        private const float MIN_DISTANCE = 70;
        public Entity Entity { get; set; }
        private float distanceFromObject = MIN_DISTANCE;
        private MouseH mouse;
        public GoProCamera(Vector3 position, float pitch, float yaw, float roll, MouseH mouse, Entity entity) : base(position, pitch, yaw, roll)
        {
            Entity = entity;
            this.mouse = mouse;
        }
        public override void Move()
        {
            calculateZoom();
            calculatePosition();
        }
        private void calculatePosition()
        {
            float vDistance = (float)(distanceFromObject * Math.Sin(MathHelper.DegreesToRadians(Pitch)));
            Position = Entity.position + new Vector3(0, vDistance,  0);
            Yaw = -90 - Entity.rotation.Y;
        }
        private void calculateZoom()
        {
            distanceFromObject = (distanceFromObject - mouse.WheelDelta > MIN_DISTANCE)
                ? distanceFromObject - mouse.WheelDelta
                : MIN_DISTANCE;
        }
    }
}
