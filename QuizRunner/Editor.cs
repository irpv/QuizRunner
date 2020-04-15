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

            // Хранят строки для расчета статистики.
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
        public Dictionary<string, double> ListOfVariables = new Dictionary<string, double>();
        private Statistics[] StaticsLines = new Statistics[0];

        /// <summary>
        /// Записывает тест в файл.
        /// </summary>
        /// <param name="direction">путь к файлу</param>
        public void Save(string direction)
        {
            File.Create(direction).Close();
            StreamWriter SW = new StreamWriter(direction);

            // Записываем имя теста.
            SW.WriteLine(Name);

            // Записываем описание теста.
            File.WriteAllLines(direction, Descrip);

            // Записывает количество вопросов.
            SW.WriteLine(ListOfQuestions.Length.ToString());
            for(var i = 0; i < ListOfQuestions.Length; i++)
            {
                // Записывает текст вопроса.
                SW.WriteLine(ListOfQuestions[i].QuestionText.Length.ToString());
                File.WriteAllLines(direction, ListOfQuestions[i].QuestionText);

                // Записывает тип вопроса.
                SW.WriteLine(ListOfQuestions[i].AnswType.ToString());

                // Записывает количество вариантов ответа.
                SW.WriteLine(ListOfQuestions[i].AnswArr.Length.ToString());
                for(var j = 0; j < ListOfQuestions[i].AnswArr.Length; j++)
                {
                    // Записываает вариант ответа.
                    SW.WriteLine(ListOfQuestions[i].AnswArr[j].AnswerText);

                    // Записывает аргумент ответа.
                    SW.WriteLine(ListOfQuestions[i].AnswArr[j].Argument.Length.ToString());
                    File.WriteAllLines(direction, ListOfQuestions[i].AnswArr[j].Argument);
                }
;            }

            // Записывает список переменных для расчета статистики.
            SW.WriteLine(ListOfVariables.Count.ToString());
            foreach (string TName in ListOfVariables.Keys)
            {
                SW.WriteLine(TName);
                SW.WriteLine(ListOfVariables[TName].ToString());
            }
            // Записывает данные для статистики.
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

            // Считывает имя теста
            Name = SR.ReadLine();
            int TCount;

            // Считывает размер массива описания.
            TCount = Convert.ToInt32(SR.ReadLine());
            Array.Resize<string>(ref Descrip, TCount);

            // Считывает описание
            for(var i = 0; i < TCount; i++)
            {
                Descrip[i] = SR.ReadLine();
            }
            // Считывает количество вопросов.
            TCount = Convert.ToInt32(SR.ReadLine());
            Array.Resize<Question>(ref ListOfQuestions, TCount);
            for (var i = 0; i < TCount; i++)
            {
                // Считывает текст вопроса.
                int TCount2 = Convert.ToInt32(SR.ReadLine());
                Array.Resize<string>(ref ListOfQuestions[i].QuestionText, TCount2);
                for (var j = 0; j < TCount2; j++)
                {
                    ListOfQuestions[i].QuestionText[j] = SR.ReadLine();
                }
                // Считывает тип ответа.
                ListOfQuestions[i].AnswType = Convert.ToBoolean(SR.ReadLine());

                // Считывает количество ответов.
                TCount2 = Convert.ToInt32(SR.ReadLine());
                Array.Resize<Answer>(ref ListOfQuestions[i].AnswArr, TCount2);
                for(var j = 0; j < TCount2; j++)
                {
                    // Считывает текст ответа.
                    ListOfQuestions[i].AnswArr[j].AnswerText = SR.ReadLine();

                    // Считывает аргументы ответа.
                    int TCount3= Convert.ToInt32(SR.ReadLine());
                    Array.Resize<string>(ref ListOfQuestions[i].AnswArr[j].Argument, TCount3);
                    for (var k = 0; k < TCount3; k++)
                    {
                        ListOfQuestions[i].AnswArr[j].Argument[k] = SR.ReadLine();
                    }
                }
            }
            // Считывает количество переменных для расчета статистики.
            TCount = Convert.ToInt32(SR.ReadLine());
            for(var i = 0; i < TCount; i++)
            {
                ListOfVariables.Add(SR.ReadLine(), Convert.ToDouble(SR.ReadLine()));
            }
            // Считывает данные для ведения статистики.
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

        /// <summary>
        /// Возвращает имя теста.
        /// </summary>
        /// <returns>имя теста</returns>
        public string GetName()
        {
            return Name;
        }

        /// <summary>
        /// Возвращает описание теста.
        /// </summary>
        /// <returns>описание теста</returns>
        public string[] GetDescription()
        {
            return Descrip;
        }

        /// <summary>
        /// Возвращает текст вопроса.
        /// </summary>
        /// <param name="numberOfQuest">номер вопроса</param>
        /// <returns>текст вопроса</returns>
        public string[] GetQuestionText(int numberOfQuest)
        {
            return ListOfQuestions[numberOfQuest].QuestionText;
        }

        /// <summary>
        /// Возвращает тип ответа.
        /// </summary>
        /// <param name="numberOfQuest">номер вопроса</param>
        /// <returns>тип ответа</returns>
        public bool GetAnswType(int numberOfQuest)
        {
            return ListOfQuestions[numberOfQuest].AnswType;
        }

        /// <summary>
        /// Возвращает текст ответа.
        /// </summary>
        /// <param name="numberOfQuest">номер вопроса</param>
        /// <param name="numberOfAnsw">номер ответа</param>
        /// <returns>текст ответа</returns>
        public string GetAnswText(int numberOfQuest, int numberOfAnsw)
        {
            return ListOfQuestions[numberOfQuest].AnswArr[numberOfAnsw].AnswerText;
        }

        /// <summary>
        /// Возвращает аргумент для расчета статистики.
        /// </summary>
        /// <param name="numberOfQuest">номер вопроса</param>
        /// <param name="numberOfAnsw">номер ответа</param>
        /// <returns>аргумент</returns>
        public string[] GetArgument(int numberOfQuest, int numberOfAnsw)
        {
            return ListOfQuestions[numberOfQuest].AnswArr[numberOfAnsw].Argument;
        }

        /// <summary>
        /// Возвращает префикс для расчета статистики.
        /// </summary>
        /// <param name="numberOfStatLines">номер строки со статистикой</param>
        /// <returns>префикс</returns>
        public string GetStatistPrefix(int numberOfStatLines)
        {
            return StaticsLines[numberOfStatLines].Prefix;
        }

        /// <summary>
        /// Возвращают строку с расчетами для статистики.
        /// </summary>
        /// <param name="numberOfStatLines">номер строки со статистикой</param>
        /// <returns>расчеты</returns>
        public string GetStatistCalculate(int numberOfStatLines)
        {
            return StaticsLines[numberOfStatLines].Calculate;
        }

        /// <summary>
        /// Возвращают строку с постфиксом для расчета статистики.
        /// </summary>
        /// <param name="numberOfStatLines">номер строки со статистикой</param>
        /// <returns>постфикс</returns>
        public string GetStatistPostfix(int numberOfStatLines)
        {
            return StaticsLines[numberOfStatLines].Postfix;
        }

        /// <summary>
        /// Задает имя теста.
        /// </summary>
        /// <param name="name">имя теста</param>
        public void SetName(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Задает описание теста.
        /// </summary>
        /// <param name="descript">описание теста</param>
        public void SetDescrip(string[] descript)
        {
            Descrip = descript;
        }

        /// <summary>
        /// Задает текст вопроса.
        /// </summary>
        /// <param name="qtext">текст вопроса</param>
        /// <param name="numOfQuest">номер вопроса</param>
        public void SetQuestText(string[] qtext, int numOfQuest)
        {
            ListOfQuestions[numOfQuest].QuestionText = qtext;
        }

        /// <summary>
        /// Задает тип ответа на вопрос.
        /// </summary>
        /// <param name="answt">тип ответа</param>
        /// <param name="numOfQuest">номер вопроса</param>
        public void SetAnswType(bool answt, int numOfQuest)
        {
            ListOfQuestions[numOfQuest].AnswType = answt;
        }

        /// <summary>
        /// Задает текст ответа на вопрос.
        /// </summary>
        /// <param name="numOfQuest">номер вопроса</param>
        /// <param name="numOfAnsw">номер ответа</param>
        /// <param name="atext">текст ответа</param>
        public void SetAnswText(int numOfQuest, int numOfAnsw, string atext)
        {
            ListOfQuestions[numOfQuest].AnswArr[numOfAnsw].AnswerText = atext;
        }

        /// <summary>
        /// Задает аргумент ответа для расчета статистики.
        /// </summary>
        /// <param name="numOfQuest">номер вопроса</param>
        /// <param name="numOfAnsw">номер ответа</param>
        /// <param name="argum">аргумент</param>
        public void SetAnswArgument(int numOfQuest, int numOfAnsw, string[] argum)
        {
            ListOfQuestions[numOfQuest].AnswArr[numOfAnsw].Argument = argum;
        }

        /// <summary>
        /// Задает префикс для расчета статистики.
        /// </summary>
        /// <param name="numOfStatLine">номер строки статистики</param>
        /// <param name="prfx">префикс</param>
        public void SetStatPrefix(int numOfStatLine, string prfx)
        {
            StaticsLines[numOfStatLine].Prefix = prfx;
        }
    }
}
