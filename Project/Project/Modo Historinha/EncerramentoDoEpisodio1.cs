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
using Microsoft.Xna.Framework.Audio;

namespace Project.Modo_Historinha
{
    public class EncerramentoDoEpisodio1 : GameState
    {

        Video video;
        VideoPlayer videoplayer;
        Texture2D TexturaDovideo;
        private bool beganPlaying;
        Texture2D Eduardo,BTxO,BTxN;
        Texture2D[] BTX;
        Rectangle vbtx;
        AudioEngine audioEngine2;
        WaveBank waveBank2;
        SoundBank soundBank2;
        Cue engineSound = null;
        int indiceDoBotão = 0;
        bool acabou = false;
        float edu = 0;
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
                audioEngine2 = new AudioEngine("Content\\Audio\\MyGameAudio2.xgs");
                waveBank2 = new WaveBank(audioEngine2, "Content\\Audio\\Wave Bank2.xwb");
                soundBank2 = new SoundBank(audioEngine2, "Content\\Audio\\Sound Bank2.xsb");
                BTX = new Texture2D[2] { BTxN, BTxO };
                vbtx = new Rectangle(950, 0, BTX[indiceDoBotão].Width / 6, BTX[indiceDoBotão].Height / 6);
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
                        acabou = true;
                    }
                    if (acabou)
                    {
                        if (engineSound == null)
                        {
                            engineSound = soundBank2.GetCue("EduardoFala");
                            engineSound.Play();
                        }
                        edu += (edu < 1 ? 0.1f : 0);
                        MouseState mouse = Mouse.GetState();
                        if (ColisaoMouseOver(mouse, vbtx))
                        {
                            indiceDoBotão = 1;
                        }
                        else { indiceDoBotão = 0; }
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            if (ColisaoMouseOver(mouse, vbtx))
                            {
                                parent.Exit();
                            }
                        }
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
            if (!acabou)
            {
                if (videoplayer.State != MediaState.Stopped)
                    TexturaDovideo = videoplayer.GetTexture();
                Rectangle screen = new Rectangle(0, 0, 1024, 768);
                if (TexturaDovideo != null)
                {
                    SpriteBatch.Begin();
                    SpriteBatch.Draw(TexturaDovideo, screen, Color.White * Alpha);
                    SpriteBatch.End();
                }
            }
            else
            {
                SpriteBatch.Begin();
                SpriteBatch.Draw(Eduardo, new Vector2(0,0), (Color.White * Alpha)*edu);
                SpriteBatch.Draw(BTX[indiceDoBotão], vbtx, (Color.White*Alpha)*edu);
                SpriteBatch.End();
            }
        }
        public override void LoadContent()
        {
            if (!contentLoaded)
            {
                video = parent.Content.Load<Video>("Video/Encerramento");
                Eduardo = parent.Content.Load<Texture2D>("Imagem/Personagem/eduardo");
                BTxN = parent.Content.Load<Texture2D>("Imagem/ui/historinha/Botao_X_menor");
                BTxO = parent.Content.Load<Texture2D>("Imagem/ui/historinha/Botao_X");
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
        public bool ColisaoMouseOver(MouseState mouse, Rectangle rec)
        {
            if ((mouse.X > rec.X && mouse.X < rec.X + rec.Width) && (mouse.Y > rec.Y && mouse.Y < rec.Y + rec.Height))
            {

                return true;
            }

            return false;
        }
    }
}
