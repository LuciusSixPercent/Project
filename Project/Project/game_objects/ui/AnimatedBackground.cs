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
    public class AnimatedBackground : DrawableGameObject
    {
        private Texture2D[] frames;
        private int frameDelay = 500;
        private int elapsed;
        private int currentFrame;
        private string framesPath;
        private string basicFrameName;
        private Renderer2D renderer2D;
        private string p;
        private string p_2;
        private int p_3;
        private int p_4;

        /// <summary>
        /// Cria um objeto com um número qualquer de frames que vão alternando de acordo com o tempo especificado.
        /// </summary>
        /// <param name="renderer">O renderizador responsável por desenhar o frame atual.</param>
        /// <param name="framesPath">O caminho para as texturas que serão desenhadas.</param>
        /// <param name="basicFrameName">O nome padrão das texturas (sem sufixo algum). Uma instância com basicFrameName="teste" e com frameCount=3 tentará carregar os arquivos "teste1", "teste2" e "teste3".</param>
        /// <param name="frameCount">A quantidade de frames que a animação terá.</param>
        /// <param name="frameDelay">O tempo que cada frame permanecerá na tela.</param>
        public AnimatedBackground(Renderer2D renderer, string framesPath, string basicFrameName, int frameCount, int frameDelay)
            : base(renderer)
        {
            this.framesPath = framesPath;
            this.frameDelay = frameDelay;
            this.basicFrameName = basicFrameName;
            this.frames = new Texture2D[frameCount];
            this.currentFrame = 0;
        }

        public override void Load(ContentManager cManager)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = cManager.Load<Texture2D>(framesPath + Path.AltDirectorySeparatorChar + basicFrameName + (i + 1));
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateFrame(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ((Renderer2D)Renderer).Draw(frames[currentFrame], Vector2.Zero, Color.White, BlendState.Opaque);
        }

        private void UpdateFrame(GameTime gameTime)
        {
            if (elapsed >= frameDelay)
            {
                elapsed = 0;
                if(++currentFrame >= frames.Length)
                    currentFrame = 0;
            }
            else
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;
            }
        }
    }
}
