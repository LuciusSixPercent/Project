using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;
using game_objects.ui;
using System.IO;
using Microsoft.Xna.Framework;

namespace game_states
{
    public class LoadingState : GameState
    {
        private GameState overseeingState;

        private Animated2DGameObject ab;


        private int elapsed;

        public GameState OverseeingState
        {
            get { return overseeingState; }
            set { overseeingState = value; }
        }

        public LoadingState(int id, Game1 parent)
            : base(id, parent)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            ab = new Animated2DGameObject(goManager.R2D, "Menu" + Path.AltDirectorySeparatorChar + "Loading" + Path.AltDirectorySeparatorChar, "loading", 4, 250);
            goManager.AddObject(ab);
            enterTransitionDuration = 300;
            exitTransitionDuration = 100;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            contentLoaded = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (stateEntered)
            {
                if (!exitingState)
                {
                    if (elapsed >= 200)
                    {
                        elapsed = 0;
                        if (overseeingState == null || overseeingState.ContentLoaded)
                        {
                            ExitState();
                        }
                        else
                        {
                            overseeingState.LoadContent();
                        }
                    }
                    else
                    {
                        elapsed += gameTime.ElapsedGameTime.Milliseconds;
                    }
                }
            }
            else if (exit)
            {
                ExitState();
            }
        }

        public override void EnterState()
        {
            if (!exitingState)
            {
                base.EnterState();
                LoadContent();
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
            }
        }
    }
}
