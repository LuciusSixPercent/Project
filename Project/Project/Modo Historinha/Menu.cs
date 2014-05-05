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
        Texture2D seta, menu1Normal, menu1Over, menu2Normal, menu2Over, menu3Normal,menu3Over, menu4Normal,menu4Over, menu5Normal,menu5Over, menu6Normal,menu6Over, Fundo,OKOver,OKNormal,CancelarOver,CancelarNormal,Barra,Medidor,VoltarOver, VoltarNormal;
        Texture2D[] menu1, menu2, menu3, menu4, menu5, menu6, OK, Cancelar, Voltar;
        Texture2D Fconf, Fnorm, Fcre, Fsair;
        Rectangle rcSeta, rcmenu1, rcmenu2, rcmenu3, rcmenu4, rcmenu5, rcmenu6, rcfundo, rcOK, rcCancelar, rcbarra, rcMedidor, vVoltar;
        Vector2[] menus;
        bool pauseFlag;
        int escolha = 0;
        bool repetir = true;
        Song Inicio,select;
        bool bepe = false;
        int mn1 = 0;
        int mn2 = 0;
        int mn3 = 0;
        int mn4 = 0;
        int mn5 = 0;
        int mn6 = 0;
        int OKin = 0;
        int CancIN = 0;
        int VoltarIndice = 0;
        SpriteFont arial;
        AudioEngine audioEngine, audioEngine3;
        WaveBank waveBank, waveBank3;
        SoundBank soundBank, soundBank3;
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
                audioEngine3 = new AudioEngine("Content\\Audio\\MyGameAudio.xgs");
                waveBank3 = new WaveBank(audioEngine3, "Content\\Audio\\Wave Bank.xwb");
                soundBank3 = new SoundBank(audioEngine3, "Content\\Audio\\Sound Bank.xsb");
                #region Inicializar Propriedades
                Historinha = false;
                BateBola = false;
                Caderno = false;
                Config = false;
                Creditos = false;
                Sair = false;
                #endregion
                menu1 = new Texture2D[2] {menu1Normal,menu1Over };
                menu2 = new Texture2D[2] { menu2Normal, menu2Over };
                menu3 = new Texture2D[2] { menu3Normal, menu3Over };
                //menu4 = new Texture2D[2] { menu4Normal, menu4Over };
                menu5 = new Texture2D[2] { menu5Normal, menu5Over };
                menu6 = new Texture2D[2] { menu6Normal, menu6Over };
                OK = new Texture2D[2] { OKNormal, OKOver };
                Cancelar = new Texture2D[2] { CancelarNormal, CancelarOver };
                Voltar = new Texture2D[2] { VoltarNormal, VoltarOver };
                #region Retangulos
                rcfundo = new Rectangle(0, 0, 1024, 768);
                rcmenu1 = new Rectangle(0, 600, menu1[mn1].Width , menu1[mn1].Height);
                rcmenu2 = new Rectangle(150, 600, menu2[mn2].Width , menu2[mn2].Height);
                rcmenu3 = new Rectangle(300, 600, menu3[mn3].Width, menu3[mn3].Height);
                //rcmenu4 = new Rectangle(450, 600, menu4[mn4].Width / 2, menu4[mn4].Height / 2);
                rcmenu5 = new Rectangle(450, 600, menu5[mn5].Width, menu5[mn5].Height);
                rcmenu6 = new Rectangle(600, 600, menu6[mn6].Width, menu6[mn6].Height);
                rcOK = new Rectangle(300, 350, OK[OKin].Width / 2, OK[OKin].Height / 2);
                rcCancelar = new Rectangle(550, 350, Cancelar[CancIN].Width / 2, Cancelar[CancIN].Height / 2);
                rcbarra = new Rectangle(120, 350, Barra.Width, Barra.Height*2);
                vVoltar = new Rectangle(850, 700, Voltar[VoltarIndice].Width / 2, Voltar[VoltarIndice].Height / 2);
                rcMedidor = new Rectangle(rcbarra.X + rcbarra.Width, rcbarra.Y - Medidor.Height/2, Medidor.Width*3, Medidor.Height);
                /*menus = new Vector2[6] { new Vector2(rcmenu1.X-seta.Width, rcmenu1.Y), new Vector2(rcmenu2.X-seta.Width, rcmenu2.Y), new Vector2(rcmenu3.X-seta.Width, rcmenu3.Y), 
                new Vector2(rcmenu4.X-seta.Width, rcmenu4.Y), new Vector2(rcmenu5.X-seta.Width, rcmenu5.Y), new Vector2(rcmenu6.X-seta.Width, rcmenu6.Y) };
                rcSeta = new Rectangle((int)menus[escolha].X, (int)menus[escolha].Y, seta.Width, seta.Height);*/
                #endregion
                //resto da inicialização
            }
        }
        Keys lastKey = Keys.A;
        public override void Update(GameTime tempo)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState teclado = Keyboard.GetState();
            MediaPlayer.Volume = (rcMedidor.X / (rcbarra.X + rcbarra.Width));
            if (engineSound == null)
            {
                engineSound = soundBank3.GetCue("Silly Fun");
                engineSound.Play();
            }
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
                if (stateEntered)
                {

                    if (engineSound.IsStopped)
                    {
                        engineSound = soundBank3.GetCue("Silly Fun");
                        engineSound.Play();
                    }
                    //rcSeta = new Rectangle((int)menus[escolha].X, (int)menus[escolha].Y, seta.Width, seta.Height);
                    
                    
                    if (Sair)
                    {
                        
                        Alpha = 0.5f;
                        if (ColisaoMouseOver(mouse, OK[OKin], rcOK))
                        {
                            OKin = 1;
                        }
                        else { OKin = 0; }
                        if (ColisaoMouseOver(mouse, Cancelar[CancIN], rcCancelar))
                        {
                            CancIN = 1;
                        }
                        else { CancIN = 0; }
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            if (ColisaoMouseOver(mouse, OK[OKin], rcOK))
                            {
                                parent.Exit();
                            }
                            if (ColisaoMouseOver(mouse, Cancelar[CancIN], rcCancelar))
                            {
                                Alpha = 1;
                                Sair = false;
                            }
                        }

                    }
                    if (Historinha)
                    {


                    }
                    if (Creditos)
                    {
                        CreditosMENU();
                        if (ColisaoMouseOver(mouse, Voltar[VoltarIndice], vVoltar))
                        {
                            VoltarIndice = 1;
                        }
                        else { VoltarIndice = 0; }
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            if (ColisaoMouseOver(mouse, Voltar[VoltarIndice], vVoltar))
                            {
                                Creditos = false;
                                
                            }
                        }
                    }
                    if (Config)
                    {
                        ConfiguracoesMENU();
                        if (ColisaoMouseOver(mouse, Voltar[VoltarIndice], vVoltar))
                        {
                            VoltarIndice = 1;
                        }
                        else { VoltarIndice = 0; }
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            if (ColisaoMouseOver(mouse, Voltar[VoltarIndice], vVoltar))
                            {
                                Config = false;

                            }
                            if (ColisaoMouseOver(mouse, Medidor, rcMedidor))
                            {
                                rcMedidor.X = mouse.X- rcMedidor.Width/2;
                                if (mouse.X > rcbarra.X + rcbarra.Width +1)
                                {
                                    rcMedidor.X = rcbarra.X + rcbarra.Width;
                                }
                                if (mouse.X < rcbarra.X)
                                {
                                    rcMedidor.X = rcbarra.X;
                                }
                            }
                            
                        }
                        
                    }
                    if (!Historinha && !BateBola && !Caderno && !Config && !Creditos && !Sair)
                    {
                        
                        #region Comandos Teclado
                        /*
                        if ((teclado.IsKeyDown(Keys.Space)) && (lastKey != Keys.Space))
                        {
                            MediaPlayer.Volume -= 0.1f;
                        }
                        if ((teclado.IsKeyDown(Keys.Down)) && (lastKey != Keys.Down))
                        {
                            escolha += escolha == 5 ? 0 : 1;

                        }
                        if (teclado.IsKeyDown(Keys.Up) && lastKey != Keys.Up)
                        {
                            escolha -= escolha == 0 ? 0 : 1;

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
                         * */
                        #endregion
                        #region Comandos Mouse
                        #region Over
                        if (ColisaoMouseOver(mouse, menu1[mn1], rcmenu1))
                        {
                            escolha = 0;
                            mn1 = 1;
                        }
                        else { mn1 = 0; }
                        if (ColisaoMouseOver(mouse, menu2[mn2], rcmenu2))
                        {
                            escolha = 1;
                            mn2 = 1;
                        }
                        else { mn2 = 0; }
                        if (ColisaoMouseOver(mouse, menu3[mn3], rcmenu3))
                        {
                            escolha = 2;
                            mn3 = 1;
                        }
                        else { mn3 = 0; }
                        //if (ColisaoMouseOver(mouse, menu4[mn4], rcmenu4))
                        //{
                        //    escolha = 3;
                        //    mn4 = 1;
                        //}
                        //else { mn4 = 0; }
                        if (ColisaoMouseOver(mouse, menu5[mn5], rcmenu5))
                        {
                            escolha = 4;
                            mn5 = 1;
                        }
                        else { mn5 = 0; }
                        if (ColisaoMouseOver(mouse, menu6[mn6], rcmenu6))
                        {
                            escolha = 5;
                            mn6 = 1;
                        }
                        else { mn6 = 0; }
                        #endregion

                        #region Click
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            bepe = true;
                            if (ColisaoMouseOver(mouse, menu1[mn1], rcmenu1))
                            {
                                Historinha = true;
                            }
                            if (ColisaoMouseOver(mouse, menu2[mn2], rcmenu2))
                            {
                                BateBola = true;
                            }
                            if (ColisaoMouseOver(mouse, menu3[mn3], rcmenu3))
                            {
                                Caderno = true;
                            }
                            //if (ColisaoMouseOver(mouse, menu4[mn4], rcmenu4))
                            //{
                            //    Config = true;
                            //}
                            if (ColisaoMouseOver(mouse, menu5[mn5], rcmenu5))
                            {
                                Creditos = true;
                            }
                            if (ColisaoMouseOver(mouse, menu6[mn6], rcmenu6))
                            {
                                Sair = true;
                            }

                        }
                        #endregion
                        if (Caderno)
                        {
                            Caderno = false;
                            parent.EnterState((int)StatesIdList.OPTIONS);
                        }
                        if (Historinha)
                        {
                            MediaPlayer.Stop();
                            engineSound.Stop(AudioStopOptions.AsAuthored);
                            Historinha = false;
                            parent.EnterState((int)StatesIdList.STORY);
                        }
                        else
                            if (BateBola)
                            {
                                engineSound.Stop(AudioStopOptions.AsAuthored);
                                MediaPlayer.Stop();
                                BateBola = false;
                                //parent.EnterState((int)StatesIdList.RUNNER);
                                parent.EnterState((int)StatesIdList.CHAR_SELECTION);
                            }
                        #endregion
                    }
                    
                }
            }
        }
        string MsgSair = "Deseja realmente sair?";
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (!Config && !Creditos)
            {
                Fundo = Fnorm;
            }
            spriteBatch.Draw(Fundo, rcfundo, Color.White * Alpha);
            if (!Config && !Creditos)
            {
                Fundo = Fnorm;
                spriteBatch.Draw(Fundo, rcfundo, Color.White * Alpha);
                spriteBatch.Draw(seta, rcSeta, Color.White * Alpha);
                spriteBatch.Draw(menu1[mn1], rcmenu1, Color.White * Alpha);
                spriteBatch.Draw(menu2[mn2], rcmenu2, Color.White * Alpha);
                spriteBatch.Draw(menu3[mn3], rcmenu3, Color.White * Alpha);
                //spriteBatch.Draw(menu4[mn4], rcmenu4, Color.White * Alpha);
                spriteBatch.Draw(menu5[mn5], rcmenu5, Color.White * Alpha);
                spriteBatch.Draw(menu6[mn6], rcmenu6, Color.White * Alpha);
            }
            else
            {
                spriteBatch.Draw(Voltar[VoltarIndice], vVoltar, Color.White);
            }
            if (Sair)
            {
                spriteBatch.DrawString(arial, MsgSair, new Vector2(350, 300), Color.White);
                spriteBatch.Draw(OK[OKin], rcOK, Color.White);
                spriteBatch.Draw(Cancelar[CancIN], rcCancelar, Color.White);
            }
            //if (Config)
            //{
            //    spriteBatch.Draw(Barra, rcbarra, Color.White);
            //    spriteBatch.Draw(Medidor, rcMedidor, Color.White);
            //}
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
        
        public override void LoadContent()
        {
            if (!contentLoaded)
            {
                #region Inicializar Variaveis
                #region Texturas
                Fnorm = parent.Content.Load<Texture2D>("Menu/Abertura");
                Fconf = parent.Content.Load<Texture2D>("Menu/FundoConfiguracoes");
                Fcre = parent.Content.Load<Texture2D>("Menu/FundoCreditos");
                seta = parent.Content.Load<Texture2D>("Menu/Seta");
                menu1Normal = parent.Content.Load<Texture2D>("Menu/historinhaN");
                menu2Normal = parent.Content.Load<Texture2D>("Menu/batebola_N");
                menu3Normal = parent.Content.Load<Texture2D>("Menu/cadernoN");
                menu4Normal = parent.Content.Load<Texture2D>("Menu/configuracoesN");
                menu5Normal = parent.Content.Load<Texture2D>("Menu/creditosN");
                menu6Normal = parent.Content.Load<Texture2D>("Menu/sairN");
                menu1Over = parent.Content.Load<Texture2D>("Menu/historinhaH");
                menu2Over = parent.Content.Load<Texture2D>("Menu/batebola_H");
                menu3Over = parent.Content.Load<Texture2D>("Menu/cadernoH");
                menu4Over = parent.Content.Load<Texture2D>("Menu/configuracoesH");
                menu5Over = parent.Content.Load<Texture2D>("Menu/creditosH");
                menu6Over = parent.Content.Load<Texture2D>("Menu/sairH");
                OKNormal = parent.Content.Load<Texture2D>("Menu/okN");
                OKOver = parent.Content.Load<Texture2D>("Menu/okH");
                CancelarNormal = parent.Content.Load<Texture2D>("Menu/cancelarN");
                CancelarOver = parent.Content.Load<Texture2D>("Menu/cancelarH");
                arial = parent.Content.Load<SpriteFont>("Fonte/Arial");
                Medidor = parent.Content.Load<Texture2D>("Menu/MedidorBarra");
                Barra = parent.Content.Load<Texture2D>("Menu/BarraBalanco");
                VoltarNormal = parent.Content.Load<Texture2D>("Menu/voltarN");
                VoltarOver = parent.Content.Load<Texture2D>("Menu/voltarH");
                #endregion

                #endregion
                //select = parent.Content.Load<Song>("Audio/magic-chime-07");
                Inicio = parent.Content.Load<Song>("Narrar/TelaInicial");
                contentLoaded = true;
            }
            
           
        }
        public void ConfiguracoesMENU()
        {
            Fundo = Fconf;
            
        }
        public void CreditosMENU()
        {
            Fundo = Fcre;
        }
        public void SairMENU()
        {

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

