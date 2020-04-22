using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Collections;

namespace QuizRunner.Editor
{
    public class Editor
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
            StreamWriter SW = File.AppendText(direction);

            // Записываем имя теста.
            SW.WriteLine(Name);

            // Записываем размера массива с описанием.
            SW.WriteLine(Descrip.Length.ToString());
            SW.Close();
           
            // Записываем описание теста.
            File.AppendAllLines(direction, Descrip);
            SW = File.AppendText(direction);
            // Записывает количество вопросов.
            SW.WriteLine(ListOfQuestions.Length.ToString());
            for (int i = 0; i < ListOfQuestions.Length; i++)
            {
                // Записывает текст вопроса.
                SW.WriteLine(ListOfQuestions[i].QuestionText.Length.ToString());
                SW.Close();
                File.AppendAllLines(direction, ListOfQuestions[i].QuestionText);
                SW = File.AppendText(direction);

                // Записывает тип вопроса.
                SW.WriteLine(ListOfQuestions[i].AnswType.ToString());

                // Записывает количество вариантов ответа.
                SW.WriteLine(ListOfQuestions[i].AnswArr.Length.ToString());
                SW.Close();
                SW = File.AppendText(direction);
                for (var j = 0; j < ListOfQuestions[i].AnswArr.Length; j++)
                {
                    // Записываает вариант ответа.
                    SW.WriteLine(ListOfQuestions[i].AnswArr[j].AnswerText);
                    SW.Close();
                    // Записывает аргумент ответа.
                    SW = File.AppendText(direction);
                    SW.WriteLine(ListOfQuestions[i].AnswArr[j].Argument.Length.ToString());
                    SW.Close();
                    File.AppendAllLines(direction, ListOfQuestions[i].AnswArr[j].Argument);
                    SW = File.AppendText(direction);
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
                ListOfQuestions[i].AnswArr = new Answer[0];
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
        public bool GetAnswerType(int numberOfQuest)
        {
            return ListOfQuestions[numberOfQuest].AnswType;
        }

        /// <summary>
        /// Возвращает текст ответа.
        /// </summary>
        /// <param name="numberOfQuest">номер вопроса</param>
        /// <param name="numberOfAnsw">номер ответа</param>
        /// <returns>текст ответа</returns>
        public string GetAnswerText(int numberOfQuest, int numberOfAnsw)
        {
            return ListOfQuestions[numberOfQuest].AnswArr[numberOfAnsw].AnswerText;
        }

        /// <summary>
        /// Возвращает аргумент для расчета статистики.
        /// </summary>
        /// <param name="numberOfQuest">номер вопроса</param>
        /// <param name="numberOfAnsw">номер ответа</param>
        /// <returns>аргумент</returns>
        public string[] GetAnswerArgument(int numberOfQuest, int numberOfAnsw)
        {
            return ListOfQuestions[numberOfQuest].AnswArr[numberOfAnsw].Argument;
        }

        /// <summary>
        /// Возвращает префикс для расчета статистики.
        /// </summary>
        /// <param name="numberOfStatLines">номер строки со статистикой</param>
        /// <returns>префикс</returns>
        public string GetStatPrefix(int numberOfStatLines)
        {
            return StaticsLines[numberOfStatLines].Prefix;
        }

        /// <summary>
        /// Возвращают строку с расчетами для статистики.
        /// </summary>
        /// <param name="numberOfStatLines">номер строки со статистикой</param>
        /// <returns>расчеты</returns>
        public string GetStatCalculate(int numberOfStatLines)
        {
            return StaticsLines[numberOfStatLines].Calculate;
        }

        /// <summary>
        /// Возвращают строку с постфиксом для расчета статистики.
        /// </summary>
        /// <param name="numberOfStatLines">номер строки со статистикой</param>
        /// <returns>постфикс</returns>
        public string GetStatPostfix(int numberOfStatLines)
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
        public void SetQuestionText(string[] qtext, int numOfQuest)
        {
            if(ListOfQuestions.Length <= numOfQuest)
            {
                int TCount = ListOfQuestions.Length;
                Array.Resize<Question>(ref ListOfQuestions, numOfQuest + 1);
                for(var i = TCount; i <= numOfQuest; i++)
                {
                    ListOfQuestions[i] = new Question();
                    ListOfQuestions[i].QuestionText = new string[0];
                }
                
            }
            ListOfQuestions[numOfQuest].QuestionText = qtext;
        }

        /// <summary>
        /// Задает тип ответа на вопрос.
        /// </summary>
        /// <param name="answt">тип ответа</param>
        /// <param name="numOfQuest">номер вопроса</param>
        public void SetAnswType(bool answt, int numOfQuest)
        {
            if (ListOfQuestions.Length <= numOfQuest)
            {
                Array.Resize<Question>(ref ListOfQuestions, numOfQuest + 1);
            }
            ListOfQuestions[numOfQuest].AnswType = answt;
        }

        /// <summary>
        /// Задает текст ответа на вопрос.
        /// </summary>
        /// <param name="numOfQuest">номер вопроса</param>
        /// <param name="numOfAnsw">номер ответа</param>
        /// <param name="atext">текст ответа</param>
        public void SetAnswText(string atext, int numOfQuest, int numOfAnsw)
        {
            if (ListOfQuestions.Length <= numOfQuest)
            {
                int TCount = ListOfQuestions.Length;
                Array.Resize<Question>(ref ListOfQuestions, numOfQuest + 1);
                for (var i = TCount; i <= numOfQuest; i++)
                {
                    ListOfQuestions[i] = new Question();
                    ListOfQuestions[numOfQuest].AnswArr = new Answer[0];
                }
            }
            Array.Resize<Answer>(ref ListOfQuestions[numOfQuest].AnswArr, 1);
            if (ListOfQuestions[numOfQuest].AnswArr.Length <= numOfAnsw)
            {
                int TCount1 = ListOfQuestions[numOfQuest].AnswArr.Length;
                Array.Resize<Answer>(ref ListOfQuestions[numOfQuest].AnswArr, numOfAnsw + 1);
                for (var j = TCount1; j <= numOfAnsw; j++)
                { 
                    ListOfQuestions[numOfQuest].AnswArr[j].AnswerText = "";
                }
            }
            
            ListOfQuestions[numOfQuest].AnswArr[numOfAnsw].AnswerText = atext;
        }

        /// <summary>
        /// Задает аргумент ответа для расчета статистики.
        /// </summary>
        /// <param name="numOfQuest">номер вопроса</param>
        /// <param name="numOfAnsw">номер ответа</param>
        /// <param name="argum">аргумент</param>
        public void SetAnswArgument(string[] argum, int numOfQuest, int numOfAnsw)
        {
            if (ListOfQuestions.Length <= numOfQuest)
            {
                int TCount = ListOfQuestions.Length;
                Array.Resize<Question>(ref ListOfQuestions, numOfQuest + 1);
                for (var i = TCount; i <= numOfQuest; i++)
                {
                    ListOfQuestions[i] = new Question();
                    ListOfQuestions[i].AnswArr = new Answer[0];
                }
            }
            if (ListOfQuestions[numOfQuest].AnswArr.Length <= numOfAnsw)
            {
                int TCount1 = ListOfQuestions[numOfQuest].AnswArr.Length;
                Array.Resize<Answer>(ref ListOfQuestions[numOfQuest].AnswArr, numOfAnsw+1);
                for (var j = TCount1; j <= numOfAnsw; j++)
                {
                    ListOfQuestions[numOfQuest].AnswArr[j].Argument = new string[0];
                }
            }
            ListOfQuestions[numOfQuest].AnswArr[numOfAnsw].Argument = argum;
        }

        /// <summary>
        /// Задает префикс для расчета статистики.
        /// </summary>
        /// <param name="numOfStatLine">номер строки статистики</param>
        /// <param name="prfx">префикс</param>
        public void SetStatPrefix(string prfx, int numOfStatLine)
        {
            if (StaticsLines.Length <= numOfStatLine)
            {
                Array.Resize<Statistics>(ref StaticsLines, numOfStatLine + 1);
            }
            StaticsLines[numOfStatLine].Prefix = prfx;
        }

        /// <summary>
        /// Задает строку с расчетами для статистики.
        /// </summary>
        /// <param name="numOfStatLine">номер строки статистики</param>
        /// <param name="calc">строка расчетов</param>
        public void SetStatCalculate(string calc, int numOfStatLine)
        {
            if (StaticsLines.Length <= numOfStatLine)
            {
                Array.Resize<Statistics>(ref StaticsLines, numOfStatLine + 1);
            }
            StaticsLines[numOfStatLine].Calculate = calc;
        }

        /// <summary>
        /// Задает постфикс для расчета статистики.
        /// </summary>
        /// <param name="numOfStatLine">номер строки статистики</param>
        /// <param name="pstfx">постфикс</param>
        public void SetStatPostfix(string pstfx, int numOfStatLine)
        {
            if (StaticsLines.Length <= numOfStatLine)
            {
                Array.Resize<Statistics>(ref StaticsLines, numOfStatLine + 1);
            }
            StaticsLines[numOfStatLine].Postfix = pstfx;
        }

        /// <summary>
        /// Возвращает количество вопросов.
        /// </summary>
        /// <returns>количество вопросов</returns>
        public int NumberOfQuestion()
        {
            return ListOfQuestions.Length;
        }

        /// <summary>
        /// Возвращает количество ответов.
        /// </summary>
        /// <param name="numOfQuest">номер вопроса</param>
        /// <returns>количество ответов</returns>
        public int NumberOfAnswers(int numOfQuest)
        {
            return ListOfQuestions[numOfQuest].AnswArr.Length;
        }

        /// <summary>
        /// Возвращает количество строк статистики.
        /// </summary>
        /// <returns>количество строк статистики</returns>
        public int NumberOfStatLine()
        {
            return StaticsLines.Length;
        }

        /// <summary>
        /// Возвращает количество аргументов ответа.
        /// </summary>
        /// <param name="numOfQuest">номер вопроса</param>
        /// <param name="numOfAnsw">номер ответа</param>
        /// <returns>количество аргументов</returns>
        public int NumberOfArgument(int numOfQuest, int numOfAnsw)
        {
            return ListOfQuestions[numOfQuest].AnswArr[numOfAnsw].Argument.Length;
        }
        public bool IsCorrect(string input)
        {
            string inpt = input.Replace(" ", "");
            bool flag = false;
            int N = inpt.Length;
            int coord = -1;
            int eq = 0;
            for (var i = 0; i < N; i++)
            {
                if (inpt[i] == '[')
                {
                    for (var j = i + 1; j < N; j++)
                    {
                        if (inpt[j] == '[')
                        {
                            flag = false;
                            break;
                        }
                        if (inpt[j] == ']')
                        {
                            flag = true;
                            coord = j;
                        }
                    }
                    for (int k = i + 1; k < coord; k++)
                    {
                        if (inpt[k] >= 'a' && inpt[k] <= 'z' || inpt[k] >= 'A' && inpt[k] <= 'Z' ||
                            inpt[k] >= '0' && inpt[k] <= '9' || inpt[k] == '_') 
                        {
                            flag = true;
                        }
                    }
                    if (flag == false)
                    {
                        break;
                    }
                }
                
            }
            if (flag == false)
            {
                goto Exit;
            }
            int meetings = 0;
            for (var i = 0; i < N; i++)
            {
                if (inpt[i] == '(')
                {
                    meetings++;
                }
                else if (inpt[i] == ')')
                {
                    meetings--;
                }
            }
            if (meetings < 0)
            {
                flag = false;
            }
            if (flag == false)
            {
                goto Exit;
            }
            for (var i = 0; i < N; i++)
            {
                if (inpt[i] == '+' || inpt[i] == '-' || inpt[i] == '/' || inpt[i] == '*') 
                {
                    if (i == 0 && inpt[i] == '-' && (inpt[i + 1] >= '0' && inpt[i + 1] <= '9'
                        || inpt[i + 1] == '[' || inpt[i + 1] == '(')) 
                    {
                        flag = true;
                    }
                    if (i != 0 && i != N - 1 && (inpt[i - 1] >= '0' && inpt[i - 1] <= '9' || inpt[i - 1] == ']' || inpt[i - 1] == '(')
                      && (inpt[i + 1] >= '0' && inpt[i + 1] <= '9' || inpt[i + 1] == '[' || inpt[i + 1] == ')')) 
                    {
                        flag = true;
                    }
                    if (i == N - 1)
                    {
                        flag = false;
                    }
                    if (inpt[i + 1] == '+' || inpt[i + 1] == '-' || inpt[i + 1] == '/' || inpt[i + 1] == '*')
                    {
                        flag = false;
                    }
                    if (flag == false)
                    {
                        break;
                    }
                }
                if (inpt[i] == '=')
                {
                    eq++;
                }
                if (eq > 1)
                {
                    flag = false;
                    break;
                }
            }
        Exit:
            return flag;
        }
        }
}
