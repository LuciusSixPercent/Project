using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project;
using Microsoft.Xna.Framework.Graphics;

namespace game_states
{
    public abstract class GameState
    {
        private int id;
        protected Game1 parent;

        private SpriteBatch spriteBatch;

        protected bool initialized;

        #region Transition
        protected bool stateEntered;
        protected bool enteringState;
        protected bool exitingState;
        protected bool exit;

        protected int enterTransitionDuration;  //qual a duração total da transição de entrada no estado
        protected int exitTransitionDuration;   //qual a duração total da transição de saida no estado
        protected int transitionTime;           //quanto tempo a transição já durou

        private float alpha;

        protected virtual float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        private float alphaIncrement;
        #endregion

        private bool freezeBelow; //determina se os estados abaixo dele devem ser atualizados também

        public bool FreezeBelow
        {
            get { return freezeBelow; }
        }
        public int ID
        {
            get { return id; }
        }

        public bool Transitioning
        {
            get { return enteringState || exitingState; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        protected GameState(int id, Game1 parent)
        {
            this.id = id;
            this.parent = parent;
            this.stateEntered = false;
            freezeBelow = true;
        }

        public virtual void EnterState()
        {
            enteringState = true;
            alphaIncrement = (float)(1 - alpha) / enterTransitionDuration;
        }

        public virtual void EnterState(bool freezeBelow)
        {
            enteringState = true;
            this.freezeBelow = freezeBelow;
            alphaIncrement = (float)(1 - alpha) / enterTransitionDuration;
        }

        public virtual void ExitState()
        {
            exitingState = true;
            alphaIncrement = (alpha-1) * (float)transitionTime / exitTransitionDuration;
        }

        protected bool tryEndTransition(GameTime gameTime, bool transitionFlag, bool result)
        {
            if (transitionEnded(gameTime))
            {
                stateEntered = result;
                transitionTime = 0;
                Alpha = 1f;
                return false;
            }
            return true;
        }

        private bool transitionEnded(GameTime gameTime)
        {
            transitionTime += gameTime.ElapsedGameTime.Milliseconds;
            return transitionTime >= (enteringState ? enterTransitionDuration : exitTransitionDuration);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (enteringState)
            {
                enteringState = tryEndTransition(gameTime, enteringState, true);
            }
            else if (exitingState)
            {
                exitingState = tryEndTransition(gameTime, exitingState, false);
                exit = !exitingState;
            }
            if (Transitioning)
            {
                Alpha += alphaIncrement*gameTime.ElapsedGameTime.Milliseconds;
                if (Alpha < 0) Alpha = 0;
                else if (Alpha > 1) Alpha = 1;
            }
        }

        protected virtual void Initialize()
        {
            initialized = true;
            spriteBatch = new SpriteBatch(parent.GraphicsDevice);
        }
        protected abstract void LoadContent();
        public abstract void Draw(GameTime gameTime);
    }
}
