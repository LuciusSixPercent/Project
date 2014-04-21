using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using components;
using Project;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using game_objects.questions;

namespace game_objects
{
    public class Character : CollidableGameObject
    {
        private Quad quad;
        private float quadWidthScale = 0.75f;
        private float quadHeightScale = 0.75f;
        private const float MAX_X = 1f;
        private const float MIN_X = -1f;
        private Texture2D[] frames;

        private int currentFrame;
        private string name;
        private GameObjectsManager oManager;

        public delegate void CollidedWithQuestion(Vector3 position, bool correctAnswer);
        public event CollidedWithQuestion collidedWithQuestion;
        public Quad Sprite
        {
            get
            {
                if (quad == null)
                    createQuad();
                return quad;
            }
        }

        public Character(GameObjectsManager oManager, Renderer3D renderer, string name)
            : base(renderer)
        {
            PlayerMovementComponent pmc = new PlayerMovementComponent(this, 30, 0.05f, MAX_X);
            addComponent(pmc);

            ConstantMovementComponent cmc = new ConstantMovementComponent(this, new Vector3(0, 0, 0.1f), 40);
            addComponent(cmc);

            currentFrame = 0;

            this.oManager = oManager;

            this.name = name;

            frames = new Texture2D[11];
        }

        public override void Translate(Vector3 amount)
        {
            float newX = Position.X + amount.X;
            if (newX > MAX_X)
                amount.X -= newX - MAX_X;
            else if (newX < MIN_X)
                amount.X += MIN_X - newX;

            if (amount.Z > 0)
            {
                currentFrame++;
                if (currentFrame >= frames.Length)
                    currentFrame = 0;
            }
            base.Translate(amount);
            quad.Translate(amount);

            Vector3 backUpperLeft = quad.Vertices[1].Position;
            backUpperLeft.Z += 0.1f;

            Vector3 frontBottomRight = quad.Vertices[2].Position;

            BoundingBox = new BoundingBox(frontBottomRight, backUpperLeft);
        }

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

        private void createQuad()
        {
            quad = new Quad(position, new Vector3(0, 0, -1), Vector3.Up, quadWidthScale, quadHeightScale);
            Vector3 backUpperLeft = quad.Vertices[1].Position;
            backUpperLeft.Z += 2;

            Vector3 frontBottomRight = quad.Vertices[2].Position;
            frontBottomRight.Z -= 2;

            BoundingBox = new BoundingBox(frontBottomRight, backUpperLeft);
        }

        public override void Load(ContentManager cManager)
        {
            for(int i = 0; i < frames.Length; i++)
                frames[i] = 
                    cManager.Load<Texture2D>(
                    "Imagem" + Path.AltDirectorySeparatorChar + 
                    "Personagem" + Path.AltDirectorySeparatorChar + 
                    name + "corre" + Path.AltDirectorySeparatorChar + 
                    name + "_corre" + (i + 1));

            quadWidthScale *= ((float)frames[0].Width / frames[0].Height);
        }

        public override void Draw(GameTime gameTime)
        {
            ((Renderer3D)Renderer).Draw(gameTime, frames[currentFrame], quad, BlendState.AlphaBlend);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (CollidableGameObject obj in oManager.CollidableGameObjects)
            {
                if (obj.Collided(this) && obj is QuestionGameObject)
                {
                    if (collidedWithQuestion != null)
                    {
                        collidedWithQuestion(Position, ((QuestionGameObject)obj).CorrecAnswer());
                    }
                }
            }
        }
    }
}
