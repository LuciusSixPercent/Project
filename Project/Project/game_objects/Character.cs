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
        private const float MAX_X = 1f;
        private const float MIN_X = -1f;

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        private Quad quad;

        private float quadWidthScale = 1.1f;
        private float quadHeightScale = 1.1f;

        private Texture2D[] framesRunning;
        private Texture2D[] framesKicking;
        private Texture2D[] framesBeingUsed;

        private int currentFrame;
        private int elapsedTime;

        public delegate void CollidedWithAnswer(Vector3 position, Answer answer);
        public event CollidedWithAnswer collidedWithAnswer;

        private Ball ball;

        private bool keepMoving;
        private bool kickBall;
        private bool kickingBall;
        private bool makeGoal;

        public bool KeepMoving
        {
            get { return keepMoving; }
            set { keepMoving = value; }
        }

        public Quad Sprite
        {
            get
            {
                if (quad == null)
                    createQuad();
                return quad;
            }
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
                ball.Position = position + new Vector3(0, 0, 0.25f);
                createQuad();
            }
        }

        public Character(Renderer3D renderer, IEnumerable<CollidableGameObject> collidableObjects, string name, Ball ball)
            : base(renderer, collidableObjects)
        {
            PlayerMovementComponent pmc = new PlayerMovementComponent(this, 30, 0.05f, MAX_X);
            addComponent(pmc);

            ConstantMovementComponent cmc = new ConstantMovementComponent(this, new Vector3(0, 0, 0.1f), 40);
            addComponent(cmc);
            
            this.name = name;

            framesRunning = new Texture2D[9];

            framesKicking = new Texture2D[7];

            this.ball = ball;
        }

        public override void ImediateTranslate(Vector3 amount)
        {
            if (keepMoving)
            {
                float newX = Position.X + amount.X;
                if (newX > MAX_X)
                    amount.X -= newX - MAX_X;
                else if (newX < MIN_X)
                    amount.X += MIN_X - newX;

                if (amount.Z > 0)
                {
                    currentFrame++;
                    if (currentFrame >= framesRunning.Length)
                        currentFrame = 0;
                }
                base.ImediateTranslate(amount);
                quad.Translate(amount);

                boundingBox.Min += amount;
                boundingBox.Max += amount;

                ball.Translate(amount * Vector3.Right); //certifica-se que a bola só se mova no eixo X aqui
            }
        }

        public void Reset()
        {
            currentFrame = 0;
            framesBeingUsed = framesRunning;
            keepMoving = true;
            kickBall = kickingBall = false;
            PlayerMovementComponent pmc = GetComponent<PlayerMovementComponent>();
            pmc.Unlock();
            Position = Vector3.Zero;
            pmc.Destiny = Position.X;
        }

        public void KickBall(bool makeGoal)
        {
            kickBall = true;
            this.makeGoal = makeGoal;
        }

        private void createQuad()
        {
            quad = new Quad(position + new Vector3(0, quadHeightScale / 2, 0), new Vector3(0, 0, -1), Vector3.Up, quadWidthScale, quadHeightScale);
            Vector3 backUpperLeft = quad.UpperLeft;
            backUpperLeft.Z += 0.1f;

            BoundingBox = new BoundingBox(quad.LowerRight, backUpperLeft);
        }

        public override void Load(ContentManager cManager)
        {
            quadWidthScale = quadHeightScale;

            framesRunning = LoadFrames(cManager, framesRunning.Length, "_correndo");
            
            quadWidthScale *= ((float)framesRunning[0].Width / framesRunning[0].Height);

            framesKicking = LoadFrames(cManager, framesKicking.Length, "_chutando");

            Reset();
        }

        private Texture2D[] LoadFrames(ContentManager cManager, int lenght, string folderSuffix)
        {
            
            Texture2D[] frames = new Texture2D[lenght];
            for (int i = 0; i < frames.Length; i++)
                frames[i] =
                    cManager.Load<Texture2D>(
                    "Imagem" + Path.AltDirectorySeparatorChar +
                    "Personagem" + Path.AltDirectorySeparatorChar +
                    Name + Path.AltDirectorySeparatorChar + 
                    Name + folderSuffix + Path.AltDirectorySeparatorChar + (i + 1));
            return frames;
        }

        public override void Draw(GameTime gameTime)
        {
            if (framesBeingUsed != null)
            {
                ((Renderer3D)Renderer).Draw(framesBeingUsed[currentFrame], quad, BlendState.AlphaBlend, BoundingBox);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (kickBall && position.Z >= ball.Position.Z - 0.1f && keepMoving)
            {
                keepMoving = false;
                currentFrame = 0;
                framesBeingUsed = framesKicking;
                kickingBall = true;
            }
            CheckCollisions();
            UpdateBallKick(gameTime);
        }

        private void CheckCollisions()
        {
            foreach (CollidableGameObject obj in CollidableObjects)
            {
                if (obj.Collided(this))
                {
                    if (obj is QuestionGameObject)
                    {
                        if (collidedWithAnswer != null)
                        {
                            collidedWithAnswer(Position, ((QuestionGameObject)obj).CollidedAnswer);
                        }
                    }
                    else if (obj is Ball && !kickBall)
                    {
                        ((Ball)obj).Kick(new Vector3(0, 0.005f, 0.08f), new Vector3(0, 0.001f, 0.002f), new Vector3(0, 1f, 1f));
                    }
                }
            }
        }

        private void UpdateBallKick(GameTime gameTime)
        {
            if (kickingBall)
            {
                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedTime >= 100)
                {
                    elapsedTime = 0;
                    currentFrame++;
                    if (currentFrame >= framesBeingUsed.Length)
                    {
                        currentFrame = 0;
                        KickBall();
                    }
                }
            }
        }

        private void KickBall()
        {
            kickingBall = false;
            Vector3 kickDeviation = Vector3.Zero;
            if (!makeGoal)
            {
                kickDeviation.Y = (float)PublicRandom.NextDouble(0, 0.05);
                kickDeviation.Z = -(float)PublicRandom.NextDouble(0, 0.1);
            }
            ball.Kick(new Vector3(0, 0.2f, 0.15f) + kickDeviation, new Vector3(0, 0.001f, 0.00001f), new Vector3(-kickDeviation.X / 10, 2f, 0.0000005f));
        }
    }
}
