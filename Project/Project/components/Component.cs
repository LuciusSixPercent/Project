using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace components
{
    public abstract class Component
    {
        public abstract void Update(GameTime gameTime);
    }
}
