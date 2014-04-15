using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project;
using game_objects;
using game_objects.questions;

namespace game_states
{
    class RunnerState : GameState
    {
        #region Variables Declaration
        private RunnerLevel level;

        public RunnerLevel Level
        {
            get { return level; }
            set { level = value; }
        }

        int columns = 10;
        int rows = 54;
        float scale = 1f;

        private Camera cam;
        private Player player;
        private Stack<Question> questions;
        private Texture2D[] questionsTex;

        bool pauseFlag;

        #region Graphics
        Texture2D floor;
        Texture2D bg;
        Texture2D character;
        private SpriteFont spriteFont;
        BasicEffect basicEffect;
        Quad[,] floorTiles;
        #endregion

        private bool contentLoaded;

        Texture2D SimpleTexture;
        private Texture2D[] answerTex;

        #endregion

        public RunnerState(int id, Game1 parent)
            : base(id, parent)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            if (!initialized)
            {
                base.Initialize();
                enterTransitionDuration = 500;
                exitTransitionDuration = 250;

                player = new Player();
                player.Position = new Vector3(0f, 1f, -3.5f);

                cam = new Camera(new Vector3(0f, 3f, -4f), Vector3.Up, new Vector2(0.25f, 50));
                cam.lookAt(new Vector3(0f, 0.25f, 2f), true);
                cam.createProjection(MathHelper.PiOver4, parent.GraphicsDevice.Viewport.AspectRatio);

                initEffect();

                initQuads();

                level = RunnerLevel.EASY;
            }
        }

        private void initEffect()
        {
            basicEffect = new BasicEffect(parent.GraphicsDevice);
            basicEffect.FogEnabled = true;
            basicEffect.FogColor = Color.Gray.ToVector3();
            basicEffect.FogStart = cam.FarView * 0.75f; ;
            basicEffect.FogEnd = cam.FarView;
            basicEffect.World = Matrix.Identity;
            basicEffect.View = cam.View;
            basicEffect.Projection = cam.Projection;
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = floor;
        }

        private void initQuads()
        {
            floorTiles = new Quad[rows, columns];

            float leftColumnX = -scale * 0.5f * columns - scale * 0.5f;

            Vector3 initialCoord = new Vector3(leftColumnX, 0f, cam.Z);
            Vector3 normal = new Vector3(0, 1, 0);
            Vector3 up = new Vector3(0, 0, -1);
            for (int row = 0; row < floorTiles.GetLength(0); row++)
            {

                for (int col = 0; col < floorTiles.GetLength(1); col++)
                {
                    initialCoord.X += scale;
                    floorTiles[row, col] = new Quad(initialCoord, normal, up, scale, scale);
                }
                initialCoord.Z += scale;
                initialCoord.X = leftColumnX;
            }
        }

        protected override void LoadContent()
        {
            if (!contentLoaded)
            {
                contentLoaded = true;
                floor = parent.Content.Load<Texture2D>("Imagem/Cenario/grass");
                bg = parent.Content.Load<Texture2D>("Imagem/Cenario/sky");
                character = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria");
                spriteFont = parent.Content.Load<SpriteFont>("Fonte/Verdana");
                questions = new Stack<Question>();
                createQuestion();
            }
        }

        private void createQuestion()
        {
            questions.Push(QuestionFactory.CreateQuestion(level, QuestionSubject.PT, 1));

            Random rdn = new Random();
            questions.Peek().Position = new Vector3(0, player.Position.Y, player.Position.Z + rdn.Next(10, 30));

            drawQuestionAnswers();
        }

