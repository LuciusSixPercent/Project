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
using game_objects.ui;

namespace game_states
{
    public class RunnerState : GameState
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
        private ProgressBar progress;

        private string charName;

        private int foundAnswer; //0 for no; 1 for correct; -1 for incorrect

        private int numberOfQuestions;
        private int maxAllowedMistakes;
        private int mistakesMade;

        private int score;
        private int perfectSscoreMultiplier;

        private bool answeredAll;
        private bool finished;

        private bool shouldWait;
        //WAVE - Musica de fundo
        Cue engineSound = null;

        private bool shouldReset;
        #endregion

        public bool ShouldReset
        {
            get { return shouldReset; }
            set { shouldReset = value; }
        }

        public int Score
        {
            get { return score; }
        }

        public int PerfectSscoreMultiplier
        {
            get { return perfectSscoreMultiplier; }
        }

        public int NumberOfAnswers
        {
            get { return progress.Total; }
            set { progress.Total = value; }
        }

        public int AnswersGot
        {
            get { return progress.Loaded; }
            set { progress.AddProgress(1); }
        }

        public int MistakesMade
        {
            get { return mistakesMade; }
        }

        public int AllowedMistakes
        {
            get { return maxAllowedMistakes; }
        }

        public string CharName
        {
            set
            {
                player.Name = value;
                player.Load(parent.Content);

                progress.MeterFileName = "meter_" + value;
                progress.LoadMeter(parent.Content);

            }
        }

