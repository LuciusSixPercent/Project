using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;
using game_objects.ui;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace game_states
{
    public class TutorialState : GameState
    {
        private Animated2DGameObject tutorialImages;
        private Button nextBtn;
        private bool finished;
        private bool playAudio;
        private Cue narrator;
        private string[] narratorLines;
        private bool started;
        int audioIndex = 0;
        private int elapsed;
        private bool frameSkipped;

        public TutorialState(int id, Game1 parent)
            : base(id, parent)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            enterTransitionDuration = 200;
            exitTransitionDuration = 500;

            char separator = Path.AltDirectorySeparatorChar;
            string path = "Imagem" + separator + "Tutorial" + separator;

            tutorialImages = new Animated2DGameObject(goManager.R2D, path, "credits", 5, -1);
            tutorialImages.AdaptToFrame = true;

            Viewport screen = parent.GraphicsDevice.Viewport;
            nextBtn = new Button(goManager.R2D, new Rectangle(screen.Width - 142, screen.Height - 142, 100, 100));
            nextBtn.mouseClicked += new Button.MouseClicked(nextBtn_mouseClicked);
            nextBtn.FilePath = path;
            nextBtn.BaseFileName = "next";

            narratorLines = new string[5] { "tut1a", "tut1b", "tut2", "tut3", "tut5" };

            goManager.AddObject(tutorialImages);
            goManager.AddObject(nextBtn);
        }

        void nextBtn_mouseClicked(Button btn)
        {
            AdvanceAudio();
        }

        private void AdvanceAudio()
        {
            if (!tutorialImages.Ended())
            {
                if (!started)
                {
                    started = true;
                }
                else
                {
                    audioIndex++;
                    if (audioIndex > 1)
                    {
                        tutorialImages.AdvanceFrames();
                        if (audioIndex == 3 && !frameSkipped)
                        {
                            tutorialImages.AdvanceFrames();
                            frameSkipped = true;
                        }
                    }
                }
                playAudio = true;
            }
            else
            {
                finished = true;
            }
        }
        private void AdvanceTutorial()
        {
            if (!tutorialImages.Ended())
            {
                if (!started)
                {
                    started = true;
                }
                else
                {
                    audioIndex++;
                    if (audioIndex > 1)
                        tutorialImages.AdvanceFrames();
                }
                playAudio = true;
            }
            else
            {
                finished = true;
            }
        }

        private void PlayNarrator()
        {
            
            if (narrator != null && narrator.IsPlaying)
            {
                narrator.Stop(AudioStopOptions.Immediate);
            }
            if (audioIndex < narratorLines.Length)
            {
                narrator = AudioManager.GetCue(narratorLines[audioIndex]);
                narrator.Play();
            }
        }

        public override void LoadContent()
        {

            if (!contentLoaded)
            {
                base.LoadContent();
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
                    if (!finished)
                    {
                        if (!nextBtn.Enabled)
                        {
                            nextBtn.Enable();
                        }
                        CheckAudio();
                        if (playAudio)
                        {
                            PlayNarrator();
                            playAudio = false;
                        }
                        if (audioIndex == 3 && !frameSkipped)
                        {
                            if (elapsed >= 500)
                            {
                                frameSkipped = true;
                                tutorialImages.AdvanceFrames();
                                elapsed = 0;
                            }
                            elapsed += gameTime.ElapsedGameTime.Milliseconds;
                        }
                    }
                    else
                    {
                        ExitState();
                    }
                }
            }
            else if (exit)
            {
                ExitState();
            }
        }

        private void CheckAudio()
        {
            if (narrator == null || narrator.IsStopped)
            {
                AdvanceTutorial();
            }
            
        }

        public override void EnterState()
        {
            if (!exitingState)
            {
                base.EnterState();
                LoadContent();
                playAudio = true;
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
                tutorialImages.CurrentFrame = 0;
                audioIndex = 0;
                parent.ExitState(ID);
                if (!narrator.IsStopped)
                {
                    narrator.Stop(AudioStopOptions.Immediate);
                }
                finished = false;
                started = false;
                frameSkipped = false;
            }
        }
    }
}
