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
        private Vector3 target;
        private Vector3 up;
        private Vector2 clip;
        private Matrix view;
        private Matrix projection;
        private BoundingFrustum viewFrustum;
        private float fieldOfView;
        private bool lockRotation;

        public delegate void CameraMoved();
        public event CameraMoved cam_moved;

        public BoundingFrustum ViewFrustum
        {
            get { return viewFrustum; }
            set { viewFrustum = value; }
        }

        public bool LockRotation
        {
            get { return lockRotation; }
            set { lockRotation = value; }
        }

        public bool KeepMoving { get; set; }

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

        public override Vector3 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                Vector3 targetTranslation = Vector3.Zero;
                if (lockRotation)
                    targetTranslation -= (position - value);
                base.Position = value;
                moveTarget(targetTranslation);
            }
        }

        public Camera(Vector3 position, Vector3 up, Vector2 clip)
            : base()
        {
            this.position = position;
            this.up = up;
            this.clip = clip;
            lookAt(Vector3.Zero, true);
            ConstantMovementComponent cmc = new ConstantMovementComponent(this, new Vector3(0, 0, 0.1f), 40);
            addComponent(cmc);
            KeepMoving = true;
        }

        public void lookAt(Vector3 target, bool lockRotation)
        {
            this.target = target;
            this.lockRotation = lockRotation;
            view = Matrix.CreateLookAt(position, target, up);

            viewFrustum = new BoundingFrustum(View * Projection);
        }

        public void createProjection(float fieldOfView, float aspectRatio)
        {
            this.fieldOfView = fieldOfView;
            projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, clip.X, clip.Y);
        }

        public override void ImediateTranslate(Vector3 amount)
        {
            if (KeepMoving)
            {
                base.ImediateTranslate(amount);
                if (lockRotation)
                    moveTarget(amount);
            }
        }

        public void moveTarget(Vector3 amount)
        {
            target += amount;
        }

        public void Rotate(Matrix rotation)
        {
            if (!lockRotation)
            {
                view *= rotation;
                projection *= rotation;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 pos = position;
            base.Update(gameTime);
            if (pos != position)
            {
                lookAt(target, true);
                if (cam_moved != null)
                {
                    cam_moved();
                }
            }
        }
    }
}
