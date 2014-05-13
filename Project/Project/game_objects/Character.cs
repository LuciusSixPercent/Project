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
        private Texture2D[] framesJumping;
        private Texture2D[] framesSad;

        private int currentFrame;
        private int elapsedTime;

        public delegate void CollidedWithAnswer(Answer answer);
        public event CollidedWithAnswer collidedWithAnswer;

        private Ball ball;

        private bool keepMoving;
        private bool kickBall;
        private bool kickingBall;
        private bool makeGoal;

        private bool touchingGround;
        private bool happyEnding;

        private int jumpCount;
        private bool finishedEnding;
        private bool sadEnding;

        public bool FinishedEnding
        {
            get { return finishedEnding; }
        }

        public bool KeepMoving
        {
            get { return keepMoving; }
            set { keepMoving = value; }
        }

        public bool KickingBall
        {
            get { return kickingBall; }
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

        public bool PlayingEnding { get { return happyEnding || sadEnding; } }

        public Character(Renderer3D renderer, IEnumerable<CollidableGameObject> collidableObjects, string name, Ball ball)
            : base(renderer, collidableObjects)
        {
            addComponent(new PlayerMovementComponent(this, 30, 0.05f, MAX_X));

            addComponent(new ConstantMovementComponent(this, new Vector3(0, 0, 0.1f), 40));

            addComponent(new VariableMovementComponent(this, 30, Vector3.Zero, Vector3.Zero));

            this.name = name;

            this.ball = ball;

            touchingGround = true;
        }

        public override void ImediateTranslate(Vector3 amount)
        {
            if (keepMoving || happyEnding)
            {
                if (happyEnding)
                {
                    amount *= (Vector3.UnitY);
                    if(amount.Y < 0 && !touchingGround)
                        amount += GetUpAmount(amount);
                }
                else
                {
                    float newX = Position.X + amount.X;
                    if (newX > MAX_X)
                        amount.X -= newX - MAX_X;
                    else if (newX < MIN_X)
                        amount.X += MIN_X - newX;

                    if (amount.Z > 0)
                    {
                        if (keepMoving)
                        {
                            currentFrame++;
                            if (currentFrame >= framesRunning.Length)
                                currentFrame = 0;
                        }

                    }
                }

                base.ImediateTranslate(amount);
                quad.Translate(amount);

                boundingBox.Min += amount;
                boundingBox.Max += amount;

                ball.Translate(amount * Vector3.Right); //certifica-se que a bola só se mova no eixo X aqui
            }
        }

        private Vector3 GetUpAmount(Vector3 amount)
        {
            Vector3 upAmount = Vector3.Zero;
            foreach (CollidableGameObject cgo in CollidableObjects)
            {
                if (cgo is Field)
                {
                    if (cgo.Collided(this))
                    {
                        upAmount = cgo.Position - (boundingBox.Min + amount);
                        upAmount *= Vector3.Up;
                        touchingGround = true;
                        VariableMovementComponent vmc = GetComponent<VariableMovementComponent>();
                        vmc.Acceleration = Vector3.Zero;
                        vmc.CurrentVelocity = Vector3.Zero;
                        currentFrame--;
                        jumpCount++;
                    }
                    break;
                }
            }
            return upAmount;
        }

        public void Reset()
        {
            framesBeingUsed = framesRunning;
            currentFrame = 0;
            jumpCount = 0;
            touchingGround = true;
            keepMoving = true;
            kickBall = kickingBall = finishedEnding = happyEnding = sadEnding = false;
            PlayerMovementComponent pmc = GetComponent<PlayerMovementComponent>();
            pmc.Unlock();
            Position = Vector3.Zero;
            pmc.Destiny = Position.X;
        }

        public void KickBall(bool makeGoal, Vector3 target)
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

            framesRunning = LoadFrames(cManager, "_correndo");
            if(framesRunning[0] != null)
                quadWidthScale *= ((float)framesRunning[0].Width / framesRunning[0].Height);

            framesKicking = LoadFrames(cManager, "_chutando");

            framesJumping = LoadFrames(cManager, "_pulando");

            framesSad = LoadFrames(cManager, "_triste");

            Reset();
        }

        private Texture2D[] LoadFrames(ContentManager cManager, string folderSuffix)
        {
            string path = "Imagem" + Path.AltDirectorySeparatorChar +
                    "Personagem" + Path.AltDirectorySeparatorChar +
                    Name + Path.AltDirectorySeparatorChar +
                    Name + folderSuffix + Path.AltDirectorySeparatorChar;

            DirectoryInfo dirInfo = new DirectoryInfo(cManager.RootDirectory + Path.AltDirectorySeparatorChar + path);
            Texture2D[] frames = new Texture2D[dirInfo.GetFiles("*.*").Length];
            for (int i = 0; i < frames.Length; i++)
            {
                try
                {
                    frames[i] = cManager.Load<Texture2D>(path + (i + 1));
                }
                catch (ContentLoadException ex) { }
            }
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
            else if (happyEnding)
            {
                UpdateJump(gameTime);
            }
            else if (sadEnding)
            {
                UpdateSadEnding(gameTime);
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
                            collidedWithAnswer(((QuestionGameObject)obj).CollidedAnswer);
                        }
                    }
                    else if (obj is Ball && !kickBall)
                    {
                        ((Ball)obj).Kick(new Vector3(0, 0.005f, 0.08f), new Vector3(0, 0.001f, 0.002f), new Vector3(0, 1f, 1f));
                    }
                }
            }
        }

        private void UpdateJump(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime >= (currentFrame == 5 ? 200 : 100))
            {
                if (jumpCount < 3)
                {
                    elapsedTime = 0;
                    if (touchingGround)
                    {
                        if (currentFrame < 6)
                            currentFrame++;

                        if (currentFrame == 6)
                        {
                            VariableMovementComponent vmc = GetComponent<VariableMovementComponent>();
                            vmc.CurrentVelocity = Vector3.Up / 5;
                            vmc.Acceleration = Vector3.Down / 20;
                            touchingGround = false;
                        }
                    }
                }
                else if (currentFrame > 3)
                {
                    currentFrame--;
                    elapsedTime = 0;
                }
                else
                {
                    finishedEnding = true;
                }
            }
        }

        private void UpdateSadEnding(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime >= 100)
            {
                elapsedTime = 0;
                if (currentFrame < framesSad.Length - 1)
                {
                    currentFrame++;
                }
                else
                {
                    finishedEnding = true;
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
                    if (currentFrame == 3)
                    {
                        KickBall();
                    }
                    if (currentFrame >= framesBeingUsed.Length)
                    {
                        currentFrame = 0;
                        kickingBall = false;
                        framesBeingUsed = framesJumping;
                    }
                }
            }
        }

        private void KickBall()
        {
            Vector3 kickDeviation = Vector3.Zero;

            if (!makeGoal)
            {
                kickDeviation.Y = (float)PublicRandom.NextDouble(0, 0.2);
                if (PublicRandom.NextDouble() > 0.5) kickDeviation.Y *= -1;

                kickDeviation.Z = -(float)PublicRandom.NextDouble(0.15, 0.25);

                kickDeviation.X = -(float)PublicRandom.NextDouble(0, 0.1);
                // if (PublicRandom.NextDouble() > 0.5) kickDeviation.X *= -1;
            }

            ball.Position = ball.Position * (Vector3.UnitX + Vector3.UnitZ);
            ball.Kick2(new Vector3(0, 0.4f, 0.3f) + kickDeviation, new Vector3(-kickDeviation.X/100, -0.05f, -0.0005f), Vector3.Zero);
        }

        public void PlayEnding()
        {
            happyEnding = makeGoal;
            sadEnding = !makeGoal;
            if (sadEnding)
                framesBeingUsed = framesSad;
        }

        public void LockMovement()
        {
            PlayerMovementComponent pmc = GetComponent<PlayerMovementComponent>();
            pmc.Destiny = 0;
            pmc.Lock();
        }
    }
}
