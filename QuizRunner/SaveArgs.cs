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

        }
    }
}