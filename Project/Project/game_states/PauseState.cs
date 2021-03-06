﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project;
using Microsoft.Xna.Framework.Input;
using game_objects;
using game_objects.ui;
using System.IO;

namespace game_states
{
    public class PauseState : GameState
    {        
        private Button resumeGame;
        private Button tutorial;
        private Button titleScreen;

        private bool toTile;

        public bool TutorialVisible {
            set
            {
                tutorial.Visible = value;
                if (value)
                    tutorial.Enable();
                else
                    tutorial.Disable();
            }
                
        }

        protected override float Alpha
        {
            get { return base.Alpha; }
            set
            {
                base.Alpha = value;
                goManager.R2D.Alpha = value;
            }
        }

        public PauseState(int id, Game1 parent)
            : base(id, parent)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            enterTransitionDuration = 100;
            exitTransitionDuration = 200;

            InitButtons();

            goManager.AddObject(resumeGame);
            goManager.AddObject(tutorial);
            goManager.AddObject(titleScreen);

            FreezeGraphicsBelow = false;
        }

        private void InitButtons()
        {
            int width = parent.GraphicsDevice.Viewport.Bounds.Width;
            int height = parent.GraphicsDevice.Viewport.Bounds.Height;
            int buttonsWidth = 180;
            int buttonsHeight = 60;
            char separator = Path.AltDirectorySeparatorChar; 
            resumeGame = CreateButton(new Rectangle(width/2 - buttonsWidth/2, height/2 - 2*buttonsHeight, buttonsWidth, buttonsHeight), "playBtn", "Menu"+separator+"Char_Selection"+separator);
            resumeGame.mouseClicked += new Button.MouseClicked(resumeGame_mouseClicked);

            titleScreen = CreateButton(new Rectangle(width / 2 - buttonsWidth / 2, height/2 + buttonsHeight, buttonsWidth, buttonsHeight), "menuInicialBtn", "Menu" + separator + "Generic" + separator);
            titleScreen.mouseClicked += new Button.MouseClicked(titleScreen_mouseClicked);

            tutorial = CreateButton(new Rectangle(width / 2 - buttonsWidth / 2, height / 2 - buttonsHeight/2, buttonsWidth, buttonsHeight), "tut", "Menu" + separator + "Pause" + separator);
            tutorial.Visible = false;
            tutorial.Disable();
            tutorial.mouseClicked += new Button.MouseClicked(tutorial_mouseClicked);
        }

        void tutorial_mouseClicked(Button btn)
        {
            parent.EnterState((int)StatesIdList.RUNNER_TUTORIAL);
        }

        void titleScreen_mouseClicked(Button btn)
        {
            if (parent.IsActive)
            {
                AudioManager.GetCue("cancel2").Play();
                toTile = true;
                ExitState();
            }
        }

        private void resumeGame_mouseClicked(Button btn)
        {
            if (parent.IsActive)
            {
                ExitState();
            }
        }

        private Button CreateButton(Rectangle bounds, string baseFileName, string filePath)
        {
            Button btn = new Button(goManager.R2D, bounds);
            btn.BaseFileName = baseFileName;
            btn.FilePath = filePath;
            btn.UseText = false;
            return btn;
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
                if (!exitingState)
                {
                    HandleKeyPress();
                    goManager.Update(gameTime);
                }
            }
            else if (exit)
            {
                ExitState();
            }
        }

        private void HandleKeyPress()
        {
            if (KeyboardHelper.IsKeyDown(Keys.Escape))
            {
                KeyboardHelper.LockKey(Keys.Escape);
                ExitState();
            }
            else if (KeyboardHelper.KeyReleased(Keys.Escape))
            {
                KeyboardHelper.UnlockKey(Keys.Escape);
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
                LoadContent();
                toTile = false;
                resumeGame.Enable();
                titleScreen.Enable();
            }
        }

        public override void ExitState()
        {
            if (!enteringState)
            {
                if (!exit)
                {
                    resumeGame.Disable();
                    titleScreen.Disable();
                    base.ExitState();
                }
                else
                {
                    if (toTile)
                    {
                        parent.ExitState(ID, (int)StatesIdList.MAIN_MENU);
                    }
                    else
                    {
                        parent.ExitState(ID);
                    }
                }
            }
        }
    }
}
