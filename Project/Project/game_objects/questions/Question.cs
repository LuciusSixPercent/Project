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

        public string[] Answers
        {
            get { return answers; }
        }

        public Question(QuestionSubject subject, string header, string[] answers)
        {
            this.subject = subject;
            this.header = header;
            this.answers = answers;
        }
    }
}
