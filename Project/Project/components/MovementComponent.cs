using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using game_objects;

namespace components
{
    public abstract class MovementComponent : Component
    {
        protected int movementInterval;
        protected int elapsed;

        public MovementComponent(GameObject owner, int interval)
            : base(owner)
        {
            movementInterval = interval;
        }

        public override void Update(GameTime gameTime) { }

        protected void move(Vector3 movementVector)
        {
            if (movementVector != null && !Vector3.Zero.Equals(movementVector))
            {
                owner.Translate(movementVector);
            }
        }
    }
}
