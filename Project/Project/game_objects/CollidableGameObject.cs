using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace game_objects
{
    public abstract class CollidableGameObject : DrawableGameObject
    {
        protected BoundingBox boundingBox;
        private IEnumerable<CollidableGameObject> collidableObjects;

        public IEnumerable<CollidableGameObject> CollidableObjects
        {
            get { return collidableObjects; }
            set { collidableObjects = value; }
        }

        public CollidableGameObject(Renderer renderer, IEnumerable<CollidableGameObject> collidableObjects)
            : base(renderer)
        {
            this.collidableObjects = collidableObjects;
        }

        public CollidableGameObject(Renderer renderer)
            : base(renderer)
        {
        }


        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }

        public virtual bool Collided(CollidableGameObject obj)
        {
            return obj != null && obj != this && boundingBox.Intersects(obj.BoundingBox);
        }
    }
}
