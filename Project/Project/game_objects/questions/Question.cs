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
    public class Question : CollidableGameObject
    {
        private QuestionSubject type;
        private Answer[] answers;

        private int correctAnswerIndex;
        private string header;

        private Quad answerQuad;

        private Texture2D texture;

        private bool quadsCreated;

        private float answersPadding = 1;

        private const float scale = 3f;
        private int collidedAnswerIndex;

        public Question(Renderer3D renderer, QuestionSubject type, string header, string[] answers, int correctAnswerIndex)
            : base(renderer)
        {
            this.type = type;
            this.header = header;
            this.correctAnswerIndex = correctAnswerIndex;

            this.answers = new Answer[3];
            for (int i = 0; i < answers.Length; i++ )
            {
                this.answers[i] = new Answer(renderer, answers[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Translate(Vector3 amount)
        {
            for (int i = 0; i < answers.Length; i++)
            {
                answers[i].Translate(amount);
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
            foreach (Answer a in answers)
                a.Load(cManager);
        }
        public override void Draw(GameTime gameTime)
        {
            foreach (Answer a in answers)
                a.Draw(gameTime);
        }

        public bool CorrecAnswerAxis(float x)
        {
            return collidedAnswerIndex == correctAnswerIndex;
        }

        public override bool Collided(CollidableGameObject obj)
        {
            for (int i = 0; i < answers.Length; i++)
            {
                Answer a = answers[i];
                if (a.Collided(obj))
                {
                    collidedAnswerIndex = i;
                    return true;
                }
            }
            return false;
        }
    }
}
