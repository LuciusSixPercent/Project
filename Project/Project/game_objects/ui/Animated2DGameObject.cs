using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace game_objects.ui
{
    public class Animated2DGameObject : Scalable2DGameObject
    {
        private Texture2D[] frames;
        private int frameDelay;
        private int elapsed;
        private int currentFrame;
        private bool adaptToFrame;

        public bool AdaptToFrame
        {
            get { return adaptToFrame; }
            set { adaptToFrame = value; }
        }

        public int FrameCount
        {
            get { return frames.Length; }
        }

        /// <summary>
        /// Cria um objeto com um número qualquer de frames que vão alternando de acordo com o tempo especificado.
        /// </summary>
        /// <param name="renderer">O renderizador responsável por desenhar o frame atual.</param>
        /// <param name="framesPath">O caminho para as texturas que serão desenhadas.</param>
        /// <param name="basicFrameName">O nome padrão das texturas (sem sufixo algum). Uma instância com basicFrameName="teste" e com frameCount=3 tentará carregar os arquivos "teste1", "teste2" e "teste3".</param>
        /// <param name="frameCount">A quantidade de frames que a animação terá.</param>
        /// <param name="frameDelay">O tempo que cada frame permanecerá na tela.</param>
        public Animated2DGameObject(Renderer2D renderer, string framesPath, string basicFrameName, int frameCount, int frameDelay)
            : base(renderer)
        {
            TextureFilePath = framesPath;
            TextureFileName = basicFrameName;
            this.frameDelay = frameDelay;
            this.frames = new Texture2D[frameCount];
            this.currentFrame = 0;
            this.adaptToFrame = true;
        }

        public override void Load(ContentManager cManager)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = cManager.Load<Texture2D>(TextureFilePath + Path.AltDirectorySeparatorChar + TextureFileName + (i + 1));
            }
            texture = frames[0];

            Width = frames[0].Width;
            Height = frames[0].Height;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateFrame(gameTime);
        }

        private void UpdateFrame(GameTime gameTime)
        {
            if (frameDelay >= 0)
            {
                if (elapsed >= frameDelay)
                {
                    elapsed = 0;
                    AdvanceFrames();
                }
                else
                {
                    elapsed += gameTime.ElapsedGameTime.Milliseconds;
                }
            }
        }

        public void AdvanceFrames()
        {
            if (++currentFrame >= frames.Length)
            {
                currentFrame = 0;
            }
                texture = frames[currentFrame];
            Addapt();
        }

        public void SetFrame(int index)
        {
            if (index < frames.Length)
            {
                currentFrame = index;
                texture = frames[currentFrame];
                Addapt();
            }
        }

        private void Addapt()
        {
            if (adaptToFrame)
            {
                Width = frames[currentFrame].Width;
                Height = frames[currentFrame].Height;
            }
        }
    }
}
