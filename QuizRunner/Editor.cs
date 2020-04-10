using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

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
        private struct Statistics
        {
            public string Prefix;
            public string Calculate;
            public string Postfix;
        }
        private string Name;
        private string[] Descrip = new string[0];
        private Question[] ListOfQuestions = new Question[0];
        Dictionary<string, double> ListOfVariables = new Dictionary<string, double>();
        private Statistics[] StaticsLines = new Statistics[0];
        public void Save(string direction)
        {
            File.Create(direction).Close();
            StreamWriter SW = new StreamWriter(direction);
            SW.WriteLine(Name);
            File.WriteAllLines(direction, Descrip);
            SW.WriteLine(ListOfQuestions.Length.ToString());
            for(var i = 0; i < ListOfQuestions.Length; i++)
            {
                SW.WriteLine(ListOfQuestions[i].QuestionText.Length.ToString());
                File.WriteAllLines(direction, ListOfQuestions[i].QuestionText);
                SW.WriteLine(ListOfQuestions[i].AnswType.ToString());
                SW.WriteLine(ListOfQuestions[i].AnswArr.Length.ToString());
                for(var j = 0; j < ListOfQuestions[i].AnswArr.Length; j++)
                {
                    SW.WriteLine(ListOfQuestions[i].AnswArr[j].AnswerText);
                    SW.WriteLine(ListOfQuestions[i].AnswArr[j].Argument.Length.ToString());
                    File.WriteAllLines(direction, ListOfQuestions[i].AnswArr[j].Argument);
                }
;            }
            SW.WriteLine(ListOfVariables.Count.ToString());
            foreach (string TName in ListOfVariables.Keys)
            {
                SW.WriteLine(TName);
                SW.WriteLine(ListOfVariables[TName].ToString());
            }
            SW.WriteLine(StaticsLines.Length.ToString());
            for(var i = 0; i < StaticsLines.Length; i++)
            {
                SW.WriteLine(StaticsLines[i].Prefix);
                SW.WriteLine(StaticsLines[i].Calculate);
                SW.WriteLine(StaticsLines[i].Postfix);
            }
            SW.Close();
        }
        public void Open(string direction)
        {

        }
    }
   
}
