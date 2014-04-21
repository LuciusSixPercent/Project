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
using System.IO;

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

        int columns = 4;
        int rows = 54;

        private Camera cam;
        private Character player;
        private Stack<QuestionGameObject> questions;

        bool pauseFlag;

        private bool contentLoaded;

        private GameObjectsManager goManager;
        private int foundAnswer; //0 for no; 1 for correct; -1 for incorrect

        #endregion

        protected override float Alpha
        {
            get
            {
                return base.Alpha;
            }
            set
            {
                base.Alpha = value;
                goManager.R3D.Alpha = goManager.R2D.Alpha = Alpha;
            }
        }

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

                level = RunnerLevel.EASY;

                enterTransitionDuration = 500;
                exitTransitionDuration = 250;

                goManager = new GameObjectsManager(parent.GraphicsDevice);

                questions = new Stack<QuestionGameObject>();

                player = new Character(goManager, goManager.R3D, "cosme");
                player.collidedWithQuestion += new Character.CollidedWithQuestion(player_collidedWithQuestion);

                cam = new Camera(new Vector3(0f, 3f, -4f), Vector3.Up, new Vector2(0.25f, 30));
                cam.lookAt(new Vector3(0f, 0.25f, 2f), true);
                cam.createProjection(MathHelper.PiOver4, parent.GraphicsDevice.Viewport.AspectRatio);
                goManager.R3D.Cam = cam;

                goManager.AddObject(cam);
                goManager.AddObject(new Sky(goManager.R2D));
                goManager.AddObject(new Field(goManager.R3D, rows, columns));
                goManager.AddObject(player);
            }
        }

        void player_collidedWithQuestion(Vector3 position, bool correctAnswer)
        {
            if (correctAnswer)
            {
                foundAnswer = 1;
            }
            else
            {
                foundAnswer = -1;
            }
        }

        private void ChangeCurrentQuestion()
        {
            if(questions.Count > 0)
                goManager.removeObject(questions.Pop());
            /*TODO: carregar todas as questões de uma vez na pilha e apenas ir removendo as que forem resolvidas*/
            questions.Push(QuestionFactory.CreateQuestion(level, QuestionSubject.PT, goManager.R3D));
            questions.Peek().Load(parent.Content);
            goManager.AddDrawableObject(questions.Peek(), player);
            questions.Peek().Position = new Vector3(0, 0.25f, player.Position.Z + 10);
        }

        protected override void LoadContent()
        {
            if (!contentLoaded)
            {
                QuestionsDatabase.LoadQuestions();

                TextureHelper.LoadDefaultFont(parent.Content);

                ChangeCurrentQuestion();

                goManager.Load(parent.Content);
                
                player.Position = new Vector3(0f, 0.5f, 0f);

                contentLoaded = true;
            }
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
                        CheckAnswer();
                        goManager.Update(gameTime);

                        handleInput(gameTime);
                    }
                }
            }
        }

        private void CheckAnswer()
        {
            if (foundAnswer != 0)
            {
                if (foundAnswer == 1)
                {
                    if (!questions.Peek().Next())
                    {
                        ChangeCurrentQuestion();
                    }
                    else
                    {
                        questions.Peek().Translate(new Vector3(0, 0, 10));
                    }
                }
                else
                {
                    questions.Peek().Translate(new Vector3(0, 0, 20));
                }
                foundAnswer = 0;
            }
        }

        private void handleInput(GameTime gameTime)
        {
            if (KeyboardHelper.IsKeyDown(Keys.Escape))
            {
                KeyboardHelper.LockKey(Keys.Escape);
                if (parent.EnterState((int)StatesIdList.PAUSE, false))
                {
                    Alpha = 0.5f;
                    goManager.R3D.Alpha = goManager.R2D.Alpha = Alpha;
                    pauseFlag = true;
                    stateEntered = false;
                }
            }
            else if (KeyboardHelper.KeyReleased(Keys.Escape))
            {
                KeyboardHelper.UnlockKey(Keys.Escape);
            }
        }

        #region DRAWING
        public override void Draw(GameTime gameTime)
        {
            goManager.Draw(gameTime);
        }

        #endregion
    }
}
