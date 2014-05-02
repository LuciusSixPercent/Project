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
using Microsoft.Xna.Framework.Audio;


class Episodio01 : GameState
{
    //Lucki eu preferir arrurmar assim mesmo para saber em qual dialogo cada fala vai ficar.
    #region Cena 01
    #region Dialogo 01
    string fala01 = "Era uma vez, em uma pequena cidade... Dois irmãos muito parecidos por serem gêmeos. O menino se chama Cosme, e o seu sonho é um dia se tornar um grande engenheiro.";
    string fala02 = "A menina se chama Maria.Seu sonho é escrever um livro com muitas histórias que farão as crianças sonharem com as maravilhas do mundo.";
    #endregion
    #region Exercicio 01 - Modo Historinha
    public string pergunta00 = "Cosme e Maria nasceram juntos, por isso os dois tem a mesma idade: sete anos. Espera, qual dos dois números é o número sete?";
    public string alternativa00 = "7";//certo
    public string alternativa01 = "1";//errado

    #endregion
    #region Dialogo 02
    string fala03 = "Muito bem! Apesar dos dois terem a mesma idade, Cosme gosta de dizer que é o irmão mais velho porque ele é um pouquinho mais alto que Maria.";
    string fala04 = "Por outro lado... Maria diz que é a mais velha porque ela precisa cuidar de Cosme toda vez que ele causa alguma confusão, como se fosse sua irmã mais velha!";
    string fala05 = "Epa... Acho que esse é um assunto delicado para os irmãos. Vamos falar de outra coisa. Cosme e Maria tem alguns amigos que adoram jogar futebol.";
    string fala06 = "Os dois são amigos muito queridos. A menina se chama Serafina. Ela adora animais e tem um pequeno cachorro de estimação que adora levar para passear.";
    string fala07 = "O menino se chama Apuã. Ele gosta de visitar Cosme e Maria para brincar, sempre vindo com uma brincadeira diferente!";
    #endregion
    #region Exercicio 02 - Modo Historinha
    public string pergunta10 = "Esses são os amigos que formam a turma de Cosme e Maria. A turma é composta por Cosme, Maria, Apuã e Serafina. Quantas pessoas existem na turma?";
    public string alternativa02 = "4";//certo
    public string alternativa03 = "2";//errado
    public string alternativa10 = "3";//errado

    #endregion
    #region Dialogo 03
    string fala08 = "Dessa vez, Apuã quer convidar todo mundo para brincar de futebol. Uma partida entre duas equipes! Cosme e Maria contra Apuã e Serafina.";
    string fala09 = "Para organizar as equipes, Cosme entregou um número para cada amigo. Maria ganhou o número 3;  Apuã ganhou o número 1;Serafina ganhou o número 2; Cosme ganhou o número 4.";
    #endregion
    #region Exercicio 03 - Modo Historinha
    public string pergunta20 = "Qual a ordem certa que eles devem ficar para dividir os times?";
    public string alternativa20 = "Maria, 3";
    public string alternativa21 = "Apuã, 1";
    public string alternativa22 = "Serafina, 2";
    public string alternativa23 = "Cosme, 4";
    #endregion
    #region Dialogo 04
    string fala10 = "Espera, há algo de errado com o que Apuã está pensando!";
    #endregion
    #region Exercicio 04 - Modo Historinha
    public string pergunta30 = "Um dos times tem mais jogadores do que o outro... Qual dos times tem mais jogadores?";
    public string alternativa30 = "TimeA";
    public string alternativa31 = "TimeB";
    #endregion
    #region Exercicio 05 - Modo Historinha
    public string pergunta40 = "Apuã está dizendo que Cosme, Serafina e ele joguem contra Maria. Mas os times de futebol sempre devem ter a mesma quantidade de pessoas! Quantas pessoas devem ficar em cada time?";
    public string alternativa40 = "2";
    public string alternativa41 = "3";
    public string alternativa42 = "5";
    #endregion
    #region Dialogo 05
    string fala11 = "Maria percebe que Apuã estava tentando enganar a todos. Apuã pede desculpas e diz que estava apenas brincando.";
    string fala12 = "Apuã treinou bastante algumas técnicas de futebol conhecidas como firulas. Passar a bola por entre as pernas do adversário; chutar a bola por cima... Dessa vez, Apuã\n diz que não perderá para os irmãos gêmeos!";
    string fala13 = "Cosme não quer perder para Apuã! Cosme pede para Maria encontra-lo no campo de futebol que fica próximo ao rio. Antes que Maria possa respondê-lo, ele sai da casa, ansioso pela partida!";
    string fala14 = "O que Maria não conseguiu falar para Cosme é que ela precisa buscar água no rio! Vamos rápido Maria, para que dê tempo de jogar futebol!";
    #endregion

