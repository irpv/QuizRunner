using System.IO;

namespace QuizRunner.SaveArgs
{
    public class SaveArgs
    {
        private struct Answer
        {
            public string AnswerText;

            // Хранят строки для расчета статистики.
            public string[] Argument;
            public bool Checked;
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

        public void Save(string path)
        {
            File.Create(path).Close();
            var SW = File.AppendText(path);

            // Записывает имя теста
            SW.WriteLine(Name);

            // Записывает размер описания
            SW.WriteLine(Descrip.Length.ToString());
            SW.Close();

            // Записывает текст описания
            File.AppendAllLines(path, Descrip);
            SW = File.AppendText(path);

            // Записывает количество вопросов
            SW.WriteLine(ListOfQuestions.Length.ToString());

            for (var i = 0; i < ListOfQuestions.Length; i++)
            {
                // Записывает размер вопроса
                SW.WriteLine(ListOfQuestions[i].QuestionText.Length.ToString());
                SW.Close();

                // Записывает текст вопроса
                File.AppendAllLines(path, ListOfQuestions[i].QuestionText);
                SW = File.AppendText(path);

                // Записывает тип вопроса
                SW.WriteLine(ListOfQuestions[i].AnswType.ToString());

                // Записывает количество ответов
                SW.WriteLine(ListOfQuestions[i].AnswArr.Length.ToString());
                SW.Close();
                SW = File.AppendText(path);

                for (var j = 0; j < ListOfQuestions[i].AnswArr.Length; j++)
                {
                    // Записывает текст ответа
                    SW.WriteLine(ListOfQuestions[i].AnswArr[j].AnswerText);
                    SW.Close();
                    SW = File.AppendText(path);

                    // Записывает параметр Checked
                    SW.WriteLine(ListOfQuestions[i].AnswArr[j].Checked);
                    SW.Close();
                    SW = File.AppendText(path);
                }
            }
        }
    }
}