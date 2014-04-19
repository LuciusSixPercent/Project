using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project;
using game_objects;
using game_objects.questions;

namespace game_states
{
    class RunnerState : GameState
    {
        #region Variables Declaration
        private RunnerLevel level;

        public RunnerLevel Level
        {
            get { return level; }
            set { level = value; }
        }

        int columns = 10;
        int rows = 54;

        private Camera cam;
        private Character player;
        private Stack<Question> questions;

        bool pauseFlag;

        #region Graphics
        private SpriteFont spriteFont;
        #endregion

        private bool contentLoaded;

        private GameObjectsManager goManager;

        #endregion

        protected override float Alpha
        {
            get
            {
                return base.Alpha;
            }
            set
            {
                base.Alpha = value;
                goManager.R3D.Alpha = goManager.R2D.Alpha = Alpha;
            }
        }

        public RunnerState(int id, Game1 parent)
            : base(id, parent)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            if (!initialized)
            {
                base.Initialize();

                enterTransitionDuration = 500;
                exitTransitionDuration = 250;

                goManager = new GameObjectsManager(parent.GraphicsDevice);                

                questions = new Stack<Question>();

                player = new Character(goManager, goManager.R3D, "Maria");                

                cam = new Camera(new Vector3(0f, 3f, -4f), Vector3.Up, new Vector2(0.25f, 50));
                cam.lookAt(new Vector3(0f, 0.25f, 2f), true);
                cam.createProjection(MathHelper.PiOver4, parent.GraphicsDevice.Viewport.AspectRatio);
                goManager.R3D.Cam = cam;
                goManager.AddObject(cam);

                goManager.AddObject(new Sky(goManager.R2D));
                goManager.AddObject(new Field(goManager.R3D, rows, columns));
                goManager.AddObject(player);

                level = RunnerLevel.EASY;
            }
        }

        protected override void LoadContent()
        {
            if (!contentLoaded)
            {
                contentLoaded = true;
                goManager.Load(parent.Content);
                player.Position = new Vector3(0f, 0.5f, 0f);
                spriteFont = parent.Content.Load<SpriteFont>("Fonte/Verdana");
            }
        }
        #region Transitioning
        public override void EnterState(bool freezeBelow)
        {
            if (!exitingState)
            {
                base.EnterState(freezeBelow);
                LoadContent();
                pauseFlag = false;
            }
        }
        public override void EnterState()
        {
            EnterState(FreezeBelow);
        }
        public override void ExitState()
        {
            if (!enteringState)
            {
                base.ExitState();
                parent.Content.Unload();
                contentLoaded = false;
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (!pauseFlag)
            {
                base.Update(gameTime);
                if (stateEntered)
                {
                    if (!exitingState)
                    {
                        goManager.Update(gameTime);

                        handleInput(gameTime);
                    }
                }
            }
        }

        private void handleInput(GameTime gameTime)
        {
            if (KeyboardHelper.IsKeyDown(Keys.Escape))
            {
                KeyboardHelper.LockKey(Keys.Escape);
                if (parent.EnterState((int)StatesIdList.PAUSE, false))
                {
                    Alpha = 0.5f;
                    goManager.R3D.Alpha = goManager.R2D.Alpha = Alpha;
                    pauseFlag = true;
                    stateEntered = false;
                }
            }
            else if (KeyboardHelper.KeyReleased(Keys.Escape))
            {
                KeyboardHelper.UnlockKey(Keys.Escape);
            }
        }

        #region DRAWING
        public override void Draw(GameTime gameTime)
        {
            goManager.Draw(gameTime);
        }

        #endregion
    }
}
