using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using game_states;
using Microsoft.Xna.Framework.Input;
using Project;
using Microsoft.Xna.Framework.Media;


class Episodio01 : GameState
{
    #region Cena 01
    #region Dialogo 01
    string fala01 = "Era uma vez, em uma pequena cidade... Dois irmãos muito parecidos por serem gêmeos. O menino se chama Cosme, e o seu sonho é um dia se tornar um grande engenheiro.";
    string fala02 = "E a menina se chama Maria. Seu sonho é lançar um livro com muitas histórias, que farão mais pessoas sonharem com as maravilhas do mundo.";
    #endregion
    #region Exercicio 01 - Modo Historinha
    string pergunta00 = "Cosme e Maria nasceram juntos, por isso os dois tem a mesma idade: sete anos. Espera, qual dos dois números é o número sete?";
    string alternativa00 = "7";//certo
    string alternativa01 = "1";//errado

    #endregion
    #region Dialogo 02
    string fala03 = "Muito bem! Apesar dos dois terem a mesma idade, Cosme gosta de dizer que ele é o irmão mais velho porque ele é um pouquinho mais alto que Maria.";
    string fala04 = "Por outro lado... Maria diz que é a mais velha porque ela precisa cuidar de Cosme toda vez que ele causa alguma confusão, como se fosse sua irmã mais velha!";
    string fala05 = "Epa... Acho que entramos em um assunto delicado para os irmãos. Vamos falar de outra coisa. Cosme e Maria tem alguns amigos que adoram jogar futebol!";
    string fala06 = "Os dois são amigos muito queridos. A menina se chama Serafina. Ela adora animais e tem um pequeno cachorro de estimação que adora levar para passear.";
    string fala07 = "O menino se chama Apuã. Ele gosta de visitar Cosme e Maria para brincar, sempre vindo com uma brincadeira diferente!";
    #endregion
    #region Exercicio 02 - Modo Historinha
    string pergunta10 = "Esses são os amigos que formam a turma de Cosme e Maria. A turma é composta por Cosme, Maria, Apuã e Serafina. Quantas pessoas existem na turma?";
    string alternativa02 = "4";//certo
    string alternativa03 = "2";//errado
    string alternativa10 = "3";//errado

