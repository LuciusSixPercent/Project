using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using game_states;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Project.Modo_Historinha
{
    class CadernoDeAtividades:GameState
    {
        bool contentLoaded;
        SpriteFont arial;
        public CadernoDeAtividades(int id, Game1 parent)
            : base(id, parent)
        {
            Initialize();
        }
        protected override void Initialize()
        {

            if (!initialized)
            {
                LoadContent();
                base.Initialize();
            }
        }
        public override void Update(GameTime tempo)
        {
           base.Update(tempo);

            
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.End();
        }
        protected override void LoadContent()
        {
            if (!contentLoaded)
            {
                arial = parent.Content.Load<SpriteFont>("Fonte/Arial");
                contentLoaded = true;
            }

        }
        #region Transitioning
        public override void EnterState(bool freezeBelow)
        {
            if (!exitingState)
            {
                base.EnterState(freezeBelow);
                LoadContent();
                
            }
        }
        public override void EnterState()
        {
            base.EnterState();

            
        }
        public override void ExitState()
        {
            if (!enteringState)
            {
                base.ExitState();
                
            }
        }
        #endregion
    }
}
