using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using components;

namespace game_objects
{
    public abstract class GameObject
    {
        private List<Component> components;

        protected Vector3 position;

        public virtual Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected GameObject()
        {
            components = new List<Component>();
            position = Vector3.Zero;
        }

        public virtual void Translate(Vector3 amount)
        {
            position += amount;
        }


        public virtual void Update(GameTime gameTime)
        {
            foreach (Component c in components)
                c.Update(gameTime);
        }

        public void addComponent(Component c)
        {
            components.Add(c);
        }
    }
}
