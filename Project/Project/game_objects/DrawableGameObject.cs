using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace game_objects
{
    public abstract class DrawableGameObject : GameObject
    {
        private bool visible;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }


        public abstract void Draw(GameTime gameTime);
    }
}
