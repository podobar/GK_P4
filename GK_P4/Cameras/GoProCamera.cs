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
        private const float MIN_DISTANCE = 20;
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
            float hDistance = (float)(distanceFromObject * Math.Cos(MathHelper.DegreesToRadians(Pitch)));
            float vDistance = (float)(distanceFromObject * Math.Sin(MathHelper.DegreesToRadians(Pitch)));

            float X = (float)(hDistance * Math.Sin(MathHelper.DegreesToRadians(Entity.rotation.Y)));
            float Z = (float)(hDistance * Math.Cos(MathHelper.DegreesToRadians(Entity.rotation.Y)));

            Position = Entity.position + new Vector3(- X, Entity.position.Y + vDistance,- Z);
            Yaw = 180 - Entity.rotation.Y;
        }
        private void calculateZoom()
        {
            distanceFromObject = (distanceFromObject - mouse.WheelDelta > MIN_DISTANCE)
                ? distanceFromObject - mouse.WheelDelta
                : MIN_DISTANCE;
        }
    }
}
