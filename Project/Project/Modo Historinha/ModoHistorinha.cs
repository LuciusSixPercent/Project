using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


class ModoHistorinha
{
    
    int i = 0;//contador
    string pergunta, res1, res2, res3, res4;
    Color Calt0, Calt1, Calt2, Calt3;
    SpriteFont arial;
    Texture2D OpcoeSprite;
    int NoDeRespostas;
    #region Sequencia
    bool primeiroclique = false;
    bool segundoclique = false;
    bool terceiroclique = false;
    bool quartoclique = false;
    bool seq = false;
    int indice = 0;
    string texto = "";
    int Limitedotexto = 80;
    int zerar = 1;
    int selecionar = 0;
    List<Song> narrador;
    bool repetir = true;
    #endregion
    Vector2  Valt0, Valt1, Valt2, Valt3, Vpergunta;
    Rectangle VOpcoes1, VOpcoes2, VOpcoes3, VOpcoes4;
    AudioEngine audioEngine2;
    WaveBank waveBank2;
    SoundBank soundBank2;
    Cue PlayerModoHistoria = null;
    int[] PosicoesDasRespostas;
    Random NumeroRandomico;
    Texture2D CaixadeTexto;
    Rectangle vCaixa;
    //teste
    bool venceu = false;
    int erro = 0;
    public ModoHistorinha(ContentManager c, string perguntaz, string alt0, string alt1, string alt2, string alt3, SpriteFont a, int n, Song narra)
        
    {
        audioEngine2 = new AudioEngine("Content\\Audio\\MyGameAudio2.xgs");
        waveBank2 = new WaveBank(audioEngine2, "Content\\Audio\\Wave Bank2.xwb");
        soundBank2 = new SoundBank(audioEngine2, "Content\\Audio\\Sound Bank2.xsb");
        venceu = false;
        narrador = new List<Song>() { narra };
        NoDeRespostas = n;
        arial = a;
        pergunta = perguntaz;
        res1 = alt0;
        res2 = alt1;
        res3 = alt2;
        res4 = alt3;
        Calt0 = Color.White;
        Calt1 = Color.White;
        Calt2 = Color.White;
        Calt3 = Color.White;
        NumeroRandomico = new Random();
        Ini(c, NumeroRandomico, n);
    }
    public ModoHistorinha(ContentManager c, string perguntaz, string alt0, string alt1, SpriteFont a, int n, Song narra)
        
    {
        audioEngine2 = new AudioEngine("Content\\Audio\\MyGameAudio2.xgs");
        waveBank2 = new WaveBank(audioEngine2, "Content\\Audio\\Wave Bank2.xwb");
        soundBank2 = new SoundBank(audioEngine2, "Content\\Audio\\Sound Bank2.xsb");
        venceu = false;
        narrador = new List<Song>() { narra };
        NoDeRespostas = n;
        arial = a;
        pergunta = perguntaz;
        res1 = alt0;
        res2 = alt1;
        Calt0 = Color.White;
        Calt1 = Color.White;
        NumeroRandomico = new Random();
        Ini(c, NumeroRandomico, n);
    }
    public ModoHistorinha(ContentManager c, string perguntaz, string alt0, string alt1, string alt2, SpriteFont a, int n, Song narra)
        
    {
        audioEngine2 = new AudioEngine("Content\\Audio\\MyGameAudio2.xgs");
        waveBank2 = new WaveBank(audioEngine2, "Content\\Audio\\Wave Bank2.xwb");
        soundBank2 = new SoundBank(audioEngine2, "Content\\Audio\\Sound Bank2.xsb");
        venceu = false;
        narrador = new List<Song>() { narra };
        NoDeRespostas = n;
        arial = a;
        pergunta = perguntaz;
        res1 = alt0;
        res2 = alt1;
        res3 = alt2;
        Calt0 = Color.White;
        Calt1 = Color.White;
        Calt2 = Color.White;
        NumeroRandomico = new Random();
        Ini(c, NumeroRandomico, n);
    }
    public ModoHistorinha(ContentManager c, string perguntaz, string alt0, string alt1, string alt2, string alt3, SpriteFont a, int n, Song narra, bool sequencia)
        
