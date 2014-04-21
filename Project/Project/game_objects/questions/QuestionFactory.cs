using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;

namespace game_objects.questions
{
    public static class QuestionFactory
    {

        private static readonly Random rdn = new Random();
        public static QuestionGameObject CreateQuestion(RunnerLevel level, QuestionSubject subject, Renderer3D renderer)
        {
            //TODO: gerar questões de forma correta utilizando o QuestionLoader
            QuestionGameObject question = 
                new QuestionGameObject(renderer, new Question(subject, "blablabla", 
                    new string[] { "U"}));
            return question;
        }
    }
}
