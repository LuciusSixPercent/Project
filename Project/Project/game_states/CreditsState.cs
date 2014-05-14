using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;
using game_objects;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace game_states
{
    public class CreditsState : GameState
    {
        private const int MAX_SCROLL_SPEED = 5;
        private const int MIN_SCROLL_SPEED = 0;

        private Scalable2DGameObject credits;
        private int elapsed;
        private float scrollSpeed;
        private Cue bgm;

        public CreditsState(int id, Game1 parent) : base(id, parent) 
        {
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();

            enterTransitionDuration = 100;
            exitTransitionDuration = 300;

            BgColor = new Color(175, 78,78);

            char separator = Path.AltDirectorySeparatorChar;
            credits = new Scalable2DGameObject(goManager.R2D);
            credits.AdaptToFrame = true;
            credits.BaseFileName = "credits";
            credits.FilePath = "Imagem" + separator + "ui" + separator;
            scrollSpeed = 1;

            goManager.AddObject(credits);
        }

        public override void LoadContent()
        {
            if (!ContentLoaded)
            {
                base.LoadContent();
                ResetCredits();
                contentLoaded = true;
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
                    elapsed += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsed >= 30)
                    {
                        elapsed = 0;
                        credits.Y-= scrollSpeed;
                        CheckCreditsEnd();
                    }
                    HandleKeyPress();
                }
            }
            else if (exit)
            {
                ExitState();
            }
        }

        private void CheckCreditsEnd()
        {
            if (credits.Y <= -credits.Height)
            {
                ExitState();
            }
        }

        public override void EnterState()
        {
            if (!exitingState)
            {
                base.EnterState();
                LoadContent();
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
                ResetCredits();
                bgm.Stop(AudioStopOptions.AsAuthored);
            }
        }

        private void PlayBGM()
        {
            if (bgm == null || bgm.IsStopped)
            {
                bgm = AudioManager.GetCue("trickster");
                bgm.Play();
            }
        }

        private void ResetCredits()
        {
            credits.Y = parent.GraphicsDevice.Viewport.Height;
            scrollSpeed = 1;
        }

        private void HandleKeyPress()
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
            float speedIncrease = 0;
            if (KeyboardHelper.IsKeyDown(Keys.Down))
            {
                speedIncrease ++;
                KeyboardHelper.LockKey(Keys.Down);
            }
            else if (KeyboardHelper.KeyReleased(Keys.Down))
            {
                KeyboardHelper.UnlockKey(Keys.Down);
            }

            if (KeyboardHelper.IsKeyDown(Keys.Up))
            {
                speedIncrease--;
                KeyboardHelper.LockKey(Keys.Up);
            }
            else if (KeyboardHelper.KeyReleased(Keys.Up))
            {
                KeyboardHelper.UnlockKey(Keys.Up);
            }
            scrollSpeed += speedIncrease;
            if (scrollSpeed > MAX_SCROLL_SPEED) scrollSpeed = MAX_SCROLL_SPEED;
            else if (scrollSpeed < MIN_SCROLL_SPEED) scrollSpeed = MIN_SCROLL_SPEED;
        }
    }
}
