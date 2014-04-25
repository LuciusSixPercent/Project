using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project;
using Microsoft.Xna.Framework.Input;
using game_objects;

namespace game_states
{
    public class PauseState : GameState
    {
        GameObjectsManager goManager;

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
        }

        protected override void LoadContent()
        {
            if (!initialized)
            {
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
            }
            else if (exit)
            {
                ExitState();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            goManager.Draw(gameTime);
        }

        public override void ExitState()
        {
            if (!enteringState)
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
}
