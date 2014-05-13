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

            titleScreen = new Button(goManager.R2D, new Rectangle(screen.Width / 2 + 25, (int)(screen.Height * 0.75), 200, 50));
            titleScreen.FilePath = "Menu" + separator + "Generic" + separator;
            titleScreen.BaseFileName = "menuInicialBtn";
            titleScreen.mouseClicked += new Button.MouseClicked(titleScreen_mouseClicked);

            tryAgain = new Button(goManager.R2D, new Rectangle(screen.Width / 2 - 225, (int)titleScreen.Position.Y, 200, 50));
            tryAgain.FilePath = "Imagem" + separator + "ui" + separator + "bate_bola" + separator + "fim" + separator;
            tryAgain.BaseFileName = "tryBtn";
            tryAgain.mouseClicked += new Button.MouseClicked(playAgain_mouseClicked);

            _continue = new Button(goManager.R2D, new Rectangle((int)tryAgain.Position.X, (int)tryAgain.Position.Y, (int)tryAgain.Dimensions.X, (int)tryAgain.Dimensions.Y));
            _continue.FilePath = tryAgain.FilePath;
            _continue.BaseFileName = "continueBtn";
            _continue.mouseClicked += new Button.MouseClicked(_continue_mouseClicked);

            results = new TextBox(goManager.R2D);
            results.Width = 500;
            results.Height = 300;
            results.Position = new Vector3((screen.Width - results.Width)/2, 300, 0);
            results.Display = DisplayType.LINE_BY_LINE;
            results.Alignment = TextAlignment.CENTER;

            goManager.AddObject(titleScreen);
            goManager.AddObject(results);
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
                goManager.AddObject(_continue);
                goManager.RemoveObject(tryAgain);
            }
            else
            {
                goManager.AddObject(tryAgain);
                goManager.RemoveObject(_continue);
            }
            EnableButtons();
            gotoState = StatesIdList.EMPTY_STATE;
            string txt = "";

            if (monitoredState.AnswersGot == monitoredState.NumberOfAnswers)
            {
                if(monitoredState.MistakesMade == 0)
                    txt = "PERFEITO! (Você acertou tudo de primeira, parabens!)\n";

                txt += "Pontuação total: " + monitoredState.Score + " x " + monitoredState.PerfectSscoreMultiplier + " = " + monitoredState.Score * monitoredState.PerfectSscoreMultiplier;

                txt += "\n";

                txt += "Erros: " + monitoredState.MistakesMade;
            }
            else
            {
                txt = "Hmmm, dessa vez não deu. Que tal tentar de novo?";
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
