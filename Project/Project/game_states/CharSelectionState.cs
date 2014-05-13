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
        private ToggleButton cosme;
        private ToggleButton maria;
        private Button titleScreen;
        private Button play;

        private TextBox difficultyLbl;
        private ToggleButton easy;
        private ToggleButton medium;
        private ToggleButton hard;

        private TextBox subjectLbl;
        private ToggleButton math;
        private ToggleButton pt;
        private ToggleButton both;

        private bool buttonsEnabled;

        private RunnerLevel chosenLevel;
        private int subjectChoice;
        private QuestionSubject[] chosenSubjects;

        private SelectedCharacter selected;
        private Microsoft.Xna.Framework.Audio.Cue bgm;

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

            difficultyLbl = new TextBox(goManager.R2D);
            difficultyLbl.Width = 300;
            difficultyLbl.Height = 40;
            difficultyLbl.X = 5;
            difficultyLbl.Y = 5;
            difficultyLbl.FontSize = 40;
            difficultyLbl.TextColor = Color.Beige;
            difficultyLbl.ShadowColor = Color.Beige * 0.5f;
            difficultyLbl.DropShadow = true;
            difficultyLbl.ShadowOffset = new Vector2(-4, -4);
            difficultyLbl.Text = "DIFICULDADE:";

            char separator = Path.AltDirectorySeparatorChar;
            string basePath = "Menu" + separator + "Char_Selection" + separator;

            Rectangle bounds = new Rectangle((int)(difficultyLbl.Y + difficultyLbl.Height + 10), (int)(difficultyLbl.Y + difficultyLbl.Height), 163, 76);

            easy = new ToggleButton(goManager.R2D, bounds);
            easy.UseText = false;
            easy.BaseFileName = "easyBtn";
            easy.FilePath = basePath + "difficulty" + separator;
            easy.mouseClicked += new Button.MouseClicked(difficultyBtn_mouseClicked);
            easy.LockToggleState = true;

            bounds.X += bounds.Width + 20;
            medium = new ToggleButton(goManager.R2D, bounds);
            medium.UseText = false;
            medium.BaseFileName = "mediumBtn";
            medium.FilePath = easy.FilePath;
            medium.mouseClicked += new Button.MouseClicked(difficultyBtn_mouseClicked);
            medium.LockToggleState = true;

            bounds.X += bounds.Width + 20;
            hard = new ToggleButton(goManager.R2D, bounds);
            hard.UseText = false;
            hard.BaseFileName = "hardBtn";
            hard.FilePath = easy.FilePath;
            hard.mouseClicked += new Button.MouseClicked(difficultyBtn_mouseClicked);
            hard.LockToggleState = true;

            subjectLbl = new TextBox(goManager.R2D);
            subjectLbl.Width = 180;
            subjectLbl.Height = 30;
            subjectLbl.X = 5;
            subjectLbl.Y = easy.Y + easy.Height + 40;
            subjectLbl.FontSize = 40;
            subjectLbl.TextColor = Color.Beige;
            subjectLbl.ShadowColor = Color.Beige * 0.5f;
            subjectLbl.DropShadow = true;
            subjectLbl.ShadowOffset = new Vector2(-4, -4);
            subjectLbl.Text = "MATÉRIA:";

            bounds.X = 50;
            bounds.Y = (int)(subjectLbl.Y + subjectLbl.Height + 10);
            math = new ToggleButton(goManager.R2D, bounds);
            math.UseText = false;
            math.BaseFileName = "mathBtn";
            math.FilePath = basePath + "subjects" + separator;
            math.mouseClicked += new Button.MouseClicked(subjectBtn_mouseClicked);
            math.LockToggleState = true;

            bounds.X += bounds.Width + 20;
            pt = new ToggleButton(goManager.R2D, bounds);
            pt.UseText = false;
            pt.BaseFileName = "ptBtn";
            pt.FilePath = basePath + "subjects" + separator;
            pt.mouseClicked += new Button.MouseClicked(subjectBtn_mouseClicked);
            pt.LockToggleState = true;

            bounds.X += bounds.Width + 20;
            both = new ToggleButton(goManager.R2D, bounds);
            both.UseText = false;
            both.BaseFileName = "ptMathBtn";
            both.FilePath = basePath + "subjects" + separator;
            both.mouseClicked += new Button.MouseClicked(subjectBtn_mouseClicked);
            both.LockToggleState = true;

            bounds = new Rectangle(screen.Width / 2 - 250, screen.Height / 2 - 25, 200, 250);
            cosme = new AnimatedButton(goManager.R2D, bounds, new int[] { 1, 1, 1, 1 }, new bool[] { false, false, false, false });
            //cosme = new ToggleButton(goManager.R2D, bounds);
            cosme.UseText = false;
            cosme.BaseFileName = "cosmeBtn";
            cosme.FilePath = basePath;
            cosme.mouseClicked += new Button.MouseClicked(cosme_mouseClicked);
            cosme.LockToggleState = true;

            bounds = new Rectangle(screen.Width / 2 + 50, screen.Height / 2 - 25, 200, 250);
            maria = new AnimatedButton(goManager.R2D, bounds, new int[] { 1, 1, 1, 1 }, new bool[] { false, false, false, false });
            //maria = new ToggleButton(goManager.R2D, bounds);
            maria.UseText = false;
            maria.BaseFileName = "mariaBtn";
            maria.FilePath = cosme.FilePath;
            maria.mouseClicked += new Button.MouseClicked(maria_mouseClicked);
            maria.LockToggleState = true;

            bounds = new Rectangle(screen.Width / 2 - 235, (int)maria.Bounds.Y + (int)maria.Bounds.Height + 50, 210, 70);
            titleScreen = new Button(goManager.R2D, bounds);
            titleScreen.BaseFileName = "menuInicialBtn";
            titleScreen.FilePath = "Menu" + separator + "Generic" + separator;
            titleScreen.UseText = false;
            titleScreen.mouseClicked += new Button.MouseClicked(titleScreen_mouseClicked);

            bounds = new Rectangle(screen.Width/2 + 25, (int)titleScreen.Bounds.Y, 210, 70);
            play = new Button(goManager.R2D, bounds);
            play.BaseFileName = "playBtn";
            play.FilePath = "Menu" + separator + "Char_Selection" + separator;
            play.mouseClicked += new Button.MouseClicked(play_mouseClicked);

            goManager.AddObject(difficultyLbl);
            goManager.AddObject(easy);
            goManager.AddObject(medium);
            goManager.AddObject(hard);
            goManager.AddObject(subjectLbl);
            goManager.AddObject(math);
            goManager.AddObject(pt);
            goManager.AddObject(both);
            goManager.AddObject(cosme);
            goManager.AddObject(maria);
            goManager.AddObject(titleScreen);
            goManager.AddObject(play);
            
            DisableButtons();
        }

        void subjectBtn_mouseClicked(Button btn)
        {
            if (btn != math)
                math.State = ButtonStates.NORMAL;
            else
                chosenSubjects = new QuestionSubject[] { QuestionSubject.MAT };

            if (btn != pt)
                pt.State = ButtonStates.NORMAL;
            else
                chosenSubjects = new QuestionSubject[] { QuestionSubject.PT };

            if (btn != both)
                both.State = ButtonStates.NORMAL;
            else
                chosenSubjects = new QuestionSubject[] { QuestionSubject.MAT, QuestionSubject.PT };
        }

        void difficultyBtn_mouseClicked(Button btn)
        {
            if (btn != easy)
                easy.State = ButtonStates.NORMAL;
            else
                chosenLevel = RunnerLevel.EASY;

            if (btn != medium)
                medium.State = ButtonStates.NORMAL;
            else 
                chosenLevel = RunnerLevel.MEDIUM;

            if (btn != hard)
                hard.State = ButtonStates.NORMAL;
            else 
                chosenLevel = RunnerLevel.HARD;
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
            }
        }

        void cosme_mouseClicked(Button btn)
        {
            if (parent.IsActive)
            {
                selected = SelectedCharacter.COSME;
                maria.State = ButtonStates.NORMAL;
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
                contentLoaded = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (stateEntered)
            {
                if (bgm == null || bgm.IsStopped)
                {
                    bgm = AudioManager.GetCue("soccer_life_97");
                    bgm.Play();
                }
                if (!buttonsEnabled && goToState == StatesIdList.EMPTY_STATE)
                {
                    EnableButtons();
                }
                else if (goToState != StatesIdList.EMPTY_STATE)
                {
                    if (cosme.State == ButtonStates.TOGGLED || maria.State == ButtonStates.TOGGLED)
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
                easy.ForceClick();
                math.ForceClick();
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
                    rs.CharName = selected.ToString().ToLower();
                    rs.Level = chosenLevel;
                    rs.Subjects = chosenSubjects;
                    rs.GoBackTo = StatesIdList.EMPTY_STATE;
                }

                bgm.Stop(Microsoft.Xna.Framework.Audio.AudioStopOptions.AsAuthored) ;
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
