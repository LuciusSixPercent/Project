using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using game_states;

namespace Project
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Dictionary<int, GameState> states;
        private List<GameState> statesStack;
        private StatesIdList querriedState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();

            IsMouseVisible = true;
            graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            querriedState = StatesIdList.EMPTY_STATE;

            states = new Dictionary<int, GameState>();

            statesStack = new List<GameState>();

            Menu menu = new Menu((int)StatesIdList.MAIN_MENU, this);
            states.Add(menu.ID, menu);

            Episodio01 md = new Episodio01((int)StatesIdList.STORY, this);
            states.Add(md.ID, md);

            RunnerState rs = new RunnerState((int)StatesIdList.RUNNER, this);
            states.Add(rs.ID, rs);

            RunnerWaitState rws = new RunnerWaitState((int)StatesIdList.RUNNER_WAIT, this);
            states.Add(rws.ID, rws);

            RunnerEndState res = new RunnerEndState((int)StatesIdList.RUNNER_END, this);
            states.Add(res.ID, res);

            CadernoDeAtividades cda = new CadernoDeAtividades((int)StatesIdList.OPTIONS, this, md);
            states.Add(cda.ID, cda);

            PauseState ps = new PauseState((int)StatesIdList.PAUSE, this);
            states.Add(ps.ID, ps);

            CharSelectionState css = new CharSelectionState((int)StatesIdList.CHAR_SELECTION, this);
            states.Add(css.ID, css);

            LoadingState ls = new LoadingState((int)StatesIdList.LOADING, this);
            states.Add(ls.ID, ls);


            EnterState(menu.ID);
        }

        public bool EnterState(int id)
        {
            if (states.ContainsKey(id))
            {
                statesStack.Add(states[id]);
                states[id].EnterState();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sai de um estado e imediatamente entra em outro.
        /// </summary>
        /// <param name="id">ID do estado do qual o jogo sairá.</param>
        /// <param name="id2">ID do estado no qual o jogo entrará.</param>
        public void ExitState(int id, int id2)
        {
            if (StateAlreadyOnStack(id2))
            {
                ExitState(id);
                querriedState = (StatesIdList)id2;
            }
            else
            {
                if (statesStack[statesStack.Count - 1].ID == id)
                {
                    statesStack.RemoveAt(statesStack.Count - 1);
                    EnterState(id2);
                }
            }
        }

        private bool StateAlreadyOnStack(int id2)
        {
            foreach (GameState state in statesStack)
                if (state.ID == id2)
                    return true;
            return false;
        }

        //sai do estado especificado
        /// <summary>
        /// Sai do estado especificado.
        /// </summary>
        /// <param name="id">ID do estado no qual o jogo entrará.</param>
        public void ExitState(int id)
        {
            //confirmamos que o ID passado bate com o ID do estado no topo de nossa pseudo pilha
            if (statesStack[statesStack.Count - 1].ID == id)
            {
                statesStack.RemoveAt(statesStack.Count - 1);
                if (statesStack.Count > 0)
                {
                    //se há um estado "abaixo" do que acabamos de sair, então entraremos nele
                    statesStack[statesStack.Count - 1].EnterState();
                }
            }
        }

        public GameState getState(int id)
        {
            if (states.ContainsKey(id))
            {
                return states[id];
            }
            return null;
        }

        protected override void LoadContent()
        {
            TextHelper.LoadDefaultFont(this.Content);
        }

        protected override void UnloadContent()
        {
        }

        //Update e Draw são os motivos pelos quais utilizamos uma List ao inves de uma Stack
        //Dessa forma podemos ter multiplos estados ativos simultaneamente se assim for necessário
        protected override void Update(GameTime gameTime)
        {
            //UpdateOrDraw(gameTime, false);
            UpdateStates(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            TextHelper.CacheQueued();
            DrawStates(gameTime);
            base.Draw(gameTime);
        }

        /// <summary>
        /// Atualiza os estados na pilha, de cima para baixo.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo.</param>
        private void UpdateStates(GameTime gameTime)
        {
            for (int stateIndex = statesStack.Count - 1; stateIndex >= 0; stateIndex--)
            {
                GameState state = statesStack[stateIndex];
                if (querriedState != StatesIdList.EMPTY_STATE)
                {
                    if (state.ID == (int)querriedState)
                    {
                        querriedState = StatesIdList.EMPTY_STATE;
                    }
                    else if (!state.ExitingState)
                    {
                        state.ExitState();
                    }
                }
                state.Update(gameTime);
                if (state.FreezeUpdatesBelow)
                    break;
            }
        }

        /// <summary>
        /// Desenha os estados na pilha, de baixo para cima.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo.</param>
        private void DrawStates(GameTime gameTime)
        {
            int stateIndex = statesStack.Count - 1;
            bool foundBottommostState = false;
            while (stateIndex < statesStack.Count)
            {
                if (statesStack[stateIndex].FreezeGraphicsBelow || stateIndex == 0)
                {
                    foundBottommostState = true;
                }

                if (foundBottommostState)
                {
                    statesStack[stateIndex].Draw(gameTime);
                    stateIndex++;
                }
                else
                {
                    stateIndex--;
                }
            }
        }
    }
}
