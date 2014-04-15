using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using game_objects;

namespace components
{
    public class ConstantMovementComponent : MovementComponent
    {
        private Vector3 constantMovementVector;

        public ConstantMovementComponent(GameObject owner, Vector3 movementVector, int interval)
            : base(owner, interval)
        {
            constantMovementVector = movementVector;
        }

        public override void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsed >= movementInterval)
            {
                elapsed = 0;
                base.move(constantMovementVector);
            }
        }
    }
}
