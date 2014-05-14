using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using components;

namespace game_objects.questions
{
    public class QuestionGameObject : CollidableGameObject
    {
        private Question question;
        private Answer[] answers;

        private Character player;

        private bool pityPoint;

        private int currentAnswerIndex; //qual das respostas da questão o jogador deve pegar atualmente
        private int currentAnswerValue;
        //private int questionScore;

        private double chanceToSpawnCorrectAnswer;

        private bool correctAnswerSpawned;
        private Answer collidedAnswer;

        private Answer correctAnswer;
        private int fakeSpawnCount;
        private int maxFakeSpawnCount;
        private float minAnswerDistance;

        private float bonusChance;
        private bool bonusSpawned;
        private int bonusCount;

        public float BonusChance
        {
            get { return bonusChance; }
            set { bonusChance = value; }
        }

        public Question Question
        {
            get { return question; }
        }

        /// <summary>
        /// Responsável por limitar o número de respostas falsas geradas entre cada geração de resposta correta. Um valor 3 significa que a cada resposta correta criada,
        /// apenas 3 falsas poderão ser criadas antes que uma nova resposta correta apareça.
        /// </summary>
        public int MaxFakeSpawnCount
        {
            get { return maxFakeSpawnCount; }
            set { maxFakeSpawnCount = value; }
        }

        public int AnswerCount
        {
            get { return question.AnswerCount; }
        }

        /// <summary>
        /// A resposta com a qual o jogador colidiu mais recentemente
        /// </summary>
        public Answer CollidedAnswer
        {
            get { return collidedAnswer; }
        }

        public int CurrentAnswerValue
        {
            get { return currentAnswerValue; }
        }

        public string Header
        {
            get { return question.Header; }
        }

        public Character Player
        {
            get { return player; }
            set { player = value; }
        }

