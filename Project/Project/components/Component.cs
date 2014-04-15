using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using game_objects;

namespace components
{
    public abstract class Component
    {
        protected GameObject owner;

        protected Component(GameObject owner)
        {
            this.owner = owner;
        }

        public abstract void Update(GameTime gameTime);
    }
}
