using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;
using game_objects.ui;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace game_states
{
    public class RunnerEndState : GameState
    {

        private Button titleScreen;
        private Button _continue;
        private Button tryAgain;
        private TextBox results;
        private StatesIdList gotoState;
        private RunnerState monitoredState;

        public RunnerEndState(int id, Game1 parent, RunnerState monitoredState)
            : base(id, parent)
        {
            this.monitoredState = monitoredState;
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();

            FreezeGraphicsBelow = false;
            enterTransitionDuration = 100;
            exitTransitionDuration = 300;

            Viewport screen = parent.GraphicsDevice.Viewport;
            char separator = Path.AltDirectorySeparatorChar;

            titleScreen = new Button(goManager.R2D, new Rectangle(screen.Width / 2 + 25, (int)(screen.Height * 0.75), 200, 66));
            titleScreen.FilePath = "Menu" + separator + "Generic" + separator;
            titleScreen.BaseFileName = "menuInicialBtn";
            titleScreen.mouseClicked += new Button.MouseClicked(titleScreen_mouseClicked);

            tryAgain = new Button(goManager.R2D, new Rectangle(screen.Width / 2 - 225, (int)titleScreen.Position.Y, (int)titleScreen.Dimensions.X, (int)titleScreen.Dimensions.Y));
            tryAgain.FilePath = "Imagem" + separator + "ui" + separator + "bate_bola" + separator + "fim" + separator;
            tryAgain.BaseFileName = "tryBtn";
            tryAgain.mouseClicked += new Button.MouseClicked(playAgain_mouseClicked);
            tryAgain.Disable();
            tryAgain.Visible = false;

            _continue = new Button(goManager.R2D, new Rectangle((int)tryAgain.Position.X, (int)tryAgain.Position.Y, (int)tryAgain.Dimensions.X, (int)tryAgain.Dimensions.Y));
            _continue.FilePath = tryAgain.FilePath;
            _continue.BaseFileName = "continueBtn";
            _continue.mouseClicked += new Button.MouseClicked(_continue_mouseClicked);
            _continue.Disable();
            _continue.Visible = false;

            results = new TextBox(goManager.R2D);
            results.Width = screen.Width;
            results.Height = 300;
            results.FontSize = 40;
            results.Outline = true;
            results.OutlineColor = Color.SandyBrown;
            results.OutlineWeight = 1;
            results.DropShadow = true;
            results.ShadowColor = Color.Gray * 0.5f;
            results.ShadowOffset = new Vector2(0, -8);
            results.Position = new Vector3((screen.Width - results.Width)/2, (screen.Height - results.Height)/2, 0);
            results.Display = DisplayType.LINE_BY_LINE;
            results.Alignment = TextAlignment.CENTER;

            goManager.AddObject(titleScreen);
            goManager.AddObject(results);
            goManager.AddObject(_continue);
            goManager.AddObject(tryAgain);
        }

        void _continue_mouseClicked(Button btn)
        {
            monitoredState.Continue();
            gotoState = StatesIdList.RUNNER;
            DisableButtons();
        }

        void playAgain_mouseClicked(Button btn)
        {
            monitoredState.ShouldReset = true;
            gotoState = StatesIdList.RUNNER;
            DisableButtons();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            contentLoaded = true;
        }

        void titleScreen_mouseClicked(Button btn)
        {
            gotoState = StatesIdList.MAIN_MENU;
            DisableButtons();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (gotoState != StatesIdList.EMPTY_STATE && !exitingState)
                ExitState();
            if (exit)
            {
                ExitState();
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
                if (gotoState != StatesIdList.EMPTY_STATE)
                {
                    parent.ExitState(ID, (int)gotoState);
                }
                else
                {
                    parent.ExitState(ID);
                }
            }
        }

        public override void EnterState()
        {
            base.EnterState();
            Reset();
            if (!ContentLoaded)
                LoadContent();
        }

        private void Reset()
        {
            if (monitoredState.MistakesMade < monitoredState.AllowedMistakes)
            {
                _continue.Visible = true;
                _continue.Enable();

                tryAgain.Visible = false;
                tryAgain.Disable();
            }
            else
            {
                _continue.Visible = false;
                _continue.Disable();

                tryAgain.Visible = true;
                tryAgain.Enable();
            }
            EnableButtons();
            gotoState = StatesIdList.EMPTY_STATE;
            string txt = "";

            if (monitoredState.AnswersGot == monitoredState.NumberOfAnswers)
            {
                if(monitoredState.MistakesMade == 0)
                    txt = "PERFEITO!\n(Você acertou tudo de primeira, merece o dobro de pontos!)\n\n";
                
                int instanceScore = monitoredState.Score * monitoredState.PerfectSscoreMultiplier;

                txt += "Pontuação obtida: " + monitoredState.Score + " x " + monitoredState.PerfectSscoreMultiplier + " = " + instanceScore;

                txt += "\n";

                txt += "Pontuação acumulada: " + monitoredState.AccumulatedScore;

                txt += "\n";

                txt += "Pontuação total: " + instanceScore + " + " + monitoredState.AccumulatedScore + " = " + (instanceScore + monitoredState.AccumulatedScore);

            }
            else
            {
                txt = "Hmmm, dessa vez não deu. Que tal tentar de novo?\n\n";

                txt += "Respostas corretas obtidas: " + monitoredState.AnswersGot + " / " + monitoredState.NumberOfAnswers + "\n";

                txt += "Erros cometidos: " + monitoredState.MistakesMade;
            }

            results.Text = txt;
        }

        private void DisableButtons()
        {

            tryAgain.Disable();
            _continue.Disable();
            titleScreen.Disable();
        }

        private void EnableButtons()
        {

            tryAgain.Enable();
            _continue.Enable();
            titleScreen.Enable();
        }

    }
}
