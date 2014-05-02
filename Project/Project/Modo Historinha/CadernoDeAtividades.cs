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
        bool contentLoaded = false;
        int Page = 0;
        SpriteFont arial;
        Texture2D livro;
        Vector2 vlivro, vQ1, vQ2, vQ3, vQ4;
        string p1, p2, p3, p4, altQues1, altQues2, altQues3, altQues4, altQues5, altQues6, altQues7, altQues8, altQues9, altQues10;
        string[] q1, q2, q3, q4,alt1,alt2,alt3,alt4;
        string Questão1, Questão2, Questão3, Questão4, Questão5, Questão6, Questão7, Questão8, Questão9, Questão10;
        int linhas = 1;
        int barra1 = 0;
        int barra2 = 0;
        int barra3 = 0;
        int barra4 = 0;
        Color cor1, cor2, cor3, cor4;
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
                q1 = new string[3] { questions.pergunta00, questions.pergunta40, questions.Pergunta9 };
                q2 = new string[3] { questions.pergunta10, questions.pergunta6, questions.Pergunta010 };
                q3 = new string[3] { questions.pergunta20, questions.Pergunta7, "Não tem mais questão." };
                q4 = new string[3] { questions.pergunta30, questions.Pergunta8, "Não tem mais questão." };
                altQues1 = "\na)"+questions.alternativa00 + "\nb)"+ questions.alternativa01;
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
                
                vlivro = new Vector2(20, 30);
                vQ1 = new Vector2(vlivro.X + 10, vlivro.Y + 20);
                vQ2 = new Vector2(vQ1.X, vQ1.Y + 400);
                vQ3 = new Vector2(vQ1.X + 500, vQ1.Y);
                vQ4 = new Vector2(vQ3.X, vQ2.Y);
                
                
                base.Initialize();
            }
        }
        Keys lastKey = Keys.A;
        public override void Update(GameTime tempo)
        {
            if (p1.Length != q1[Page].Length+barra1)
            {
                for (int i = 0; i < q1[Page].Length; i++)
                {
                    if (i % 10 * linhas == 0)
                    {
                        if (q1[Page][i] == ' ')
                        {
                            p1 += "\n";
                            barra1++;
                            linhas = 0;
                        }
                        else
                        {
                            linhas++;
                        }
                    }
                    if (i == q1[Page].Length - 1)
                    {
                        linhas = 0;
                    }

                    p1 += q1[Page][i];
                }
            }
            if (p2.Length != q2[Page].Length + barra2)
            {
                for (int i = 0; i < q2[Page].Length; i++)
                {
                    if (i % 10 * linhas == 0)
                    {
                        if (q2[Page][i] == ' ')
                        {
                            p2 += "\n";
                            barra2++;

                        }
                        else
                        {
                            linhas++;
                        }
                    }
                    if (i == q2[Page].Length - 1)
                    {
                        linhas = 0;
                    }
                    p2 += q2[Page][i];
                }
            }
            if (p3.Length != q3[Page].Length + barra3)
            {
                for (int i = 0; i < q3[Page].Length; i++)
                {
                    if (i % 10 * linhas == 0)
                    {
                        if (q3[Page][i] == ' ')
                        {
                            p3 += "\n";
                            barra3++;

                        }
                        else
                        {
                            linhas++;
                        }
                    }
                    if (i == q3[Page].Length - 1)
                    {
                        linhas = 0;
                    }
                    p3 += q3[Page][i];
                }
            }
            if (p4.Length != q4[Page].Length + barra4)
            {
                for (int i = 0; i < q4[Page].Length; i++)
                {
                    if (i % 10 * linhas == 0)
                    {
                        if (q4[Page][i] == ' ')
                        {
                            p4 += "\n";
                            barra4++;

                        }
                        else
                        {
                            linhas++;
                        }
                    }
                    if (i == q4[Page].Length - 1)
                    {
                        linhas = 0;
                    }
                  
                    p4 += q4[Page][i];
                }
            }
            KeyboardState teclado = Keyboard.GetState();
            if (teclado.IsKeyDown(Keys.Back))
            {
                questions.Cad(false);
                ExitState();
                
            }
            if ((teclado.IsKeyDown(Keys.Right)) && (lastKey != Keys.Right))
            {
                Page++;
                p1 = "";
                p2 = "";
                p3 = "";
                p4 = "";
                barra1 = 0;
                barra2 = 0;
                barra3 = 0;
                barra4 = 0;
                
                if (Page >= 2)
                {
                    Page = 2;
                }

            }
            if ((teclado.IsKeyDown(Keys.Left)) && (lastKey != Keys.Left))
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
                if (Page <= 0)
                {
                    Page = 0;
                }

            }
            Keys[] ks = teclado.GetPressedKeys();

            if (ks.Length == 0) lastKey = Keys.A;
            else lastKey = ks[0];
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
                SpriteBatch.DrawString(arial, p1 + alt1[Page], vQ1, cor1);
            }
            else
            {
                SpriteBatch.DrawString(arial, "Questão não feita", vQ1, Color.Black);
            }
            if (cor2 == Color.Black)
            {
                SpriteBatch.DrawString(arial, p2 + alt2[Page], vQ2, cor2);
            }
            else
            {
                SpriteBatch.DrawString(arial, "Questão não feita", vQ2, Color.Black);
            }
            if (cor3 == Color.Black)
            {
                SpriteBatch.DrawString(arial, p3 + alt3[Page], vQ3, cor3);
            }
            else
            {
                SpriteBatch.DrawString(arial, "Questão não feita", vQ3, Color.Black);
            }
            if (cor4 == Color.Black)
            {
                SpriteBatch.DrawString(arial, p4 + alt4[Page], vQ4, cor4);
            }
            else
            {
                SpriteBatch.DrawString(arial, "Questão não feita", vQ4, Color.Black);
            }           
                    
                
            
            SpriteBatch.End();
        }
        protected override void LoadContent()
        {
            if (!contentLoaded)
            {
                arial = parent.Content.Load<SpriteFont>("Fonte/Arial");
                livro = parent.Content.Load<Texture2D>("Imagem/Book");
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
                


            }
            
        }
        public override void EnterState()
        {
            EnterState(FreezeBelow);
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
