using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;


class ModoHistorinha : DateBase
{
    String VariavelInutil;
    int i = 0;//contador
    string pergunta, res1, res2, res3, res4;
    Color Calt0, Calt1, Calt2, Calt3;
    SpriteFont arial;
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
    //teste
    bool venceu = false;
    public ModoHistorinha(ContentManager c, string perguntaz, string alt0, string alt1, string alt2, string alt3, int episodio, SpriteFont a, int n, Song narra)
        : base(c)
    {
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
        switch (episodio)
        {
            case 1:
                EP1();
                break;
        }
    }
    public ModoHistorinha(ContentManager c, string perguntaz, string alt0, string alt1, int episodio, SpriteFont a, int n, Song narra)
        : base(c)
    {
        venceu = false;
        narrador = new List<Song>() { narra };
        NoDeRespostas = n;
        arial = a;
        pergunta = perguntaz;
        res1 = alt0;
        res2 = alt1;
        Calt0 = Color.White;
        Calt1 = Color.White;
        switch (episodio)
        {
            case 1:
                EP1();
                break;
        }
    }
    public ModoHistorinha(ContentManager c, string perguntaz, string alt0, string alt1, string alt2, int episodio, SpriteFont a, int n, Song narra)
        : base(c)
    {
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
        switch (episodio)
        {
            case 1:
                EP1();
                break;
        }
    }
    public ModoHistorinha(ContentManager c, string perguntaz, string alt0, string alt1, string alt2, string alt3, int episodio, SpriteFont a, int n, Song narra, bool sequencia)
        : base(c)
    {
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
        switch (episodio)
        {
            case 1:
                EP1();
                break;
        }
    }
    public ModoHistorinha(ContentManager c, string perguntaz, string alt0, string alt1, string alt2, string alt3, int episodio, SpriteFont a, int n, Song narra1, Song narrar2, bool sequencia)
        : base(c)
    {
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
        Calt0 = Color.White;
        Calt1 = Color.White;
        Calt2 = Color.White;
        Calt3 = Color.White;
        switch (episodio)
        {
            case 1:
                EP1();
                break;
        }
    }
    int incrementoTexto = 0;
    public void Atualizar()
    {
        if (repetir)
        {
            MediaPlayer.Play(narrador[selecionar]);
            repetir = false;
        }
       
        if (texto.Length % Limitedotexto == 0 && texto.Length != 0)//Quando o texto atingir um limite da tela e tiver um espaço em branco ele pula uma linha;
        {
            if (texto[indice - 1] == ' ')
            {
                texto += "\n";

                zerar++;
                incrementoTexto = 0;
                
            }
            else
            {
                
                incrementoTexto++;
            }
            Limitedotexto = ((80 * (zerar + 1)) + incrementoTexto);
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
            if ((mouse.X > VOpcoes1.X && mouse.X < VOpcoes1.X + OpcoeSprite.Width) && (mouse.Y >= VOpcoes1.Y && mouse.Y <= VOpcoes1.Y + OpcoeSprite.Height))
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
                        terceiroclique = true;
                        Calt0 = Color.Transparent;
                    }
                    else
                    {
                        primeiroclique = false;
                        segundoclique = false;
                        terceiroclique = false;
                        
                    }
                }
                
            }
            if ((mouse.X > VOpcoes2.X && mouse.X < VOpcoes2.X + OpcoeSprite.Width) && (mouse.Y >= VOpcoes2.Y && mouse.Y <= VOpcoes2.Y + OpcoeSprite.Height))
            {

                
                if (!seq)
                {
                    Calt1 = Color.Transparent;
                }
                else
                {
                    primeiroclique = true;
                    Calt1 = Color.Transparent;
                }
            }
            if ((mouse.X > VOpcoes3.X && mouse.X < VOpcoes3.X + OpcoeSprite.Width) && (mouse.Y >= VOpcoes3.Y && mouse.Y <= VOpcoes3.Y + OpcoeSprite.Height))
            {
                
                if (!seq)
                {
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
                        segundoclique = true;
                        Calt2 = Color.Transparent;
                    }
                    else
                    {
                        primeiroclique = false;
                        segundoclique = false;
                        
                    }
                }
            }
            if ((mouse.X > VOpcoes4.X && mouse.X < VOpcoes4.X + OpcoeSprite.Width) && (mouse.Y >= VOpcoes4.Y && mouse.Y <= VOpcoes4.Y + OpcoeSprite.Height))
            {
                
                if (!seq)
                {
                    Calt3 = Color.Transparent;
                }
                else
                {
                    if (primeiroclique && segundoclique && terceiroclique)
                    {
                        quartoclique = true;
                        Calt3 = Color.Transparent;
                    }
                    else
                    {
                        primeiroclique = false;
                        segundoclique = false;
                        terceiroclique = false;
                    }
                }

            }

        }
        #region Mouse Over (Mouse sobre o objeto)
        if ((mouse.X > VOpcoes1.X && mouse.X < VOpcoes1.X + OpcoeSprite.Width) && (mouse.Y >= VOpcoes1.Y && mouse.Y <= VOpcoes1.Y + OpcoeSprite.Height))
        {

            if (VOpcoes1.Y > 190)
            {
                VOpcoes1.Y -= 1;

            }
        }
        if ((mouse.X > VOpcoes2.X && mouse.X < VOpcoes2.X + OpcoeSprite.Width) && (mouse.Y >= VOpcoes2.Y && mouse.Y <= VOpcoes2.Y + OpcoeSprite.Height))
        {
            if (VOpcoes2.Y > 190)
            {
                VOpcoes2.Y -= 1;
            }
        }
        if ((mouse.X > VOpcoes3.X && mouse.X < VOpcoes3.X + OpcoeSprite.Width) && (mouse.Y >= VOpcoes3.Y && mouse.Y <= VOpcoes3.Y + OpcoeSprite.Height))
        {
            if (VOpcoes3.Y > 190)
            {
                VOpcoes3.Y -= 1;
            }
        }
        if ((mouse.X > VOpcoes4.X && mouse.X < VOpcoes4.X + OpcoeSprite.Width) && (mouse.Y >= VOpcoes4.Y && mouse.Y <= VOpcoes4.Y + OpcoeSprite.Height))
        {
            if (VOpcoes4.Y > 190)
            {
                VOpcoes4.Y -= 1;
            }
        }
        #endregion
        #region Mouse fora do objeto
        if ((mouse.X < VOpcoes1.X || mouse.X > VOpcoes1.X + OpcoeSprite.Width) || (mouse.Y < VOpcoes1.Y || mouse.Y > VOpcoes1.Y + OpcoeSprite.Height))
        {
            if (VOpcoes1.Y < 200)
            {
                VOpcoes1.Y += 1;
            }
        }
        if ((mouse.X < VOpcoes2.X || mouse.X > VOpcoes2.X + OpcoeSprite.Width) || (mouse.Y < VOpcoes2.Y || mouse.Y > VOpcoes2.Y + OpcoeSprite.Height))
        {
            if (VOpcoes2.Y < 200)
            {
                VOpcoes2.Y += 1;
            }
        }
        if ((mouse.X < VOpcoes3.X || mouse.X > VOpcoes3.X + OpcoeSprite.Width) || (mouse.Y < VOpcoes3.Y || mouse.Y > VOpcoes3.Y + OpcoeSprite.Height))
        {
            if (VOpcoes3.Y < 200)
            {
                VOpcoes3.Y += 1;
            }
        }
        if ((mouse.X < VOpcoes4.X || mouse.X > VOpcoes4.X + OpcoeSprite.Width) || (mouse.Y < VOpcoes4.Y || mouse.Y > VOpcoes4.Y + OpcoeSprite.Height))
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
        spriteBatch.DrawString(arial, texto, Vpergunta, Color.Red);
        for (int respotas = 0; respotas < NoDeRespostas; respotas++)
        {
            spriteBatch.Draw(OpcoeSprite, respotas == 3 ? VOpcoes4 : respotas == 2 ? VOpcoes3 : respotas == 1 ? VOpcoes2 : VOpcoes1, respotas == 3 ? Calt3 : respotas == 2 ? Calt2 : respotas == 1 ? Calt1 : Calt0);

        }
        switch (NoDeRespostas)
        {
            case 2:
                spriteBatch.DrawString(arial, res1, Valt0, Calt0);
                spriteBatch.DrawString(arial, res2, Valt1, Calt1);
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

        return venceu ;

    }
}

