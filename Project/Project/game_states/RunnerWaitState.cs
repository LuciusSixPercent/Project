using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;
using Microsoft.Xna.Framework;

namespace game_states
{
    public class RunnerWaitState : GameState
    {
        private int elapsed;
        private const int WAIT_TIME = 3000;

        public RunnerWaitState(int id, Game1 parent)
            : base(id, parent)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();

            FreezeUpdatesBelow = true;
            FreezeGraphicsBelow = false;
        }

        public override void EnterState()
        {
            base.EnterState();
            if (!ContentLoaded)
            {
                LoadContent();
                elapsed = 0;
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (stateEntered)
            {
                if (!exitingState)
                {
                    if (elapsed >= WAIT_TIME)
                    {
                        ExitState();
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

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
