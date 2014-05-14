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
    public class EncerramentoDoEpisodio1 : GameState
    {

        Video video;
        VideoPlayer videoplayer;
        Texture2D TexturaDovideo;
        private bool beganPlaying;
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
                videoplayer = new VideoPlayer();
                videoplayer.IsLooped = false;
            }
        }
        public override void Update(GameTime tempo)
        {
            base.Update(tempo);
            if (stateEntered)
            {
                if (!exitingState)
                {
                    if (!beganPlaying)
                    {
                        videoplayer.Play(video);
                        beganPlaying = true;
                    }
                    HandleKeyPress();
                    if (videoplayer.PlayPosition == video.Duration)
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

        private void HandleKeyPress()
        {

            if (KeyboardHelper.IsKeyDown(Keys.Escape))
            {
                ExitState();
            }
            else if (KeyboardHelper.KeyReleased(Keys.Escape))
            {
                KeyboardHelper.UnlockKey(Keys.Escape);
            }
        }
        public override void Draw(GameTime gameTime)
        {

            if (videoplayer.State != MediaState.Stopped)
                TexturaDovideo = videoplayer.GetTexture();
            Rectangle screen = new Rectangle(parent.GraphicsDevice.Viewport.X, parent.GraphicsDevice.Viewport.Y, parent.GraphicsDevice.Viewport.Width, parent.GraphicsDevice.Viewport.Height);
            if (TexturaDovideo != null)
            {
                SpriteBatch.Begin();
                SpriteBatch.Draw(TexturaDovideo, screen, Color.White*Alpha);
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
                videoplayer.Stop();
                beganPlaying = false;
                parent.ExitState(ID);
            }
        }
    }
}