        public override Vector3 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                PositionAnswers();
            }
        }

        public QuestionGameObject(Renderer3D renderer, List<CollidableGameObject> collidableObjects, Question question)
            : base(renderer, collidableObjects)
        {
            this.question = question;
            this.currentAnswerIndex = 0;
            this.maxFakeSpawnCount = 1;
            this.minAnswerDistance = 3;
            this.bonusChance = 0.75f;
            CreateAnswers();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (Answer a in answers)
            {
                if (a.Position.Z <= player.Position.Z + 7)
                    a.Visible = true;
                a.Update(gameTime);
            }
        }

        public override void ImediateTranslate(Vector3 amount)
        {
            if (amount != Vector3.Zero)
            {
                base.ImediateTranslate(amount);
                foreach (Answer a in answers)
                {
                    a.ImediateTranslate(amount);
                }
            }
        }

        public override void Load(ContentManager cManager)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Answer a in answers)
                a.Draw(gameTime);
        }

        public override bool Collided(CollidableGameObject obj)
        {
            float smallestDistance = -1;
            bool collided = false;
            for (int i = 0; i < answers.Length; i++)
            {
                Answer a = answers[i];

                if (a.Collided(obj))
                {
                    if (obj is Character)
                    {
                        float distance = (float)Math.Abs(obj.Position.X - a.Position.X);
                        if (smallestDistance == -1 || distance < smallestDistance)
                        {
                            collidedAnswer = a;
                            smallestDistance = distance;
                            collided = true;
                        }
                    }
                }
            }
            return collided;
        }

        /// <summary>
        /// Gera duas respostas falsas para serem apresentadas juntamente da correta
        /// </summary>
        private void CreateAnswers()
        {
            chanceToSpawnCorrectAnswer = 0.1;
            correctAnswerSpawned = false;
            correctAnswer = null;
            this.answers = new Answer[3];

            for (int i = 0; i < 3; i++)
            {

                if (PublicRandom.NextDouble() > chanceToSpawnCorrectAnswer || correctAnswerSpawned)
                {
                    this.answers[i] = AnswerFactory.CreateAnswer((Renderer3D)Renderer, CollidableObjects, question, currentAnswerIndex, false, answers);
                }
                else
                {
                    correctAnswerSpawned = true;
                    this.answers[i] = AnswerFactory.CreateAnswer((Renderer3D)Renderer, CollidableObjects, question, currentAnswerIndex, true, answers);
                    correctAnswer = this.answers[i];
                }
            }

            //se o jogador não recebeu 1 ponto de graça, então somamos 1 ao valor da questão atual
            if (!pityPoint)
                currentAnswerValue += 1;
            pityPoint = false;
        }

        /// <summary>
        /// Reposiciona as questões, deixando-as espaçadas aleatóriamente no eixo Z.
        /// </summary>
        private void PositionAnswers()
        {
            float zIncrement = 0;
            bool[] answersPositioned = new bool[answers.Length];
            int max = 3;
            bool visible = true;
            while (max > 0)
            {
                int i = PublicRandom.Next(max);
                while (answersPositioned[i])
                {
                    i += (PublicRandom.Next(2) == 0 ? 1 : -1);
                    if (i >= answers.Length)
                        i -= answers.Length;
                    else if (i < 0)
                        i += answers.Length;
                }
                answersPositioned[i] = true;
                Vector3 offset = new Vector3(i - 1, PublicRandom.Next(3, 5), zIncrement);
                this.answers[i].Position = position + offset;
                this.answers[i].Visible = visible;
                visible = false;
                zIncrement += PublicRandom.Next(3, 6);
                max--;
            }
        }

        /// <summary>
        /// Altera o texto de uma resposta.
        /// </summary>
        /// <param name="a">A resposta que terá seu texto alterado.</param>
        private void ChangeAnswer(Answer a)
        {
            string text;
            double bonusLottery = PublicRandom.NextDouble();
            if (correctAnswerSpawned || (PublicRandom.NextDouble() > chanceToSpawnCorrectAnswer && fakeSpawnCount < maxFakeSpawnCount || (bonusLottery > bonusChance  && !bonusSpawned))) //alterar a resposta para alguma outra incorreta
            {
                if (!bonusSpawned && bonusLottery <= bonusChance && !correctAnswerSpawned && bonusCount < 3)
                {
                    text = "+1";
                    a.IsBonus = true;
                    bonusSpawned = true;
                    bonusCount++;
                }
                else
                {
                    text = IncorrectAnswer();
                    a.IsBonus = false;
                    bonusSpawned = false;
                }

                //se a resposta sendo alterada for a correta, avisar o sistema para que uma nova resposta correta possa ser gerada futuramente
                if (correctAnswer != null && a.Equals(correctAnswer))
                {
                    correctAnswerSpawned = false;
                    correctAnswer = null;
                }
                chanceToSpawnCorrectAnswer *= 3; //triplica a chance de "spawnar" a resposta correta na próxima vez
                fakeSpawnCount++;
            }
            else //alterar a resposta para a resposta correta
            {
                a.IsBonus = false;
                bonusSpawned = false;
                text = CorrectAnswer();
                chanceToSpawnCorrectAnswer = 0.1;
                fakeSpawnCount = 0;
                bonusCount = 0;
                correctAnswerSpawned = true;
                this.correctAnswer = a;
            }

            a.Text = text;
        }

        /// <summary>
        /// Gera uma resposta correta de acordo com a matéria da qual a questão trata.
        /// </summary>
        /// <returns>Uma string contendo a resposta correta.</returns>
        private string CorrectAnswer()
        {
            string correctAnswerText;

            if (question.Subject == QuestionSubject.PT)
            {
                correctAnswerText = AnswerFactory.CreatePTAnswer(question, currentAnswerIndex, answers, true);
            }
            else
            {
                correctAnswerText = AnswerFactory.CreateMathAnswer(question, currentAnswerIndex, answers, true);
            }

            return correctAnswerText;
        }

        /// <summary>
        /// Gera uma resposta incorreta de acordo com a matéria da qual a questão trata.
        /// </summary>
        /// <returns>Uma string contendo a resposta incorreta.</returns>
        private string IncorrectAnswer()
        {
            string incorrectAnswerText;

            if (question.Subject == QuestionSubject.PT)
            {
                incorrectAnswerText = AnswerFactory.CreatePTAnswer(question, currentAnswerIndex, answers, false);
            }
            else
            {
                incorrectAnswerText = AnswerFactory.CreateMathAnswer(question, currentAnswerIndex, answers, false);
            }

            return incorrectAnswerText;
        }

        /// <summary>
        /// Retorna todos os caracteres que estão sendo utilizados (tanto das respostas corretas quanto das incorretas.
        /// </summary>
        /// <returns>Os caracteres que estão sendo utilizados.</returns>
        private string GetUsedChars()
        {
            string usedChars = "";
            for (int i = 0; i < question.AnswerCount; i++)
            {
                usedChars += question.GetAnswer(i);
            }

            foreach (Answer answer in answers)
            {
                if (answer != null)
                    usedChars += answer.Text;
            }
            return usedChars;
        }

        /// <summary>
        /// Busca a resposta com o menor valor de coordenada no eixo Z.
        /// </summary>
        /// <returns>A resposta com menor coordenada no eixo Z.</returns>
        public Answer GetClosestAnswer()
        {
            Answer closest = answers[0];
            for (int i = 1; i < answers.Length; i++)
            {
                if (answers[i].Position.Z < closest.Position.Z)
                    closest = answers[i];
            }
            return closest;
        }

        /// <summary>
        /// Verifica se a questão já foi completamente respondida e, caso contrario, passa para a próxima resposta (mais para questões de soletrar)
        /// </summary>
        /// <returns>true se ainda houverem respostas a serem adivinhadas; false do contrário.</returns>
        public bool Next()
        {
            bool hasNext = ++currentAnswerIndex < question.AnswerCount;
            if (hasNext)
            {
                CreateAnswers();
            }
            return hasNext;
        }

        /// <summary>
        /// Move uma resposta no eixo Z por um valor aleatório entre 3 e 5.
        /// </summary>
        /// <param name="a">A resposta a ser movida.</param>
        public void MoveAnswer(Answer a)
        {
            float newZ = player.Position.Z + PublicRandom.Next(4, 7);
            bool usableZ = false;
            int tries = 0;
            do
            {
                newZ += (float)PublicRandom.NextDouble(0.25);
                foreach (Answer answer in answers)
                {
                    usableZ = (answer.Position.Z >= newZ + minAnswerDistance || answer.Position.Z <= newZ - minAnswerDistance);
                    if (!usableZ) break;
                }
                tries++;
            } while (!usableZ && tries < 50);

            a.Position = new Vector3(a.Position.X, PublicRandom.Next(3, 5), newZ);
            a.Visible = false;
            ChangeAnswer(a);
        }

        /// <summary>
        /// Checa se uma dada resposta é correta ou não ou se é um bonus e atualiza a pontuação conforme necessário.
        /// </summary>
        /// <param name="a">A resposta a ser checada.</param>
        /// <param name="passingBy">True Se o jogador passou pela questão; False se ele colidiu com a questão.</param>
        /// <returns>true se a questão for correta; false do contrário.</returns>
        public bool CheckAnswer(Answer a, bool passingBy)
        {
            bool correct = correctAnswerSpawned && a.Equals(correctAnswer);

            if (correct)
            {
                if (passingBy)
                    ReducePoints();
            }
            else if (!passingBy)
            {
                if (!a.IsBonus)
                {
                    ReducePoints();
                }
            }

            return correct;
        }

        /// <summary>
        /// Soma o valor da resposta atual ao score da questão.
        /// </summary>
        //private void AddPoints(int amount)
        //{
        //    questionScore += amount;
        //}

        /// <summary>
        /// Reduz o valor da resposta atual.
        /// </summary>
        private void ReducePoints()
        {
            if (!pityPoint && currentAnswerValue > 1) //se o jogador errou e a resposta vale mais de 1 ponto, ele ainda recebe 1 ponto de graça
            {
                pityPoint = true;
                currentAnswerValue = 1;
            }
            else //caso a questão valha 1 ponto e o mesmo seja de graça, a resposta agora vale 0 pontos
            {
                currentAnswerValue = 0;
                pityPoint = false;
            }
        }
    }
}
