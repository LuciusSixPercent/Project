using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace components
{
    public class PlayerMovementComponent : MovementComponent
    {
        private Keys lastPressedKey;
        private float stepSize;

        public PlayerMovementComponent(int keyRepeatInterval, float stepSize)
            : base(keyRepeatInterval)
        {
            lastPressedKey = Keys.None;
            this.stepSize = stepSize;
            elapsed = movementInterval;
        }

        public override void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;

            KeyboardState ks = Keyboard.GetState();

            Vector3 amount = Vector3.Zero;

            int count = 0;
            if (ks.GetPressedKeys().Contains(Keys.Left))
            {
                amount.X+=stepSize;
                if (lastPressedKey != Keys.Left)
                {
                    lastPressedKey = Keys.Left;
                }
                count++;
            }
            if (ks.GetPressedKeys().Contains(Keys.Right))
            {
                amount.X -= stepSize;
                if (lastPressedKey != Keys.Right)
                {
                    lastPressedKey = Keys.Right;
                }
                count++;
            }

            if (count == 0)
            {
                lastPressedKey = Keys.None;
                elapsed = movementInterval;
            }

            if (elapsed >= movementInterval && lastPressedKey != Keys.None)
            {
                elapsed = 0;

                if (amount.X != 0)
                    move(amount);                
            }
        }

    }
}
