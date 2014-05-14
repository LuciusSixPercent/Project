using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using game_states;
using Project;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Project.Modo_Historinha
{
    class EncerramentoDoEpisodio1:GameState
    {
        bool pauseFlag;
        Video video;
        VideoPlayer Videoplayer;
        Texture2D TexturaDovideo;
        public EncerramentoDoEpisodio1(int id, Game1 parent)
            : base(id, parent)
        {

            Initialize();
        }
        protected override void Initialize()
        {

            if (!initialized)
            {
                enterTransitionDuration = 200;
                exitTransitionDuration = 1000;
                base.Initialize();
                LoadContent();
                Videoplayer = new VideoPlayer();
            }
        }
        public override void Update(GameTime tempo)
        {
            base.Update(tempo);

            if (!pauseFlag)
            {
                if (stateEntered)
                {
                    if (Videoplayer.State == MediaState.Stopped)
                    {
                        Videoplayer.IsLooped = false;
                        Videoplayer.Play(video);
                    }
                    if (Videoplayer.State == MediaState.Paused)
                    {
                        Videoplayer.Resume();
                    }
                    if (KeyboardHelper.IsKeyDown(Keys.Escape))
                    {

                        KeyboardHelper.LockKey(Keys.Escape);
                        if (parent.EnterState((int)StatesIdList.PAUSE))
                        {
                            Alpha = 0.5f;
                            pauseFlag = true;
                            stateEntered = false;
                        }
                    }
                    else if (KeyboardHelper.KeyReleased(Keys.Escape))
                    {
                        KeyboardHelper.UnlockKey(Keys.Escape);
                    }
                    if (Videoplayer.PlayPosition == video.Duration)
                    {
                        ExitState();
                    }
                }
                else if (!exitingState)
                {
                    ExitState();
                }
            }
            else
            {
                Videoplayer.Pause();
            }
        }
        public override void Draw(GameTime gameTime)
        {
            
            if (Videoplayer.State != MediaState.Stopped)
                TexturaDovideo = Videoplayer.GetTexture();
            Rectangle screen = new Rectangle(parent.GraphicsDevice.Viewport.X, parent.GraphicsDevice.Viewport.Y, parent.GraphicsDevice.Viewport.Width, parent.GraphicsDevice.Viewport.Height);
            if (TexturaDovideo != null)
            {
                SpriteBatch.Begin();
                SpriteBatch.Draw(TexturaDovideo, screen, Color.White);
                SpriteBatch.End();
            }
        }
        public override void LoadContent()
        {
            if (!contentLoaded)
            {
                video = parent.Content.Load<Video>("Video/Encerramento");
                contentLoaded = true;
            }
        }
        #region Transitioning
        public override void EnterState()
        {
            if (!exitingState)
            {
                base.EnterState();
                LoadContent();
                pauseFlag = false;
            }
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
    }
}
