using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace components
{
    public abstract class MovementComponent : Component
    {
        public delegate void Moved(Vector3 amount);
        public event Moved moved;
        protected int movementInterval;
        protected int elapsed;

        public MovementComponent(int interval)
        {
            movementInterval = interval;
        }

        public override void Update(GameTime gameTime){}

        protected void move(Vector3 movementVector)
        {
            if (movementVector != null && !Vector3.Zero.Equals(movementVector))
            {
                if (moved != null)
                {
                    moved(movementVector);
                }
            }
        }
    }
}