    #endregion
    #region Dialogo 03
    string fala08 = "Dessa vez, Apuã quer convidar todo mundo para brincar de futebol. Uma partida entre duas equipes! Cosme e Maria contra Apuã e Serafina.";
    string fala09 = "Para organizar as equipes, Cosme entregou um número para cada amigo. Maria ganhou o número 3;  Apuã ganhou o número 1;Serafina ganhou o número 2; Cosme ganhou o número 4.";
    #endregion
    #region Exercicio 03 - Modo Historinha
    string pergunta20 = "Qual a ordem certa que eles devem ficar para dividir os times?";
    string alternativa20 = "Maria, 3";
    string alternativa21 = "Apuã, 1";
    string alternativa22 = "Serafina, 2";
    string alternativa23 = "Cosme, 4";
    #endregion
    #region Dialogo 04
    string fala10 = "Espera, há algo de errado com o que Apuã está pensando!";
    #endregion
    #region Exercicio 04 - Modo Historinha
    string pergunta30 = "Um dos times tem mais jogadores do que o outro. Qual time tem mais jogadores?";
    string alternativa30 = "TimeA";
    string alternativa31 = "TimeB";
    #endregion
    #region Exercicio 05 - Modo Historinha
    string pergunta40 = "Apuã está dizendo que Cosme, Serafina e ele joguem contra Maria. Mas os times de futebol sempre devem ter a mesma quantidade de pessoas! Quantas pessoas devem ficar em cada time?";
    string alternativa40 = "2";
    string alternativa41 = "3";
    string alternativa42 = "5";
    #endregion
    #region Dialogo 05
    string fala11 = "Maria percebe que Apuã estava tentando enganar a todos. Apuã pede desculpas e diz que estava apenas brincando.";
    string fala12 = "Apuã treinou bastante algumas técnicas de futebol conhecidas como firulas. Passar a bola por entre as pernas do adversário; chutar a bola por cima... Dessa vez, Apuã diz que não perderá para os irmãos gêmeos!";
    string fala13 = "Cosme não quer perder para Apuã! Cosme pede para Maria encontra-lo no campo de futebol que fica próximo ao rio. Antes que Maria possa respondê-lo, ele sai da casa, ansioso pela partida!";
    string fala14 = "O que Maria não conseguiu falar para Cosme é que ela precisa buscar água no rio! Vamos rápido Maria, para que dê tempo de jogar futebol!";
    #endregion
    #endregion
    #region Bate-Bola
    #endregion
    #region Variaveis
    #region Dialogos - strings
    string[] dialogo01;
    string[] dialogo02;
    string[] dialogo03;
    string[] dialogo04;
    string[] dialogo05;
    #endregion
    #region Criada as questões
    ModoHistorinha Exercicio01, Exercicio02, Exercicio03, Exercicio04, Exercicio05;
    #endregion
    #region Variaveis para o funcionamento da episódio
    int Segundos = 0;
    #region Cena 01
    bool parte1 = false;
    bool parte2 = false;
    bool parte3 = false;
    bool parte4 = false;
    bool parte5 = false;
    bool exercicio1 = false;
    bool exercicio2 = false;
    bool exercicio3 = false;
    bool exercicio4 = false;
    bool exercicio5 = false;
    bool primeiro = false;
    bool segundo = false;
    bool terceiro = false;
    bool quarto = false;
    bool quinto = false;
    #endregion
    #endregion
    #region Incrementos para o Dialogo
    int Incremento0 = 0;
    
    #endregion
    #region RESTO
    SpriteFont arial;
    Vector2 posicaoText;
    
    int FalasPorSegundo = 10;
    int ritimo = 10;
    int mile;
    string texto = "";
    int indice = 0;
    bool pauseFlag;
    private bool contentLoaded;
    int CaixaTexto;
    int zerar = 1;
    Color cor;
    int selecionar = 0; //seleciona a musica
    bool repetir = true;
    int NoAlbum = 0;
    bool ModoExercicios = false;
    #endregion
    //-------------------------------------------//
    Song audio01, audio02, audio03, audio04, audio05, audio06, audio07, audio08, audio09, audio10, audio11, audio12, audio13, audio14, audio15;
    Song audioEx01, audioEx02, audioEx03, audioEx04, audioEx05;
    List<Song> Album1, Album2, Album3, Album4, Album5;
    List<List<Song>> AlbumPrincipal;
    #endregion

