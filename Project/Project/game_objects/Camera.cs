using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using components;

namespace game_objects
{
    public class Camera : GameObject
    {
        private Vector3 position;
        private Vector3 target;
        private Vector3 up;
        private Vector2 clip;
        private Matrix view;
        private Matrix projection;
        private float fieldOfView;

        public Vector3 Target
        {
            get { return target; }
        }

        public int NearView
        {
            get { return (int)clip.X; }
        }

        public int FarView
        {
            get { return (int)clip.Y; }
        }

        public Matrix View
        {
            get { return view; }
        }

        public Matrix Projection
        {
            get { return projection; }
        }

        public float X
        {
            get { return position.X; }
        }

        public float Y
        {
            get { return position.Y; }
        }
        public float Z
        {
            get { return position.Z; }
        }

        public Camera(Vector3 position, Vector3 up, Vector2 clip) : base()
        {
            this.position = position;
            this.up = up;
            this.clip = clip;
            lookAt(Vector3.Zero);
            ConstantMovementComponent cmc = new ConstantMovementComponent(new Vector3(0, 0, 0.1f), 10);
            cmc.moved += new MovementComponent.Moved(cmc_moved);
            addComponent(cmc);
        }

        void cmc_moved(Vector3 amount)
        {
            move(amount);
            moveTarget(amount);
        }

        public void lookAt(Vector3 target)
        {
            this.target = target;
            view = Matrix.CreateLookAt(position, target, up);
        }

        public void createProjection(float fieldOfView, float aspectRatio)
        {
            this.fieldOfView = fieldOfView;
            projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, clip.X, clip.Y);
        }

        public void move(Vector3 amount)
        {
            position += amount;
        }

        public void moveTarget(Vector3 amount)
        {
            target += amount;
        }

        public void Rotate(Matrix rotation)
        {
            view *= rotation;
            projection *= rotation;
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 pos = position;
            base.Update(gameTime);
            if (pos != position)
                lookAt(target);
        }
    }
}
