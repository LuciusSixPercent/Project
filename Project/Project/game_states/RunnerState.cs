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
using components;
using Microsoft.Xna.Framework.Audio;

namespace game_states
{
    class RunnerState : GameState
    {
        #region Variables Declaration
        private RunnerLevel level;
        private QuestionSubject[] subjects;

        int columns = 4;
        int rows = 35;

        private Camera cam;
        private Character player;
        private List<QuestionGameObject> questions;
        private Field field;

        bool pauseFlag;

        private bool contentLoaded;

        private GameObjectsManager goManager;
        private int foundAnswer; //0 for no; 1 for correct; -1 for incorrect

        private int score;
        private int perfectSscoreMultiplier;

        private bool answeredAll;
        private bool finished;
        //WAVE - Musica de fundo
        AudioEngine audioEngine3;
        WaveBank waveBank3;
        SoundBank soundBank3;
        Cue engineSound = null;
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

        public RunnerLevel Level
        {
            get { return level; }
            set { level = value; }
        }

        public QuestionSubject[] Subjects
        {
            get { return subjects; }
            set { subjects = value; }
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
                //Wave
                audioEngine3 = new AudioEngine("Content\\Audio\\MyGameAudio2.xgs");
                waveBank3 = new WaveBank(audioEngine3, "Content\\Audio\\Wave Bank2.xwb");
                soundBank3 = new SoundBank(audioEngine3, "Content\\Audio\\Sound Bank2.xsb");
                //
                level = RunnerLevel.EASY;
                subjects = new QuestionSubject[] { QuestionSubject.PT };
                score = 0;
                perfectSscoreMultiplier = 2;

                enterTransitionDuration = 500;
                exitTransitionDuration = 1000;

                goManager = new GameObjectsManager(parent.GraphicsDevice);

                questions = new List<QuestionGameObject>();

                player = new Character(goManager.R3D, goManager.CollidableGameObjects, "cosme");
                player.collidedWithQuestion += new Character.CollidedWithQuestion(player_collidedWithQuestion);

                cam = new Camera(new Vector3(0f, 3f, -4f), Vector3.Up, new Vector2(0.25f, 30));
                cam.lookAt(new Vector3(0f, 0.25f, 2f), true);
                cam.createProjection(MathHelper.PiOver4, parent.GraphicsDevice.Viewport.AspectRatio);
                goManager.R3D.Cam = cam;

                field = new Field(goManager.R3D, goManager.CollidableGameObjects, rows, columns);

                goManager.AddObject(cam);
                goManager.AddObject(new Sky(goManager.R2D));
                goManager.AddObject(field);
                goManager.AddObject(player);
            }
        }

        private void player_collidedWithQuestion(Vector3 position, bool correctAnswer)
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

        public void LoadQuestions(int numberOfQuestions)
        {
            questions.Clear();
            for (int i = 0; i < numberOfQuestions; i++)
            {
                questions.Add(QuestionFactory.CreateQuestion(level, subjects[PublicRandom.Next(subjects.Length)], goManager.R3D, goManager.CollidableGameObjects, questions));
            }
            goManager.AddObject(questions[questions.Count - 1]);
            questions[questions.Count - 1].Position = new Vector3(0, 0.25f, player.Position.Z + 5);
        }

        private void ChangeCurrentQuestion()
        {

            QuestionGameObject answeredQuestion = questions[questions.Count - 1];
            questions.RemoveAt(questions.Count - 1);
            score += answeredQuestion.Score;
            goManager.removeObject(answeredQuestion);

            if (questions.Count > 0)
            {
                goManager.AddObject(questions[questions.Count - 1]);
                questions[questions.Count - 1].Position = new Vector3(0, 0.25f, player.Position.Z + 1);
            }
            else
            {
                answeredAll = true;
            }
        }

        protected override void LoadContent()
        {
            if (!contentLoaded)
            {
                QuestionsDatabase.LoadQuestions();

                TextHelper.LoadDefaultFont(parent.Content);

                goManager.Load(parent.Content);

                player.Position = new Vector3(0f, 0f, 0f);

                LoadQuestions(1);

                contentLoaded = true;
            }
        }

