using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace game_objects.questions
{
    public class QuestionGameObject : CollidableGameObject
    {
        //private QuestionSubject type;
        private Question question;
        private Answer[] answers;

        private int correctAnswerIndex;

        private const float scale = 3f;
        private int collidedAnswerIndex;

        private int currentAnswerIndex;


        public string Header
        {
            get { return question.Header; }
        }
        /*
        public QuestionGameObject(Renderer3D renderer, QuestionSubject subject, string header, string[] answers, int correctAnswerIndex)
            : base(renderer)
        {
            this.type = subject;
            this.header = header;
            this.correctAnswerIndex = correctAnswerIndex;

            question = new Question(subject, header, answers);

            InitAnswers(renderer);
        }
        */
        public QuestionGameObject(Renderer3D renderer, Question question)
            : base(renderer)
        {
            this.question = question;
            currentAnswerIndex = 0;
            CreateAnswers(renderer);
        }

        //gerar duas respostas falsas para serem apresentadas juntamente da correta
        private void CreateAnswers(Renderer3D renderer)
        {
            Random rdn = new Random();
            correctAnswerIndex = 1;// rdn.Next(3);
            this.answers = new Answer[3];
            char start = 'A';
            char end = 'Z';
            string usedChars = question.Answers[currentAnswerIndex];
            for (int i = 0; i < 3; i++)
            {
                string s = question.Answers[currentAnswerIndex];
                if (i != correctAnswerIndex)
                {
                    s = GenerateFalseAnswer(usedChars, rdn, (int)start, (int)end + 1);
                    usedChars += s;
                }
                this.answers[i] = new Answer(renderer, s);
                this.answers[i].Position = position + new Vector3(i-1, 0, 0);
            }

        }

        private string GenerateFalseAnswer(string usedChars, Random rdn, int start, int end)
        {
            string s = null;
            do
            {
                if (question.Subject == QuestionSubject.PT)
                {
                    s = ((char)rdn.Next(start, end)).ToString();
                }
                else
                {
                    s = rdn.Next(100).ToString();
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
            for (int i = 0; i < answers.Length; i++)
            {
                answers[i].ImediateTranslate(amount);
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
                    answers[i].Position = value + new Vector3(i - 1, 0, 0);
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
            return collidedAnswerIndex == correctAnswerIndex;
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
