using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using game_objects;
using Microsoft.Xna.Framework;
using game_objects.questions;

namespace components
{
    public class VariableMovementComponent : MovementComponent
    {
        private Vector3 initialVelocity;
        private Vector3 currentVelocity;
        private Vector3 upperVelocityThreshold;
        private Vector3 lowerVelocityThreshold;
        private Vector3 initialAcceleration;
        private Vector3 acceleration;
        private Vector3 accelerationVariation;

        public Vector3 InitialVelocity
        {
            get { return initialVelocity; }
            set { 
                initialVelocity = value;
            }
        }

        public Vector3 CurrentVelocity
        {
            get { return currentVelocity; }
            set { currentVelocity = value; }
        }

        public Vector3 UpperVelocityThreshold
        {
            get { return upperVelocityThreshold; }
            set
            {
                upperVelocityThreshold = value;
                CheckThreshold(ref upperVelocityThreshold, ref lowerVelocityThreshold, false, true);
            }
        }

        public Vector3 LowerVelocityThreshold
        {
            get { return lowerVelocityThreshold; }
            set
            {
                lowerVelocityThreshold = value;
                CheckThreshold(ref lowerVelocityThreshold, ref upperVelocityThreshold, true, true);
            }
        }

        public Vector3 InitialAcceleration
        {
            get { return initialAcceleration; }
            set { initialAcceleration = value; }
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
            this.initialAcceleration = acceleration;
            this.accelerationVariation = Vector3.Zero;
            this.upperVelocityThreshold = new Vector3(float.MaxValue);
            this.lowerVelocityThreshold = new Vector3(float.MinValue);
        }

        public override void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsed >= movementInterval)
            {
                elapsed = 0;
                base.move(currentVelocity);

                currentVelocity += acceleration;

                CheckThreshold(ref currentVelocity, ref upperVelocityThreshold, true, false);
                CheckThreshold(ref currentVelocity, ref lowerVelocityThreshold, false, false);

                acceleration += accelerationVariation;
            }
        }

        private void CheckThreshold(ref Vector3 value, ref Vector3 threshold, bool upperThreshold, bool pushThreshold)
        {
            if (upperThreshold)
            {
                if (pushThreshold)
                    Vector3.Max(ref value, ref threshold, out threshold);
                else
                    Vector3.Min(ref value, ref threshold, out value);
            }
            else
            {
                if (pushThreshold)
                    Vector3.Min(ref value, ref threshold, out threshold);
                else
                    Vector3.Max(ref value, ref threshold, out value);
            }
        }
    }
}
