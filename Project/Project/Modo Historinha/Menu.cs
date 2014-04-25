using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using game_states;
using Project;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;



    class Menu : GameState
    {
        private SpriteBatch spriteBatch;
        Texture2D seta, menu1, menu2, menu3, menu4, menu5, menu6, Fundo;
        Rectangle rcSeta, rcmenu1, rcmenu2, rcmenu3, rcmenu4, rcmenu5, rcmenu6, rcfundo;
        Vector2[] menus;
        bool pauseFlag;
        private bool contentLoaded;
        int escolha = 0;
        bool repetir = true;
        Song Inicio,select;
        bool bepe = false;
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue engineSound = null;
       
        public Menu(int id, Game1 parent)
            : base(id, parent)
        {

            Initialize();
        }
        protected override void Initialize()
        {

            if (!initialized)
            {

                base.Initialize();
                LoadContent();
                spriteBatch = new SpriteBatch(parent.GraphicsDevice);
                audioEngine = new AudioEngine("Content\\Audio\\MyGameAudio.xgs");
                waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
                soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");
                #region Inicializar Propriedades
                Historinha = false;
                BateBola = false;
                Caderno = false;
                Config = false;
                Creditos = false;
                Sair = false;
                #endregion
                #region Retangulos
                rcfundo = new Rectangle(0, 0, 1024, 768);
                rcmenu1 = new Rectangle(500, 300, menu1.Width/2, menu1.Height/2);
                rcmenu2 = new Rectangle(500, 370, menu2.Width/2, menu2.Height/2);
                rcmenu3 = new Rectangle(500, 430, menu3.Width/2, menu3.Height/2);
                rcmenu4 = new Rectangle(500, 520, menu4.Width/2, menu4.Height/2);
                rcmenu5 = new Rectangle(500, 590, menu5.Width/2, menu5.Height/2);
                rcmenu6 = new Rectangle(500, 650, menu6.Width/2, menu6.Height/2);
                menus = new Vector2[6] { new Vector2(rcmenu1.X-seta.Width, rcmenu1.Y), new Vector2(rcmenu2.X-seta.Width, rcmenu2.Y), new Vector2(rcmenu3.X-seta.Width, rcmenu3.Y), 
                new Vector2(rcmenu4.X-seta.Width, rcmenu4.Y), new Vector2(rcmenu5.X-seta.Width, rcmenu5.Y), new Vector2(rcmenu6.X-seta.Width, rcmenu6.Y) };
                rcSeta = new Rectangle((int)menus[escolha].X, (int)menus[escolha].Y, seta.Width, seta.Height);
                #endregion
                //resto da inicialização
            }
        }
        Keys lastKey = Keys.A;
        public override void Update(GameTime tempo)
        {
            if (!pauseFlag)
            {
                base.Update(tempo);
                if (repetir)
                {
                    
                    MediaPlayer.Play(Inicio);
                    repetir = false;
                }
                if (bepe)
                {
                    
                        //engineSound = soundBank.GetCue("magic-chime-07");
                        //engineSound.Play();
                        soundBank.PlayCue("magic-chime-07");
                        
                        bepe = false;
                   
                }
                if (KeyboardHelper.IsKeyDown(Keys.Escape))
                {
                    KeyboardHelper.LockKey(Keys.Escape);
                    if (parent.EnterState((int)StatesIdList.PAUSE, false))
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
                if (stateEntered)
                {
                    rcSeta = new Rectangle((int)menus[escolha].X, (int)menus[escolha].Y, seta.Width, seta.Height);
                    MouseState mouse = Mouse.GetState();
                    KeyboardState teclado = Keyboard.GetState();
                    if (Sair)
                    {
                        Alpha = 0.5f;
                        pauseFlag = true;
                        stateEntered = false;
                    }
                    if (Historinha)
                    {


                    }
                    #region Comandos Teclado
                    if((teclado.IsKeyDown(Keys.Space)) && (lastKey!= Keys.Space))
                    {
                        MediaPlayer.Volume -=0.1f;
                    }
                    if ((teclado.IsKeyDown(Keys.Down)) && (lastKey != Keys.Down))
                    {
                        escolha += escolha == 5 ? 0 : 1;
                        bepe = true;
                    }
                    if (teclado.IsKeyDown(Keys.Up) && lastKey != Keys.Up)
                    {
                        escolha -= escolha == 0 ? 0 : 1;
                        bepe = true;
                    }
                    if (PressionarTecla(Keys.Enter, teclado))
                    {
                        switch (escolha)
                        {
                            case 0:
                                Historinha = true;
                                break;
                            case 1:
                                BateBola = true;
                                break;
                            case 2:
                                Caderno = true;
                                break;
                            case 3:
                                Config = true;
                                break;
                            case 4:
                                Creditos = true;
                                break;
                            case 5:
                                Sair = true;
                                break;
                        }
                    }
                    Keys[] ks = teclado.GetPressedKeys();

                    if (ks.Length == 0) lastKey = Keys.A;
                    else lastKey = ks[0];
                    #endregion
                    #region Comandos Mouse
                    #region Over
                    if (ColisaoMouseOver(mouse, menu1, rcmenu1))
                    {
                        escolha = 0;
                    }
                    if (ColisaoMouseOver(mouse, menu2, rcmenu2))
                    {
                        escolha = 1;
                    }
                    if (ColisaoMouseOver(mouse, menu3, rcmenu3))
                    {
                        escolha = 2;
                    }
                    if (ColisaoMouseOver(mouse, menu4, rcmenu4))
                    {
                        escolha = 3;
                    }
                    if (ColisaoMouseOver(mouse, menu5, rcmenu5))
                    {
                        escolha = 4;
                    }
                    if (ColisaoMouseOver(mouse, menu6, rcmenu6))
                    {
                        escolha = 5;
                    }
                    #endregion
                    #region Click
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        if (ColisaoMouseOver(mouse, menu1, rcmenu1))
                        {
                            Historinha = true;
                        }
                        if (ColisaoMouseOver(mouse, menu2, rcmenu2))
                        {
                            BateBola = true;
                        }
                        if (ColisaoMouseOver(mouse, menu3, rcmenu3))
                        {
                            Caderno = true;
                        }
                        if (ColisaoMouseOver(mouse, menu4, rcmenu4))
                        {
                            Config = true;
                        }
                        if (ColisaoMouseOver(mouse, menu5, rcmenu5))
                        {
                            Creditos = true;
                        }
                        if (ColisaoMouseOver(mouse, menu6, rcmenu6))
                        {
                            Sair = true;
                        }

                    }
                    #endregion

                    if (Historinha)
                    {
                        MediaPlayer.Stop();
                        Historinha = false;
                        parent.EnterState((int)StatesIdList.STORY);
                    } else
                    if (BateBola)
                    {
                        MediaPlayer.Stop();
                        BateBola = false;
                        parent.EnterState((int)StatesIdList.RUNNER);
                    }
                    #endregion
                }
            }
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Fundo, rcfundo, Color.White * Alpha);
            spriteBatch.Draw(seta, rcSeta, Color.White * Alpha);
            spriteBatch.Draw(menu1, rcmenu1, Color.White * Alpha);
            spriteBatch.Draw(menu2, rcmenu2, Color.White * Alpha);
            spriteBatch.Draw(menu3, rcmenu3, Color.White * Alpha);
            spriteBatch.Draw(menu4, rcmenu4, Color.White * Alpha);
            spriteBatch.Draw(menu5, rcmenu5, Color.White * Alpha);
            spriteBatch.Draw(menu6, rcmenu6, Color.White * Alpha);
            spriteBatch.End();
        }
        public bool Sair { get; set; }
        public bool Historinha { get; set; }
        public bool BateBola { get; set; }
        public bool Config { get; set; }
        public bool Creditos { get; set; }
        public bool Caderno { get; set; }
        public bool ColisaoMouseOver(MouseState mouse, Texture2D textura, Rectangle rec)
        {
            if ((mouse.X > rec.X && mouse.X < rec.X + rec.Width) && (mouse.Y > rec.Y && mouse.Y < rec.Y + rec.Height))
            {

                return true;
            }
            
            return false;
        }
        public bool PressionarTecla(Keys tecla, KeyboardState teclado)
        {
            Keys lastKey = Keys.A;
            if (teclado.IsKeyDown(tecla) && lastKey != tecla)
            {
                return true;
            }
            Keys[] ks = teclado.GetPressedKeys();

            if (ks.Length == 0) lastKey = Keys.A;
            else lastKey = ks[0];
            return false;
        }
        
        protected override void LoadContent()
        {
            if (!contentLoaded)
            {
                #region Inicializar Variaveis
                #region Texturas
                Fundo = parent.Content.Load<Texture2D>("Menu/Abertura");
                seta = parent.Content.Load<Texture2D>("Menu/Seta");
                menu1 = parent.Content.Load<Texture2D>("Menu/historinhaN");
                menu2 = parent.Content.Load<Texture2D>("Menu/batebola_N");
                menu3 = parent.Content.Load<Texture2D>("Menu/cadernoN");
                menu4 = parent.Content.Load<Texture2D>("Menu/configuracoesN");
                menu5 = parent.Content.Load<Texture2D>("Menu/creditosN");
                menu6 = parent.Content.Load<Texture2D>("Menu/sairN");
                #endregion

                #endregion
                //select = parent.Content.Load<Song>("Audio/magic-chime-07");
                Inicio = parent.Content.Load<Song>("Narrar/TelaInicial");
                contentLoaded = true;
            }
            
           
        }
        
        #region Transitioning
        public override void EnterState(bool freezeBelow)
        {
            if (!exitingState)
            {
                base.EnterState(freezeBelow);
                LoadContent();
                pauseFlag = false;
            }
        }
        public override void EnterState()
        {
            base.EnterState();
            
            pauseFlag = false;
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