    #endregion
    #region  Cena 02
    #region Dialogo 6
    string fala15 = "Cosme e Maria precisam buscar água no rio quase todo dia. Hoje é a vez de Maria buscar a água. Ela sempre busca a água cedo pela manhã, mas como Apuã e Serafina os visitaram hoje, ela acabou se atrasando.";
    string fala16 = "Maria, o que aconteceu? Esqueceu-se de alguma coisa?";
    #endregion
    #region Exercicio06
    public string pergunta6 = "É algo que começa com a letra B? Vamos tentar escrever o que é! Qual é mesmo a letra B?";
    public string alt60 = "b";
    public string alt61 = "6";
    #endregion
    #region Dialogo 7
    string fala17 = "Então é uma palavra que começa com a letra B... Espera, por acaso seria algo para jogar futebol?";
    string fala18 = "Onde será que está o que você está procurando, Maria? Falando nisso... O que é que você está procurando? Eu sei que é algo usado para jogar futebol e começa com a letra B... É uma palavra com duas sílabas?";
    #endregion
    #region Exercicio07
    public string Pergunta7 = "Maria está procurando algo que começa com a letra B, tem duas sílabas e é usada no futebol... O que poderia ser?";
    public string alt70 = "BO-LA";//Certa
    public string alt71 = "BA-NA-NA";
    public string alt72 = "BO-LO";//Errado, mas com fala do narrador
    public string alt73 = "BE-RIN-GE-LA";
    #endregion
    //Exercicio especial, criar um novo construtor que atenda as necessidades.
    #region Dialogo 8
    string fala19 = "Maria não está conseguindo encontrar a bola. Hmm... Olhando bem, estou vendo a bola bem ali!";
    string fala20 = "Vamos ajudar Maria a encontra-la!";
    #endregion
    #region Exercicio08
    public string Pergunta8 = "Maria, a bola está atrás de um objeto que começa com a letra...";
    public string alt80 = "L";
    public string alt81 = "O";
    public string alt82 = "J";
    #endregion
    #region Dialogo 9
    string fala21 = "Agora que Maria está com a bola de futebol e o balde, ela pode ir até o rio buscar água e depois jogar com os seus amigos.";
    string fala22 = "Maria, veja só quem está próximo ao rio! É Cosme. Cosme está ansioso esperando Maria, porque ela está trazendo a bola de futebol.";
    string fala23 = "Maria precisa levar a água para casa, por isso não pode jogar futebol agora.";
    string fala24 = "Cosme diz para Maria buscar água no rio mais tarde. Para que eles possam jogar futebol, Maria deve participar do time também!";
    string fala25 = "Cosme promete que ele irá ajuda-la depois a buscar água.";
    #endregion
    #region Exercicio09
    public string Pergunta9 = "Cosme diz que Maria pode sempre contar com outras pessoas para ajuda-la porque ela é muito importante para eles. A palavra que Cosme quer falar tem três sílabas. De quem Cosme está falando?";
    public string alt90 = "A-MI-GOS";
    public string alt91 = "SOL";
    public string alt92 = "RI-A-CHO";
    #endregion
    #region Dialogo 10
    string fala26 = "Maria conColor.Whiteda em jogar com Cosme, pois sabe que seus amigos irão ajuda-la sempre que ela precisar!";
    #endregion
    #region Exercicio10
    public string Pergunta010 = "Agora, finalmente nós iremos jogar...";
    public string alt100 = "FU-TE-BOL";
    public string alt101 = "FUT-E-BOL";
    public string alt102 = "FUTE-BOL";
    #endregion
    #endregion
    #region Variaveis
    #region Dialogos - strings
    string[] dialogo01, dialogo02, dialogo03, dialogo04, dialogo05, dialogo06, dialogo07, dialogo08, dialogo09, dialogo10;
    #endregion
    #region Criada as questões
    ModoHistorinha Exercicio01, Exercicio02, Exercicio03, Exercicio04, Exercicio05, Exercicio06, Exercicio07, Exercicio08, Exercicio09, Exercicio10;
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
    #region Cena 02
    bool parte6 = false;
    bool parte7 = false;
    bool parte8 = false;
    bool parte9 = false;
    bool parte10 = false;
    bool sexto = false;
    bool setimo = false;
    bool oitavo = false;
    bool nono = false;
    bool decimo = false;
    bool exercicio6 = false;
    bool exercicio7 = false;
    bool exercicio8 = false;
    bool exercicio9 = false;
    bool exercicio10 = false;
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
    int CaixaTexto, Cy;
    int zerar = 0;
    Color cor;
    int selecionar = 0; //seleciona a musica
    bool repetir = true;
    int NoAlbum = 0;
    bool ModoExercicios = false;
    bool pause = false;
    int Limitedotexto = 80;
    #endregion
    //-------------------------------------------//
    Song audio01, audio02, audio03, audio04, audio05, audio06, audio07, audio08, audio09, audio10, audio11, audio12, audio13, audio14, audio15, audio16, audio17, audio18,
        audio19, audio20, audio21, audio22, audio23, audio24, audio25, audio26, audio27;
    Song audioEx01, audioEx02, audioEx03, audioEx04, audioEx05, audioEx06, audioEx07, audioEx08, audioEx09, audioEx10, audioEx072;
    List<Song> Album1, Album2, Album3, Album4, Album5, Album6, Album7, Album8, Album9, Album10;
    List<List<Song>> AlbumPrincipal;
    AudioEngine audioEngine2;
    WaveBank waveBank2;
    SoundBank soundBank2;
    Cue engineSound = null;
    Texture2D VoltarOver, VoltarNormal,CenarioInterior,CenarioExterior,CenarioCampo,CenarioRio;
    Texture2D[] Voltar,Cenario;
    Rectangle vVoltar,rcCenario;
    int VoltarIndice = 0;
    int CenarioIndice = 0;
    bool VoltarBool = false;
    /// <summary>
    Texture2D Pcosme, Pmaria, Papua, Pserafina;
    Texture2D CosmeA01, CosmeA02, CosmeA03, CosmeA04, CosmeA05, CosmeA06, CosmeA07, CosmeA08;
    Texture2D MariaA01, MariaA02, MariaA03, MariaA04, MariaA05, MariaA06, MariaA07, MariaA08;//Animação de Maria Andando
    Texture2D MariaACBola01, MariaACBola02, MariaACBola03, MariaACBola04, MariaACBola05, MariaACBola06, MariaACBola07, MariaACBola08;//Animação de Maria andando com o balde e com a bola
    Texture2D MariaASBola01, MariaASBola02, MariaASBola03, MariaASBola04, MariaASBola05, MariaASBola06, MariaASBola07, MariaASBola08;//Animação de Maria andando com o balde sem a bola
    Texture2D SerafinaAndando01, SerafinaAndando02, SerafinaAndando03, SerafinaAndando04, SerafinaAndando05, SerafinaAndando06, SerafinaAndando07, SerafinaAndando08;
    Texture2D ApuaAndando01, ApuaAndando02, ApuaAndando03, ApuaAndando04, ApuaAndando05, ApuaAndando06, ApuaAndando07, ApuaAndando08;
    Texture2D ApuaPose01, ApuaPose02, ApuaPose03, ApuaPose04;
    Texture2D CachorroAndando01, CachorroAndando02, CachorroAndando03, CachorroAndando04, CachorroAndando05, CachorroAndando06, CachorroAndando07, CachorroAndando08;
    Texture2D CachorroParado01, CachorroParado02, CachorroParado03, CachorroParado04, CachorroParado05, CachorroParado06, CachorroParado07, CachorroParado08;
    Texture2D[] CosmeAndando, MariaAndando, MariaAndandoComBola, MariaAndandoSemBola, ApuaPose, ApuaAndando, SerafinaAndando,CachorroSentado,CachorroAndando;
    Rectangle rcCosme, rcMaria, rcApua, rcSerafina;
    Color CosmeColor, MariaColor,ApuaColor,SerafinaColor,CachorroColor;
    Vector2 vCosme,vMaria,vApua,vSerafina,vCachorro,vCosmeObj,vMariaObj,vApuaObj,vSerafinaObj,vCachorroObj;
    Texture2D Cosme, Maria, Apua, Serafina,Cachorro;
    #region Bolean para animação
    bool CachorroAndar = false;
    bool CachorroSentar = false;
    bool CosmePare = true;
    bool CosmeAndar = false;
    bool MariaPare = true;
    bool MariaAndar = false;
    bool MariaAndeComBaldeSemBola = false;
    bool MariaAndeComBaldeComBola = false;
    bool SerafinaAnde = false;
    bool SerafinaPare = true;
    bool ApuaPare = true;
    bool ApuaAndar = false;
    bool ApuaPoseEstranha = false;
    #endregion
    /// </summary>
    #endregion
    bool ex = true;
    int Frame = 0;
    int FrameApua = 0;
    bool caderno = false;
    int tick = 100;
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
            CosmeColor = Color.Black;
            MariaColor = Color.Black;
            ApuaColor = Color.Transparent;
            SerafinaColor = Color.Transparent;
            CachorroColor = Color.Transparent;
            VoltarBool = false;
            Voltar = new Texture2D[2] { VoltarNormal, VoltarOver };
            vVoltar = new Rectangle(850, 700, Voltar[VoltarIndice].Width / 2, Voltar[VoltarIndice].Height / 2);
            audioEngine2 = new AudioEngine("Content\\Audio\\MyGameAudio2.xgs");
            waveBank2 = new WaveBank(audioEngine2, "Content\\Audio\\Wave Bank2.xwb");
            soundBank2 = new SoundBank(audioEngine2, "Content\\Audio\\Sound Bank2.xsb");
            // Vetores
            posicaoText = new Vector2(20, 600);
            //Dialogos
            dialogo01 = new string[2] { fala01, fala02 };
            dialogo02 = new string[5] { fala03, fala04, fala05, fala06, fala07 };
            dialogo03 = new string[2] { fala08, fala09 };
            dialogo04 = new string[1] { fala10 };
            dialogo05 = new string[4] { fala11, fala12, fala13, fala14 };
            dialogo06 = new string[2] { fala15, fala16 };
            dialogo07 = new string[2] { fala17, fala18 };
            dialogo08 = new string[2] { fala19, fala20 };
            dialogo09 = new string[5] { fala21, fala22, fala23, fala24, fala25 };
            dialogo10 = new string[1] { fala26 };

