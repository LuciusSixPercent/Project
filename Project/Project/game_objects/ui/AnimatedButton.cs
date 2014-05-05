using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace game_objects.ui
{
    public class AnimatedButton : Button
    {
        private int animationCycleStart;
        private int currentFrame;
        private int[] framesPerState;
        private bool[] loopStates;
        private ButtonStates lastState;
        private int elapsed;
        private bool clicked;
        private bool finishedAnimation;

        public bool FinishedAnimation
        {
            get { return finishedAnimation; }
        }

        public AnimatedButton(Renderer2D r2D, Rectangle bounds, int[] framesPerState, bool[] loopStates)
            : base(r2D, bounds)
        {
            if (framesPerState.Length != loopStates.Length)
                throw new ArgumentException("framesPerState necessita ter o mesmo número de elementos que loopStates");
            this.framesPerState = framesPerState;
            this.loopStates = loopStates;
            lastState = State;
        }

        protected override void cc_click()
        {
            if (Enabled)
            {
                if (!clicked)
                {
                    clicked = true;
                    ResetCycleStart(framesPerState.Length - 1);
                }
                base.cc_click();                
            }
        }

        public override void Load(ContentManager cManager)
        {
            int totalFrames = 0;
            foreach (int frames in framesPerState)
            {
                totalFrames += frames;
            }
            textures = new Texture2D[totalFrames];
            string[] suffixes = new string[] { "N", "H", "P", "C" };
            int suffixIndex = 0;
            int frameIndex = 0;
            foreach (int frames in framesPerState)
            {
                for (int i = 0; i < frames; i++)
                {
                    textures[frameIndex] = cManager.Load<Texture2D>(FilePath + BaseFileName + suffixes[suffixIndex] + (i + 1));
                    frameIndex++;
                }
                suffixIndex++;
            }
            CenterText();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateFrames(gameTime);
        }

        private void UpdateFrames(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsed >= 60)
            {
                if (Enabled || !finishedAnimation)
                {
                    elapsed = 0;
                    if (lastState.Equals(State) || clicked)
                    {
                        UpdateCurrentFrame();
                    }
                    else
                    {
                        lastState = State;
                        ResetCycleStart((int)State);
                    }
                }
                else
                {
                    animationCycleStart = 0;
                    currentFrame = animationCycleStart;
                }
            }
        }

        private void UpdateCurrentFrame()
        {
            currentFrame++;
            finishedAnimation = false;
            int index = clicked? framesPerState.Length-1 : (int)State;
            if (currentFrame >= framesPerState[index] + animationCycleStart)
            {
                finishedAnimation = true;
                lastState = State;
                if (loopStates[index])
                {
                    currentFrame = animationCycleStart;
                }
                else
                {
                    if (clicked)
                    {
                        clicked = false;
                        ResetCycleStart((int)State);
                    }
                    else
                    {
                        currentFrame--;
                    }
                }
            }
        }

        private void CheckCycleStart()
        {
            int frameCount = 0;
            bool ok = false;
            for (int i = 0; i < framesPerState.Length; i++)
            {
                if (animationCycleStart == frameCount)
                {
                    ok = true;
                    break;
                }
                frameCount += framesPerState[i];
            }
            if (!ok)
            {
                animationCycleStart = 0;
            }
        }

        private void ResetCycleStart(int state)
        {
            animationCycleStart = 0;

            for (int i = 0; i < state; i++)
            {
                animationCycleStart += framesPerState[i];
            }
            CheckCycleStart();
            currentFrame = animationCycleStart;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                ((Renderer2D)Renderer).Draw(textures[currentFrame], Bounds, BtnColor * ColorModifier, BlendState.Opaque);
                base.DrawText(gameTime);
            }
        }
    }
}