    // Desça o código até Initialize
    public Episodio01(int id, Game1 parent)
        : base(id, parent)
    {
        FimDaHistoria = false;
        Initialize();

    }
    protected override void Initialize()
    {

        if (!initialized)
        {
            LoadContent();
            base.Initialize();
            #region Vetores
            posicaoText = new Vector2(20, 600);
            #endregion
            #region Dialogos
            dialogo01 = new string[2] { fala01, fala02 };
            dialogo02 = new string[5] { fala03, fala04, fala05, fala06, fala07 };
            dialogo03 = new string[2] { fala08, fala09 };
            dialogo04 = new string[1]{ fala10};
            dialogo05 = new string[4] { fala11, fala12, fala13, fala14 };
            #endregion
            #region Exercicios
            #region Cena 01
            Exercicio01 = new ModoHistorinha(parent.Content, pergunta00, alternativa00, alternativa01, 1, arial, 2,audioEx01);
            Exercicio02 = new ModoHistorinha(parent.Content, pergunta10, alternativa02, alternativa03, alternativa10, 1, arial, 3,audioEx02);
            Exercicio03 = new ModoHistorinha(parent.Content, pergunta20, alternativa20, alternativa21, alternativa22, alternativa23, 1, arial, 4,audioEx03, true);
            Exercicio04 = new ModoHistorinha(parent.Content, pergunta30, alternativa30, alternativa31, 1, arial, 2, audioEx04);
            Exercicio05 = new ModoHistorinha(parent.Content, pergunta40, alternativa40, alternativa41, alternativa42, 1, arial, 3,audioEx05);
            #endregion
            #endregion
            #region Transições
            enterTransitionDuration = 50;
            exitTransitionDuration = 50;
            #endregion
            #region Narrações
            #region Cena 01
            Album1 = new List<Song>() { audio01, audio02, audio03 };
            Album2 = new List<Song>() { audio04, audio05, audio06, audio07, audio08 };
            Album3 = new List<Song>() { audio09, audio10 };
            Album4 = new List<Song>() { audio11 };
            Album5 = new List<Song>() { audio12, audio13, audio14, audio15 };
            #endregion
            AlbumPrincipal = new List<List<Song>>() { Album1, Album2, Album3, Album4, Album5 };
            #endregion

        }
    }
    public override void Update(GameTime tempo)
    {
        //if (!pauseFlag)
        //{

            base.Update(tempo);
            //MediaPlayer.IsRepeating = false;
            //////////////////////////////////////////////////////////////////Aqui é onde a narração vai acontecer////////////////////////////////////////////////
            
            if (repetir && selecionar < AlbumPrincipal[NoAlbum].Count && !ModoExercicios)//Aqui eu vou verificar se tem algum audio rodando, se não  tiver eu toco uma nova narração.
            {
                MediaPlayer.Play(AlbumPrincipal[NoAlbum][selecionar]);//Narração.
                repetir = false;//Repetir serve para não deixar a música tocar sem parar
                
            }
            if (AlbumPrincipal[NoAlbum][selecionar].Duration == MediaPlayer.PlayPosition && !ModoExercicios)//Se a narração chegou ao tempo final
            {
                if (AlbumPrincipal[NoAlbum].Count == selecionar + 1)//Se o Album que está tocando chegou a sua ultima música
                {
                    NoAlbum += NoAlbum < AlbumPrincipal.Count ? 1 : 0;//Trocar de album
                    selecionar = 0;//eu zero o contador de musicas
                    Incremento0++;// Incremento mais 1 na variavel que permite que o texto continue

                }
                else
                {
                    selecionar += selecionar < AlbumPrincipal[NoAlbum].Count ? 1 : 0;//Ele troca de música
                }
                repetir = true;
            }
            if (KeyboardHelper.IsKeyDown(Keys.Z))
            {
                KeyboardHelper.LockKey(Keys.Z);
                if (AlbumPrincipal[NoAlbum].Count == selecionar + 1)//Se o Album que está tocando chegou a sua ultima música
                {
                    NoAlbum += NoAlbum < AlbumPrincipal.Count ? 1 : 0;//Trocar de album
                    selecionar = 0;//eu zero o contador de musicas
                    Incremento0++;// Incremento mais 1 na variavel que permite que o texto continue

                }
                else
                {
                    selecionar += selecionar < AlbumPrincipal[NoAlbum].Count ? 1 : 0;//Ele troca de música
                }
                repetir = true;
            }
            //////////////////////////////////////Fim da Parte de Narração//////////////////////////////////////////////////////////////////
            if (stateEntered)
            {
                CaixaTexto = (int)arial.MeasureString(texto).X * zerar;
                if (CaixaTexto > 800 &&  texto[indice-1] == ' ')//Quando o texto atingir um limite da tela e tiver um espaço em branco ele pula uma linha;
                {
                    texto += "\n";
                    zerar = 0;
                    
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
            }

        //}
    }

    public override void Draw(GameTime gameTime)
    {
        

        string Tmusica = MediaPlayer.PlayPosition.Minutes.ToString() + " : " + MediaPlayer.PlayPosition.Seconds;
        string TpLAYER = AlbumPrincipal[NoAlbum][selecionar].Duration.Minutes.ToString() + " : " + AlbumPrincipal[NoAlbum][selecionar].Duration.Seconds;
        string NomeMusica = AlbumPrincipal[NoAlbum][selecionar].Name;
        SpriteBatch.Begin();
        cor = Color.White * Alpha;
        SpriteBatch.DrawString(arial, "Help key = [z]", new Vector2(200, 720), cor);
        SpriteBatch.DrawString(arial, Tmusica + " / " + TpLAYER + " = " + NomeMusica, new Vector2(200, 700), cor);//Aqui eu vejo em quanto tempo está a narração
        //Aqui em cima eu imprimo o tempo total da musica;
        //*
        
        #region Desenho
        //Irei explicar a primeira parte, pois as seguintes são iguais.
        if (!primeiro)//Primeira parte do jogo possui uma sequencia de dialogos e um exercicio.
        {
            if (Incremento0 == dialogo01.Length)//Verifico se o dialogo chegou ao seu fim
            {
                Console.Beep();//Isso é só para testes
                parte1 = true;//Se chegou a parte 1 termina

            }
            
            if (!parte1)//S e a parte um não terminou
            {
                
                
                if (!pauseFlag)//Verifica se não está em Pause
                {

                    if (indice < dialogo01[Incremento0].Length) { texto += dialogo01[Incremento0][indice]; }//Verifica se o texto impresso tem menos letras que o dialogo, se verdadeiro coloca mais uma letra;
                    indice = indice + (indice < dialogo01[Incremento0].Length ? 1 : 0);//Aumento o indice para que sempre vá para próxima letra na hora de imprimir.
                    if (texto.Length == dialogo01[Incremento0].Length +1)//Se o texto impresso tiver o mesmo numero de letras do dialogo...O MAIS UM É POR CAUSA DO \N QUE EU COLOCO AUTOMATICAMENTE
                    {
                        //mile += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        //Segundos = mile / 1000;
                        
                        if (selecionar >1 )//Como a primeira Narração está quebrada em dois audios, eu falei que ele só vai mudar de texto a partir do segundo audio.
                        {
                            if (repetir)//Se quando puder mudar de audio
                            {
                                
                                Incremento0++;//Mudo o texto
                                indice = 0;//Falo que o numero de letras é zero;
                                zerar = 1;//Falo que agora ele poderar pular uma linha no texto, caso a condição seja feita.(Vá no Update para ver a condição)
                                texto = "";//O texto a ser impresso agora não possui nada;
                                //if (Incremento0 == 2) // Isso é só para testes
                                //{
                                //    Console.Beep();
                                //}
                                //FalasPorSegundo += ritimo;
                                
                            }
                        }
                    }
                }
                
                SpriteBatch.DrawString(arial, texto, posicaoText, cor);//Imprimir texto
                
            }

            if (parte1 && !exercicio1)//Caso a primeira parte tenha chegado ao fim, mas o exercicio não foi feito.
            {
                
                texto = "";//Reseto tudo, como foi feito acima
                indice = 0;//
                Incremento0 = 0;// Estou usando o mesmo incremento para todas os dialogos;
                zerar = 1;//
                //mile = 0;
                //FalasPorSegundo = ritimo;
                Exercicio01.Atualizar();//Eu chamo o atualizar do exercicio
                Exercicio01.Desenhar(SpriteBatch);//Desenho ele na tela
                exercicio1 = Exercicio01.Continuar();// Caso o jogador vença, O Continuar() vai retornar verdadeiro, caso o contrario falso.
                repetir = exercicio1;// Se repetir for falso, nenhum audio será tocado.
                primeiro = exercicio1;//Caso o jogador vença o exercicio ele termina a primeira parte.
            }
        }

        if (primeiro && !segundo)
        {
            if (Incremento0 == dialogo02.Length)
            {
                parte2 = true;

            }
            if (!parte2)
            {
                
                if (!pauseFlag)
                {
                    if (indice < dialogo02[Incremento0].Length) { texto += dialogo02[Incremento0][indice]; }
                    indice = indice + (indice < dialogo02[Incremento0].Length ? 1 : 0);
                    if (texto.Length == dialogo02[Incremento0].Length + 1)
                    {
                        mile += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        Segundos = mile / 1000;
                        if (repetir)
                        {

                            Incremento0++;
                            indice = 0;
                            zerar = 1;
                            texto = "";
                            FalasPorSegundo += ritimo;
                            
                        }

                    }
                }
                SpriteBatch.DrawString(arial, texto, posicaoText, cor);
                
            }
            if (!exercicio2 && parte2)
            {
                texto = "";
                indice = 0;
                Incremento0 = 0;
                zerar = 1;
                mile = 0;
                FalasPorSegundo = ritimo;
                Exercicio02.Atualizar();
                Exercicio02.Desenhar(SpriteBatch);
                exercicio2 = Exercicio02.Continuar();
                repetir = exercicio2;
                segundo = exercicio2;
            }

        }
        if (primeiro && segundo && !terceiro)
        {
            if (Incremento0 == dialogo03.Length)
            {
                parte3 = true;

            }
            if (!parte3)
            {
                ModoExercicios = false;
                if (!pauseFlag)
                {
                    if (indice < dialogo03[Incremento0].Length) { texto += dialogo03[Incremento0][indice]; }
                    indice = indice + (indice < dialogo03[Incremento0].Length ? 1 : 0);
                    if (texto.Length == dialogo03[Incremento0].Length + 1)
                    {
                        //mile += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        //Segundos = mile / 1000;
                        if (repetir)
                        {

                            Incremento0++;
                            indice = 0;
                            zerar = 1;
                            texto = "";
                            FalasPorSegundo += ritimo;
                            
                        }
                    }
                }
                SpriteBatch.DrawString(arial, texto, posicaoText, cor);
                
            }
            if (parte3 && !exercicio3)
            {
                texto = "";
                indice = 0;
                Incremento0 = 0;
                zerar = 1;
                mile = 0;
                Exercicio03.Atualizar();
                Exercicio03.Desenhar(SpriteBatch);
                exercicio3 = Exercicio03.Continuar();
                repetir = exercicio3;
                terceiro = exercicio3;
            }

        }
        if (primeiro && segundo && terceiro && !quarto)
        {
            if (Incremento0 == dialogo04.Length)
            {
                parte4 = true;

            }
            if (!parte4)
            {
                ModoExercicios = false;
                if (!pauseFlag)
                {
                    if (indice < dialogo04[Incremento0].Length) { texto += dialogo04[Incremento0][indice]; }
                    indice = indice + (indice < dialogo04[Incremento0].Length ? 1 : 0);
                    if (texto.Length == dialogo04[Incremento0].Length + 1)
                    {
                        //mile += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        //Segundos = mile / 1000;
                        if (repetir)
                        {

                            Incremento0++;
                            indice = 0;
                            zerar = 1;
                            texto = "";
                            FalasPorSegundo += ritimo;

                        }
                    }
                }
                SpriteBatch.DrawString(arial, texto, posicaoText, cor);

            }
            if (parte4 && !exercicio4)
            {
                texto = "";
                indice = 0;
                Incremento0 = 0;
                zerar = 1;
                mile = 0;
                Exercicio04.Atualizar();
                Exercicio04.Desenhar(SpriteBatch);
                exercicio4 = Exercicio04.Continuar();
                repetir = false;
                quarto = exercicio4;
            }

        }

        if (primeiro && segundo && terceiro && quarto &&!quinto)
        {
            mile += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            Segundos = mile / 1000;
            if (Segundos > 1)
            {
                if (!exercicio5 && !parte5)
                {

                    mile = 0;
                    Exercicio05.Atualizar();
                    Exercicio05.Desenhar(SpriteBatch);
                    exercicio5 = Exercicio05.Continuar();
                    repetir = exercicio5;
                }
                if (Incremento0 == dialogo05.Length && exercicio5)
                {
                    texto = "";
                    indice = 0;
                    Incremento0 = 0;
                    zerar = 1;
                    parte5 = true;
                    quinto = true;

                }
                if (!parte5 && exercicio5)
                {
                    if (!pauseFlag)
                    {
                        if (indice < dialogo05[Incremento0].Length) { texto += dialogo05[Incremento0][indice]; }
                        indice = indice + (indice < dialogo05[Incremento0].Length ? 1 : 0);
                        if (texto.Length == dialogo05[Incremento0].Length + 1)
                        {
                            //mile += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                            //Segundos = mile / 1000;
                            if (repetir)
                            {

                                Incremento0++;
                                indice = 0;
                                zerar = 1;
                                texto = "";
                                FalasPorSegundo += ritimo;

                            }
                        }
                    }
                    SpriteBatch.DrawString(arial, texto, posicaoText, cor);
                }
            }
        }
        if (primeiro && segundo && terceiro && quarto && quinto)
        {
            FimDaHistoria = true;
            
        }
        #endregion
        //*/
        SpriteBatch.End();
    }
    protected override void LoadContent()
    {
        if (!contentLoaded)
        {
            arial = parent.Content.Load<SpriteFont>("Fonte/Arial");
            FalasDoNarrador();
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
    void FalasDoNarrador()
    {
        #region Falas
        #region Cena01
        #region Parte 1
        audio01 = parent.Content.Load<Song>("Narrar/Cena01/Parte 1 - Fala 1");
        audio02 = parent.Content.Load<Song>("Narrar/Cena01/Parte 1 - Fala 2");
        audio03 = parent.Content.Load<Song>("Narrar/Cena01/Parte 1 - Fala 3");
        #endregion
        #region Parte 2
        audio04 = parent.Content.Load<Song>("Narrar/Cena01/Parte 2 - Fala 5");
        audio05 = parent.Content.Load<Song>("Narrar/Cena01/Parte 2 - Fala 6");
        audio06 = parent.Content.Load<Song>("Narrar/Cena01/Parte 2 - Fala 7");
        audio07 = parent.Content.Load<Song>("Narrar/Cena01/Parte 2 - Fala 8");
        audio08 = parent.Content.Load<Song>("Narrar/Cena01/Parte 2 - Fala 9");
        #endregion
        #region Parte 3
        audio09 = parent.Content.Load<Song>("Narrar/Cena01/Parte 3 - Fala 11");
        audio10 = parent.Content.Load<Song>("Narrar/Cena01/Parte 3 - Fala 12");
        #endregion
        #region Parte 4
        audio11 = parent.Content.Load<Song>("Narrar/Cena01/Parte 4 - Fala 14");
        #endregion
        #region Parte 5
        audio12 = parent.Content.Load<Song>("Narrar/Cena01/Parte 5 - Fala 15");
        audio13 = parent.Content.Load<Song>("Narrar/Cena01/Parte 5 - Fala 16");
        audio14 = parent.Content.Load<Song>("Narrar/Cena01/Parte 5 - Fala 17");
        audio15 = parent.Content.Load<Song>("Narrar/Cena01/Parte 5 - Fala 18");
        #endregion
        #endregion
        #endregion
        #region Exercicios
        #region Cena01
        audioEx01 = parent.Content.Load<Song>("Narrar/Cena01/Exercicio01");
        audioEx02 = parent.Content.Load<Song>("Narrar/Cena01/Exercicio02");
        audioEx03 = parent.Content.Load<Song>("Narrar/Cena01/Exercicio03");
        audioEx04 = parent.Content.Load<Song>("Narrar/Cena01/Exercicio04");
        audioEx05 = parent.Content.Load<Song>("Narrar/Cena01/Exercicio05");
        #endregion
        #endregion

    }
    public bool FimDaHistoria{ get; set; }


}

