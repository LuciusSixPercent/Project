using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;
using Microsoft.Xna.Framework;
using game_objects.ui;
using System.IO;
using game_objects.questions;

namespace game_states
{

    enum SelectedCharacter
    {
        COSME,
        MARIA
    }

    public class CharSelectionState : GameState
    {
        private StatesIdList goToState;
        private AnimatedButton cosme;
        private AnimatedButton maria;
        private Button titleScreen;
        private Button play;
        private bool buttonsEnabled;

        private QuestionSubject[] chosenSubjects;
        private int chosenLevel;

        private SelectedCharacter selected;

        public CharSelectionState(int id, Game1 parent)
            : base(id, parent)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            enterTransitionDuration = 500;
            exitTransitionDuration = 100;
            goToState = StatesIdList.EMPTY_STATE;

            Rectangle screen = parent.GraphicsDevice.Viewport.Bounds;

            Rectangle bounds = new Rectangle(screen.Width / 2 - 250, screen.Height / 2 - 25, 200, 250);
            cosme = new AnimatedButton(goManager.R2D, bounds, new int[] { 1, 1, 1, 1 }, new bool[] { false, false, false, false });
            cosme.UseText = false;
            cosme.BaseFileName = "cosmeBtn";
            cosme.FilePath = "Menu" + Path.AltDirectorySeparatorChar + "Char_Selection" + Path.AltDirectorySeparatorChar;
            cosme.mouseClicked += new Button.MouseClicked(cosme_mouseClicked);
            cosme.LockOnClick = true;

            bounds = new Rectangle(screen.Width / 2 + 50, screen.Height / 2 - 25, 200, 250);
            maria = new AnimatedButton(goManager.R2D, bounds, new int[] { 1, 1, 1, 1 }, new bool[] { false, false, false, false });
            maria.UseText = false;
            maria.BaseFileName = "mariaBtn";
            maria.FilePath = "Menu" + Path.AltDirectorySeparatorChar + "Char_Selection" + Path.AltDirectorySeparatorChar;
            maria.mouseClicked += new Button.MouseClicked(maria_mouseClicked);
            maria.LockOnClick = true;

            bounds = new Rectangle(10, 10, 200, 50);
            titleScreen = new Button(goManager.R2D, bounds);
            titleScreen.BaseFileName = "menu_inicial";
            titleScreen.FilePath = "Menu" + Path.AltDirectorySeparatorChar + "Pause" + Path.AltDirectorySeparatorChar;
            titleScreen.UseText = false;
            titleScreen.mouseClicked += new Button.MouseClicked(titleScreen_mouseClicked);

            bounds = new Rectangle(screen.Width - 210, screen.Height-60, 200, 50);
            play = new Button(goManager.R2D, bounds);
            play.Text = "JOGAR";
            play.HoverColor = Color.Orange;
            play.PressColor = Color.DarkRed;
            //play.BaseFileName = "menu_inicial";
            //play.FilePath = "Menu" + Path.AltDirectorySeparatorChar + "Pause" + Path.AltDirectorySeparatorChar;
            play.UseText = true;
            play.mouseClicked += new Button.MouseClicked(play_mouseClicked);

            goManager.AddObject(cosme);
            goManager.AddObject(maria);
            goManager.AddObject(titleScreen);
            goManager.AddObject(play);

            chosenLevel = 0;
            chosenSubjects = new QuestionSubject[] { QuestionSubject.PT };

            DisableButtons();
        }

        void play_mouseClicked(Button btn)
        {
            if (parent.IsActive)
            {
                goToState = StatesIdList.RUNNER;
            }
        }

        void maria_mouseClicked(Button btn)
        {
            if (parent.IsActive)
            {
                selected = SelectedCharacter.MARIA;
                cosme.State = ButtonStates.NORMAL;
                cosme.Clicked = false;
            }
        }

        void cosme_mouseClicked(Button btn)
        {
            if (parent.IsActive)
            {
                selected = SelectedCharacter.COSME;
                maria.State = ButtonStates.NORMAL;
                maria.Clicked = false;
            }
        }

        void titleScreen_mouseClicked(Button btn)
        {
            if (parent.IsActive)
            {
                goToState = StatesIdList.MAIN_MENU;
                AudioManager.GetCue("cancel2").Play();
            }
        }

        public override void LoadContent()
        {
            if (!ContentLoaded)
            {
                base.LoadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (stateEntered)
            {
                if (!buttonsEnabled && goToState == StatesIdList.EMPTY_STATE)
                {
                    EnableButtons();
                }
                else if (goToState != StatesIdList.EMPTY_STATE)
                {
                    if (cosme.FinishedAnimation && maria.FinishedAnimation)
                        ExitState();
                }
            }
            if (exit)
            {
                ExitState();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void EnterState()
        {
            if (!exitingState)
            {
                base.EnterState();

                cosme.ForceClick();

                LoadContent();
            }
        }

        public override void ExitState()
        {
            if (!exit)
            {
                base.ExitState();
                DisableButtons();
            }
            else
            {
                if (goToState == StatesIdList.RUNNER)
                {
                    RunnerState rs = (RunnerState)parent.getState((int)goToState);
                    rs.NumberOfQuestions = 1;
                    rs.CharName = selected.ToString().ToLower();
                    rs.Level = (RunnerLevel)chosenLevel;
                    rs.Subjects = chosenSubjects;
                }
                parent.ExitState(ID, (int)goToState);
                goToState = StatesIdList.EMPTY_STATE;

            }
        }

        private void EnableButtons()
        {
            buttonsEnabled = true;
            titleScreen.Enable();
            play.Enable();
            cosme.Enable();
            maria.Enable();
        }

        private void DisableButtons()
        {
            titleScreen.Disable();
            play.Disable();
            cosme.Disable();
            maria.Disable();
            buttonsEnabled = false;
        }
    }
}
