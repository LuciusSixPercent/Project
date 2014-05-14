using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using game_objects;
using Microsoft.Xna.Framework.Content;

namespace game_objects
{
    public abstract class DrawableGameObject : GameObject
    {
        private bool visible;
        private Renderer renderer;

        public Renderer Renderer
        {
            get { return renderer; }
        }

        public DrawableGameObject(Renderer renderer) : base()
        {
            this.renderer = renderer;
            this.visible = true;
        }

        public bool is3D
        {
            get { return renderer is Renderer3D; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public abstract void Load(ContentManager cManager);

        public abstract void Draw(GameTime gameTime);
    }
}
