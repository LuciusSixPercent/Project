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

        protected Vector3 acumulatedMovement;
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
            acumulatedMovement = Vector3.Zero;
        }

        //utilizado apenas no update ou em casos muito específicos
        public virtual void ImediateTranslate(Vector3 amount)
        {
            position += amount;
        }

        //para uso geral, dar preferência a esse método de translação
        public virtual void Translate(Vector3 amount)
        {
            acumulatedMovement += amount;
        }

        public virtual void Update(GameTime gameTime)
        {
            ImediateTranslate(acumulatedMovement);
            acumulatedMovement = Vector3.Zero;
            foreach (Component c in components)
                c.Update(gameTime);
        }

        public void addComponent(Component c)
        {
            components.Add(c);
        }
    }
}