    {
        audioEngine2 = new AudioEngine("Content\\Audio\\MyGameAudio2.xgs");
        waveBank2 = new WaveBank(audioEngine2, "Content\\Audio\\Wave Bank2.xwb");
        soundBank2 = new SoundBank(audioEngine2, "Content\\Audio\\Sound Bank2.xsb");
        venceu = false;
        narrador = new List<Song>() { narra };
        seq = sequencia;
        NoDeRespostas = n;
        arial = a;
        pergunta = perguntaz;
        res1 = alt0;
        res2 = alt1;
        res3 = alt2;
        res4 = alt3;
        Calt0 = Color.White;
        Calt1 = Color.White;
        Calt2 = Color.White;
        Calt3 = Color.White;
        NumeroRandomico = new Random();
        Ini(c, NumeroRandomico, n);
    }
    public ModoHistorinha(ContentManager c, string perguntaz, string alt0, string alt1, string alt2, string alt3, SpriteFont a, int n, Song narra1, Song narrar2, bool sequencia)
        
    {
        audioEngine2 = new AudioEngine("Content\\Audio\\MyGameAudio2.xgs");
        waveBank2 = new WaveBank(audioEngine2, "Content\\Audio\\Wave Bank2.xwb");
        soundBank2 = new SoundBank(audioEngine2, "Content\\Audio\\Sound Bank2.xsb");
        venceu = false;
        narrador = new List<Song>() { narra1, narrar2 };
        seq = sequencia;
        NoDeRespostas = n;
        arial = a;
        pergunta = perguntaz;
        res1 = alt0;
        res2 = alt1;
        res3 = alt2;
        res4 = alt3;
        Calt0 = Color.Black;
        Calt1 = Color.Black;
        Calt2 = Color.Black;
        Calt3 = Color.Black;
        NumeroRandomico = new Random();
        Ini(c, NumeroRandomico, n);
    }
    int incrementoTexto = 0;
    public void Atualizar()
    {
        MouseState mouse = Mouse.GetState();
        if (repetir)
        {
            MediaPlayer.Play(narrador[selecionar]);
            repetir = false;
        }

        if (texto.Length % Limitedotexto == 0 && texto.Length != 0)//Quando o texto atingir um limite da tela e tiver um espaço em branco ele pula uma linha;
        {
            if (pergunta[indice - 1] == ' ')
            {
                texto += "\n";
                zerar++;
                Limitedotexto = 80 * zerar;
            }
            else
            {
                Limitedotexto++;  
                incrementoTexto++;

            }
            //Limitedotexto = ((80 * (zerar + 1)) + incrementoTexto);
        }
        mouse = Mouse.GetState();
        if (!primeiroclique && seq)
        {
            Calt1 = Color.White;
        }
        if (!segundoclique && seq)
        {
            Calt2 = Color.White;
        }
        if (!terceiroclique && seq)
        {
            Calt0 = Color.White;
        }
        if (!quartoclique && seq)
        {
            Calt3 = Color.White;
        }
        if (primeiroclique && segundoclique && terceiroclique && quartoclique)
        {
            MediaPlayer.Stop();
            venceu = true;
        }
        Click(mouse);
        
    }
    public void Click(MouseState mouse)
    {
        if (mouse.LeftButton == ButtonState.Pressed)
        {
            if ((mouse.X > VOpcoes1.X && mouse.X < VOpcoes1.X + VOpcoes1.Width) && (mouse.Y >= VOpcoes1.Y && mouse.Y <= VOpcoes1.Y + VOpcoes1.Height))
            {
                if (!seq)
                {
                    MediaPlayer.Stop();
                    venceu = true;
                }
                else
                {
                    if (primeiroclique && segundoclique)
                    {
                        PlayerModoHistoria = null;
                        PlayerModoHistoria = soundBank2.GetCue("decision7");
                        PlayerModoHistoria.Play();
                        terceiroclique = true;
                        Calt0 = Color.Transparent;
                    }
                    else
                    {
                        PlayerModoHistoria = null;
                        PlayerModoHistoria = soundBank2.GetCue("warning1");
                        PlayerModoHistoria.Play();
                        erro++;
                        primeiroclique = false;
                        segundoclique = false;
                        terceiroclique = false;
                        
                    }
                }
                
            }
            if ((mouse.X > VOpcoes2.X && mouse.X < VOpcoes2.X + VOpcoes2.Width) && (mouse.Y >= VOpcoes2.Y && mouse.Y <= VOpcoes2.Y + VOpcoes2.Height))
            {

                
                if (!seq)
                {
                    PlayerModoHistoria = null;
                    PlayerModoHistoria = soundBank2.GetCue("warning1");
                    PlayerModoHistoria.Play();
                    erro++;
                    Calt1 = Color.Transparent;
                }
                else
                {
                    PlayerModoHistoria = null;
                    PlayerModoHistoria = soundBank2.GetCue("decision7");
                    PlayerModoHistoria.Play();
                    primeiroclique = true;
                    Calt1 = Color.Transparent;
                }
            }
            if ((mouse.X > VOpcoes3.X && mouse.X < VOpcoes3.X + VOpcoes3.Width) && (mouse.Y >= VOpcoes3.Y && mouse.Y <= VOpcoes3.Y + VOpcoes3.Height))
            {
                
                if (!seq)
                {
                    PlayerModoHistoria = null;
                    PlayerModoHistoria = soundBank2.GetCue("warning1");
                    PlayerModoHistoria.Play();
                    erro++;
                    if (narrador.Count == 2)
                    {
                        selecionar = 1;
                        repetir = true;
                    }
                    Calt2 = Color.Transparent;
                }
                else
                {
                    if (primeiroclique)
                    {
                        PlayerModoHistoria = null;
                        PlayerModoHistoria = soundBank2.GetCue("decision7");
                        PlayerModoHistoria.Play();
                        segundoclique = true;
                        Calt2 = Color.Transparent;
                    }
                    else
                    {
                        PlayerModoHistoria = null;
                        PlayerModoHistoria = soundBank2.GetCue("warning1");
                        PlayerModoHistoria.Play();
                        erro++;
                        primeiroclique = false;
                        segundoclique = false;
                        
                    }
                }
            }
            if ((mouse.X > VOpcoes4.X && mouse.X < VOpcoes4.X + VOpcoes4.Width) && (mouse.Y >= VOpcoes4.Y && mouse.Y <= VOpcoes4.Y + VOpcoes4.Height))
            {
                
                if (!seq)
                {
                    PlayerModoHistoria = null;
                    PlayerModoHistoria = soundBank2.GetCue("warning1");
                    PlayerModoHistoria.Play();
                    erro++;
                    Calt3 = Color.Transparent;
                }
                else
                {
                    if (primeiroclique && segundoclique && terceiroclique)
                    {
                        PlayerModoHistoria = null;
                        PlayerModoHistoria = soundBank2.GetCue("decision7");
                        PlayerModoHistoria.Play();
                        quartoclique = true;
                        Calt3 = Color.Transparent;
                    }
                    else
                    {
                        PlayerModoHistoria = null;
                        PlayerModoHistoria = soundBank2.GetCue("warning1");
                        PlayerModoHistoria.Play();
                        erro++;
                        primeiroclique = false;
                        segundoclique = false;
                        terceiroclique = false;
                    }
                }

            }

        }
        #region Mouse Over (Mouse sobre o objeto)
        if ((mouse.X > VOpcoes1.X && mouse.X < VOpcoes1.X + VOpcoes1.Width) && (mouse.Y >= VOpcoes1.Y && mouse.Y <= VOpcoes1.Y + VOpcoes1.Height))
        {

            if (VOpcoes1.Y > 190)
            {
                VOpcoes1.Y -= 1;

            }
        }
        if ((mouse.X > VOpcoes2.X && mouse.X < VOpcoes2.X + VOpcoes2.Width) && (mouse.Y >= VOpcoes2.Y && mouse.Y <= VOpcoes2.Y + VOpcoes2.Height))
        {
            if (VOpcoes2.Y > 190)
            {
                VOpcoes2.Y -= 1;
            }
        }
        if ((mouse.X > VOpcoes3.X && mouse.X < VOpcoes3.X + VOpcoes3.Width) && (mouse.Y >= VOpcoes3.Y && mouse.Y <= VOpcoes3.Y + VOpcoes3.Height))
        {
            if (VOpcoes3.Y > 190)
            {
                VOpcoes3.Y -= 1;
            }
        }
        if ((mouse.X > VOpcoes4.X && mouse.X < VOpcoes4.X + VOpcoes4.Width) && (mouse.Y >= VOpcoes4.Y && mouse.Y <= VOpcoes4.Y + VOpcoes4.Height))
        {
            if (VOpcoes4.Y > 190)
            {
                VOpcoes4.Y -= 1;
            }
        }
        #endregion
        #region Mouse fora do objeto
        if ((mouse.X < VOpcoes1.X || mouse.X > VOpcoes1.X + VOpcoes1.Width) || (mouse.Y < VOpcoes1.Y || mouse.Y > VOpcoes1.Y + VOpcoes1.Height))
        {
            if (VOpcoes1.Y < 200)
            {
                VOpcoes1.Y += 1;
            }
        }
        if ((mouse.X < VOpcoes2.X || mouse.X > VOpcoes2.X + VOpcoes2.Width) || (mouse.Y < VOpcoes2.Y || mouse.Y > VOpcoes2.Y + VOpcoes2.Height))
        {
            if (VOpcoes2.Y < 200)
            {
                VOpcoes2.Y += 1;
            }
        }
        if ((mouse.X < VOpcoes3.X || mouse.X > VOpcoes3.X + VOpcoes3.Width) || (mouse.Y < VOpcoes3.Y || mouse.Y > VOpcoes3.Y + VOpcoes3.Height))
        {
            if (VOpcoes3.Y < 200)
            {
                VOpcoes3.Y += 1;
            }
        }
        if ((mouse.X < VOpcoes4.X || mouse.X > VOpcoes4.X + VOpcoes4.Width) || (mouse.Y < VOpcoes4.Y || mouse.Y > VOpcoes4.Y + VOpcoes4.Height))
        {
            if (VOpcoes4.Y < 200)
            {
                VOpcoes4.Y += 1;
            }
        }
        #endregion
    }
    public void Desenhar(SpriteBatch spriteBatch)
    {
        //spriteBatch.Draw(TelaDeFundo, VTelaFundo, Color.White);
        //spriteBatch.Draw(PersonagemEmCena, VPersonagens, Color.White);
        if (indice < pergunta.Length) { texto += pergunta[indice]; }
        indice = indice + (indice < pergunta.Length ? 1 : 0);
        spriteBatch.Draw(CaixadeTexto, vCaixa, Color.White);
        spriteBatch.DrawString(arial, texto, Vpergunta, Color.White);
        for (int respotas = 0; respotas < NoDeRespostas; respotas++)
        {
            spriteBatch.Draw(OpcoeSprite, respotas == 3 ? VOpcoes4 : respotas == 2 ? VOpcoes3 : respotas == 1 ? VOpcoes2 : VOpcoes1, respotas == 3 ? Calt3 : respotas == 2 ? Calt2 : respotas == 1 ? Calt1 : Calt0);

        }
        switch (NoDeRespostas)
        {
            case 2:
                spriteBatch.DrawString(arial, res1, Valt0, Color.Black);
                spriteBatch.DrawString(arial, res2, Valt1, Color.Black);
                break;
            case 3:
                spriteBatch.DrawString(arial, res1, Valt0, Calt0);
                spriteBatch.DrawString(arial, res2, Valt1, Calt1);
                spriteBatch.DrawString(arial, res3, Valt2, Calt2);
                break;
            case 4:
                spriteBatch.DrawString(arial, res1, Valt0, Calt0);
                spriteBatch.DrawString(arial, res2, Valt1, Calt1);
                spriteBatch.DrawString(arial, res3, Valt2, Calt2);
                spriteBatch.DrawString(arial, res4, Valt3, Calt3);
                break;
        }



    }
    public bool Continuar()
    {
        if (venceu)
        {
            PlayerModoHistoria = null;
            PlayerModoHistoria = soundBank2.GetCue("decision5");
            PlayerModoHistoria.Play();
        }
        return venceu ;

    }
    int sorte = 0;
    int[] numerosPegos;
    public void Ini(ContentManager Content, Random aleatorio, int quantidade)
    {
        numerosPegos = new int[4];
        OpcoeSprite = Content.Load<Texture2D>("Imagem/Sprites/op");
        CaixadeTexto = Content.Load<Texture2D>("Imagem/Caixa_texto");
        PosicoesDasRespostas = new int[4] { 230, 400, 570, 740 };
        for (int j = 0; j < quantidade; j++)
        {
            if (j != 0)
            {
                if (numerosPegos[j - 1] == sorte)
                {
                    while (numerosPegos[j - 1] == sorte)
                    {
                        sorte = aleatorio.Next(0, quantidade);
                        numerosPegos[j] = sorte;
                    }
                }

                if (j > 1)
                {
                    if (numerosPegos[j - 2] == sorte || numerosPegos[j - 1] == sorte)
                    {
                        while (numerosPegos[j - 2] == sorte || numerosPegos[j - 1] == sorte)
                        {
                            sorte = aleatorio.Next(0, quantidade);
                            numerosPegos[j] = sorte;
                        }
                    }
                }
                if (j == 3)
                {
                    if (numerosPegos[j - 2] == sorte || numerosPegos[j - 1] == sorte || numerosPegos[j - 3] == sorte)
                    {
                        while (numerosPegos[j - 2] == sorte || numerosPegos[j - 1] == sorte || numerosPegos[j - 3] == sorte)
                        {
                            sorte = aleatorio.Next(0, quantidade);
                            numerosPegos[j] = sorte;
                        }
                    }
                }
            }
            if (j == 0)
            {
                sorte = aleatorio.Next(0, quantidade);
                numerosPegos[j] = sorte;
            }

        }
        
        VOpcoes1 = new Rectangle(PosicoesDasRespostas[numerosPegos[0]], 200, OpcoeSprite.Width/4, OpcoeSprite.Height/4);
        VOpcoes2 = new Rectangle(PosicoesDasRespostas[numerosPegos[1]], 200, VOpcoes1.Width, VOpcoes1.Height);
        VOpcoes3 = new Rectangle(PosicoesDasRespostas[numerosPegos[2]], 200, VOpcoes1.Width, VOpcoes1.Height);
        VOpcoes4 = new Rectangle(PosicoesDasRespostas[numerosPegos[3]], 200, VOpcoes1.Width, VOpcoes1.Height);
        Valt0 = new Vector2(VOpcoes1.Left, VOpcoes1.Location.Y);
        Valt1 = new Vector2(VOpcoes2.X, VOpcoes2.Y + 20);
        Valt2 = new Vector2(VOpcoes3.X, VOpcoes3.Y + 20);
        Valt3 = new Vector2(VOpcoes4.X, VOpcoes4.Y + 20);
        Vpergunta = new Vector2(20, 20);
        vCaixa = new Rectangle(0, 0, CaixadeTexto.Width, CaixadeTexto.Height);
    }
}

