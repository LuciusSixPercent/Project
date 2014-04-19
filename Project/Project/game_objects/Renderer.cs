using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace game_objects
{
    public abstract class Renderer
    {
        protected GraphicsDevice gDevice;
        private float alpha;

        public virtual float Alpha
        {
            get { return alpha; }
            set { alpha = value;}
        }
        public Renderer(GraphicsDevice gDevice)
        {
            this.gDevice = gDevice;
        }
    }
}
