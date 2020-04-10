using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizRunner.Editor
{
    class Editor
    {
        private struct Answer
        {
            public string AnswerText;
            public string[] Argument;
        }
        private struct Question
        {
            public string[] QuestionText;
            public bool AnswType;
            public Answer[] AnswArr;
        }
        private string Name;
        private string[] Descrip = new string[0];
        private Question[] ListOfQuestions = new Question[0];
    }

}
