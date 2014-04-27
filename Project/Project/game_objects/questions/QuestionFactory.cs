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
            bool[] usedIndexes;
            switch (subject)
            {
                case (QuestionSubject.PT):
                    usedIndexes = new bool[QuestionsDatabase.PT_Questions[(int)level].Length];
                    int testingIndex = PublicRandom.Next(QuestionsDatabase.PT_Questions[(int)level].Length);
                    while (testingIndex == 3)
                    {
                        testingIndex = PublicRandom.Next(QuestionsDatabase.PT_Questions[(int)level].Length);
                    }
                    usedIndexes[testingIndex] = true;

                    q = QuestionsDatabase.PT_Questions[(int)level][testingIndex];
                    for (int i = 0; i < existingQuestions.Count; i++)
                    {
                        if(q.Header.Equals(existingQuestions[i].Header))
                        {
                            i = 0;
                            while(usedIndexes[testingIndex])
                                testingIndex = PublicRandom.Next(QuestionsDatabase.PT_Questions[(int)level].Length);

                            q = QuestionsDatabase.PT_Questions[(int)level][testingIndex];
                        }
                    }
                    return new QuestionGameObject(renderer, collidableObjects, q);
                default:
                    q = QuestionsDatabase.MAT_Questions[(int)level][PublicRandom.Next(QuestionsDatabase.MAT_Questions[(int)level].Length)];
                    return new QuestionGameObject(renderer, collidableObjects, q);
            }
        }
    }
}
