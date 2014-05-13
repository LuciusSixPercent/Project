﻿using System;
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
    public enum RunnerLevel
    {
        EASY,
        MEDIUM,
        HARD
    }

    public class RunnerState : GameState
    {
        #region Variables Declaration
        private RunnerLevel level;
        private QuestionSubject[] subjects;
        private StatesIdList goBackTo;

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

        private TextBox questionHeader;
        private List<TextBox> answersGotLbl;
        private Repeatable2DGameObject answersSupports;
        #endregion


        public StatesIdList GoBackTo
        {
            get { return goBackTo; }
            set { goBackTo = value; }
        }

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
            set { 
                level = value;
                switch (level)
                {
                    case RunnerLevel.MEDIUM:
                        numberOfQuestions = 4;
                        break;
                    default:
                        numberOfQuestions = 3;
                        break;
                }
            }
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

                goBackTo = StatesIdList.EMPTY_STATE;

                questions = new List<QuestionGameObject>();

                questionHeader = new TextBox(goManager.R2D);
                questionHeader.FontSize = 60;
                questionHeader.TextColor = new Color(245, 236, 222);
                questionHeader.Outline = true;
                questionHeader.OutlineColor = new Color(111, 129, 129);
                questionHeader.OutlineWeight = 2;
                questionHeader.Alignment = TextAlignment.CENTER;
                questionHeader.Width = 1024;
                questionHeader.Height = 100;
                questionHeader.Y = 5;
                
                string path = "Imagem" + Path.AltDirectorySeparatorChar + "ui" + Path.AltDirectorySeparatorChar + "bate_bola"+ 
                    Path.AltDirectorySeparatorChar+"respostas_e_pontos"+ Path.AltDirectorySeparatorChar;
                
                answersSupports = new Repeatable2DGameObject(goManager.R2D, path, "suporte", 2, -1);
                answersSupports.Y = 100;
                answersSupports.RepeatAmount = Vector2.Zero;
                answersSupports.AdaptToFrame = false;

                answersGotLbl = new List<TextBox>();

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
                goManager.AddObject(questionHeader);
                goManager.AddObject(answersSupports);
            }
        }

        private void player_collidedWithAnswer(Answer answer)
        {
            if (questions[questions.Count - 1].CheckAnswer(answer, false))
            {
                foundAnswer = 1;
                foreach (char c in answer.Text)
                {
                    Vector3 position = answersSupports.GetClonePosition(answersGotLbl.Count);
                    TextBox answerLbl = CreateLabel(c.ToString(), position);
                    answerLbl.Y -= answersSupports.Height;
                    answerLbl.Y -= answerLbl.Height;
                    answersSupports.AdvanceCloneFrame(answersGotLbl.Count);
                    answersGotLbl.Add(answerLbl);
                    goManager.AddObject(answerLbl);
                }
            }
            else
            {
                foundAnswer = -1;
            }
        }

        private TextBox CreateLabel(string text, Vector3 position)
        {
            TextBox answerLbl = new TextBox(goManager.R2D);
            answerLbl.Text = text;
            answerLbl.TextColor = Color.LightPink;
            answerLbl.Alignment = TextAlignment.CENTER;
            answerLbl.Position = position;
            answerLbl.FontSize = (int)(60 * (answersSupports.Width/93)); //escalona o tamanho da fonte caso o suporte tenha sido reduzido
            answerLbl.DropShadow = true;
            answerLbl.Width = answersSupports.Width;
            answerLbl.Height = 25;

            return answerLbl;
        }

        private void LoadQuestions()
        {
            questions.Clear();
            for (int i = 0; i < numberOfQuestions; i++)
            {
                questions.Add(QuestionFactory.CreateQuestion(level, subjects[PublicRandom.Next(subjects.Length)], goManager.R3D, goManager.CollidableGameObjects, questions));
            }

            goManager.AddObject(questions[questions.Count - 1]);
            questionHeader.Text = questions[questions.Count - 1].Header;

            ResetAnswersDisplay();
            CreateAnswersSupports();

            questions[questions.Count - 1].Player = player;
            questions[questions.Count - 1].Position = new Vector3(0, 0f, 5);

        }

        private void ResetAnswersDisplay()
        {
            for (int i = 0; i < answersGotLbl.Count; i++)
            {
                goManager.RemoveObject(answersGotLbl[i]);
            } 
            answersGotLbl.Clear();
            answersSupports.RepeatAmount = Vector2.Zero;
        }

        private void CreateAnswersSupports()
        {
            string path = "Imagem" + Path.AltDirectorySeparatorChar + "ui" + Path.AltDirectorySeparatorChar + "bate_bola" + Path.AltDirectorySeparatorChar + "repostas_e_pontos" + Path.AltDirectorySeparatorChar;
            int charCount = 0;
            for (int j = 0; j < questions[questions.Count - 1].Question.AnswerCount; j++)
            {
                string answer = questions[questions.Count - 1].Question.GetAnswer(j);
                charCount += answer.Length;
            }

            answersSupports.RepeatAmount = new Vector2(charCount, 1);

            float widthScale = 1;
            float baseWidth = 93;
            float padding = 2;
            float totalWidth = baseWidth * (answersSupports.RepeatAmount.X) +padding * (answersSupports.RepeatAmount.X);

            Viewport screen = parent.GraphicsDevice.Viewport;

            if (totalWidth > screen.Width)
            {
                widthScale = screen.Width / totalWidth;
            }

            totalWidth *= widthScale;

            answersSupports.Width = baseWidth * widthScale;
            
            answersSupports.X = (screen.Width - totalWidth) / 2;

            float remaining = screen.Width - answersSupports.X - totalWidth;
        }

        private void ChangeCurrentQuestion()
        {

            QuestionGameObject answeredQuestion = questions[questions.Count - 1];
            questions.RemoveAt(questions.Count - 1);
            score += answeredQuestion.Score;
            goManager.RemoveObject(answeredQuestion);

            if (questions.Count > 0)
            {
                goManager.AddObject(questions[questions.Count - 1]);
                questions[questions.Count - 1].Position = new Vector3(0, 0.25f, player.Position.Z + 16);
                questions[questions.Count - 1].Player = player;
                questionHeader.Text = questions[questions.Count - 1].Header;
                ResetAnswersDisplay();
                CreateAnswersSupports();
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
            answersSupports.Load(parent.Content);
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
                goManager.RemoveObject(q);

            LoadQuestions();

            mistakesMade = 0;
            NumberOfAnswers = 0;
            foreach (QuestionGameObject q in questions)
            {
                NumberOfAnswers += q.AnswerCount;
            }
            maxAllowedMistakes = (int) Math.Round((double)NumberOfAnswers / ((int)Level + 2), MidpointRounding.AwayFromZero);

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
                if (goBackTo == StatesIdList.EMPTY_STATE)
                    parent.ExitState(ID);
                else
                    parent.ExitState(ID, (int)goBackTo);
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
                engineSound = AudioManager.GetCue("quotsports_fanaticq");
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

                    if (cam.ViewFrustum.Contains(a.BoundingBox) == ContainmentType.Disjoint && a.Z < player.Z)
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
                    goManager.RemoveObject(q);
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

        public void Continue()
        {
            if (goBackTo == StatesIdList.EMPTY_STATE)
            {
                if (level == RunnerLevel.HARD)
                {
                    numberOfQuestions++;
                }
                shouldReset = true;
            }
            else
            {
                ExitState();
            }
        }

        #region DRAWING
        public override void Draw(GameTime gameTime)
        {
            if (ContentLoaded)
            {
                base.Draw(gameTime);
                //drawDebugData();
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
