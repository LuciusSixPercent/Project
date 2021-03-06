﻿using System;
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
        private float scale = 0.2f;
        private Texture2D[] frames;
        private int currentFrame;
        private float maxArcHeight;
        private Vector3 lastPos;
        private VariableMovementComponent vmc;

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

        public override float X
        {
            get
            {
                return base.X;
            }
            set
            {
                base.X = value;
                createQuad();
            }
        }
        public override float Y
        {
            get
            {
                return base.Y;
            }
            set
            {
                base.Y = value;
                createQuad();
            }
        }
        public override float Z
        {
            get
            {
                return base.Z;
            }
            set
            {
                base.Z = value;
                createQuad();
            }
        }

        public bool Bouncing
        {
            get { return maxArcHeight > 0 || position.Y > 0 || vmc.CurrentVelocity.Y > 0; }
        }

        public Ball(Renderer3D renderer, IEnumerable<CollidableGameObject> collidableObjects)
            : base(renderer, collidableObjects)
        {
            vmc = new VariableMovementComponent(this, 30, Vector3.Zero, Vector3.Zero);
            addComponent(vmc);
            frames = new Texture2D[1];
        }

        public override void Load(ContentManager cManager)
        {
            string path = "Imagem" + Path.AltDirectorySeparatorChar + "Cenario" + Path.AltDirectorySeparatorChar + "Bate_Bola" + Path.AltDirectorySeparatorChar;
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = cManager.Load<Texture2D>(path + "ball" + (i + 1));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ((Renderer3D)Renderer).Draw(frames[currentFrame], quad, BlendState.AlphaBlend, BoundingBox);
        }

        public override void Update(GameTime gameTime)
        {
            lastPos = position;
            base.Update(gameTime);
        }

        public override void ImediateTranslate(Vector3 amount)
        {
            base.ImediateTranslate(amount);
            boundingBox.Min += amount;
            boundingBox.Max += amount;

            amount += GetUpAmount();

            quad.Translate(amount);
            if (position.Y > maxArcHeight || vmc.CurrentVelocity.Y > 0)
            {
                maxArcHeight = position.Y;
            }
        }

        private Vector3 GetUpAmount()
        {
            Vector3 upAmount = Vector3.Zero;
            foreach (CollidableGameObject cgo in CollidableObjects)
            {
                if (cgo is Field)
                {
                    if (cgo.Collided(this))
                    {
                        upAmount = cgo.Position - boundingBox.Min;
                        upAmount *= Vector3.Up;
                        base.ImediateTranslate(upAmount);
                        boundingBox.Max += upAmount;
                        boundingBox.Min += upAmount;
                        Bounce();
                    }
                    break;
                }
            }
            return upAmount;
        }

        private void Bounce()
        {
            if (vmc.CurrentVelocity.Z > 0 || maxArcHeight >= 0.1f)
            {
                vmc.InitialVelocity /= 1.75f;
                vmc.CurrentVelocity = vmc.InitialVelocity;
            }
            else
            {
                vmc.Acceleration = Vector3.Zero;
                vmc.AccelerationVariation = Vector3.Zero;
                vmc.CurrentVelocity = Vector3.Zero;
            }
        }

        private void createQuad()
        {
            quad = new Quad(position + new Vector3(0, scale / 2, 0), new Vector3(0, 0, -1), Vector3.Up, scale, scale);
            Vector3 backUpperLeft = quad.Vertices[1].Position;

            Vector3 frontBottomRight = quad.Vertices[2].Position;

            BoundingBox = new BoundingBox(frontBottomRight, backUpperLeft);
        }

        public void Kick(Vector3 velocity, Vector3 acceleration, Vector3 decelerationFactor)
        {
            if (boundingBox.Min.Y > 0)
            {
                velocity.Y /= 5;
            }
            vmc.InitialVelocity = velocity;
            vmc.CurrentVelocity = velocity;
            vmc.Acceleration = acceleration;
            vmc.InitialAcceleration = acceleration;
            vmc.AccelerationVariation = acceleration * -decelerationFactor;
            vmc.LowerVelocityThreshold = new Vector3(0, -1, 0);
        }

        public void Kick2(Vector3 velocity, Vector3 acceleration, Vector3 accelerationVariation)
        {
            vmc.InitialVelocity = velocity;
            vmc.CurrentVelocity = velocity;
            vmc.Acceleration = acceleration;
            vmc.InitialAcceleration = acceleration;
            vmc.AccelerationVariation = accelerationVariation;
            vmc.LowerVelocityThreshold = new Vector3(-1, -1, 0);
        }

        internal void Reset()
        {
            currentFrame = 0;
        }
    }
}
