using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project;
using game_objects;

namespace components
{
    public class PlayerMovementComponent : MovementComponent
    {
        private float movementAmount;
        private float stepSize;
        private int direction;
        private float origin;
        private float relativeOrigin;
        private float destiny;
        private Vector3 ownerLastPos;

        public PlayerMovementComponent(GameObject owner, int stepInterval, float stepSize, float movementAmount)
            : base(owner, stepInterval)
        {
            this.movementAmount = movementAmount;
            this.stepSize = stepSize;
            this.elapsed = stepInterval;
            direction = 0;
            origin = owner.Position.X;
            relativeOrigin = origin;
            destiny = origin;
        }

        public override void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsed > movementInterval)
            {
                elapsed = 0;

                if (ownerLastPos.Equals(owner.Position))
                {
                    direction = 0;
                    origin = destiny;
                }

                int newDirection = getPressedKey();

                //Se uma nova tecla foi pressionada, alterar a direção
                if (newDirection != 0 && newDirection != direction)
                {
                    direction = newDirection;
                    if (destiny == origin)
                    {
                        //relativeOrigin = owner.Position.X;
                        destiny = owner.Position.X + movementAmount * direction;
                    }
                    else
                    {
                        //lógica falha aqui (origin vira valor quebrado ao inverter a direção a meio caminho e, quando inverte novamente, destiny se perde)
                        float tmp = destiny;
                        destiny = origin;
                        origin = tmp;
                        //relativeOrigin = owner.Position.X;
                    }
                }

                if (direction != 0)
                {
                    Vector3 amount = Vector3.Zero;
                    float actualStepSize = stepSize * direction;

                    float newX = owner.Position.X + actualStepSize;
                    if ((direction == 1 && newX > destiny) || (direction == -1 && newX < destiny))
                    {
                        float absX = Math.Abs(newX);
                        float absDestiny = Math.Abs(destiny);
                        actualStepSize += (absX - absDestiny) * (direction * -1);
                    }

                    ownerLastPos = owner.Position;
                    amount.X += actualStepSize;
                    base.move(amount);
                    /*
                    if (ownerLastPos.Equals(owner.Position))
                    {
                        direction = 0;
                        origin = destiny;
                    }
                     */
                }
            }
        }

        private int getPressedKey()
        {
            int dir = 0;
            if (KeyboardHelper.IsKeyDown(Keys.Left))
                dir++;
            if (KeyboardHelper.IsKeyDown(Keys.Right))
                dir--;

            return dir;
        }

    }
}