        private void drawQuestionAnswers()
        {
            GraphicsDevice graphicsDevice = parent.GraphicsDevice;
            string[] answers = questions.Peek().Answers;
            answerTex = new Texture2D[answers.Length];
            for (int i = 0; i < answers.Length; i++)
            {
                Vector2 size = spriteFont.MeasureString(answers[i]);
                RenderTarget2D rt = new RenderTarget2D(graphicsDevice, (int)size.X, (int)size.Y, true, graphicsDevice.DisplayMode.Format, DepthFormat.Depth24, 4, RenderTargetUsage.PreserveContents);
                graphicsDevice.SetRenderTarget(rt);
                graphicsDevice.Clear(Color.Transparent);
                answerTex[i] = (Texture2D)rt;
                SpriteBatch.Begin();
                SpriteBatch.DrawString(spriteFont, answers[i], Vector2.Zero, Color.Tomato, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                SpriteBatch.End();
            }
            graphicsDevice.SetRenderTarget(null);
        }

        #region Transitioning
        public override void EnterState(bool freezeBelow)
        {
            if (!exitingState)
            {
                base.EnterState(freezeBelow);
                LoadContent();
                pauseFlag = false;
            }
        }
        public override void EnterState()
        {
            EnterState(FreezeBelow);
        }
        public override void ExitState()
        {
            if (!enteringState)
            {
                base.ExitState();
                parent.Content.Unload();
                contentLoaded = false;
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (!pauseFlag)
            {
                base.Update(gameTime);
                if (stateEntered)
                {
                    if (!exitingState)
                    {
                        updateObjects(gameTime);

                        translateQuads();

                        handleInput(gameTime);
                    }
                }
            }
        }

        private void handleInput(GameTime gameTime)
        {
            if (KeyboardHelper.IsKeyDown(Keys.Escape))
            {
                KeyboardHelper.LockKey(Keys.Escape);
                if (parent.EnterState((int)StatesIdList.PAUSE, false))
                {
                    alpha = 0.5f;
                    pauseFlag = true;
                    stateEntered = false;
                }
            }
            else if (KeyboardHelper.KeyReleased(Keys.Escape))
            {
                KeyboardHelper.UnlockKey(Keys.Escape);
            }
        }

        private void translateQuads()
        {
            foreach (Quad quad in floorTiles)
            {
                if (quad.Coord.Z <= cam.Z)
                {
                    quad.translate(new Vector3(0, 0, scale * rows));
                }
            }
        }

        private void updateObjects(GameTime gameTime)
        {
            cam.Update(gameTime);
            player.Update(gameTime);
            basicEffect.View = cam.View;
            basicEffect.Projection = cam.Projection;

            if (questions.Peek().Position.Z <= cam.Z)
            {
                questions.Pop();
                createQuestion();
            }
        }

        #region DRAWING
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphicsDevice = parent.GraphicsDevice;

            basicEffect.Alpha = alpha;

            DrawBackGround(graphicsDevice);

            DrawFloor(graphicsDevice);

            DrawQuestions(graphicsDevice);

            DrawCharacter(graphicsDevice);

            #region DEBUG CODE

            if (SimpleTexture == null)
            {
                SimpleTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
                SimpleTexture.SetData(new[] { Color.White });
            }
            SpriteBatch.Begin();
            DrawLine(SpriteBatch, SimpleTexture, 1, Color.Red, new Vector2(Mouse.GetState().X, 0), new Vector2(Mouse.GetState().X, graphicsDevice.Viewport.Bounds.Height));
            DrawLine(SpriteBatch, SimpleTexture, 1, Color.Red, new Vector2(0, Mouse.GetState().Y), new Vector2(graphicsDevice.Viewport.Bounds.Width, Mouse.GetState().Y));
            SpriteBatch.End();


            #endregion
        }

        void DrawLine(SpriteBatch batch, Texture2D blank,
        float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }

        private void DrawQuestions(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.BlendState = BlendState.AlphaBlend;
            Question q = questions.Peek();
            for (int i = 0; i < q.Answers.Length; i++)
            {
                Texture2D tex = answerTex[i];
                basicEffect.Texture = tex;

                Quad quad = q.AnswersQuads[i];
                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserIndexedPrimitives
                            <VertexPositionNormalTexture>(
                            PrimitiveType.TriangleList,
                            quad.Vertices, 0, 4,
                            Quad.Indexes, 0, 2);
                }
            }
        }

        private void DrawCharacter(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.BlendState = BlendState.AlphaBlend;
            basicEffect.Texture = character;
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives
                        <VertexPositionNormalTexture>(
                        PrimitiveType.TriangleList,
                        player.Sprite.Vertices, 0, 4,
                        Quad.Indexes, 0, 2);
            }
        }

        private void DrawFloor(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.BlendState = BlendState.Opaque;
            basicEffect.Texture = floor;

            BoundingFrustum frustum = new BoundingFrustum(basicEffect.View * basicEffect.Projection);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (Quad quad in floorTiles)
                {
                    BoundingBox box = new BoundingBox(quad.Vertices[1].Position, quad.Vertices[2].Position);
                    if (frustum.Contains(box) != ContainmentType.Disjoint)
                    {
                        graphicsDevice.DrawUserIndexedPrimitives
                            <VertexPositionNormalTexture>(
                            PrimitiveType.TriangleList,
                            quad.Vertices, 0, 4,
                            Quad.Indexes, 0, 2);
                    }
                }
            }
        }

        private void DrawBackGround(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.BlendState = BlendState.AlphaBlend;
            SpriteBatch.Begin();
            SpriteBatch.Draw(bg, Vector2.Zero, Color.White * alpha);
            SpriteBatch.End();
        }

        #endregion
    }
}
