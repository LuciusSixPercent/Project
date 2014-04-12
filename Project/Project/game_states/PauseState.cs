using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project;
using Microsoft.Xna.Framework.Input;

namespace game_states
{
    public class PauseState : GameState
    {
        public PauseState(int id, Game1 parent)
            : base(id, parent)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            enterTransitionDuration = 100;
            exitTransitionDuration = 50;
        }

        protected override void LoadContent()
        {
            
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
            else if (!enteringState)
            {
                parent.ExitState(ID);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            
        }
    }
}
