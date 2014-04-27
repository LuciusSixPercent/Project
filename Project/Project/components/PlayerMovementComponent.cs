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
        private float destiny;
        private Vector3 ownerLastPos;
        private bool lockMovement;

        public int Direction
        {
            get { return direction; }
            set
            {
                if (value >= -1 && value <= 1)
                    direction = value;
            }
        }

        public float Destiny
        {
            get { return destiny; }
            set { 
                destiny = value;
                if (destiny > owner.Position.X)
                    Direction = 1;
                else if (destiny < owner.Position.X)
                    Direction = -1;
            }
        }

        public PlayerMovementComponent(GameObject owner, int stepInterval, float stepSize, float movementAmount)
            : base(owner, stepInterval)
        {
            this.movementAmount = movementAmount;
            this.stepSize = stepSize;
            this.elapsed = stepInterval;
            Direction = 0;
            origin = owner.Position.X;
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
                    Direction = 0;
                    origin = destiny;
                }

                if (!lockMovement)
                {
                    int newDirection = getPressedKey();

                    //Se uma nova tecla foi pressionada, alterar a direção
                    if (newDirection != 0 && newDirection != Direction)
                    {
                        Direction = newDirection;
                        float oldDestiny = destiny;
                        if (destiny == origin)
                        {
                            destiny = owner.Position.X + movementAmount * Direction;
                        }
                        else
                        {
                            float tmp = destiny;
                            destiny = origin;
                            origin = tmp;
                        }
                        if (destiny > owner.Position.X + movementAmount || destiny < owner.Position.X - movementAmount)
                        {
                            destiny = oldDestiny;
                        }
                    }
                }
               
                if (Direction != 0)
                {
                    Vector3 amount = Vector3.Zero;
                    float actualStepSize = stepSize * Direction;

                    float newX = owner.Position.X + actualStepSize;
                    if ((Direction == 1 && newX > destiny) || (Direction == -1 && newX < destiny))
                    {
                        float absX = Math.Abs(newX);
                        float absDestiny = Math.Abs(destiny);
                        actualStepSize += (absX - absDestiny) * (direction * -1);
                    }

                    ownerLastPos = owner.Position;
                    amount.X += actualStepSize;
                    base.move(amount);
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

        public void Lock()
        {
            lockMovement = true;
        }

        public void Unlock()
        {
            lockMovement = false;
        }

    }
}
