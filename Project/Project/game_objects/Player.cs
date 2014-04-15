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
        private const float quadScale = 1f;
        private const float MAX_X = 2f;
        private const float MIN_X = -2f;

        public Quad Sprite
        {
            get { return quad; }
        }

        public Player()
            : base()
        {
            PlayerMovementComponent pmc = new PlayerMovementComponent(this, 15, 0.2f, MAX_X);
            addComponent(pmc);

            ConstantMovementComponent cmc = new ConstantMovementComponent(this, new Vector3(0, 0, 0.1f), 10);
            addComponent(cmc);

            quad = new Quad(Vector3.Zero, new Vector3(0, 0, -1), Vector3.Up, quadScale, quadScale);
            position = quad.Coord;
        }

        public override void Translate(Vector3 amount)
        {
            float newX = Position.X + amount.X;
            if (newX > MAX_X)
                amount.X -= newX - MAX_X;
            else if (newX < MIN_X)
                amount.X += MIN_X - newX;
            base.Translate(amount);
            quad.translate(amount);
        }

    }
}
