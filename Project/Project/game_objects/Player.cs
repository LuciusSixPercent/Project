using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using components;
using Project;

namespace game_objects
{
    public class Player : GameObject
    {
        private Quad quad;
        private const float MAX_X = 2f;
        private const float MIN_X = -2f;

        public Quad Sprite
        {
            get { return quad; }
        }

        public Player()
            : base()
        {
            PlayerMovementComponent pmc = new PlayerMovementComponent(175, 1.5f);
            addComponent(pmc);
            pmc.moved += new MovementComponent.Moved(pmc_moved);

            ConstantMovementComponent cmc = new ConstantMovementComponent(new Vector3(0, 0, 0.1f), 10);
            addComponent(cmc);
            cmc.moved += new MovementComponent.Moved(pmc_moved);

            quad = new Quad(Vector3.Zero, new Vector3(0, 0, -1), Vector3.Up, 0.75f, 0.75f);
        }

        void pmc_moved(Vector3 amount)
        {
            float newX = Position.X + amount.X;
            if(newX < MAX_X && newX > MIN_X)
                quad.translate(amount);
        }

        public Vector3 Position { 
            get { return quad.Coord; }
            set { quad.translate(value); }
        }
    }
}
