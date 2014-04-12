using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace game_objects.questions
{
    public class Question : GameObject
    {
        private QuestionType type;
        private Answer[] answers;
        private string header;
        private int correctAnswer;

        public Question(QuestionType type, string header, Answer[] answers, int corretctAnswer)
        {
            this.type = type;
            this.header = header;
            this.answers = answers;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Answer a in answers)
            {
                a.Update(gameTime);
            }        
        }

        public void Draw(GameTime gameTime)
        {
            foreach(Answer a in answers)
            {
                a.Draw(gameTime);
            }
        }
    }
}
