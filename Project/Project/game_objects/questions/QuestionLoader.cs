using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace game_objects.questions
{
    public static class QuestionsDatabase
    {
        //as questões são separadas de acordo com sua matéria e cada matéria é subdividida em 3 niveis, sendo que cada nível contem um número variável de questões
        private static Question[][] ptQuestions = new Question[3][];
        private static Question[][] matQuestions = new Question[3][];


        public static void LoadQuestions()
        {
            XDocument xDoc = XDocument.Load("Content/questions.xml");
            foreach (XElement xmlSubject in xDoc.Elements("subject"))
            {
                QuestionSubject subject;
                if (QuestionSubject.TryParse(xmlSubject.FirstAttribute.Value.ToString(), out subject))
                {
                    foreach (XElement xmlDifficulty in xmlSubject.Elements("difficulty"))
                    {
                        int difficulty;
                        if (Int32.TryParse(xmlDifficulty.FirstAttribute.Value.ToString(), out difficulty))
                        {
                            CreatePositions(subject, difficulty, xmlDifficulty.Nodes().Count());
                            int indexCount = 0;
                            foreach (XElement xmlQuestion in xmlDifficulty.Elements("question"))
                            {
                                List<string> answers = new List<string>();
                                foreach (XElement xmlAnswer in xmlQuestion.Elements("answer"))
                                    answers.Add(xmlAnswer.Value);

                                createQuestion(subject, xmlQuestion.Element("header").Value, (string[])answers.ToArray(), difficulty, indexCount);

                                indexCount++;
                            }
                        }
                    }
                }
            }
        }

        private static void createQuestion(QuestionSubject qSubject, string header, string[] answers, int difficulty, int indexCount)
        {
            Question q = new Question(qSubject, header, answers);
            if (qSubject == QuestionSubject.PT)
            {
                ptQuestions[difficulty][indexCount] = q;
            }
            else
            {
                matQuestions[difficulty][indexCount] = q;
            }
        }

        private static void CreatePositions(QuestionSubject qSubject, int difficulty, int elementsCount)
        {
            if (qSubject == QuestionSubject.PT)
            {
                ptQuestions[difficulty] = new Question[elementsCount];
            }
            else
            {
                matQuestions[difficulty] = new Question[elementsCount];
            }
        }
    }
}