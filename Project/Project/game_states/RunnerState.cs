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

        private int currentLoadingStep;
        private int totalLoadingSteps;

        int columns = 33;
        int rows = 31;

        private Camera cam;
        private Character player;
        private Ball ball;
        private List<QuestionGameObject> questions;
        private Background bg;
        private Field field;

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

                questions = new List<QuestionGameObject>();

                ball = new Ball(goManager.R3D, goManager.CollidableGameObjects);

                player = new Character(goManager.R3D, goManager.CollidableGameObjects, "cosme", ball);
                player.collidedWithAnswer += new Character.CollidedWithAnswer(player_collidedWithQuestion);

                cam = new Camera(new Vector3(0f, 3f, -4f), Vector3.Up, new Vector2(0.25f, 20f));
                cam.lookAt(new Vector3(0f, 0.25f, 2f), true);
                cam.createProjection(MathHelper.PiOver4, parent.GraphicsDevice.Viewport.AspectRatio);
                goManager.R3D.Cam = cam;

                bg = new Background(goManager.R2D);
                field = new Field(goManager.R3D, rows, columns);

                totalLoadingSteps = 5;
                currentLoadingStep = 0;

                goManager.AddObject(cam);
                goManager.AddObject(bg);
                goManager.AddObject(field);
                goManager.AddObject(ball);
                goManager.AddObject(player);
            }
        }

        private void player_collidedWithQuestion(Vector3 position, Answer answer)
        {
            if (questions[questions.Count - 1].CheckAnswer(answer, false))
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
            questions[questions.Count - 1].Player = player;
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
                questions[questions.Count - 1].Position = new Vector3(0, 0.25f, player.Position.Z + 5);
                questions[questions.Count - 1].Player = player;
            }
            else
            {
                answeredAll = true;
                centerPlayer();
            }
        }

        private void centerPlayer()
        {
            PlayerMovementComponent pmc = player.GetComponent<PlayerMovementComponent>();
            pmc.Destiny = 0;
            pmc.Lock();
        }

        public override void LoadContent()
        {
            if (!ContentLoaded)
            {
                //base.LoadContent();
                if (currentLoadingStep <= totalLoadingSteps)
                {
                    switch (currentLoadingStep)
                    {
                        case 0:
                            CacheLetters();
                            break;
                        case 1:
                            QuestionsDatabase.LoadQuestions();
                            break;
                        case 2:
                            bg.Load(parent.Content);
                            break;
                        case 3:
                            player.Load(parent.Content);
                            break;
                        case 4:
                            field.Load(parent.Content);
                            break;
                        case 5:
                            LoadQuestions(1);
                            break;
                    }
                    currentLoadingStep++;
                }
                else
                {
                    player.Position = Vector3.Zero;
                    contentLoaded = true;
                }

                /*
                QuestionsDatabase.LoadQuestions();

                player.Position = Vector3.Zero;

                for (char c = 'A'; c <= 'Z'; c++)
                {
                    TextHelper.AddToCache(c.ToString());
                }

                LoadQuestions(1);

                contentLoaded = true;
                */
            }
        }

        private void CacheLetters()
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                TextHelper.AddToCache(c.ToString());
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
            field.Reset();
            score = 0;
            perfectSscoreMultiplier = 2;
            foreach (QuestionGameObject q in questions)
                goManager.removeObject(q);
            LoadQuestions(1);
        }

        #region Transitioning
        public override void EnterState(bool freezeBelow)
        {
            if (!exitingState)
            {
                base.EnterState(freezeBelow);
                if (!ContentLoaded)
                {
                    LoadingState ls = (LoadingState)parent.getState((int)StatesIdList.LOADING);
                    ls.OverseeingState = this;
                    parent.EnterState(ls.ID);
                }
                //LoadContent();
            }
        }

        public override void EnterState()
        {
            EnterState(FreezeBelow);
        }

        public override void ExitState()
        {
            if (!exit)
            {
                base.ExitState();
            }
            else
            {
                parent.ExitState(ID);
                Reset();
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (ContentLoaded)
            {
                base.Update(gameTime);
                if (stateEntered)
                {
                    if (!exitingState)
                    {
                        if (!finished)
                        {
                            //Wave
                            if (engineSound == null)
                            {
                                engineSound = soundBank3.GetCue("540428_quotSports-Fanaticq");
                                //engineSound.Play();
                            }
                            if (engineSound.IsPaused)
                            {
                                //engineSound.Resume();
                            }
                            //
                            CheckAnswer();
                            if (answeredAll)
                            {
                                field.KeepMoving = false;
                                if (field.Goal.Position.Z - player.Position.Z <= 8 && player.KeepMoving)
                                {
                                    player.KickBall(perfectSscoreMultiplier == 2);
                                    cam.KeepMoving = false;
                                }
                            }

                            handleInput();
                        }

                    }

                }
                else if (exit)
                {
                    engineSound.Stop(AudioStopOptions.AsAuthored);
                    ExitState();
                }
            }
        }

        /// <summary>
        /// Verifica se o jogador colidiu ou passou por alguma resposta.
        /// </summary>
        private void CheckAnswer()
        {
            if (foundAnswer != 0)
            {
                UpdateScoreAndQuestion();
            }
            else if (!answeredAll)
            {
                QuestionGameObject question = questions[questions.Count - 1];
                Answer a = question.GetClosestAnswer();
                if (a.Position.Z <= player.Position.Z - 2)
                {
                    if (question.CheckAnswer(a, true))
                        perfectSscoreMultiplier = 1;    //a partir do momento que o jogador errou uma questão, não ganha mais o dobro de pontos
                    question.MoveAnswer(a);
                }
            }
        }

        /// <summary>
        /// Atualiza a posição das respostas e muda o multiplicador do score caso o jogador tenha errado.
        /// </summary>
        private void UpdateScoreAndQuestion()
        {
            if (foundAnswer == 1)
            {
                if (!questions[questions.Count - 1].Next())
                {
                    ChangeCurrentQuestion();
                }
                else
                {
                    questions[questions.Count - 1].Position = (new Vector3(0, 0, player.Position.Z + 5));
                }
            }
            else
            {
                perfectSscoreMultiplier = 1;    //a partir do momento que o jogador errou uma questão, não ganha mais o dobro de pontos
                questions[questions.Count - 1].MoveAnswer(questions[questions.Count - 1].GetClosestAnswer());
            }
            foundAnswer = 0;
        }

        /// <summary>
        /// Verifica quais teclas estão sendo pressionadas e realiza alguma ação de acordo (quando necessário)
        /// </summary>
        private void handleInput()
        {
            if (KeyboardHelper.IsKeyDown(Keys.Escape))
            {
                KeyboardHelper.LockKey(Keys.Escape);
                if (!answeredAll)
                {
                    if (parent.EnterState((int)StatesIdList.PAUSE, false))
                    {
                        //engineSound.Pause();
                        Alpha = 0.5f;
                        goManager.R3D.Alpha = goManager.R2D.Alpha = Alpha;
                        stateEntered = false;
                    }
                }
                else
                {
                    finished = true;
                    ExitState();
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
            if (ContentLoaded)
            {
                base.Draw(gameTime);
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
        }

        #endregion
    }
}
