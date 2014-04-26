using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Project;
using components;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace game_objects
{
    public class Ball : CollidableGameObject
    {
        private Quad quad;
        private float scale = 0.125f;
        private Texture2D[] frames;
        private int currentFrame;

        public override Vector3 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                createQuad();
            }
        }

        public Ball(Renderer3D renderer, IEnumerable<CollidableGameObject> collidableObjects)
            : base(renderer, collidableObjects)
        {
            addComponent(new VariableMovementComponent(this, 30, Vector3.Zero, Vector3.Zero));
            frames = new Texture2D[1];
        }

        public override void Load(ContentManager cManager)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = cManager.Load<Texture2D>("ball" + (i+1));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ((Renderer3D)Renderer).Draw(frames[currentFrame], quad, BlendState.AlphaBlend, BoundingBox);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void ImediateTranslate(Vector3 amount)
        {
            base.ImediateTranslate(amount);
            boundingBox.Min += amount;
            boundingBox.Max += amount;

            Vector3 upAmount = Vector3.Zero;
            foreach (CollidableGameObject cgo in CollidableObjects)
            {
                if (cgo is Field)
                {
                    while (cgo.Collided(this))
                    {
                        upAmount = cgo.Position - boundingBox.Min;
                        upAmount *= Vector3.Up;
                        base.ImediateTranslate(upAmount);
                        boundingBox.Max += upAmount;
                        boundingBox.Min += upAmount;
                        VariableMovementComponent vmc = GetComponent<VariableMovementComponent>();
                        vmc.Acceleration = Vector3.Zero;
                        vmc.CurrentVelocity = Vector3.Zero;
                        vmc.AccelerationVariation = Vector3.Zero;
                        amount += upAmount;
                    }
                    break;
                }
            }
            quad.Translate(amount);
        }
        private void createQuad()
        {
            quad = new Quad(position + new Vector3(0, scale / 2, 0), new Vector3(0, 0, -1), Vector3.Up, scale, scale);
            Vector3 backUpperLeft = quad.Vertices[1].Position;

            Vector3 frontBottomRight = quad.Vertices[2].Position;

            BoundingBox = new BoundingBox(frontBottomRight, backUpperLeft);
        }

        public void KickOff()
        {
            VariableMovementComponent vmc = GetComponent<VariableMovementComponent>();
            vmc.CurrentVelocity = new Vector3(0, boundingBox.Min.Y == 0 ? 0.01f : 0f, 0.125f);
            vmc.Acceleration = new Vector3(0, boundingBox.Min.Y == 0 ? 0.01f : 0f, 0.005f);
            vmc.AccelerationVariation = new Vector3(0, -0.005f, -0.001f);
        }

        internal void Reset()
        {
            currentFrame = 0;
        }
    }
}
