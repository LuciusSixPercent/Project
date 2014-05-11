using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project;
using Microsoft.Xna.Framework.Graphics;
using game_objects;

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
        private float alphaIncrement;
        #endregion
        
        private bool freezeUpdatesBelow;

        private bool freezeGraphicsBelow;

        protected GameObjectsManager goManager;

        protected bool contentLoaded;

        private bool justEnteredState;

        public bool ContentLoaded
        {
            get { return contentLoaded; }
        }

        public bool FreezeUpdatesBelow
        {
            get { return freezeUpdatesBelow; }
            set { freezeUpdatesBelow = value; }
        }

        public bool FreezeGraphicsBelow
        {
            get { return freezeGraphicsBelow; }
            set { freezeGraphicsBelow = value; }
        }

        public int ID
        {
            get { return id; }
        }

        public bool ExitingState
        {
            get { return exitingState; }
        }

        public bool Transitioning
        {
            get { return enteringState || exitingState; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        protected virtual float Alpha
        {
            get { return alpha; }
            set
            {
                alpha = value;
                goManager.R3D.Alpha = goManager.R2D.Alpha = Alpha;
            }
        }

        protected GameState(int id, Game1 parent)
        {
            this.id = id;
            this.parent = parent;
            this.stateEntered = false;
            freezeUpdatesBelow = true;
            freezeGraphicsBelow = true;
        }

        public virtual void EnterState()
        {
            enteringState = true;
            justEnteredState = true;
            exit = false;
            alphaIncrement = (float)1 / (enterTransitionDuration == 0 ? 1 : enterTransitionDuration);
        }

        public virtual void ExitState()
        {
            exitingState = true;
            enteringState = false;
            alphaIncrement = -alpha / (exitTransitionDuration == 0 ? 1 : exitTransitionDuration);
        }

        protected bool tryEndTransition(GameTime gameTime, bool transitionFlag, bool result)
        {
            if (transitionEnded(gameTime))
            {
                stateEntered = result;
                transitionTime = 0;
                return false;
            }
            return true;
        }

        private bool transitionEnded(GameTime gameTime)
        {
            transitionTime += gameTime.ElapsedGameTime.Milliseconds;
            return transitionTime >= (enteringState ? enterTransitionDuration : exitTransitionDuration);
        }

        /// <summary>
        /// Atualiza a transição do estado.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo</param>
        public virtual void Update(GameTime gameTime)
        {
            if (enteringState)
            {
                enteringState = tryEndTransition(gameTime, enteringState, true);
                if (!enteringState)
                    Alpha = 1f;
            }
            else if (exitingState)
            {
                exitingState = tryEndTransition(gameTime, exitingState, false);
                exit = !exitingState;
                if (exit)
                    Alpha = 0f;
            }
            if (Transitioning)
            {
                Alpha += alphaIncrement * gameTime.ElapsedGameTime.Milliseconds;
                if (Alpha < 0) Alpha = 0;
                else if (Alpha > 1) Alpha = 1;
            }
            if (stateEntered || justEnteredState)
            {
                justEnteredState = false;
                goManager.Update(gameTime);
            }
        }

        protected virtual void Initialize()
        {
            initialized = true;
            spriteBatch = new SpriteBatch(parent.GraphicsDevice);
            goManager = new GameObjectsManager(parent.GraphicsDevice);
        }
        public virtual void LoadContent()
        {
            if(!ContentLoaded)
                goManager.Load(parent.Content);
        }

        public virtual void Draw(GameTime gameTime)
        {
            if(ContentLoaded)
                goManager.Draw(gameTime);
        }
    }
}
