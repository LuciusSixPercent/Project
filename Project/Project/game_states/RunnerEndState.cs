using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;
using game_objects.ui;
using Microsoft.Xna.Framework;
using System.IO;

namespace game_states
{
    public class RunnerEndState : GameState
    {

        private Button titleScreen;
        private TextBox results;
        private StatesIdList gotoState;

        public RunnerEndState(int id, Game1 parent)
            : base(id, parent)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();

            FreezeGraphicsBelow = false;
            enterTransitionDuration = 100;
            exitTransitionDuration = 300;

            titleScreen = new Button(goManager.R2D, new Rectangle(10, 10, 200, 50));
            titleScreen.FilePath = "Menu" + Path.AltDirectorySeparatorChar + "Pause" + Path.AltDirectorySeparatorChar;
            titleScreen.BaseFileName = "menu_inicial";
            titleScreen.mouseClicked += new Button.MouseClicked(titleScreen_mouseClicked);

            results = new TextBox(goManager.R2D, new Vector3(400, 300, 0));
            results.Position = new Vector3(300, 300, 0);
            results.Text = "testando\nquebra de linhas predefinidas\n\na partir de agora estarei testando a quebra de linhas automática, ou seja, desde que esta frase começou, nenhum caractere especial para quebra de linha foi adicionado a essa string, então, com sorte, esta linha estará quebrada e legível sem trabalho manual por minha parte.";

            goManager.AddObject(titleScreen);
            goManager.AddObject(results);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            contentLoaded = true;
        }

        void titleScreen_mouseClicked(Button btn)
        {
            gotoState = StatesIdList.MAIN_MENU;
            titleScreen.Disable();
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
            titleScreen.Enable();
            gotoState = StatesIdList.EMPTY_STATE;
            if (!ContentLoaded)
                LoadContent();
        }

    }
}