            // Exercicios
            //Cena 01
            Exercicio01 = new ModoHistorinha(parent.Content, pergunta00, alternativa00, alternativa01, arial, 2, audioEx01);
            Exercicio02 = new ModoHistorinha(parent.Content, pergunta10, alternativa02, alternativa03, alternativa10, arial, 3, audioEx02);
            Exercicio03 = new ModoHistorinha(parent.Content, pergunta20, alternativa20, alternativa21, alternativa22, alternativa23, arial, 4, audioEx03, true);
            Exercicio04 = new ModoHistorinha(parent.Content, pergunta30, alternativa30, alternativa31, arial, 2, audioEx04);
            Exercicio05 = new ModoHistorinha(parent.Content, pergunta40, alternativa40, alternativa41, alternativa42, arial, 3, audioEx05);
            //Cena02
            Exercicio06 = new ModoHistorinha(parent.Content, pergunta6, alt60, alt61, arial, 2, audioEx06);
            Exercicio07 = new ModoHistorinha(parent.Content, Pergunta7, alt70, alt71, alt72, alt73, arial, 4, audioEx07, audioEx072, false);
            Exercicio08 = new ModoHistorinha(parent.Content, Pergunta8, alt80, alt81, alt82, arial, 3, audioEx08);
            Exercicio09 = new ModoHistorinha(parent.Content, Pergunta9, alt90, alt91, alt92, arial, 3, audioEx09);
            Exercicio10 = new ModoHistorinha(parent.Content, Pergunta010, alt100, alt101, alt102, arial, 3, audioEx10);
            //Transições
            enterTransitionDuration = 50;
            exitTransitionDuration = 50;
            //Narrações
            //Cena 01
            Album1 = new List<Song>() { audio01, audio03 };
            Album2 = new List<Song>() { audio04, audio05, audio06, audio07, audio08 };
            Album3 = new List<Song>() { audio09, audio10 };
            Album4 = new List<Song>() { audio11 };
            Album5 = new List<Song>() { audio12, audio13, audio14, audio15 };
            Album6 = new List<Song>() { audio16, audio17 };
            Album7 = new List<Song>() { audio18, audio19 };
            Album8 = new List<Song>() { audio20, audio21 };
            Album9 = new List<Song>() { audio22, audio23, audio24, audio25, audio26 };
            Album10 = new List<Song>() { audio27 };
            //Album onde todos os album devem ficar.
            AlbumPrincipal = new List<List<Song>>() { Album1, Album2, Album3, Album4, Album5, Album6, Album7, Album8, Album9, Album10 };
            //Animação
            CosmeAndando = new Texture2D[8] { CosmeA01, CosmeA02, CosmeA03, CosmeA04, CosmeA05, CosmeA06, CosmeA07, CosmeA08 };
            MariaAndando = new Texture2D[8] { MariaA01, MariaA02, MariaA03, MariaA04, MariaA05, MariaA06, MariaA07, MariaA08 };
            MariaAndandoComBola = new Texture2D[8] { MariaACBola01, MariaACBola02, MariaACBola03, MariaACBola04, MariaACBola05, MariaACBola06, MariaACBola07, MariaACBola08 };
            MariaAndandoSemBola = new Texture2D[8] { MariaASBola01, MariaASBola02, MariaASBola03, MariaASBola04, MariaASBola05, MariaASBola06, MariaASBola07, MariaASBola08 };
            ApuaAndando = new Texture2D[8] { ApuaAndando01, ApuaAndando02, ApuaAndando03, ApuaAndando04, ApuaAndando05, ApuaAndando06, ApuaAndando07, ApuaAndando08 };
            ApuaPose = new Texture2D[4] { ApuaPose01, ApuaPose02, ApuaPose03, ApuaPose04 };
            SerafinaAndando = new Texture2D[8] { SerafinaAndando01, SerafinaAndando02, SerafinaAndando03, SerafinaAndando04, SerafinaAndando05, SerafinaAndando06, SerafinaAndando07, SerafinaAndando08 };
            CachorroSentado = new Texture2D[8] { CachorroParado01, CachorroParado02, CachorroParado03, CachorroParado04, CachorroParado05, CachorroParado06, CachorroParado07, CachorroParado08 };
            CachorroAndando = new Texture2D[8] { CachorroAndando01, CachorroAndando02, CachorroAndando03, CachorroAndando04, CachorroAndando05, CachorroAndando06, CachorroAndando07, CachorroAndando08 };
            //Cenario
            Cosme = Pcosme;
            Maria = Pmaria;
            Apua = Papua;
            Serafina = Pserafina;
            Cachorro = CachorroSentado[Frame];
            Cenario = new Texture2D[4] { CenarioInterior, CenarioExterior, CenarioRio, CenarioCampo };
            vMaria = new Vector2(400, 400);
            vCosme = new Vector2(300, 400);
            vApua = new Vector2(500, 400);
            vSerafina = new Vector2(600, 400);
            vCachorro = new Vector2(700, 400);
            rcCenario = new Rectangle(0, 0, 1024, 768);
            vCosmeObj = vCosme;
            vMariaObj = vMaria;
            vApuaObj = vApua;
            vSerafinaObj = vSerafina;
            //rcApua = new Rectangle(300, 200, Papua.Width / 2, Papua.Height / 2);
            //rcSerafina = new Rectangle(400, 200, Pserafina.Width / 2, Pserafina.Height / 2);
           
        }
    }
    Keys lastKey = Keys.A;
    int incrementoTexto = 0;

    public override void Update(GameTime tempo)
    {
        if (!caderno)
        {
            rcCosme = new Rectangle(300, 400, CosmeAndando[Frame].Width, CosmeAndando[Frame].Height);
            MouseState mouse = Mouse.GetState();
            if (!pauseFlag)
            {

                base.Update(tempo);
                if (pause)
                {
                    MediaPlayer.Resume();
                    pause = false;
                }
                if (engineSound == null || engineSound.IsStopped)
                {
                    engineSound = soundBank2.GetCue("385591_Night_sea_ln");
                    engineSound.Play();
                }
                if (engineSound.IsPaused)
                {
                    engineSound.Resume();
                }
                if (texto.Length % Limitedotexto == 0 && texto.Length != 0 && zerar < 2) //Quando o texto atingir um limite da tela e tiver um espaço em branco ele pula uma linha;
                {
                    if (texto[indice - 1] == ' ' && texto[indice-2] != ' ')
                    {
                        texto += "\n";
                        zerar++;
                        Limitedotexto = 80;

                    }
                    else
                    {
                        Limitedotexto++;


                    }

                }
                if (!FimDaHistoria)
                {
                    AnimacaoDoEpisodio();
                    if (ColisaoMouseOver(mouse, vVoltar))
                    {
                        VoltarIndice = 1;

                    }
                    else { VoltarIndice = 0; }
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        if (ColisaoMouseOver(mouse, vVoltar))
                        {

                            //engineSound.Stop(AudioStopOptions.AsAuthored);
                            //MediaPlayer.Stop();
                            VoltarBool = true;
                            //FimDaHistoria = true;
                        }
                    }
                    KeyboardState teclado = Keyboard.GetState();
                    //MediaPlayer.IsRepeating = false;
                    //////////////////////////////////////////////////////////////////Aqui é onde a narração vai acontecer////////////////////////////////////////////////

                    if (repetir && selecionar < AlbumPrincipal[NoAlbum].Count && !ModoExercicios)//Aqui eu vou verificar se tem algum audio rodando, se não  tiver eu toco uma nova narração.
                    {
                        MediaPlayer.Play(AlbumPrincipal[NoAlbum][selecionar]);//Narração.
                        repetir = false;//Repetir serve para não deixar a música tocar sem parar

                    }
                    if (ex)
                    {
                        if (teclado.IsKeyDown(Keys.Z) && lastKey != Keys.Z)//Se a narração chegou ao tempo final
                        {

                            if (AlbumPrincipal[NoAlbum].Count == selecionar + 1)//Se o Album que está tocando chegou a sua ultima música
                            {
                                NoAlbum += (NoAlbum < AlbumPrincipal.Count ? 1 : 0);//Trocar de album
                                selecionar = 0;//eu zero o contador de musicas
                                Incremento0++;// Incremento mais 1 na variavel que permite que o texto continue
                                if (NoAlbum == 10)
                                {
                                    NoAlbum = 9;
                                }

                            }
                            else
                            {
                                selecionar += selecionar < AlbumPrincipal[NoAlbum].Count ? 1 : 0;//Ele troca de música
                                Incremento0++;//Mudo o texto
                                indice = 0;//Falo que o numero de letras é zero;
                                zerar = 0;//Falo que agora ele poderar pular uma linha no texto, caso a condição seja feita.(Vá no Update para ver a condição)
                                texto = "";//O texto a ser impresso agora não possui nada;

                                Limitedotexto = 80;
                            }

                            repetir = true;
                        }
                    }
                    if (teclado.IsKeyDown(Keys.C) && lastKey != Keys.C)
                    {
                        MediaPlayer.Pause();
                        caderno = true;
                    }
                    Keys[] ks = teclado.GetPressedKeys();

                    if (ks.Length == 0) lastKey = Keys.A;
                    else lastKey = ks[0];

                    //////////////////////////////////////Fim da Parte de Narração//////////////////////////////////////////////////////////////////
                    if (stateEntered)
                    {



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

                }
                else if (!exitingState)
                {
                    MediaPlayer.Stop();
                    engineSound.Stop(AudioStopOptions.AsAuthored);

                    ExitState();
                }
                if (VoltarBool)
                {
                    MediaPlayer.Stop();
                    engineSound.Stop(AudioStopOptions.AsAuthored);

                    ExitState();
                }

            }
            else
            {
                MediaPlayer.Pause();
                pause = true;
                engineSound.Pause();

            }
        }
        else
        {
            parent.EnterState((int)StatesIdList.OPTIONS);
        }
    }
    string Tmusica, TpLAYER, NomeMusica;
    int frames = 30;

    public override void Draw(GameTime gameTime)
    {


        SpriteBatch.Begin();
        cor = Color.White * Alpha;
        
        if ((int)gameTime.TotalGameTime.TotalMilliseconds % tick == 0)
        {
            Frame = (Frame + 1) % 8;
            FrameApua = (FrameApua + 1) % 4;
        }
        if (!FimDaHistoria)
        {
            SpriteBatch.Draw(Cenario[CenarioIndice], rcCenario, Color.White);
            SpriteBatch.Draw(Voltar[VoltarIndice], vVoltar, Color.White);
            //Tmusica = MediaPlayer.PlayPosition.Minutes.ToString() + " : " + MediaPlayer.PlayPosition.Seconds;
            //TpLAYER = AlbumPrincipal[NoAlbum][selecionar].Duration.Minutes.ToString() + " : " + AlbumPrincipal[NoAlbum][selecionar].Duration.Seconds;
            //NomeMusica = AlbumPrincipal[NoAlbum][selecionar].Name;
            SpriteBatch.DrawString(arial, "Pressione a tecla [z] para avançar o texto", new Vector2(200, 720), Color.White);
            SpriteBatch.DrawString(arial, Frame.ToString(), new Vector2(200, 700), Color.White);//Aqui eu vejo em quanto tempo está a narração
            //Aqui em cima eu imprimo o tempo total da musica;
        }
        SpriteBatch.Draw(Cosme, vCosme, CosmeColor);
        SpriteBatch.Draw(Maria, vMaria, MariaColor);
        SpriteBatch.Draw(Apua, vApua,ApuaColor);
        SpriteBatch.Draw(Serafina, vSerafina, SerafinaColor);
        SpriteBatch.Draw(Cachorro, vCachorro, CachorroColor);
        //*
        if (!caderno)
        {
            if (!pauseFlag)
            {
                #region Desenho
                //Irei explicar a primeira parte, pois as seguintes são iguais.
                if (!primeiro)//Primeira parte do jogo possui uma sequencia de dialogos e um exercicio.
                {
                    if (Incremento0 == dialogo01.Length)//Verifico se o dialogo chegou ao seu fim
                    {

                        parte1 = true;//Se chegou a parte 1 termina

                    }

                    if (!parte1)//S e a parte um não terminou
                    {
                        if (indice < dialogo01[Incremento0].Length) { texto += dialogo01[Incremento0][indice]; }//Verifica se o texto impresso tem menos letras que o dialogo, se verdadeiro coloca mais uma letra;
                        indice = indice + (indice < dialogo01[Incremento0].Length ? 1 : 0);//Aumento o indice para que sempre vá para próxima letra na hora de imprimir.
                        if (texto.Length == dialogo01[Incremento0].Length + zerar)//Se o texto impresso tiver o mesmo numero de letras do dialogo...O MAIS UM É POR CAUSA DO \N QUE EU COLOCO AUTOMATICAMENTE
                        {
                            //mile += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                            //Segundos = mile / 1000;


                        }
                        if (Incremento0 == 0)
                        {
                            CosmeColor = Color.White;
                        }
                        if (Incremento0 == 1)
                        {
                            MariaColor = Color.White;
                        }

                        SpriteBatch.DrawString(arial, texto, posicaoText, Color.White);//Imprimir texto

                    }

                    if (parte1 && !exercicio1)//Caso a primeira parte tenha chegado ao fim, mas o exercicio não foi feito.
                    {

                        texto = "";//Reseto tudo, como foi feito acima
                        indice = 0;//
                        Incremento0 = 0;// Estou usando o mesmo incremento para todas os dialogos;
                        zerar = 0;//
                        //mile = 0;
                        Limitedotexto = 80;
                        Exercicio01.Atualizar();//Eu chamo o atualizar do exercicio
                        Exercicio01.Desenhar(SpriteBatch);//Desenho ele na tela
                        exercicio1 = Exercicio01.Continuar();// Caso o jogador vença, O Continuar() vai retornar verdadeiro, caso o contrario falso.
                        ex = exercicio1;
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


                        if (indice < dialogo02[Incremento0].Length) { texto += dialogo02[Incremento0][indice]; }
                        indice = indice + (indice < dialogo02[Incremento0].Length ? 1 : 0);
                        if (Incremento0 == 3)
                        {
                            SerafinaColor = Color.White;
                            CachorroColor = Color.White;
                            CachorroSentar = true;
                            
                        }
                        if(Incremento0 == 4)
                        {
                            ApuaColor = Color.White;
                            
                        }

                        SpriteBatch.DrawString(arial, texto, posicaoText, Color.White);

                    }
                    if (!exercicio2 && parte2)
                    {
                        CachorroColor = Color.Transparent;
                        
                        texto = "";
                        indice = 0;
                        Incremento0 = 0;
                        zerar = 0;
                        mile = 0;
                        Limitedotexto = 80;
                        Exercicio02.Atualizar();
                        Exercicio02.Desenhar(SpriteBatch);
                        exercicio2 = Exercicio02.Continuar();
                        ex = exercicio2;
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
                        if (indice < dialogo03[Incremento0].Length) { texto += dialogo03[Incremento0][indice]; }
                        indice = indice + (indice < dialogo03[Incremento0].Length ? 1 : 0);

                        SpriteBatch.DrawString(arial, texto, posicaoText, Color.White);
                    }
                    if (parte3 && !exercicio3)
                    {
                        texto = "";
                        indice = 0;
                        Incremento0 = 0;
                        zerar = 0;
                        mile = 0;
                        Limitedotexto = 80;
                        Exercicio03.Atualizar();
                        Exercicio03.Desenhar(SpriteBatch);
                        exercicio3 = Exercicio03.Continuar();
                        ex = exercicio3;
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

                        if (indice < dialogo04[Incremento0].Length) { texto += dialogo04[Incremento0][indice]; }
                        indice = indice + (indice < dialogo04[Incremento0].Length ? 1 : 0);
                        if (Incremento0 == 0)
                        {
                            vMariaObj.X = 700;
                            MariaAndar = true;
                            vSerafinaObj.X = 400;
                        }
                        
                        SpriteBatch.DrawString(arial, texto, posicaoText, Color.White);

                    }
                    if (parte4 && !exercicio4)
                    {
                        vMaria.X = 700;
                        vSerafina.X = 400;
                        texto = "";
                        indice = 0;
                        Incremento0 = 0;
                        zerar = 0;
                        mile = 0;
                        Limitedotexto = 80;
                        Exercicio04.Atualizar();
                        Exercicio04.Desenhar(SpriteBatch);
                        exercicio4 = Exercicio04.Continuar();
                        ex = false;
                        repetir = false;
                        quarto = exercicio4;
                        if (quarto)
                        {
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                            {
                                Mouse.SetPosition(0, 0);
                            }
                        }
                    }

                }

                if (primeiro && segundo && terceiro && quarto && !quinto)
                {

                    if (!exercicio5)
                    {

                        mile = 0;
                        Exercicio05.Atualizar();
                        Exercicio05.Desenhar(SpriteBatch);
                        exercicio5 = Exercicio05.Continuar();
                        ex = exercicio5;
                        repetir = exercicio5;
                    }

                    if (Incremento0 == dialogo05.Length)
                    {
                        parte5 = true;
                        texto = "";
                        indice = 0;
                        Incremento0 = 0;
                        zerar = 0;
                        Limitedotexto = 80;
                        quinto = true;
                        repetir = true;
                        ex = true;
                    }
                    if (!parte5 && exercicio5)
                    {
                        if (indice < dialogo05[Incremento0].Length) { texto += dialogo05[Incremento0][indice]; }
                        indice = indice + (indice < dialogo05[Incremento0].Length ? 1 : 0);
                        if (Incremento0 == 0)
                        {
                            vCosmeObj.X = 600;
                            vApuaObj.X = 300;
                        }
                        if (Incremento0 == 1)
                        {
                            vCosme.X = 600;
                            vApua.X = 300;
                            ApuaPare = false;
                            ApuaPoseEstranha = true;
                        }
                        if (Incremento0 == 2)
                        {
                            ApuaPoseEstranha = false;
                            vApuaObj.X = 850;
                            vSerafinaObj.X = 850;

                        }
                        if (Incremento0 == 3)
                        {
                            vCosmeObj.X = 850;
                        }
                        if (vApua.X == 850)
                        {
                            ApuaColor = Color.Transparent;
                        }
                        if (vSerafina.X == 850)
                        {
                            SerafinaColor = Color.Transparent;
                        }
                        if (vCosme.X == 850)
                        {
                            CosmeColor = Color.Transparent;
                        }
                        SpriteBatch.DrawString(arial, texto, posicaoText, Color.White);
                    }

                }
                if (primeiro && segundo && terceiro && quarto && quinto && !sexto)
                {
                    CenarioIndice = 1;
                    if (Incremento0 == dialogo06.Length)
                    {
                        parte6 = true;

                    }
                    if (!parte6)
                    {
                        if (indice < dialogo06[Incremento0].Length) { texto += dialogo06[Incremento0][indice]; }
                        indice = indice + (indice < dialogo06[Incremento0].Length ? 1 : 0);
                        MariaColor = Color.Transparent;
                        SpriteBatch.DrawString(arial, texto, posicaoText, Color.White);
                    }
                    if (parte6 && !exercicio6)
                    {
                        texto = "";
                        indice = 0;
                        Incremento0 = 0;
                        zerar = 0;
                        mile = 0;
                        Limitedotexto = 80;
                        Exercicio06.Atualizar();
                        Exercicio06.Desenhar(SpriteBatch);
                        exercicio6 = Exercicio06.Continuar();
                        repetir = exercicio6;
                        ex = exercicio6;
                        sexto = exercicio6;
                    }

                }
                if (primeiro && segundo && terceiro && quarto && quinto && sexto && !setimo)
                {
                    CenarioIndice = 0;
                    if (Incremento0 == dialogo07.Length)
                    {
                        parte7 = true;

                    }
                    if (!parte7)
                    {
                        if (indice < dialogo07[Incremento0].Length) { texto += dialogo07[Incremento0][indice]; }
                        indice = indice + (indice < dialogo07[Incremento0].Length ? 1 : 0);

                        SpriteBatch.DrawString(arial, texto, posicaoText, Color.White);
                    }
                    if (parte7 && !exercicio7)
                    {
                        texto = "";
                        indice = 0;
                        Incremento0 = 0;
                        zerar = 0;
                        mile = 0;
                        Limitedotexto = 80;
                        Exercicio07.Atualizar();
                        Exercicio07.Desenhar(SpriteBatch);
                        exercicio7 = Exercicio07.Continuar();
                        repetir = exercicio7;
                        ex = exercicio7;
                        setimo = exercicio7;
                    }

                }
                if (primeiro && segundo && terceiro && quarto && quinto && sexto && setimo && !oitavo)
                {
                    if (Incremento0 == dialogo08.Length)
                    {
                        parte8 = true;

                    }
                    if (!parte8)
                    {
                        if (indice < dialogo08[Incremento0].Length) { texto += dialogo08[Incremento0][indice]; }
                        indice = indice + (indice < dialogo08[Incremento0].Length ? 1 : 0);

                        SpriteBatch.DrawString(arial, texto, posicaoText, Color.White);
                    }
                    if (parte8 && !exercicio8)
                    {
                        texto = "";
                        indice = 0;
                        Incremento0 = 0;
                        zerar = 0;
                        mile = 0;
                        Limitedotexto = 80;
                        Exercicio08.Atualizar();
                        Exercicio08.Desenhar(SpriteBatch);
                        exercicio8 = Exercicio08.Continuar();
                        repetir = exercicio8;
                        ex = exercicio8;
                        oitavo = exercicio8;
                    }

                }
                if (primeiro && segundo && terceiro && quarto && quinto && sexto && setimo && oitavo && !nono)
                {
                    if (Incremento0 == 0)
                    {
                        CenarioIndice = 1;
                    }
                    if (Incremento0 > 0)
                    {
                        CenarioIndice = 2;
                    }
                    if (Incremento0 == dialogo09.Length)
                    {
                        parte9 = true;

                    }
                    if (!parte9)
                    {
                        if (indice < dialogo09[Incremento0].Length) { texto += dialogo09[Incremento0][indice]; }
                        indice = indice + (indice < dialogo09[Incremento0].Length ? 1 : 0);

                        SpriteBatch.DrawString(arial, texto, posicaoText, Color.White);
                    }
                    if (parte9 && !exercicio9)
                    {
                        texto = "";
                        indice = 0;
                        Incremento0 = 0;
                        zerar = 0;
                        mile = 0;
                        Limitedotexto = 80;
                        Exercicio09.Atualizar();
                        Exercicio09.Desenhar(SpriteBatch);
                        exercicio9 = Exercicio09.Continuar();
                        repetir = exercicio9;
                        ex = exercicio9;
                        nono = exercicio9;
                    }

                }
                if (primeiro && segundo && terceiro && quarto && quinto && sexto && setimo && oitavo && nono && !decimo)
                {
                    if (Incremento0 == dialogo10.Length)
                    {
                        parte10 = true;

                    }
                    if (!parte10)
                    {
                        if (indice < dialogo10[Incremento0].Length) { texto += dialogo10[Incremento0][indice]; }
                        indice = indice + (indice < dialogo10[Incremento0].Length ? 1 : 0);

                        SpriteBatch.DrawString(arial, texto, posicaoText, Color.White);
                    }
                    if (parte10 && !exercicio10)
                    {
                        texto = "";
                        indice = 0;
                        Incremento0 = 0;
                        zerar = 0;
                        mile = 0;
                        Limitedotexto = 80;
                        Exercicio10.Atualizar();
                        Exercicio10.Desenhar(SpriteBatch);
                        exercicio10 = Exercicio10.Continuar();
                        repetir = exercicio10;
                        ex = exercicio10;
                        decimo = exercicio10;
                    }

                }
                if (primeiro && segundo && terceiro && quarto && quinto && sexto && setimo && oitavo && nono && decimo)
                {
                    FimDaHistoria = true;

                }

                #endregion
            }
        }
        //*/
        SpriteBatch.End();
    }
    protected override void LoadContent()
    {
        if (!contentLoaded)
        {
            arial = parent.Content.Load<SpriteFont>("Fonte/Arial");
            VoltarNormal = parent.Content.Load<Texture2D>("Menu/voltarN");
            VoltarOver = parent.Content.Load<Texture2D>("Menu/voltarH");
            CenarioInterior = parent.Content.Load<Texture2D>("Imagem/Cenario/Casa_Int");
            CenarioExterior = parent.Content.Load<Texture2D>("Imagem/Cenario/Casa_Ext");
            CenarioRio = parent.Content.Load<Texture2D>("Imagem/Cenario/CenarioRio");
            CenarioCampo = parent.Content.Load<Texture2D>("Imagem/Cenario/Campinho");
            Pmaria = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Andando/0");
            Pcosme = parent.Content.Load<Texture2D>("Imagem/Personagem/Cosme/Cosme_Andando/0");
            Papua = parent.Content.Load<Texture2D>("Imagem/Personagem/Apua/Apua_Andando/0");
            Pserafina = parent.Content.Load<Texture2D>("Imagem/Personagem/Serafina/Serafina_Andando/0");
            #region Cosme Andando
            CosmeA01 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cosme/Cosme_Andando/1");
            CosmeA02 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cosme/Cosme_Andando/2");
            CosmeA03 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cosme/Cosme_Andando/3");
            CosmeA04 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cosme/Cosme_Andando/4");
            CosmeA05 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cosme/Cosme_Andando/5");
            CosmeA06 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cosme/Cosme_Andando/6");
            CosmeA07 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cosme/Cosme_Andando/7");
            CosmeA08 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cosme/Cosme_Andando/8");
            #endregion
            #region Maria Andando
            MariaA01 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Andando/1");
            MariaA02 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Andando/2");
            MariaA03 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Andando/3");
            MariaA04 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Andando/4");
            MariaA05 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Andando/5");
            MariaA06 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Andando/6");
            MariaA07 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Andando/7");
            MariaA08 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Andando/8");
            #endregion
            #region Maria Andando Com Balde - Bola
            MariaACBola01 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde_bola/1");
            MariaACBola02 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde_bola/2");
            MariaACBola03 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde_bola/3");
            MariaACBola04 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde_bola/4");
            MariaACBola05 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde_bola/5");
            MariaACBola06 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde_bola/6");
            MariaACBola07 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde_bola/7");
            MariaACBola08 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde_bola/8");
            #endregion
            #region Maria Andando Com Balde
            MariaASBola01 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde/1");
            MariaASBola02 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde/2");
            MariaASBola03 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde/3");
            MariaASBola04 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde/4");
            MariaASBola05 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde/5");
            MariaASBola06 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde/6");
            MariaASBola07 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde/7");
            MariaASBola08 = parent.Content.Load<Texture2D>("Imagem/Personagem/Maria/Maria_Balde/8");
            #endregion
            #region Serafina Andando
            SerafinaAndando01 = parent.Content.Load<Texture2D>("Imagem/Personagem/Serafina/Serafina_Andando/1");
            SerafinaAndando02 = parent.Content.Load<Texture2D>("Imagem/Personagem/Serafina/Serafina_Andando/2");
            SerafinaAndando03 = parent.Content.Load<Texture2D>("Imagem/Personagem/Serafina/Serafina_Andando/3");
            SerafinaAndando04 = parent.Content.Load<Texture2D>("Imagem/Personagem/Serafina/Serafina_Andando/4");
            SerafinaAndando05 = parent.Content.Load<Texture2D>("Imagem/Personagem/Serafina/Serafina_Andando/5");
            SerafinaAndando06 = parent.Content.Load<Texture2D>("Imagem/Personagem/Serafina/Serafina_Andando/6");
            SerafinaAndando07 = parent.Content.Load<Texture2D>("Imagem/Personagem/Serafina/Serafina_Andando/7");
            SerafinaAndando08 = parent.Content.Load<Texture2D>("Imagem/Personagem/Serafina/Serafina_Andando/8");
            #endregion
            #region Apua Andando
            ApuaAndando01 = parent.Content.Load<Texture2D>("Imagem/Personagem/Apua/Apua_Andando/0001");
            ApuaAndando02 = parent.Content.Load<Texture2D>("Imagem/Personagem/Apua/Apua_Andando/0002");
            ApuaAndando03 = parent.Content.Load<Texture2D>("Imagem/Personagem/Apua/Apua_Andando/0003");
            ApuaAndando04 = parent.Content.Load<Texture2D>("Imagem/Personagem/Apua/Apua_Andando/0004");
            ApuaAndando05 = parent.Content.Load<Texture2D>("Imagem/Personagem/Apua/Apua_Andando/0005");
            ApuaAndando06 = parent.Content.Load<Texture2D>("Imagem/Personagem/Apua/Apua_Andando/0006");
            ApuaAndando07 = parent.Content.Load<Texture2D>("Imagem/Personagem/Apua/Apua_Andando/0007");
            ApuaAndando08 = parent.Content.Load<Texture2D>("Imagem/Personagem/Apua/Apua_Andando/0008");
            #endregion
            #region Apua Pose
            ApuaPose01 = parent.Content.Load<Texture2D>("Imagem/Personagem/Apua/Apua_Pose/0001");
            ApuaPose02 = parent.Content.Load<Texture2D>("Imagem/Personagem/Apua/Apua_Pose/0002");
            ApuaPose03 = parent.Content.Load<Texture2D>("Imagem/Personagem/Apua/Apua_Pose/0003");
            ApuaPose04 = parent.Content.Load<Texture2D>("Imagem/Personagem/Apua/Apua_Pose/0004");
            #endregion
            #region Cachorro Andando
            CachorroAndando01 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Andando/0001");
            CachorroAndando02 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Andando/0002");
            CachorroAndando03 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Andando/0003");
            CachorroAndando04 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Andando/0004");
            CachorroAndando05 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Andando/0005");
            CachorroAndando06 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Andando/0006");
            CachorroAndando07 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Andando/0007");
            CachorroAndando08 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Andando/0008");
            #endregion
            #region Cachorro Parado
            CachorroParado01 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Sentado/0001");
            CachorroParado02 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Sentado/0002");
            CachorroParado03 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Sentado/0003");
            CachorroParado04 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Sentado/0004");
            CachorroParado05 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Sentado/0005");
            CachorroParado06 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Sentado/0006");
            CachorroParado07 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Sentado/0007");
            CachorroParado08 = parent.Content.Load<Texture2D>("Imagem/Personagem/Cachorro/Cachorro_Sentado/0008");
            #endregion
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
        if (VoltarBool || FimDaHistoria)
        {
            base.EnterState(freezeBelow);
            
            //LoadContent();
            pauseFlag = false;
            Resetar();
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
            if (!VoltarBool)
            {
                parent.ExitState(ID, (int)StatesIdList.RUNNER);
            }
            else
            {
                
                parent.ExitState(ID);
            }

        }
    }
    #endregion
    void FalasDoNarrador()
    {
        #region Falas
        audio01 = parent.Content.Load<Song>("Narrar/Cena01/Parte 1 - Fala 1");
        audio02 = parent.Content.Load<Song>("Narrar/Cena01/Parte 1 - Fala 2");
        audio03 = parent.Content.Load<Song>("Narrar/Cena01/Parte 1 - Fala 3");
        audio04 = parent.Content.Load<Song>("Narrar/Cena01/Parte 2 - Fala 5");
        audio05 = parent.Content.Load<Song>("Narrar/Cena01/Parte 2 - Fala 6");
        audio06 = parent.Content.Load<Song>("Narrar/Cena01/Parte 2 - Fala 7");
        audio07 = parent.Content.Load<Song>("Narrar/Cena01/Parte 2 - Fala 8");
        audio08 = parent.Content.Load<Song>("Narrar/Cena01/Parte 2 - Fala 9");
        audio09 = parent.Content.Load<Song>("Narrar/Cena01/Parte 3 - Fala 11");
        audio10 = parent.Content.Load<Song>("Narrar/Cena01/Parte 3 - Fala 12");
        audio11 = parent.Content.Load<Song>("Narrar/Cena01/Parte 4 - Fala 14");
        audio12 = parent.Content.Load<Song>("Narrar/Cena01/Parte 5 - Fala 15");
        audio13 = parent.Content.Load<Song>("Narrar/Cena01/Parte 5 - Fala 16");
        audio14 = parent.Content.Load<Song>("Narrar/Cena01/Parte 5 - Fala 17");
        audio15 = parent.Content.Load<Song>("Narrar/Cena01/Parte 5 - Fala 18");
        audio16 = parent.Content.Load<Song>("Narrar/Cena02/Parte 6 - Fala 19");
        audio17 = parent.Content.Load<Song>("Narrar/Cena02/Parte 6 - Fala 20");
        audio18 = parent.Content.Load<Song>("Narrar/Cena02/Parte 7 - Fala 21");
        audio19 = parent.Content.Load<Song>("Narrar/Cena02/Parte 7 - Fala 22");
        audio20 = parent.Content.Load<Song>("Narrar/Cena02/Parte 8 - Fala 23");
        audio21 = parent.Content.Load<Song>("Narrar/Cena02/Parte 8 - Fala 24");
        audio22 = parent.Content.Load<Song>("Narrar/Cena02/Parte 9 - Fala 25");
        audio23 = parent.Content.Load<Song>("Narrar/Cena02/Parte 9 - Fala 26");
        audio24 = parent.Content.Load<Song>("Narrar/Cena02/Parte 9 - Fala 27");
        audio25 = parent.Content.Load<Song>("Narrar/Cena02/Parte 9 - Fala 28");
        audio26 = parent.Content.Load<Song>("Narrar/Cena02/Parte 9 - Fala 29");
        audio27 = parent.Content.Load<Song>("Narrar/Cena02/Parte 10 - Fala 30");

        #endregion
        //Exercicios
        audioEx01 = parent.Content.Load<Song>("Narrar/Cena01/Exercicio01");
        audioEx02 = parent.Content.Load<Song>("Narrar/Cena01/Exercicio02");
        audioEx03 = parent.Content.Load<Song>("Narrar/Cena01/Exercicio03");
        audioEx04 = parent.Content.Load<Song>("Narrar/Cena01/Exercicio04");
        audioEx05 = parent.Content.Load<Song>("Narrar/Cena01/Exercicio05");
        audioEx06 = parent.Content.Load<Song>("Narrar/Cena02/Exercicio06");
        audioEx07 = parent.Content.Load<Song>("Narrar/Cena02/Exercicio07");
        audioEx072 = parent.Content.Load<Song>("Narrar/Cena02/Exercicio07 - Wrong");
        audioEx08 = parent.Content.Load<Song>("Narrar/Cena02/Exercicio08");
        audioEx09 = parent.Content.Load<Song>("Narrar/Cena02/Exercicio09");
        audioEx10 = parent.Content.Load<Song>("Narrar/Cena02/Exercicio10");



    }
    public bool ColisaoMouseOver(MouseState mouse, Rectangle rec)
    {
        if ((mouse.X > rec.X && mouse.X < rec.X + rec.Width) && (mouse.Y > rec.Y && mouse.Y < rec.Y + rec.Height))
        {

            return true;
        }

        return false;
    }
    public void Resetar()
    {
        CosmeColor = Color.Black;
        MariaColor = Color.Black;
        CenarioIndice = 0;
        FimDaHistoria = false;
        selecionar = 0; //seleciona a musica
         repetir = true;
        NoAlbum = 0;
        ModoExercicios = false;
        pause = false;
        Limitedotexto = 80;
        indice = 0;
        Cosme = Pcosme;
        Maria = Pmaria;
        Apua = Papua;
        Serafina = Pserafina;
        #region Cena 01
        parte1 = false;
         parte2 = false;
         parte3 = false;
         parte4 = false;
         parte5 = false;
         exercicio1 = false;
         exercicio2 = false;
         exercicio3 = false;
         exercicio4 = false;
         exercicio5 = false;
         primeiro = false;
         segundo = false;
         terceiro = false;
         quarto = false;
         quinto = false;
        #endregion
        #region Cena 02
         parte6 = false;
         parte7 = false;
         parte8 = false;
         parte9 = false;
         parte10 = false;
         sexto = false;
         setimo = false;
         oitavo = false;
         nono = false;
         decimo = false;
         exercicio6 = false;
         exercicio7 = false;
         exercicio8 = false;
         exercicio9 = false;
         exercicio10 = false;
        #endregion
        texto = "";
        VoltarBool = false;
    }
    public void Cad(bool cd)
    {
        if (MediaPlayer.State == MediaState.Paused)
        {
            MediaPlayer.Resume();
        }
        caderno = cd;
    }
    void AnimacaoDoEpisodio()
    {
        #region Comandos de Animação
        if (CachorroColor == Color.White)
        {
            if (CachorroSentar)
            {
                
                Cachorro = CachorroSentado[Frame];
            }
            if (CachorroAndar)
            {
                
                Cachorro = CachorroAndando[Frame];
            }
        }
        if (CosmeColor == Color.White)
        {
            if (CosmeAndar)
            {
                Cosme = CosmeAndando[Frame];
            }
            if (CosmePare)
            {
                Cosme = Pcosme;
            }
        }
        if (MariaColor == Color.White)
        {
            if (MariaAndar)
            {
                Maria = MariaAndando[Frame];
            }
            if (MariaAndeComBaldeComBola)
            {
                Maria = MariaAndandoComBola[Frame];
            }
            if (MariaAndeComBaldeSemBola)
            {
                Maria = MariaAndandoSemBola[Frame];
            }
            if (MariaPare)
            {
                Maria = Pmaria;
            }
        }
        if (ApuaColor == Color.White)
        {
            if (ApuaPare)
            {
                Apua = Papua;
            }
            if (ApuaPoseEstranha)
            {
                Apua = ApuaPose[FrameApua];
            }
            if (ApuaAndar)
            {
                Apua = ApuaAndando[Frame];
            }
        }
        if (SerafinaColor == Color.White)
        {
            if (SerafinaAnde)
            {
                Serafina = SerafinaAndando[Frame];
            }
            if (SerafinaPare)
            {
                Serafina = Pserafina;
            }
        }
#endregion
        #region Andar dos Personagens
        if (vCosmeObj != vCosme)
        {
            CosmePare = false;
            CosmeAndar = true;
            if (vCosmeObj.X > vCosme.X)
            {
                vCosme.X++;
            }
            if (vCosmeObj.X < vCosme.X)
            {
                vCosme.X--;
            }
            if (vCosmeObj.Y > vCosme.Y)
            {
                vCosme.Y++;
            }
            if (vCosmeObj.Y < vCosme.Y)
            {
                vCosme.Y--;
            }
        }
        else
        {
            CosmeAndar = false;
            CosmePare = true;
        }
        if (vMariaObj != vMaria)
        {
            MariaPare = false;
            if (vMariaObj.X > vMaria.X)
            {
                vMaria.X++;
            }
            if (vMariaObj.X < vMaria.X)
            {
                vMaria.X--;
            }
            if (vMariaObj.Y > vMaria.Y)
            {
                vMaria.Y++;
            }
            if (vMariaObj.Y < vMaria.Y)
            {
                vMaria.Y--;
            }
        }
        else
        {
            MariaAndar = false;
            MariaAndeComBaldeComBola = false;
            MariaAndeComBaldeSemBola = false;
            MariaPare = true;
        }
        if (vApuaObj != vApua)
        {
            ApuaPare = false;
            ApuaPoseEstranha = false;
            ApuaAndar = true;
            if (vApuaObj.X > vApua.X)
            {
                vApua.X++;
            }
            if (vApuaObj.X < vApua.X)
            {
                vApua.X--;
            }
            if (vApuaObj.Y > vApua.Y)
            {
                vApua.Y++;
            }
            if (vApuaObj.Y < vApua.Y)
            {
                vApua.Y--;
            }
        }
        else
        {
            ApuaAndar = false;
            ApuaPare = true;
        }
        if (vSerafinaObj != vSerafina)
        {
            SerafinaPare = false;
            SerafinaAnde = true;
            if (vSerafinaObj.X > vSerafina.X)
            {
                vSerafina.X++;
            }
            if (vSerafinaObj.X < vSerafina.X)
            {
                vSerafina.X--;
            }
            if (vSerafinaObj.Y > vSerafina.Y)
            {
                vSerafina.Y++;
            }
            if (vSerafinaObj.Y < vSerafina.Y)
            {
                vSerafina.Y--;
            }
        }
        else 
        {
            SerafinaAnde = false;
            SerafinaPare = true;
        }
        #endregion
    }
    public bool FimDaHistoria { get; set; }
    public bool EX1 { get { return exercicio1; } }
    public bool EX2 { get { return exercicio2; } }
    public bool EX3 { get { return exercicio3; } }
    public bool EX4 { get { return exercicio4; } }
    public bool EX5 { get { return exercicio5; } }
    public bool EX6 { get { return exercicio6; } }
    public bool EX7 { get { return exercicio7; } }
    public bool EX8 { get { return exercicio8; } }
    public bool EX9 { get { return exercicio9; } }
    public bool EX10 { get { return exercicio10; } }
}

