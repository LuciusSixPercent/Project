using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using game_states;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    class CadernoDeAtividades:GameState 
    {
        Episodio01 questions;
        int Page = 0;
        SpriteFont arial;
        Texture2D livro, BtAvancarN, BtAvancarH,VoltarOver, VoltarNormal,CaixaDeTexto,BTx;
        Texture2D[] BtAvancar, Voltar;
        Rectangle vBtAvancar, vVoltar,vBTx;
        Vector2 vlivro, vQ1, vQ2, vQ3, vQ4;
        string p1, p2, p3, p4, altQues1, altQues2, altQues3, altQues4, altQues5, altQues6, altQues7, altQues8, altQues9, altQues10, explica01, explica02, explica03, explica04, explica05, explica06, explica07, explica08, explica09, explica10,exp1,exp2,exp3,exp4;
        string[] q1, q2, q3, q4, alt1, alt2, alt3, alt4, explicaP1, explicaP2, explicaP3, explicaP4;
        
        int linhas = 30;
        int barra1 = 0;
        int barra2 = 0;
        int barra3 = 0;
        int barra4 = 0;
        int qbr1 = 0;
        int qbr2 = 0;
        int qbr3 = 0;
        int qbr4 = 0;
        Color cor1, cor2, cor3, cor4;
        int VoltarIndice = 0;
        int indiceDoAvancar = 0;
        int[] RespostasQ1, RespostasQ2, RespostasQ3, RespostasQ4;
        bool clique = false;
        public CadernoDeAtividades(int id, Game1 parent,Episodio01 ep)
            : base(id, parent)
        {
            questions = ep;
            Initialize();
        }
        protected override void Initialize()
        {

            if (!initialized)
            {
                LoadContent();
                p1 = "";
                p2 = "";
                p3 = "";
                p4 = "";
                exp1 = "";
                exp2 = "";
                exp3 = "";
                exp4 = "";
                explica01 = "Explicação: O número SETE é escrito como 7. O número UM é escrito como 1.";
                explica02 = "Explicação: Cosme, Maria, Apuã e Serafina são 4 pessoas.";
                explica03 = "Explicação: Cada personagem recebeu um número. Eles devem se organizar da seguinte maneira para que fiquem em ordem numérica: Apuã, Serafina, Maria e Cosme (1, 2, 3 e 4).";
                explica04 = "Explicação: O time A tem 3 pessoas (Apuã, Cosme e Serafina). O time B tem apenas uma pessoa (Maria).";
                explica05 = "Explicação: Como há 4 pessoas e 2 times, cada time deve ter 2 pessoas. Assim, cada time terá a mesma quantidade de jogadores.";
                explica06 = "Explicação: O b minúsculo é mais reto que o número 6.";
                explica07 = "Explicação: As alternativas que começam com a letra B e tem duas sílabas são BO-LA e BO-LO. Porém, apenas a BOLA pode ser usada para jogar futebol.";
                explica08 = "Explicação: A bola está atrás de um Livro. A palavra começa com a letra L.";
                explica09 = "Explicação: As palavras com três sílabas são A-MI-GOS e RI-A-CHO. Mas Cosme diz que Maria pode contar com as outras pessoas para ajudá-la, no caso, seus AMIGOS.";
                explica10 = "Explicação: Com a bola de futebol e o campo preparado, os amigos vão jogar FUTEBOL, palavra que possui 3 sílabas.";
                explicaP1 = new string[3] { explica01, explica05, explica09 };
                explicaP2 = new string[3] { explica02, explica06, explica10 };
                explicaP3 = new string[3] { explica03, explica07, " " };
                explicaP4 = new string[3] { explica04, explica08, " " };
                q1 = new string[3] { "Exercício 01) " + questions.pergunta00, "Exercício 05) " + questions.pergunta40, "Exercício 09) " + questions.Pergunta9 };
                q2 = new string[3] { "Exercício 02) " + questions.pergunta10, "Exercício 06) " + questions.pergunta6, "Exercício 10) " + questions.Pergunta010 };
                q3 = new string[3] { "Exercício 03) " + questions.pergunta20, "Exercício 07) " + questions.Pergunta7, "Não tem mais questão." };
                q4 = new string[3] { "Exercício 04) " + questions.pergunta30, "Exercício 08) " + questions.Pergunta8, "Não tem mais questão." };
                altQues1 = "\na)" + questions.alternativa00 + "\nb)" + questions.alternativa01;
                altQues2 = "\na)"+questions.alternativa02 +"\nb)"+questions.alternativa03+"\nc)"+questions.alternativa10;
                altQues3 = "\na)" + questions.alternativa20 + "\nb)" + questions.alternativa21 + "\nc)" + questions.alternativa22 +"\nd)"+questions.alternativa23;
                altQues4 = "\na)" + questions.alternativa30 + "\nb)" + questions.alternativa31;
                altQues5 = "\na)" + questions.alternativa40 + "\nb)" + questions.alternativa41 + "\nc)" + questions.alternativa42;
                altQues6 = "\na)" + questions.alt60 + "\nb)" + questions.alt61;
                altQues7 = "\na)" + questions.alt70 + "\nb)" + questions.alt71 + "\nc)" + questions.alt72 +"\nd)" +questions.alt73 ;
                altQues8 = "\na)" + questions.alt80 + "\nb)" + questions.alt81 + "\nc)" + questions.alt82;
                altQues9 = "\na)" + questions.alt90 + "\nb)" + questions.alt91 + "\nc)" + questions.alt92;
                altQues10 = "\na)" + questions.alt100 + "\nb)" + questions.alt101 + "\nc)" + questions.alt102;
                alt1 = new string[3] { altQues1, altQues5, altQues9 };
                alt2 = new string[3] { altQues2, altQues6, altQues10 };
                alt3 = new string[3] { altQues3, altQues7, " " };
                alt4 = new string[3] { altQues4, altQues8, " " };
                RespostasQ1 = new int[3] { 2, 3, 3 };
                RespostasQ2 = new int[3] { 3, 2, 3 };
                RespostasQ3 = new int[3] { 4, 4, 0 };
                RespostasQ4 = new int[3] { 5, 3 ,0};
                vlivro = new Vector2(0, 0);
                vQ1 = new Vector2(vlivro.X + 100, vlivro.Y + 80);
                vQ2 = new Vector2(vQ1.X, vQ1.Y + 350);
                vQ3 = new Vector2(vQ1.X + 450, vQ1.Y);
                vQ4 = new Vector2(vQ3.X, vQ2.Y);
                Voltar = new Texture2D[2] { VoltarNormal, VoltarOver };
                vVoltar = new Rectangle(0, 700, Voltar[VoltarIndice].Width / 3, Voltar[VoltarIndice].Height / 3);
                BtAvancar = new Texture2D[2] { BtAvancarN, BtAvancarH };
                vBtAvancar = new Rectangle(950, 700, BtAvancar[indiceDoAvancar].Width / 3, BtAvancar[indiceDoAvancar].Height / 3);
                vBTx = new Rectangle(950, 0, BTx.Width / 6, BTx.Height / 6);
                
                base.Initialize();
            }
        }
        Keys lastKey = Keys.A;
        public override void Update(GameTime tempo)
        {
            #region Quebra de Linhas
            #region Qubrar linha Perguntas
            if (p1.Length != q1[Page].Length+barra1)
            {
                for (int i = 0; i < q1[Page].Length; i++)
                {
                    if (i % linhas == 0 && i!=0)
                    {
                        if (q1[Page][i-1] == ' ')
                        {
                            p1 += "\n";
                            barra1++;
                            linhas = 30 * (barra1+1);
                        }
                        else
                        {
                            linhas++;
                        }
                    }
                    if (i == q1[Page].Length - 1)
                    {
                        linhas = 30;
                    }

                    p1 += q1[Page][i];
                }
            }
            if (p2.Length != q2[Page].Length + barra2)
            {
                for (int i = 0; i < q2[Page].Length; i++)
                {
                    

                        if (i % linhas == 0&& i!=0)
                        {
                            if (q2[Page][i-1] == ' ')
                            {
                                p2 += "\n";
                                barra2++;
                                linhas = 30 * (barra2+1);
                            }
                            else
                            {
                                linhas++;
                            }
                        }
                        if (i == q2[Page].Length - 1)
                        {
                            linhas = 30;
                        }
                        p2 += q2[Page][i];
                    }
                
            }
            if (p3.Length != q3[Page].Length + barra3)
            {
                for (int i = 0; i < q3[Page].Length; i++)
                {
                    if (i % linhas == 0&& i!=0)
                    {
                        if (q3[Page][i-1] == ' ')
                        {
                            p3 += "\n";
                            barra3++;
                            linhas = 30 * (barra3 + 1);

                        }
                        else
                        {
                            linhas++;
                        }
                    }
                    if (i == q3[Page].Length - 1)
                    {
                        linhas = 30;
                    }
                    p3 += q3[Page][i];
                }
            }
            if (p4.Length != q4[Page].Length + barra4)
            {
                for (int i = 0; i < q4[Page].Length; i++)
                {
                    if (i % linhas == 0 && i!= 0)
                    {
                        if (q4[Page][i-1] == ' ')
                        {
                            p4 += "\n";
                            barra4++;
                            linhas = 30 * (barra4 + 1);
                        }
                        else
                        {
                            linhas++;
                        }
                    }
                    if (i == q4[Page].Length - 1)
                    {
                        linhas = 30;
                    }
                  
                    p4 += q4[Page][i];
                }
            }
            #endregion
            #region Quebrar de linhas Explicações
            if (exp1.Length != explicaP1[Page].Length + qbr1)
            {
                for (int i = 0; i < explicaP1[Page].Length; i++)
                {
                    if (i % linhas == 0 && i != 0)
                    {
                        if (explicaP1[Page][i - 1] == ' ')
                        {
                            exp1 += "\n";
                            qbr1++;
                            linhas = 30 * (qbr1 + 1);
                        }
                        else
                        {
                            linhas++;
                        }
                    }
                    if (i == explicaP1[Page].Length - 1)
                    {
                        linhas = 30;
                    }

                    exp1 += explicaP1[Page][i];
                }
            }
            if (exp2.Length !=  explicaP2[Page].Length + qbr2)
            {
                for (int i = 0; i < explicaP2[Page].Length; i++)
                {
                    if (i % linhas == 0 && i != 0)
                    {
                        if (explicaP2[Page][i - 1] == ' ')
                        {
                            exp2 += "\n";
                            qbr2++;
                            linhas = 30 * (qbr2 + 1);
                        }
                        else
                        {
                            linhas++;
                        }
                    }
                    if (i == explicaP2[Page].Length - 1)
                    {
                        linhas = 30;
                    }
                    exp2 += explicaP2[Page][i];
                }

            }
            if (exp3.Length != explicaP3[Page].Length + qbr3)
            {
                for (int i = 0; i < explicaP3[Page].Length; i++)
                {
                    if (i % linhas == 0 && i != 0)
                    {
                        if (explicaP3[Page][i - 1] == ' ')
                        {
                            exp3 += "\n";
                            qbr3++;
                            linhas = 30 * (qbr3 + 1);

                        }
                        else
                        {
                            linhas++;
                        }
                    }
                    if (i == explicaP3[Page].Length - 1)
                    {
                        linhas = 30;
                    }
                    exp3 += explicaP3[Page][i];
                }
            }
            if (exp4.Length != explicaP4[Page].Length + qbr4)
            {
                for (int i = 0; i < explicaP4[Page].Length; i++)
                {
                    if (i % linhas == 0 && i != 0)
                    {
                        if (explicaP4[Page][i - 1] == ' ')
                        {
                            exp4 += "\n";
                            qbr4++;
                            linhas = 30 * (qbr4 + 1);
                        }
                        else
                        {
                            linhas++;
                        }
                    }
                    if (i == explicaP4[Page].Length - 1)
                    {
                        linhas = 30;
                    }

                    exp4 += explicaP4[Page][i];
                }
            }
            #endregion
            #endregion
            KeyboardState teclado = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            #region Comandos
            if (ColisaoMouseOver(mouse, vBtAvancar))
            {
                indiceDoAvancar = 1;
            }
            else { indiceDoAvancar = 0; }
            if (ColisaoMouseOver(mouse, vVoltar))
            {
                VoltarIndice = 1;
            }
            else { VoltarIndice = 0; }
            if (mouse.LeftButton == ButtonState.Pressed && !clique)
            {
                clique = true;
                if (ColisaoMouseOver(mouse, vBTx))
                {
                    questions.Cad(false);
                    ExitState();
                }
                if (ColisaoMouseOver(mouse, vBtAvancar))
                {
                    Page++;
                    p1 = "";
                    p2 = "";
                    p3 = "";
                    p4 = "";
                    exp1 = "";
                    exp2 = "";
                    exp3 = "";
                    exp4 = "";
                    barra1 = 0;
                    barra2 = 0;
                    barra3 = 0;
                    barra4 = 0;
                    qbr1 = 0;
                    qbr2 = 0;
                    qbr3 = 0;
                    qbr4 = 0;

                    if (Page >= 2)
                    {
                        Page = 2;
                    }

                }
                if (ColisaoMouseOver(mouse, vVoltar))
                {
                    if (Page > 0)
                    {
                        Page--;
                        p1 = "";
                        p2 = "";
                        p3 = "";
                        p4 = "";
                        barra1 = 0;
                        barra2 = 0;
                        barra3 = 0;
                        barra4 = 0;
                        exp1 = "";
                        exp2 = "";
                        exp3 = "";
                        exp4 = "";
                        qbr1 = 0;
                        qbr2 = 0;
                        qbr3 = 0;
                        qbr4 = 0;
                        if (Page <= 0)
                        {
                            Page = 0;
                        }
                    }
                    
                }
            }
            if (mouse.LeftButton == ButtonState.Released)
            {
                clique = false;
            }
            
            #endregion
            if (Page == 0)
            {
                if (questions.EX1)
                {
                    cor1 = Color.Black;
                }
                else
                {
                    cor1 = Color.Transparent;
                }
                if (questions.EX2)
                {
                    cor2 = Color.Black;
                }
                else
                {
                    cor2 = Color.Transparent;
                }
                if (questions.EX3)
                {
                    cor3 = Color.Black;
                }
                else
                {
                    cor3 = Color.Transparent;
                }
                if (questions.EX4)
                {
                    cor4 = Color.Black;
                }
                else
                {
                    cor4 = Color.Transparent;
                }
            }
            if (Page == 1)
            {
                if (questions.EX5)
                {
                    cor1 = Color.Black;
                }
                else
                {
                    cor1 = Color.Transparent;
                }
                if (questions.EX6)
                {
                    cor2 = Color.Black;
                }
                else
                {
                    cor2 = Color.Transparent;
                }
                if (questions.EX7)
                {
                    cor3 = Color.Black;
                }
                else
                {
                    cor3 = Color.Transparent;
                }
                if (questions.EX8)
                {
                    cor4 = Color.Black;
                }
                else
                {
                    cor4 = Color.Transparent;
                }
            }
            if (Page == 2)
            {
                if (questions.EX9)
                {
                    cor1 = Color.Black;
                }
                else
                {
                    cor1 = Color.Transparent;
                }
                if (questions.EX10)
                {
                    cor2 = Color.Black;
                }
                else
                {
                    cor2 = Color.Transparent;
                }
                
            }
            base.Update(tempo);
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(livro, vlivro, Color.White);
            SpriteBatch.DrawString(arial, Page.ToString() + "/2", new Vector2(900, 700), Color.Black);
            if (cor1 == Color.Black)
            {
                SpriteBatch.Draw(CaixaDeTexto, new Rectangle((int)vQ1.X, (int)vQ1.Y, (int)(linhas + arial.MeasureString(p1).X), ((int)arial.MeasureString(p1).Y)), Color.White);
                SpriteBatch.Draw(CaixaDeTexto, new Rectangle((int)vQ1.X, (int)vQ1.Y + (int)arial.MeasureString(p1).Y+arial.LineSpacing, (int)(linhas + arial.MeasureString(p1).X), arial.LineSpacing*RespostasQ1[Page]), Color.White);
                SpriteBatch.Draw(CaixaDeTexto, new Rectangle((int)vQ1.X, (((int)vQ1.Y + (int)arial.MeasureString(p1).Y + arial.LineSpacing) + (arial.LineSpacing * RespostasQ1[Page])), (int)(linhas + arial.MeasureString(p1).X), (int)arial.MeasureString(exp1).Y), Color.White);
                SpriteBatch.DrawString(arial, p1 +"\n"+ alt1[Page]+"\n"+exp1, vQ1, cor1);
            }
            else
            {
                SpriteBatch.DrawString(arial, "Questão não realizada", vQ1, Color.Black);
            }
            if (cor2 == Color.Black)
            {
                SpriteBatch.Draw(CaixaDeTexto, new Rectangle((int)vQ2.X, (int)vQ2.Y, (int)(linhas + arial.MeasureString(p2).X), ((int)arial.MeasureString(p2).Y)), Color.White);
                SpriteBatch.Draw(CaixaDeTexto, new Rectangle((int)vQ2.X, (int)vQ2.Y + (int)arial.MeasureString(p2).Y + arial.LineSpacing, (int)(linhas + arial.MeasureString(p2).X), arial.LineSpacing * RespostasQ2[Page]), Color.White);
                SpriteBatch.Draw(CaixaDeTexto, new Rectangle((int)vQ2.X, ((int)vQ2.Y + (int)arial.MeasureString(p2).Y + arial.LineSpacing) + (arial.LineSpacing * RespostasQ2[Page]), (int)(linhas + arial.MeasureString(p2).X), (int)arial.MeasureString(exp2).Y), Color.White);
                SpriteBatch.DrawString(arial, p2 + "\n" + alt2[Page] + "\n" + exp2, vQ2, cor2);
            }
            else
            {
                SpriteBatch.DrawString(arial, "Questão não realizada", vQ2, Color.Black);
            }
            if (cor3 == Color.Black)
            {
                if (Page != 2)
                {
                    SpriteBatch.Draw(CaixaDeTexto, new Rectangle((int)vQ3.X, (int)vQ3.Y, (int)(linhas + arial.MeasureString(p3).X), ((int)arial.MeasureString(p3).Y)), Color.White);
                    SpriteBatch.Draw(CaixaDeTexto, new Rectangle((int)vQ3.X, (int)vQ3.Y + (int)arial.MeasureString(p3).Y + arial.LineSpacing, (int)(linhas + arial.MeasureString(p3).X), arial.LineSpacing * RespostasQ3[Page]), Color.White);
                    SpriteBatch.Draw(CaixaDeTexto, new Rectangle((int)vQ3.X, ((int)vQ3.Y + (int)arial.MeasureString(p3).Y + arial.LineSpacing) + (arial.LineSpacing * RespostasQ3[Page]), (int)(linhas + arial.MeasureString(p3).X), (int)arial.MeasureString(exp3).Y), Color.White);
                }
                SpriteBatch.DrawString(arial, p3 + "\n" + alt3[Page] + "\n" + exp3, vQ3, cor3);
            }
            else
            {
                SpriteBatch.DrawString(arial, "Questão não realizada", vQ3, Color.Black);
            }
            if (cor4 == Color.Black)
            {
                if (Page != 2)
                {
                    SpriteBatch.Draw(CaixaDeTexto, new Rectangle((int)vQ4.X, (int)vQ4.Y, (int)(linhas + arial.MeasureString(p4).X), ((int)arial.MeasureString(p4).Y)), Color.White);
                    SpriteBatch.Draw(CaixaDeTexto, new Rectangle((int)vQ4.X, (int)vQ4.Y + (int)arial.MeasureString(p4).Y + arial.LineSpacing, (int)(linhas + arial.MeasureString(p4).X), (arial.LineSpacing * RespostasQ4[Page])), Color.White);
                    SpriteBatch.Draw(CaixaDeTexto, new Rectangle((int)vQ4.X, ((int)vQ4.Y + (int)arial.MeasureString(p4).Y + arial.LineSpacing) + (arial.LineSpacing * RespostasQ4[Page]), (int)(linhas + arial.MeasureString(p4).X), (int)arial.MeasureString(exp4).Y), Color.White);
                }
                SpriteBatch.DrawString(arial, p4 + "\n" + alt4[Page] + "\n" + exp4, vQ4, cor4);
            }
            else
            {
                SpriteBatch.DrawString(arial, "Questão não realizada", vQ4, Color.Black);
            }

            if (Page > 0)
            {
                SpriteBatch.Draw(Voltar[VoltarIndice], vVoltar, Color.White);
            }
            if (Page < 2)
            {
                SpriteBatch.Draw(BtAvancar[indiceDoAvancar], vBtAvancar, Color.White);
            }
            SpriteBatch.Draw(BTx, vBTx, Color.White);
            SpriteBatch.End();
        }
        public override void LoadContent()
        {
            if (!contentLoaded)
            {
                arial = parent.Content.Load<SpriteFont>("Fonte/caderno");
                livro = parent.Content.Load<Texture2D>("Imagem/Book");
                BtAvancarN = parent.Content.Load<Texture2D>("Imagem/Botao_Avancar");
                BtAvancarH = parent.Content.Load<Texture2D>("Imagem/Botao_Avancar_Sel");
                VoltarNormal = parent.Content.Load<Texture2D>("Imagem/Botao_Voltar");
                VoltarOver = parent.Content.Load<Texture2D>("Imagem/Botao_Voltar_Sel");
                CaixaDeTexto = parent.Content.Load<Texture2D>("Imagem/textBoxCaderno");
                BTx = parent.Content.Load<Texture2D>("Imagem/ui/historinha/Botao_X");
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


            }            
        }

        public override void ExitState()
        {
            if (!enteringState)
            {
                base.ExitState();
                parent.ExitState(ID);

            }
        }
        #endregion
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