        public int NumberOfQuestions
        {
            get { return numberOfQuestions; }
            set { numberOfQuestions = value; }
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
                level = RunnerLevel.EASY;
                subjects = new QuestionSubject[] { QuestionSubject.PT };
                score = 0;
                perfectSscoreMultiplier = 2;

                enterTransitionDuration = 500;
                exitTransitionDuration = 1000;

                shouldWait = true;

                questions = new List<QuestionGameObject>();

                numberOfQuestions = 1;

                ball = new Ball(goManager.R3D, goManager.CollidableGameObjects);

                player = new Character(goManager.R3D, goManager.CollidableGameObjects, "cosme", ball);
                player.collidedWithAnswer += new Character.CollidedWithAnswer(player_collidedWithAnswer);

                cam = new Camera(new Vector3(0f, 3f, -4f), Vector3.Up, new Vector2(0.25f, 20f));
                cam.lookAt(new Vector3(0f, 0.25f, 2f), true);
                cam.createProjection(MathHelper.PiOver4, parent.GraphicsDevice.Viewport.AspectRatio);
                goManager.R3D.Cam = cam;

                bg = new Background(goManager.R2D);
                field = new Field(goManager.R3D, rows, columns);

                progress = new ProgressBar(goManager.R2D);
                progress.Orientation = BarOrientation.VERTICAL;
                char separator = Path.AltDirectorySeparatorChar;
                progress.FilePath = "Imagem" + separator + "ui" + separator + "progress_bar";
                progress.FillOpacity = 0.75f;

                totalLoadingSteps = 6;
                currentLoadingStep = 0;

                goManager.AddObject(cam);
                goManager.AddObject(bg);
                goManager.AddObject(field);
                goManager.AddObject(ball);
                goManager.AddObject(player);
                goManager.AddObject(progress);
            }
        }

        private void player_collidedWithAnswer(Vector3 position, Answer answer)
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

        private void LoadQuestions()
        {
            questions.Clear();
            for (int i = 0; i < numberOfQuestions; i++)
            {
                questions.Add(QuestionFactory.CreateQuestion(level, subjects[PublicRandom.Next(subjects.Length)], goManager.R3D, goManager.CollidableGameObjects, questions));
            }
            goManager.AddObject(questions[questions.Count - 1]);
            questions[questions.Count - 1].Player = player;
            questions[questions.Count - 1].Position = new Vector3(0, 0.25f, 5);

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
            player.LockMovement();
        }

        public override void LoadContent()
        {
            if (!ContentLoaded)
            {
                if (currentLoadingStep <= totalLoadingSteps)
                {
                    switch (currentLoadingStep)
                    {
                        case 0:
                            QuestionsDatabase.LoadQuestions();
                            break;
                        case 1:
                            bg.Load(parent.Content);
                            break;
                        case 2:
                            player.Load(parent.Content);
                            break;
                        case 3:
                            ball.Load(parent.Content);
                            break;
                        case 4:
                            field.Load(parent.Content);
                            break;
                        case 5:
                            LoadUI();
                            break;
                    }
                    currentLoadingStep++;
                }
                else
                {
                    shouldReset = true;
                    contentLoaded = true;
                }
            }
        }

        private void LoadUI()
        {
            progress.Load(parent.Content);
        }

        public void Reset()
        {
            finished = false;
            answeredAll = false;
            shouldWait = true;
            shouldReset = false;

            player.Reset();
            progress.Reset();
            progress.Visible = false;

            if (progress.Position.Equals(Vector3.Zero))
            {
                Viewport viewport = parent.GraphicsDevice.Viewport;
                progress.Position = new Vector3(0, viewport.Height / 2 - progress.Dimensions.Y / 2, 0);
            }


            cam.KeepMoving = true;
            cam.Position = new Vector3(0f, 3f, -4f);
            cam.lookAt(new Vector3(0f, 0.25f, 2f), true);
            goManager.R3D.updateEffect(cam.View, cam.Projection);
            field.Reset();
            score = 0;
            perfectSscoreMultiplier = 2;
            foreach (QuestionGameObject q in questions)
                goManager.removeObject(q);

            LoadQuestions();

            mistakesMade = 0;
            NumberOfAnswers = 0;
            mistakesMade = 0;
            foreach (QuestionGameObject q in questions)
            {
                NumberOfAnswers += q.AnswerCount;
            }
            maxAllowedMistakes = NumberOfAnswers / ((int)Level + 1);

        }

        #region Transitioning
        public override void EnterState()
        {
            if (!exitingState)
            {
                base.EnterState();
                if (!ContentLoaded)
                {
                    LoadingState ls = (LoadingState)parent.getState((int)StatesIdList.LOADING);
                    ls.OverseeingState = this;
                    parent.EnterState(ls.ID);
                }
            }
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
                shouldReset = true;
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (ContentLoaded)
            {
                if (shouldReset)
                {
                    Reset();
                }
                else
                {
                    base.Update(gameTime);
                    if (shouldWait && Alpha >= 0.5f)
                    {
                        shouldWait = false;
                        parent.EnterState((int)StatesIdList.RUNNER_WAIT);
                    }
                    else
                    {
                        if (stateEntered)
                        {
                            if (!exitingState && !finished)
                            {

                                if (!progress.Visible)
                                {
                                    progress.Visible = true;
                                }

                                PlayBGM();
                                CheckAnswer();
                                if (answeredAll)
                                {
                                    field.KeepMoving = false;
                                    if (player.KeepMoving)
                                    {
                                        if (field.Goal.Position.Z - player.Position.Z <= 8)
                                        {
                                            player.KickBall(MistakesMade < maxAllowedMistakes, field.Goal.Position);
                                            cam.KeepMoving = false;
                                        }
                                    }
                                    else if (!player.PlayingEnding)
                                    {
                                        if (!ball.Bouncing && !player.KickingBall)
                                        {
                                            player.PlayEnding();
                                        }
                                    }
                                    else if (player.FinishedEnding)
                                    {
                                        finished = true;
                                        Alpha = 0.5f;
                                        parent.EnterState((int)StatesIdList.RUNNER_END);
                                    }
                                }

                                handleInput();
                            }
                        }
                        else if (exit)
                        {
                            engineSound.Stop(AudioStopOptions.AsAuthored);
                            ExitState();
                        }
                    }
                }
            }
        }

        private void PlayBGM()
        {
            if (engineSound == null || engineSound.IsStopped)
            {
                engineSound = AudioManager.GetCue("540428_quotSports-Fanaticq");
            }

            if (!engineSound.IsPlaying)
            {
                engineSound.Play();
            }
            if (engineSound.IsPaused)
            {
                engineSound.Resume();
            }
        }

        /// <summary>
        /// Verifica se o jogador colidiu ou passou por alguma resposta.
        /// </summary>
        private void CheckAnswer()
        {
            if (MistakesMade < maxAllowedMistakes)
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
                        {
                            perfectSscoreMultiplier = 1;    //a partir do momento que o jogador errou uma questão, não ganha mais o dobro de pontos
                            mistakesMade++;
                            AudioManager.GetCue("miss_answer_" + (PublicRandom.Next(2) + 1)).Play();
                        }
                        question.MoveAnswer(a);
                    }
                }
            }
            else if (!answeredAll)
            {
                answeredAll = true;
                centerPlayer();
                foreach (QuestionGameObject q in questions)
                {
                    goManager.removeObject(q);
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
                AnswersGot++;
                if (!questions[questions.Count - 1].Next())
                {
                    ChangeCurrentQuestion();
                    AudioManager.GetCue("BB00" + PublicRandom.Next(6, 8)).Play();
                }
                else
                {
                    questions[questions.Count - 1].Position = (new Vector3(0, 0, player.Position.Z + 5));
                    AudioManager.GetCue("BB00" + PublicRandom.Next(3, 6)).Play();
                }
            }
            else
            {
                perfectSscoreMultiplier = 1;    //a partir do momento que o jogador errou uma questão, não ganha mais o dobro de pontos
                mistakesMade++;
                questions[questions.Count - 1].MoveAnswer(questions[questions.Count - 1].GetClosestAnswer());
                AudioManager.GetCue("wrong_answer_1").Play();
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
                    if (parent.EnterState((int)StatesIdList.PAUSE))
                    {
                        if (!engineSound.IsStopped)
                            engineSound.Pause();
                        Alpha = 0.5f;
                        goManager.R3D.Alpha = goManager.R2D.Alpha = Alpha;
                        stateEntered = false;
                    }
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
                drawDebugData();
            }
        }

        private void drawDebugData()
        {

            //* DEBUG

            string header = "";
            int questionScore = 0;
            int answerValue = 0;
            if (questions.Count > 0)
            {
                header = questions[questions.Count - 1].Header;
                questionScore = questions[questions.Count - 1].Score;
                answerValue = questions[questions.Count - 1].CurrentAnswerValue;
            }
            Viewport screen = parent.GraphicsDevice.Viewport;
            Color c = new Color(120, 20, 60);

            float scale = 0.5f;
            goManager.R2D.DrawString(header, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(header).X*scale, 0), c, 0, Vector2.Zero, scale);
            c *= 0.95f;
            string s = "Pontos acumulados na iteração: " + score + " x (" + perfectSscoreMultiplier + ")";

            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale, 30), c, 0, Vector2.Zero, scale);

            s = "Pontos acumulados na questão: " + questionScore;
            c *= 0.9f;
            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale, 60), c, 0, Vector2.Zero, scale);

            s = "Valor da resposta atual: " + answerValue;
            c *= 0.9f;
            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale, 90), c, 0, Vector2.Zero, scale);

            s = "Questões restantes " + questions.Count;
            c *= 0.9f;
            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale, 120), c, 0, Vector2.Zero, scale);

            s = "Erros: " + MistakesMade;
            c *= 0.9f;
            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale, 150), c, 0, Vector2.Zero, scale);

            s = "Acertos: " + AnswersGot;
            c *= 0.9f;
            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale, 180), c, 0, Vector2.Zero, scale);

            s = "MAX. Erros: " + maxAllowedMistakes;
            c *= 0.9f;
            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale, 210), c, 0, Vector2.Zero, scale);

            c = new Color(20, 10, 30);

            goManager.R2D.DrawString(header, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(header).X * scale - 1, 1), c, 0, Vector2.Zero, scale);
            c *= 0.95f;
            s = "Pontos acumulados na iteração: " + score + " x (" + perfectSscoreMultiplier + ")";
            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale -1, 31), c, 0, Vector2.Zero, scale);

            s = "Pontos acumulados na questão: " + questionScore;
            c *= 0.9f;
            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale - 1, 61), c, 0, Vector2.Zero, scale);

            s = "Valor da resposta atual: " + answerValue;
            c *= 0.9f;
            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale - 1, 91), c, 0, Vector2.Zero, scale);

            s = "Questões restantes " + questions.Count;
            c *= 0.9f;
            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale - 1, 121), c, 0, Vector2.Zero, scale);

            s = "Erros: " + MistakesMade;
            c *= 0.9f;
            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale - 1, 151), c, 0, Vector2.Zero, scale);

            s = "Acertos: " + AnswersGot;
            c *= 0.9f;
            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale - 1, 181), c, 0, Vector2.Zero, scale);

            s = "MAX. Erros: " + maxAllowedMistakes;
            c *= 0.9f;
            goManager.R2D.DrawString(s, new Vector2(screen.Width - TextHelper.SpriteFont.MeasureString(s).X * scale - 1, 211), c, 0, Vector2.Zero, scale);

            if (exitingState)
            {
                goManager.R2D.DrawString("Alpha " + Alpha, new Vector2(400, 150), Color.Silver);
            }
            goManager.R2D.End();
            //*/
        }

        #endregion

    }
}
