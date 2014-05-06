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
        //private QuestionSubject type;
        private Question question;
        private Answer[] answers;

        private Character player;

        private bool pityPoint;

        private int currentAnswerIndex; //qual das respostas da questão o jogador deve pegar atualmente
        private int currentAnswerValue;
        private int questionScore;

        private double chanceToSpawnCorrectAnswer;

        private bool correctAnswerSpawned;
        private Answer collidedAnswer;

        private string correctAnswer;
        private string currentAcceptedAnswer;
        private string currentModifier;
        private string CorrectAnswer
        {
            get
            {
                string s;
                if (string.IsNullOrEmpty(currentAcceptedAnswer))
                {
                    int min = 0;
                    int max = 100;
                    if (currentModifier.Equals("<"))
                    {
                        max = Int32.Parse(correctAnswer);
                    }
                    else
                    {
                        min = Int32.Parse(correctAnswer) + 1;
                    }
                    s = PublicRandom.Next(min, max).ToString();
                }
                else
                {
                    s = currentAcceptedAnswer;
                }
                return s;
            }
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

        public int Score
        {
            get { return questionScore; }
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
            UpdateAnswer();
            CreateAnswers(renderer);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (Answer a in answers)
            {
                if (a.Position.Z <= player.Position.Z + 5)
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
        /// <param name="renderer">O renderizador que desenhará as respostas na tela</param>
        private void CreateAnswers(Renderer3D renderer)
        {
            chanceToSpawnCorrectAnswer = 0.1;
            this.answers = new Answer[3];
            correctAnswerSpawned = false;

            for (int i = 0; i < 3; i++)
            {
                string s = CorrectAnswer;                
                
                if (PublicRandom.NextDouble() > chanceToSpawnCorrectAnswer || correctAnswerSpawned)
                {
                    s = GenerateFalseAnswer();
                }
                else
                {
                    correctAnswerSpawned = true;
                }
                this.answers[i] = new Answer(renderer, CollidableObjects, s);
            }

            //se o jogador não recebeu 1 ponto de graça, então somamos 1 ao valor da questão atual
            if (!pityPoint)
                currentAnswerValue += 1;
            pityPoint = false;
        }


        /// <summary>
        /// Gera uma resposta falsa que seja diferente de qualquer outra que exista no momento.
        /// </summary>
        /// <returns>Uma string contendo uma resposta incorreta</returns>
        private string GenerateFalseAnswer()
        {
            
            string s;
            string usedChars = GetUsedChars();
            if (string.IsNullOrEmpty(currentModifier))
            {                
                do
                {
                    s = GetValidAnswer(usedChars);
                } while (usedChars.Contains(s));
            }
            else
            {
                int min = 0;
                int max = 100;
                if (currentModifier.Equals(">"))
                {
                    max = Int32.Parse(correctAnswer);
                }
                else
                {
                    min = Int32.Parse(correctAnswer)+1;
                }
                do
                {
                    s = GetValidAnswer(usedChars, min, max);
                } while (usedChars.Contains(s));
            }

            return s;
        }

        private string GetValidAnswer(string usedChars, int min = 0, int max=100)
        {
            if (question.Subject == QuestionSubject.PT)
                return ((char)PublicRandom.Next((int)'A', (int)'Z')).ToString();
            else
            {
                return PublicRandom.Next(min, max).ToString();
            }
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
                zIncrement += PublicRandom.Next(4, 8);
                max--;
            }
        }

        /// <summary>
        /// Altera o texto de uma resposta.
        /// </summary>
        /// <param name="a">A resposta que terá seu texto alterado.</param>
        private void ChangeAnswer(Answer a)
        {
            string text = CorrectAnswer;
            if (correctAnswerSpawned || PublicRandom.NextDouble() > chanceToSpawnCorrectAnswer)
            {
                text = GenerateFalseAnswer();
                chanceToSpawnCorrectAnswer *= 2; //dobra a chance de "spawnar" a resposta correta na próxima vez
                if (a.Text.Equals(CorrectAnswer) || ValidMathAnswer(a.Text))
                {
                    correctAnswerSpawned = false;
                }
            }
            else
            {
                correctAnswerSpawned = true;
            }
            if (text == null)
            {
                int i = 0;
            }
            a.Text = text;
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
                UpdateAnswer();

                CreateAnswers((Renderer3D)Renderer);
            }
            return hasNext;
        }

        private void UpdateAnswer()
        {
            correctAnswer = question.GetAnswer(currentAnswerIndex);
            currentModifier = question.GetModifier(currentAnswerIndex);
            if (string.IsNullOrEmpty(currentModifier))
                currentAcceptedAnswer = correctAnswer;
        }

        /// <summary>
        /// Move uma resposta no eixo Z por um valor aleatório entre 3 e 5.
        /// </summary>
        /// <param name="a">A resposta a ser movida.</param>
        public void MoveAnswer(Answer a)
        {
            float newZ = player.Position.Z + PublicRandom.Next(5, 8);
            bool usableZ = false; //utilizado para garantir que as respostas mantenham uma distância mínima de 2.5 (no eixo Z) umas das outras
            int tries = 0;
            do
            {
                newZ += PublicRandom.Next(1, 4);
                foreach (Answer answer in answers)
                {
                    usableZ = (answer.Position.Z >= newZ + 3 || answer.Position.Z <= newZ - 3);
                    if (!usableZ) break;
                }
                tries++;
            } while (!usableZ && tries < 10);

            a.Position = new Vector3(a.Position.X, PublicRandom.Next(3, 5), newZ);
            a.Visible = false;
            ChangeAnswer(a);
        }

        /// <summary>
        /// Checa se uma dada resposta é correta ou não.
        /// </summary>
        /// <param name="a">A resposta a ser checada.</param>
        /// <param name="passingBy">True Se o jogador passou pela questão; False se ele colidiu com a questão.</param>
        /// <returns>true se a questão for correta; false do contrário.</returns>
        public bool CheckAnswer(Answer a, bool passingBy)
        {
            //bool correct = a.Text.Equals(currentAnswer);
            bool correct = verifyAnswer(a);
            if (correct)
            {
                if (passingBy) //se a resposta é correta e o jogador passou por ela, ele cometeu um erro e perderá pontos
                    ReducePoints();
                else
                    questionScore += currentAnswerValue;
            }
            else if (!passingBy) //se a resposta é incorreta, porém o jogador apenas passou por ela, não faz sentido fazê-lo perder pontos
            {
                ReducePoints();
            }
            return correct;
        }

        private bool verifyAnswer(Answer a)
        {
            bool correct = false;
            if (question.Subject == QuestionSubject.PT || string.IsNullOrEmpty(currentModifier))
            {
                correct = a.Text.Equals(currentAcceptedAnswer);
            }
            else if (question.Subject == QuestionSubject.MAT)
            {
                correct = ValidMathAnswer(a.Text);

            }

            return correct;
        }

        private bool ValidMathAnswer(string s)
        {
            bool valid = false;
            if (!string.IsNullOrEmpty(currentModifier))
            {
                switch (currentModifier)
                {
                    case ">":
                        valid = Int32.Parse(s) > Int32.Parse(correctAnswer);
                        break;
                    case "<":
                        valid = Int32.Parse(s) < Int32.Parse(correctAnswer);
                        break;
                }
            }
            return valid;
        }

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
