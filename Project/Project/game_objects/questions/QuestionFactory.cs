using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;
using game_states;

namespace game_objects.questions
{
    public static class QuestionFactory
    {
        private static Question lastCreated;

        public static QuestionGameObject CreateQuestion(RunnerLevel level, QuestionSubject subject, Renderer3D renderer, List<CollidableGameObject> collidableObjects, List<QuestionGameObject> existingQuestions)
        {
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
            int tries = 0;
            for (int i = 0; i < existingQuestions.Count && tries < 100; i++)
            {
                if (q.Header.Equals(existingQuestions[i].Header))
                {
                    usedIndexes[testingIndex] = true;
                    i = 0;
                    
                    while ((usedIndexes[testingIndex] || q.Equals(lastCreated)) && tries < 100)
                    {
                        testingIndex = PublicRandom.Next(questions[level].Length);
                        tries++;
                    }
                    q = questions[level][testingIndex];
                }
            }
            lastCreated = q;
            return q;
        }
    }
}