        public void Reset()
        {
            exit = false;
            finished = false;
            answeredAll = false;
            player.Reset();
            cam.KeepMoving = true;
            cam.Position = new Vector3(0f, 3f, -4f);
            cam.lookAt(new Vector3(0f, 0.25f, 2f), true);
            goManager.R3D.updateEffect(cam.View, cam.Projection);
            field.KeepMoving = true;
            field.Position = Vector3.Backward;
            score = 0;
            perfectSscoreMultiplier = 2;
            LoadQuestions(1);
        }

        #region Transitioning
        public override void EnterState(bool freezeBelow)
        {
            if (!exitingState)
            {
                base.EnterState(freezeBelow);
                LoadContent();
                pauseFlag = false;
                if (finished)
                {
                    Reset();
                }
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
                if (!exit)
                {
                    base.ExitState();
                }
                else
                {
                    parent.ExitState(ID);
                }
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            if (stateEntered)
            {
                if (!exitingState)
                {
                    if (!finished)
                    {
                        if (!pauseFlag)
                        {
                            //Wave
                            if (engineSound == null)
                            {
                                engineSound = soundBank3.GetCue("515728_Soccer-Life-97");
                                engineSound.Play();
                            }
                            if (engineSound.IsPaused)
                            {

                                engineSound.Resume();
                                
                            }
                            //
                            CheckAnswer();
                            if (answeredAll)
                            {
                                field.KeepMoving = false;
                                if (field.Goal.Position.Z - player.Position.Z <= 5 && player.KeepMoving)
                                {
                                    player.KickBall();
                                    cam.KeepMoving = false;
                                }
                            }

                            goManager.Update(gameTime);

                            handleInput(gameTime);


                        }
                        
                    }
                    else
                    {
                        engineSound.Stop(AudioStopOptions.AsAuthored);
                        ExitState();
                    }
                }
            }
            else if (exit)
            {
                engineSound.Stop(AudioStopOptions.AsAuthored);
                ExitState();
            }
        }

        private void CheckAnswer()
        {
            if (foundAnswer != 0)
            {
                if (foundAnswer == 1)
                {
                    if (!questions[questions.Count - 1].Next())
                    {
                        ChangeCurrentQuestion();
                    }
                    else
                    {
                        questions[questions.Count - 1].Position = (new Vector3(0, 0, player.Position.Z + 3));
                    }
                }
                else
                {
                    perfectSscoreMultiplier = 1;    //a partir do momento que o jogador errou uma questão, não ganha mais o dobro de pontos
                    questions[questions.Count - 1].Retreat();
                }
                foundAnswer = 0;
            }
        }

        private void handleInput(GameTime gameTime)
        {
            if (KeyboardHelper.IsKeyDown(Keys.Escape))
            {
                KeyboardHelper.LockKey(Keys.Escape);
                if (!answeredAll)
                {
                    if (parent.EnterState((int)StatesIdList.PAUSE, false))
                    {
                        engineSound.Pause();
                        Alpha = 0.5f;
                        goManager.R3D.Alpha = goManager.R2D.Alpha = Alpha;
                        pauseFlag = true;
                        stateEntered = false;
                    }
                }
                else
                {
                    finished = true;
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
            string header = "";
            int questionScore = 0;
            int answerValue = 0;
            if (questions.Count > 0)
            {
                header = questions[questions.Count - 1].Header;
                questionScore = questions[questions.Count - 1].Score;
                answerValue = questions[questions.Count - 1].CurrentAnswerValue;
            }

            //*
            goManager.R2D.DrawString(header, Vector2.Zero, Color.RosyBrown);
            goManager.R2D.DrawString("Pontos acumulados na iteração: " + score + " x (" + perfectSscoreMultiplier + ")", new Vector2(0, 30), Color.RosyBrown);
            goManager.R2D.DrawString("Pontos acumulados na questão: " + questionScore, new Vector2(0, 60), Color.RosyBrown);
            goManager.R2D.DrawString("Valor da resposta atual: " + answerValue, new Vector2(0, 90), Color.RosyBrown);
            goManager.R2D.DrawString("Questões restantes " + questions.Count, new Vector2(0, 120), Color.RosyBrown);
            if (exitingState)
            {
                goManager.R2D.DrawString("Alpha " + Alpha, new Vector2(400, 150), Color.Black);
            }
            goManager.R2D.End();
            //*/
        }

        #endregion
    }
}
