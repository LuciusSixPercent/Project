using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;

namespace game_objects.questions
{
    public static class QuestionFactory
    {
        public static QuestionGameObject CreateQuestion(RunnerLevel level, QuestionSubject subject, Renderer3D renderer, List<CollidableGameObject> collidableObjects)
        {
            //TODO: gerar questões de forma correta utilizando o QuestionLoader
            switch (subject)
            {
                case (QuestionSubject.PT):
                    return new QuestionGameObject(renderer, collidableObjects, QuestionsDatabase.PT_Questions[(int)level][PublicRandom.Next(QuestionsDatabase.PT_Questions[(int)level].Length)]);
                default:
                    return new QuestionGameObject(renderer, collidableObjects, QuestionsDatabase.MAT_Questions[(int)level][PublicRandom.Next(QuestionsDatabase.MAT_Questions[(int)level].Length)]);
            }
        }
    }
}
