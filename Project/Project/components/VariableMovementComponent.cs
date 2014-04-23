using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using game_objects;
using Microsoft.Xna.Framework;

namespace components
{
    public class VariableMovementComponent : MovementComponent
    {
        private readonly Vector3 initialVelocity;
        private Vector3 currentVelocity;
        private Vector3 terminalVelocity;
        private Vector3 acceleration;
        private Vector3 accelerationVariation;

        public Vector3 InitialVelocity
        {
            get { return initialVelocity; }
        }

        public Vector3 CurrentVelocity
        {
            get { return currentVelocity; }
            set { currentVelocity = value; }
        }

        public Vector3 TerminalVelocity
        {
            get { return terminalVelocity; }
            set { terminalVelocity = value; }
        }

        public Vector3 Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        public Vector3 AccelerationVariation
        {
            get { return accelerationVariation; }
            set { accelerationVariation = value; }
        }

        public VariableMovementComponent(GameObject owner, int interval, Vector3 initialVelocity, Vector3 acceleration)
            : base(owner, interval)
        {
            this.initialVelocity = initialVelocity;
            this.currentVelocity = initialVelocity;
            this.acceleration = acceleration;
            this.accelerationVariation = Vector3.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsed >= movementInterval)
            {
                elapsed = 0;
                base.move(currentVelocity);
                
                currentVelocity += acceleration;
                if (terminalVelocity != null)
                {
                    if (currentVelocity != terminalVelocity && currentVelocity != terminalVelocity * -1)
                    {
                        if (currentVelocity.X > Math.Abs(terminalVelocity.X))
                            currentVelocity.X = Math.Abs(terminalVelocity.X) * (currentVelocity.X < 0 ? -1 : 1);

                        if (currentVelocity.Y > Math.Abs(terminalVelocity.Y))
                            currentVelocity.Y = Math.Abs(terminalVelocity.Y) * (currentVelocity.Y < 0 ? -1 : 1);

                        if (currentVelocity.Z > Math.Abs(terminalVelocity.Z))
                            currentVelocity.Z = Math.Abs(terminalVelocity.Z) * (currentVelocity.Z < 0 ? -1 : 1);
                    }
                }
                acceleration += accelerationVariation;
            }
        }
    }
}
