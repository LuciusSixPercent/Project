using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;

namespace game_objects.questions
{
    public static class QuestionFactory
    {
        public static QuestionGameObject CreateQuestion(RunnerLevel level, QuestionSubject subject, Renderer3D renderer, List<CollidableGameObject> collidableObjects, List<QuestionGameObject> existingQuestions)
        {
            //TODO: gerar questões de forma correta utilizando o QuestionLoader
            Question q = null;
            switch (subject)
            {
                case (QuestionSubject.PT):
                    q = GenerateQuestion((int)level, existingQuestions, QuestionsDatabase.PT_Questions);
                    break;
                default:
                    q = GenerateQuestion((int)level, existingQuestions, QuestionsDatabase.MAT_Questions);
                    break;
            }
            return new QuestionGameObject(renderer, collidableObjects, q);
        }

        private static Question GenerateQuestion(int level, List<QuestionGameObject> existingQuestions, Question[][] questions)
        {
            Question q;
            bool[] usedIndexes = new bool[questions[level].Length];
            int testingIndex = PublicRandom.Next(questions[level].Length);

            q = questions[level][testingIndex];
            for (int i = 0; i < existingQuestions.Count; i++)
            {
                if (q.Header.Equals(existingQuestions[i].Header))
                {
                    usedIndexes[testingIndex] = true;
                    i = 0;
                    while (usedIndexes[testingIndex])
                        testingIndex = PublicRandom.Next(questions[level].Length);

                    q = questions[level][testingIndex];
                }
            }
            return q;
        }
    }
}
