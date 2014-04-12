using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Projeto_Historinha;
using game_states;
using Microsoft.Xna.Framework.Input;


class Episodio01 : GameState
{
    #region Dialogo 01
    string fala01 = "Era uma vez, em uma pequena cidade... Dois irmãos muito parecidos por serem gêmeos.\n O menino se chama Cosme, e o seu sonho é um dia se tornar um grande engenheiro.";
    string fala02 = "E a menina se chama Maria. Seu sonho é lançar um livro\ncom muitas histórias, que farão mais pessoas sonharem com as maravilhas do mundo.";
    #endregion
    #region Exercicio 01 - Modo Historinha
    string pergunta00 = "Cosme e Maria nasceram juntos, por isso os dois tem\na mesma idade: sete anos. Espera, qual dos dois números é o número sete?";
    string alternativa00 = "7";//certo
    string alternativa01 = "1";//errado
    
    #endregion
    #region Dialogo 02
    string fala03 = "Muito bem! Apesar dos dois terem a mesma idade, Cosme gosta de dizer que ele é o irmão mais\nvelho porque ele é um pouquinho mais alto que Maria.";
    string fala04 = "Por outro lado... Maria diz que é a mais velha porque ela precisa\ncuidar de Cosme toda vez que ele causa alguma confusão, como se fosse sua irmã mais velha!";
    string fala05 = "Epa... Acho que entramos em um assunto delicado para os irmãos. Vamos falar de outra coisa.\nCosme e Maria tem alguns amigos que adoram jogar futebol!";
    string fala06 = "Os dois são amigos muito queridos. A menina se chama Serafina.\nEla adora animais e tem um pequeno cachorro de estimação que adora levar para passear.";
    string fala07 = "O menino se chama Apuã. Ele gosta de visitar Cosme e Maria\npara brincar, sempre vindo com uma brincadeira diferente!";
    //string fala08 = "Maria tem duas estantes com livros em casa. Ela sempre deixa a bola escondida na\nestante com o maior numero de livros; qual das duas estantes tem mais livros?";
    //string fala09 = "Cosme e o resto da turma esperavam a chegada de Maria para o campinho. Eles nao podem jogar futebol sem a bola.\nMaria explica que quer participar do jogo tambem, mas precisa levar agua ate sua casa.";
    #endregion
    #region Exercicio 02 - Modo Historinha
    string pergunta10 = "Esses são os amigos que formam a turma de Cosme e Maria.\nNo total, quantas pessoas formam a turma de Cosme e Maria?";
    string alternativa02 = "2";//certo
    string alternativa03 = "4";//errado
    string alternativa10 = "3";//errado
    //string alternativa11 = "BOLA DE FUTEBOL";//Errado
    //string alternativa12 = "BALDE COM aGUA";//Errado
    //string alternativa13 = "CASA";//Errado
    #endregion
    #region Dialogo 03
    string fala10 = "Maria concorda em participar do jogo de futebol. Mas jogaria so um pouco!\nDepois, ela iria levar a agua para a casa.";
    string fala11 = "O grupo de amigos quer jogar futebol, mas eles precisam se preparar para o jogo.\nMaria ja trouxe a sua bola, mas eles precisam organizar o time. Quantas pessoas vao jogar futebol hoje?";
    string fala13 = "Cosme adora futebol. Ele conta para seus amigos do seu idolo do futebol, que joga na selecao brasileira.\nTodos querem saber quem e o idolo, mas Cosme nao quer falar.";
    #endregion
    #region Exercicio 03 - Modo Historinha
    string pergunta20 = "Cosme entao diz que seu idolo sempre anda vestindo um sapato especial para o futebol,\nchamado chuteira. Qual desses objetos e uma chuteira?";
    string alternativa20 = "CHUTEIRA";
    string alternativa21 = "CHALEIRA";
    string alternativa22 = "CHAFARIZ";
    string alternativa23 = "CHICLETE";
    #endregion
    #region Bate-Bola
    #endregion
    #region Variaveis
    //--------------------------------------------------------------//
    string[] dialogo01;
    string[] dialogo02;
    string[] dialogo03;
    //--------------------------------------------------------------//
    ModoHistorinha Exercicio01, Exercicio02, Exercicio03;
    //--------------------------------------------------------------//
    int Segundos = 0;
    bool parte1 = false;
    bool parte2 = false;
    bool parte3 = false;
    bool exercicio1 = false;
    bool exercicio2 = false;
    bool exercicio3 = false;
    bool primeiro = false;
    bool segundo = false;
    bool terceiro = false;
    //--------------------------------------------------------------//
    int Incremento0 = 0;
    int Incremento1 = 0;
    int Incremento2 = 0;
    //--------------------------------------------------------------//
    SpriteFont arial;
    Vector2 posicaoText;
    SpriteBatch spritebatch;
    int FalasPorSegundo = 5;
    int ritimo = 5;
    int mile;
    string texto = "";
    int indice = 0;
    #endregion

