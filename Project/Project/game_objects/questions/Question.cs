using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game_objects.questions
{
    public class Question
    {
        QuestionSubject subject;
        string header;
        string[] answers;

        public QuestionSubject Subject
        {
            get { return subject; }
        }

        public string Header
        {
            get { return header; }
        }

        public int AnswerCount
        {
            get { return answers.Length; }
        }

        public string GetAnswer(int index)
        {
            string answer = answers[index].Substring(answers[index].LastIndexOf('|')+1);
            return answer;
        }

        public string GetModifier(int index)
        {
            string modifier = "";
            if (answers[index].Contains('|'))
            {
                int index1 = answers[index].LastIndexOf('=')+1;
                int length = answers[index].LastIndexOf('|') - index1;
                modifier = answers[index].Substring(index1, length);
            }
            return modifier;
        }

        public Question(QuestionSubject subject, string header, string[] answers)
        {
            this.subject = subject;
            this.header = header;
            this.answers = answers;
        }
    }
}
