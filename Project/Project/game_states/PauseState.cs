using System;
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
        GameObjectsManager goManager;
        
        Button resumeGame;
        Button configMenu;
        Button titleScreen;

        bool toTile;

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
            goManager = new GameObjectsManager(parent.GraphicsDevice);

            InitButtons();

            goManager.AddObject(resumeGame);
            goManager.AddObject(configMenu);
            goManager.AddObject(titleScreen);

            FreezeGraphicsBelow = false;
        }

        private void InitButtons()
        {
            int width = parent.GraphicsDevice.Viewport.Bounds.Width;
            int height = parent.GraphicsDevice.Viewport.Bounds.Height;
            int buttonsWidth = 180;
            int buttonsHeight = 60;
            resumeGame = CreateButton(new Rectangle(width/2 - buttonsWidth/2, height/2 - 2*buttonsHeight, buttonsWidth, buttonsHeight), "voltar_ao_jogo");
            resumeGame.mouseClicked += new Button.MouseClicked(resumeGame_mouseClicked);

            configMenu = CreateButton(new Rectangle(width/2 - buttonsWidth/2, height/2 - buttonsHeight/2, buttonsWidth, buttonsHeight), "configuracoes");
            configMenu.mouseClicked += new Button.MouseClicked(optionsMenu_mouseClicked);

            titleScreen = CreateButton(new Rectangle(width / 2 - buttonsWidth / 2, height/2 + buttonsHeight, buttonsWidth, buttonsHeight), "menu_inicial");
            titleScreen.mouseClicked += new Button.MouseClicked(titleScreen_mouseClicked);
        }

        void titleScreen_mouseClicked(Button btn)
        {
            if (parent.IsActive)
            {
                toTile = true;
                ExitState();
            }
        }

        void optionsMenu_mouseClicked(Button btn)
        {
        }

        private void resumeGame_mouseClicked(Button btn)
        {
            ExitState();
        }

        private Button CreateButton(Rectangle bounds, string baseFileName)
        {
            Button btn = new Button(goManager.R2D, bounds);
            btn.BaseFileName = baseFileName;
            btn.FilePath = "Menu" + Path.AltDirectorySeparatorChar + "Pause" + Path.AltDirectorySeparatorChar;
            btn.UseText = false;
            return btn;
        }

        public override void LoadContent()
        {
            if (!ContentLoaded)
            {
                base.LoadContent();
                goManager.Load(parent.Content);
                initialized = true;
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
            goManager.Draw(gameTime);
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
                configMenu.Enable();
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
                    configMenu.Disable();
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
