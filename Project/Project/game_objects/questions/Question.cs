using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project;
using Microsoft.Xna.Framework.Content;

namespace game_objects.questions
{
    public class Question : DrawableGameObject
    {
        private QuestionSubject type;
        private string[] answers;

        private int correctAnswerIndex;
        private string header;

        private Quad[] answersQuads;

        private bool quadsCreated;

        public string[] Answers
        {
            get { return answers; }
        }

        public Quad[] AnswersQuads
        {
            get {
                if (!quadsCreated)
                    createQuads();
                return answersQuads; 
            }
        }

        public Question(Renderer renderer, QuestionSubject type, string header, string[] answers, int correctAnswerIndex) : base(renderer)
        {
            this.type = type;
            this.header = header;
            this.answers = answers;
            this.correctAnswerIndex = correctAnswerIndex;
            answersQuads = new Quad[answers.Length];
        }

        private void createQuads()
        {
            Vector3 nrm = new Vector3(0,0,-1);
            quadsCreated = true;
            float padding = 1.5f;
            float startX = position.X + padding/2 * (answersQuads.Length-1);
            for (int i = 0; i < answersQuads.Length; i++)
            {
                answersQuads[i] = new Quad(new Vector3(startX - i*padding, position.Y, position.Z), nrm, Vector3.Up, 0.5f, 0.5f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Translate(Vector3 amount)
        {
            base.Translate(amount);
            foreach (Quad q in AnswersQuads)
                q.translate(amount);
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
                createQuads();
            }
        }

        public override void Load(ContentManager cManager)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
