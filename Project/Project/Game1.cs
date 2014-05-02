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
        Menu menu;
        Episodio01 md;
        RunnerState rs;
        CadernoDeAtividades cda;

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

            states = new Dictionary<int, GameState>();

            statesStack = new List<GameState>();

            menu = new Menu((int)StatesIdList.MAIN_MENU, this);
            states.Add(menu.ID, menu);
            EnterState(menu.ID);

            md = new Episodio01((int)StatesIdList.STORY, this);
            states.Add(md.ID, md);

            rs = new RunnerState((int)StatesIdList.RUNNER, this);
            states.Add(rs.ID, rs);

            cda = new CadernoDeAtividades((int)StatesIdList.OPTIONS, this,md);
            states.Add(cda.ID, cda);

            PauseState ps = new PauseState((int)StatesIdList.PAUSE, this);
            states.Add(ps.ID, ps);
        }

        public bool EnterState(int id)
        {
            return EnterState(id, true);
        }

        public bool EnterState(int id, bool freezeBelow)
        {
            if (states.ContainsKey(id))
            {
                statesStack.Add(states[id]);
                states[id].EnterState(freezeBelow);
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
            if (statesStack[statesStack.Count - 1].ID == id)
            {
                statesStack.RemoveAt(statesStack.Count - 1);
                EnterState(id2);
            }
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
        }

        protected override void UnloadContent()
        {
        }

        //Update e Draw são os motivos pelos quais utilizamos uma List ao inves de uma Stack
        //Dessa forma podemos ter multiplos estados ativos simultaneamente se assim for necessário
        protected override void Update(GameTime gameTime)
        {
            UpdateOrDraw(gameTime, false);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            UpdateOrDraw(gameTime, true);
            base.Draw(gameTime);
        }

        /// <summary>
        /// Atualiza ou desenha o jogo.
        /// </summary>
        /// <param name="gameTime">Tempo do jogo.</param>
        /// <param name="methodFlag">Indica se o método deve desenhar (true) ou atualizar (false) os estados.</param>
        private void UpdateOrDraw(GameTime gameTime, bool methodFlag)
        {
            int stateIndex = statesStack.Count - 1;
            bool foundBottommostState = false;
            while (stateIndex < statesStack.Count)
            {
                if (statesStack[stateIndex].FreezeBelow == true)
                {
                    foundBottommostState = true;
                }

                if (foundBottommostState)
                {
                    if (methodFlag)
                    {
                        statesStack[stateIndex].Draw(gameTime);
                    }
                    else
                    {
                        statesStack[stateIndex].Update(gameTime);
                    }
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
