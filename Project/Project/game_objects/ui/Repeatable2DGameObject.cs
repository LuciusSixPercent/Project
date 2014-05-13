using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace game_objects.ui
{
    public class Repeatable2DGameObject : Animated2DGameObject
    {
        private Vector2 repeatAmount;
        private Vector2 padding;
        private int[] currentFrames;

        public Vector2 RepeatAmount
        {
            get { return repeatAmount; }
            set { 
                repeatAmount = value;
                currentFrames = new int[(int)((1 + repeatAmount.X) * (1 + repeatAmount.Y))];
            }
        }

        public Vector2 Padding
        {
            get { return padding; }
            set { padding = value; }
        }


        public Repeatable2DGameObject(Renderer2D r2d, string framesPath, string basicFrameName, int frameCount, int frameDelay) : base(r2d, framesPath, basicFrameName, frameCount, frameDelay) 
        {
        }

        public override void Load(ContentManager cManager)
        {
            base.Load(cManager);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector3 actualPositon = position;
            for (int i = 0; i < repeatAmount.X; i++)
            {
                for (int j = 0; j < repeatAmount.Y; j++)
                {
                    SetFrame(currentFrames[(int)((i * repeatAmount.Y) + j)]);
                    base.Draw(gameTime);
                    Y += Height + padding.Y;
                }
                X += Width + padding.X;
                Y = actualPositon.Y;
            }
            position = actualPositon;
        }

        public void AdvanceCloneFrame(int cloneIndex)
        {
            if (cloneIndex < currentFrames.Length)
            {
                if(currentFrames[cloneIndex] < FrameCount)
                    currentFrames[cloneIndex]++;
                else
                    currentFrames[cloneIndex] = 0;
            }
        }

        public Vector3 GetClonePosition(int cloneIndex)
        {
            Vector3 pos = Vector3.Zero;
            if(cloneIndex < currentFrames.Length){
                pos.X = padding.X * ((cloneIndex+1) % currentFrames.Length) + Width * ((cloneIndex+1) % currentFrames.Length) + X;
                pos.Y = padding.Y * ((cloneIndex+1) / currentFrames.Length) + Height * ((cloneIndex+1) / currentFrames.Length) + Y;
            }
            return pos;
        }
    }
}
