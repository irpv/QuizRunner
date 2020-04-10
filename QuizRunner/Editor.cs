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
        /// <summary>
        /// Записывает тест в файл.
        /// </summary>
        /// <param name="direction">путь к файлу</param>
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
        /// <summary>
        /// Считывает тест из файла по данному пути.
        /// </summary>
        /// <param name="direction">путь к файлу</param>
        public void Open(string direction)
        {
            StreamReader SR = new StreamReader(direction);
            Name = SR.ReadLine();
            int TCount;
            TCount = Convert.ToInt32(SR.ReadLine());
            Array.Resize<string>(ref Descrip, TCount);
            for(var i = 0; i < TCount; i++)
            {
                Descrip[i] = SR.ReadLine();
            }
            TCount = Convert.ToInt32(SR.ReadLine());
            Array.Resize<Question>(ref ListOfQuestions, TCount);
            for (var i = 0; i < TCount; i++)
            {
                int TCount2 = Convert.ToInt32(SR.ReadLine());
                Array.Resize<string>(ref ListOfQuestions[i].QuestionText, TCount2);
                for (var j = 0; j < TCount2; j++)
                {
                    ListOfQuestions[i].QuestionText[j] = SR.ReadLine();
                }
                ListOfQuestions[i].AnswType = Convert.ToBoolean(SR.ReadLine());
                TCount2 = Convert.ToInt32(SR.ReadLine());
                Array.Resize<Answer>(ref ListOfQuestions[i].AnswArr, TCount2);
                for(var j = 0; j < TCount2; j++)
                {
                    ListOfQuestions[i].AnswArr[j].AnswerText = SR.ReadLine();
                    int TCount3= Convert.ToInt32(SR.ReadLine());
                    Array.Resize<string>(ref ListOfQuestions[i].AnswArr[j].Argument, TCount3);
                    for (var k = 0; k < TCount3; k++)
                    {
                        ListOfQuestions[i].AnswArr[j].Argument[k] = SR.ReadLine();
                    }
                }
            }
            TCount = Convert.ToInt32(SR.ReadLine());
            for(var i = 0; i < TCount; i++)
            {
                ListOfVariables.Add(SR.ReadLine(), Convert.ToDouble(SR.ReadLine()));
            }
            TCount = Convert.ToInt32(SR.ReadLine());
            Array.Resize<Statistics>(ref StaticsLines, TCount);
            for(var i = 0; i < TCount; i++)
            {
                StaticsLines[i].Prefix = SR.ReadLine();
                StaticsLines[i].Calculate = SR.ReadLine();
                StaticsLines[i].Postfix = SR.ReadLine();
            }
            SR.Close();
        }
    }
   
}
