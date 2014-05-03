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

        private bool freezeBelow; //determina se os estados abaixo dele devem ser atualizados também

        protected GameObjectsManager goManager;

        private bool contentLoaded;

        public bool ContentLoaded
        {
            get { return contentLoaded; }
        }

        public bool FreezeBelow
        {
            get { return freezeBelow; }
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
            freezeBelow = true;
        }

        public virtual void EnterState()
        {
            EnterState(freezeBelow);
        }

        public virtual void EnterState(bool freezeBelow)
        {
            enteringState = true;
            this.freezeBelow = freezeBelow;
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
            if (stateEntered)
            {
                goManager.Update(gameTime);
            }
        }

        protected virtual void Initialize()
        {
            initialized = true;
            spriteBatch = new SpriteBatch(parent.GraphicsDevice);
            goManager = new GameObjectsManager(parent.GraphicsDevice);
        }
        protected virtual void LoadContent()
        {
            contentLoaded = true;
            goManager.Load(parent.Content);
        }
        public virtual void Draw(GameTime gameTime)
        {
            goManager.Draw(gameTime);
        }
    }
}
