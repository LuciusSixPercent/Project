using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;


class DateBase
{
    #region Geral
    public ContentManager Content;
    protected MouseState mouse = Mouse.GetState();
    protected KeyboardState teclado = Keyboard.GetState();
    protected SpriteFont arial;
    #endregion
    #region Variaveis Ep1
    protected Texture2D TelaDeFundo, OpcoeSprite, PersonagemEmCena, BalaoDeFala;
    protected Vector2 VTelaFundo, VOpcoes1, VOpcoes2, VOpcoes3,VOpcoes4, VPersonagens, Valt0,Valt1,Valt2,Valt3,Vpergunta;
    protected Song narrador;
    #endregion
    #region Construtor
    public DateBase(ContentManager c)
    {
        Content = c;
    }
#endregion
    #region Episodios
    protected void EP1()
    {
        TelaDeFundo = Content.Load<Texture2D>("Imagem/Cenario/Cena01");
        PersonagemEmCena = Content.Load<Texture2D>("Imagem/Personagem/Maria");
        OpcoeSprite = Content.Load<Texture2D>("Imagem/Sprites/op");
        VTelaFundo = new Vector2(0, 0);
        VPersonagens = new Vector2(50, 200);
        VOpcoes1 = new Vector2(230, 200);
        VOpcoes2 = new Vector2(VOpcoes1.X + 200, VOpcoes1.Y);
        VOpcoes3 = new Vector2(VOpcoes2.X + 200, VOpcoes2.Y);
        VOpcoes4 = new Vector2(VOpcoes3.X + 200, VOpcoes3.Y);
        narrador = Content.Load<Song>("Audio/N0002");
        Valt0 = new Vector2(VOpcoes1.X, VOpcoes1.Y + 20);
        Valt1 = new Vector2(VOpcoes2.X, VOpcoes2.Y + 20);
        Valt2 = new Vector2(VOpcoes3.X, VOpcoes3.Y + 20);
        Valt3 = new Vector2(VOpcoes4.X, VOpcoes4.Y + 20);
        Vpergunta = new Vector2(10, 100);
    }
    #endregion
}

