using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;

namespace game_objects.questions
{
    public class AnswerFactory
    {
        private static Dictionary<string, Answer> cachedAnswers;
        private static readonly int MAX_ATTEMPTS = 100;

        private static Dictionary<string, Answer> CachedAnswers
        {
            get
            {
                if (cachedAnswers == null)
                    cachedAnswers = new Dictionary<string, Answer>();
                return cachedAnswers;
            }
        }

        public static Answer CreateAnswer(Renderer3D r3D, IEnumerable<CollidableGameObject> collidableObjects, Question q, int answerIndex, bool correct, Answer[] otherAnswers = null)
        {
            Answer a;

            switch (q.Subject)
            {
                case QuestionSubject.MAT:
                    a = new Answer(r3D, collidableObjects, CreateMathAnswer(q, answerIndex, otherAnswers, correct));
                    break;
                default:
                    a = new Answer(r3D, collidableObjects, CreatePTAnswer(q, answerIndex, otherAnswers, correct));

                    break;
            }

            return a;
        }

        public static string CreatePTAnswer(Question q, int answerIndex, Answer[] otherAnswers, bool correct)
        {
            string answer;
            string correctAnswer = q.GetAnswer(answerIndex);
            if (correct)
            {
                answer = correctAnswer;
            }
            else
            {
                int attempts = 0;
                bool alreadyInUse;
                do
                {
                    alreadyInUse = false;
                    answer = "";
                    int chance = 10;
                    do
                    {
                        answer += ((char)PublicRandom.Next((int)'A', (int)'Z')).ToString();
                        chance = (int)(chance * 1.5);
                    } while (answer.Length < correctAnswer.Length && PublicRandom.Next(100) > chance);

                    alreadyInUse = SearchAnswer(answer, otherAnswers);

                    attempts++;
                } while (attempts < MAX_ATTEMPTS || answer.Equals(correctAnswer));

            }

            return answer;
        }

        public static string CreateMathAnswer(Question q, int answerIndex, Answer[] otherAnswers, bool correct)
        {
            int min = 0;
            int max = 100;

            string answer;

            string currentModifier = q.GetModifier(answerIndex);

            string correctAnswer = q.GetAnswer(answerIndex);

            int numericAnswer = 0;
            if (Int32.TryParse(correctAnswer, out numericAnswer))
            {
                if (currentModifier.Equals((correct ? "<" : ">")))
                {
                    max = Int32.Parse(correctAnswer);
                }
                else if (currentModifier.Equals((correct ? ">" : "<")))
                {
                    min = Int32.Parse(correctAnswer) + 1;
                }
                else if (correct)
                {
                    min = max = numericAnswer;
                }

                int attempts = 0;
                bool alreadyInUse;
                do
                {
                    answer = PublicRandom.Next(min, max).ToString();
                    alreadyInUse = false;

                    alreadyInUse = SearchAnswer(answer, otherAnswers);

                    attempts++;

                } while ((attempts < MAX_ATTEMPTS && alreadyInUse) || (!correct && answer.Equals(correctAnswer)));
            }
            else
            {
                answer = CreatePTAnswer(q, answerIndex, otherAnswers, correct);
            }

            return answer;
        }

        /// <summary>
        /// Busca por uma determinada resposta em meios a um array de respostas.
        /// </summary>
        /// <param name="answer">A resposta que se procura.</param>
        /// <param name="otherAnswers">O array no qual a resposta será procurada.</param>
        /// <returns>true caso a resposta exista em meio ao conjunto, false do contrário.</returns>
        private static bool SearchAnswer(string answer, Answer[] otherAnswers)
        {
            bool found = false;
            if (otherAnswers != null)
            {
                foreach (Answer a in otherAnswers)
                {
                    if (a != null && a.Text.Equals(answer))
                    {
                        found = true;
                        break;
                    }
                }
            }
            return found;
        }
    }
}