   // Desça o código até Initialize
    public Episodio01(int id, Game1 parent)
        : base(id, parent)
    {
        #region Inicializacao

        
        
        spritebatch = new SpriteBatch(parent.GraphicsDevice);
        #endregion
        

    }
    protected override void Initialize()
    {

        if (!initialized)
        {
            
            //Aqui você muda a posição do texto;
            posicaoText = new Vector2(20, 600);
            dialogo01 = new string[2] { fala01, fala02 };
            dialogo02 = new string[5] { fala03, fala04, fala05, fala06, fala07 };
            dialogo03 = new string[3] { fala10, fala11, fala13 };
            Exercicio01 = new ModoHistorinha(parent.Content, pergunta00, alternativa00, alternativa01, 1, arial, 2);
            Exercicio02 = new ModoHistorinha(parent.Content, pergunta10, alternativa03, alternativa02, alternativa10, 1, arial, 3);
        }
    }
    public override void Update(GameTime tempo)
    {
        
        base.Update(tempo);
        if (stateEntered)
        {
           
        }
    }

    public override void Draw(GameTime gameTime)
    {
        spritebatch.Begin();

       
            spritebatch.DrawString(arial, Segundos.ToString(), new Vector2(200, 700), Color.Black);
            #region Desenho
            if (!primeiro)
            {
                if (!parte1)
                {
                    if (indice < dialogo01[Incremento0].Length) { texto += dialogo01[Incremento0][indice]; }
                    indice = indice + (indice < dialogo01[Incremento0].Length ? 1 : 0);
                    spritebatch.DrawString(arial, texto, posicaoText, Color.White);


                    if (texto.Length == dialogo01[Incremento0].Length)
                    {
                        mile += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        Segundos = mile / 1000;
                        if (Segundos != 0 && Segundos % FalasPorSegundo == 0)
                        {

                            Incremento0++;
                            indice = 0;
                            texto = "";
                            FalasPorSegundo += ritimo;
                            if (Incremento0 == dialogo01.Length)
                            {
                                parte1 = true;

                            }
                        }
                    }
                }

                if (parte1 && !exercicio1)
                {

                    mile = 0;
                    FalasPorSegundo = ritimo;
                    Exercicio01.Atualizar();
                    Exercicio01.Desenhar(spritebatch);
                    exercicio1 = Exercicio01.Continuar();
                    primeiro = exercicio1;
                }
            }

            if (primeiro && !segundo)
            {
                if (!parte2)
                {
                    if (indice < dialogo02[Incremento1].Length) { texto += dialogo02[Incremento1][indice]; }
                    indice = indice + (indice < dialogo02[Incremento1].Length ? 1 : 0);
                    spritebatch.DrawString(arial, texto, posicaoText, Color.White);
                    if (texto.Length == dialogo02[Incremento1].Length)
                    {
                        mile += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        Segundos = mile / 1000;
                        if (Segundos != 0 && Segundos % FalasPorSegundo == 0)
                        {

                            Incremento1++;
                            indice = 0;
                            texto = "";
                            FalasPorSegundo += ritimo;
                            if (Incremento1 == dialogo02.Length)
                            {
                                parte2 = true;

                            }
                        }

                    }
                }
                if (!exercicio2 && parte2)
                {
                    mile = 0;
                    FalasPorSegundo = ritimo;
                    Exercicio02.Atualizar();
                    Exercicio02.Desenhar(spritebatch);
                    exercicio2 = Exercicio02.Continuar();
                    segundo = exercicio2;
                }

            }
            //else
            //if (parte1 && parte2 && !parte3)
            //{
            //    spritebatch.DrawString(arial, dialogo03[Incremento2], posicaoText, Color.White);
            //    if ((int)Segundos % FalasPorSegundo == 0 && (int)Segundos > 0)
            //    {

            //        Incremento2++;
            //        FalasPorSegundo += FalasPorSegundo;
            //        if (Incremento2 == dialogo03.Length)
            //        {
            //            parte3 = true;

            //        }
            //    }
            //}
            //else
            if (primeiro && segundo)
            {
                spritebatch.DrawString(arial, "Fim da Demo", posicaoText, Color.White);
                if ((int)Segundos % FalasPorSegundo == 0 && (int)Segundos > 0)
                {

                }
            }
            #endregion
       
            spritebatch.End();
    }
    protected override void LoadContent()
    {
        arial = parent.Content.Load<SpriteFont>("Fonte/Verdana");
        
    }
    public override void EnterState()
    {
        if (!exitingState)
        {
            base.EnterState();
            LoadContent();
            Initialize();

        }
    }

    public override void ExitState()
    {
        if (!enteringState)
        {
            base.ExitState();
            parent.Content.Unload();
        }
    }
    
    
}

