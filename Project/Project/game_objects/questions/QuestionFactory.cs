using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;

namespace game_objects.questions
{
    public static class QuestionFactory
    {

        private static readonly Random rnd = new Random();
        public static Question CreateQuestion(RunnerLevel level, QuestionSubject subject, int amount)
        {
            //TODO: gerar questões de forma correta
            Question question = new Question(subject, "blablabla", new string[] { ((char)rnd.Next(65, 91)).ToString(), ((char)rnd.Next(65, 91)).ToString(), ((char)rnd.Next(65, 91)).ToString() });
            return question;
        }
    }
}
