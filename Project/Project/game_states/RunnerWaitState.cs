using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;
using Microsoft.Xna.Framework;
using game_objects;
using System.IO;
using game_objects.ui;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace game_states
{
    public class RunnerWaitState : GameState
    {
        private const int WAIT_TIME = 1000;
        private int elapsed;
        private int count;
        
        private Scalable2DGameObject clock;
        private Animated2DGameObject number;

        private Cue bgm;

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

            char separator = Path.AltDirectorySeparatorChar;

            clock = new Scalable2DGameObject(goManager.R2D);
            clock.AdaptToFrame = true;
            clock.BaseFileName = "clock";
            clock.FilePath = "Imagem" + separator + "ui" + separator + "bate_bola" + separator + "espera" + separator;

            number = new Animated2DGameObject(goManager.R2D, clock.FilePath, "num", 3, -1);
            number.AdaptToFrame = true;

            goManager.AddObject(clock);
            goManager.AddObject(number);

        }

        public override void LoadContent()
        {
            base.LoadContent();

            clock.X = (parent.GraphicsDevice.Viewport.Width - clock.Width) / 2;
            clock.Y = (parent.GraphicsDevice.Viewport.Height - clock.Height) / 2 - 30;
            number.CurrentFrame = (2);

            contentLoaded = true;
        }

        public override void EnterState()
        {
            base.EnterState();
            if (!ContentLoaded)
            {
                LoadContent();
            }
            else
            {
                number.RecedeFrames();
            }

            count = 0;
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
                    PlayBGM();
                    if (elapsed >= WAIT_TIME)
                    {
                        elapsed = 0;

                        if (count < 2)
                            number.RecedeFrames();

                        count++;
                    }
                    else
                    {
                        if (elapsed % 10 == 0)
                        {
                            number.Width *= 0.75f;
                            number.Height *= 0.75f;
                        }

                        elapsed += gameTime.ElapsedGameTime.Milliseconds;
                    }

                    Viewport screen = parent.GraphicsDevice.Viewport;
                    number.X = (screen.Width - number.Width) / 2;
                    number.Y = (screen.Height - number.Height) / 2;

                    if (count >= 3)
                    {
                        ExitState();
                    }
                }
            }
            else if (exit)
            {
                ExitState();
                bgm.Stop(AudioStopOptions.AsAuthored);
                AudioManager.GetCue("whistle").Play();
            }
        }

        private void PlayBGM()
        {
            if (bgm == null || bgm.IsStopped)
            {
                bgm = AudioManager.GetCue("soccer_life_97");
                bgm.Play();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
