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

        private int correctAnswerIndex;

        private const float scale = 3f;

        bool pityPoint;

        private int collidedAnswerIndex;

        private int currentAnswerIndex;

        private int currentAnswerValue;

        private int questionScore;

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

        public QuestionGameObject(Renderer3D renderer, List<CollidableGameObject> collidableObjects, Question question)
            : base(renderer, collidableObjects)
        {
            this.question = question;
            currentAnswerIndex = 0;
            CreateAnswers(renderer);
        }

        //gerar duas respostas falsas para serem apresentadas juntamente da correta
        private void CreateAnswers(Renderer3D renderer)
        {
            this.answers = new Answer[3];
            correctAnswerIndex = 1;// PublicRandom.Next(0, answers.Length);

            char start = 'A';
            char end = 'Z';
            string usedChars = "";
            foreach (string answer in question.Answers)
                usedChars += answer;

            for (int i = 0; i < 3; i++)
            {
                string s = question.Answers[currentAnswerIndex];
                if (i != correctAnswerIndex)
                {
                    s = GenerateFalseAnswer(usedChars, (int)start, (int)end + 1);
                    usedChars += s;
                }
                this.answers[i] = new Answer(renderer, CollidableObjects, s);
            }

            //se o jogador não recebeu 1 ponto de graça, então somamos 1 ao valor da questão atual
            if(!pityPoint)
                currentAnswerValue += 1;
            pityPoint = false;
        }

        private string GenerateFalseAnswer(string usedChars, int start, int end)
        {
            string s = null;
            do
            {
                if (question.Subject == QuestionSubject.PT)
                {
                    s = ((char)PublicRandom.Next(start, end)).ToString();
                }
                else
                {
                    s = PublicRandom.Next(100).ToString();
                }
            } while (usedChars.Contains(s));
            return s;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (Answer a in answers)
                a.Update(gameTime);
        }

        public override void ImediateTranslate(Vector3 amount)
        {
            base.ImediateTranslate(amount);
            foreach (Answer a in answers)
            {
                a.ImediateTranslate(amount);
            }
        }

        public void Retreat()
        {
            foreach (Answer a in answers)
            {
                VariableMovementComponent c = a.GetComponent<VariableMovementComponent>();
                if (c != null)
                {
                    c.Acceleration = new Vector3(0, 0.05f, 0.02f);
                    c.AccelerationVariation = Vector3.Up * - 0.01f;
                    c.CurrentVelocity = new Vector3(0, 0.05f, 0.2f);
                    c.UpperVelocityThreshold = new Vector3(0, 1, 1);
                }
            }
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
                for (int i = 0; i < answers.Length; i++)
                {
                    Vector3 offset = new Vector3(i - 1, PublicRandom.Next(3, 5), 0);
                    this.answers[i].Position = position + offset;
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

        //verifica se a questão já foi completamente respondida e, caso contrario, passa para a próxima resposta (mais para questões de soletrar)
        public bool Next()
        {
            bool hasNext = ++currentAnswerIndex < question.Answers.Length;
            if (hasNext)
            {
                CreateAnswers((Renderer3D)Renderer);
            }
            return hasNext;
        }

        public bool CorrecAnswer()
        {
            bool correct = collidedAnswerIndex == correctAnswerIndex;
            if (correct)
            {
                questionScore += currentAnswerValue;
            }
            else if (!pityPoint && currentAnswerValue > 1) //se o jogador errou e a resposta vale mais de 1 ponto, ainda damos 1 ponto de graça
            {
                pityPoint = true;
                currentAnswerValue = 1;
            }
            else //caso a questão valha 1 ponto ou menos, ou o jogador esteja recebendo um ponto de graça, se ele errar novamente não há mais chances e a resposta vale 0 pontos
            {
                currentAnswerValue = 0;
                pityPoint = false;
            }
            return correct;
        }

        public override bool Collided(CollidableGameObject obj)
        {
            float smallestDistance = -1;
            collidedAnswerIndex = -1;
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
                            collidedAnswerIndex = i;
                            smallestDistance = distance;
                        }
                    }
                }
            }
            return collidedAnswerIndex > -1;
        }
    }
}
