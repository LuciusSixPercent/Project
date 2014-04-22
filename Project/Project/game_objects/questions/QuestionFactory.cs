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
            switch (subject)
            {
                case (QuestionSubject.PT):
                    return new QuestionGameObject(renderer, QuestionsDatabase.PT_Questions[(int)level][rdn.Next(QuestionsDatabase.PT_Questions[(int)level].Length)]);
                default:
                    return new QuestionGameObject(renderer, QuestionsDatabase.MAT_Questions[(int)level][rdn.Next(QuestionsDatabase.MAT_Questions[(int)level].Length)]);
            }
        }
    }
}
