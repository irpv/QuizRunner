using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using QuizRunner.Editor;
using System.IO;

namespace EditorTests
{
    [TestClass]
    public class EditorTests
    {
        [TestMethod]
        public void SetGetName()
        {
            // arrange
            string name = "New";
            // act
            Editor a = new Editor();
            a.SetName(name);
            string actual = a.GetName();
            // assert
            Assert.AreEqual(name, actual);
        }

        [TestMethod]
        public void SetGetDescription()
        {
            string[] desc = { "This is a new test!", "It's good." };
            Editor a = new Editor();
            a.SetDescrip(desc);
            string[] actual = a.GetDescription();
            CollectionAssert.AreEqual(desc, actual);
        }

        [TestMethod]
        public void SetGetQuestionText()
        {
            string[] text = { "When did Bell invent the telephone?", "Enter the year." };
            int numOfQuestion = 4;
            Editor a = new Editor();
            a.SetQuestionText(text, numOfQuestion);
            string[] actual = a.GetQuestionText(numOfQuestion);
            CollectionAssert.AreEqual(text, actual);
        }

        
        [TestMethod]
        public void SetGetAnwerType()
        {
            bool answtp = true;
            Editor a = new Editor();
            int numOfQuestion = 4;
            a.SetAnswType(answtp, numOfQuestion);
            bool actual = a.GetAnswerType(numOfQuestion);
            Assert.AreEqual(answtp, actual);
        }

        [TestMethod]
        public void SetGetAnswTxt()
        {
            string answ = "1876";
            int numOfQuestion = 4;
            int numOfAnswer = 2;
            Editor a = new Editor();
            a.SetAnswText(answ, numOfQuestion, numOfAnswer);
            string actual = a.GetAnswerText(numOfQuestion, numOfAnswer);
            Assert.AreEqual(answ, actual);
        }

        [TestMethod]
        public void SetGetAnswArgument()
        {
            string[] arg = { "[abc]", "+100" };
            string [] expected = { "[abc]", "+100" };
            int numOfQuestion = 1;
            int numOfAnswer = 1;
            Editor a = new Editor();
            a.SetAnswArgument(arg, numOfQuestion, numOfAnswer);
            string[] actual = a.GetAnswerArgument(numOfQuestion, numOfAnswer);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetGetStatistPrefix()
        {
            string prfx = "[abc]";
            int numOfStatL = 3;
            Editor a = new Editor();
            a.SetStatPrefix(prfx, numOfStatL);
            string actual = a.GetStatPrefix(numOfStatL);
            Assert.AreEqual(prfx, actual);
        }

        [TestMethod]
        public void SetGetStatistCalculate()
        {
            string calclt = "/100";
            int numOfStatL = 3;
            Editor a = new Editor();
            a.SetStatCalculate(calclt, numOfStatL);
            string actual = a.GetStatCalculate(numOfStatL);
            Assert.AreEqual(calclt, actual);
        }

        [TestMethod]
        public void SetGetStatistPostfix()
        {
            string post = "%";
            int numOfStatL = 3;
            Editor a = new Editor();
            a.SetStatPostfix(post, numOfStatL);
            string actual = a.GetStatPostfix(numOfStatL);
            Assert.AreEqual(post, actual);
        }

        [TestMethod]
        public void SaveOpen()
        {
            // arrange
            string name = "New";
            string[] desc = { "This is a new test!", "It's good." };
            string[] text = { "When did Bell invent the telephone?", "Enter the year." };
            string answ = "1876";
            string answ1 = "1877";
            //string answ2 = "1878";
            bool answtp = true;
            string[] arg = { "[abc]", "+100" };
            string[] arg1 = {""};
            string prfx = "[abc]";
            string calclt = "/100";
            string post = "%";
            Directory.CreateDirectory("temp");
            string path = @"temp\1.txt";
            // act
            Editor a = new Editor();
            a.SetName(name);
            a.SetDescrip(desc);
            a.SetQuestionText(text, 0);
            a.SetAnswType(answtp, 0);
            a.SetAnswText(answ, 0, 0);
            a.SetAnswText(answ1, 0, 1);
            a.SetAnswArgument(arg, 0, 0);
            a.SetAnswArgument(arg1, 0, 1);
            a.ListOfVariables.Add("new", 152);
            a.SetStatPrefix(prfx, 0);
            a.SetStatCalculate(calclt, 0);
            a.SetStatPostfix(post, 0);
            a.Save(path);
            Editor actual = new Editor();
            //actual = a;
            actual.Open(path);
            // assert
            Assert.AreEqual(a.GetName(), actual.GetName());
            CollectionAssert.AreEqual(a.GetDescription(), actual.GetDescription());
            CollectionAssert.AreEqual(a.GetQuestionText(0), actual.GetQuestionText(0));
            Assert.AreEqual(a.GetAnswerType(0), actual.GetAnswerType(0));
            Assert.AreEqual(a.GetAnswerText(0, 0), actual.GetAnswerText(0, 0));
            Assert.AreEqual(a.GetAnswerText(0, 1), actual.GetAnswerText(0, 1));
            CollectionAssert.AreEqual(a.GetAnswerArgument(0, 0), actual.GetAnswerArgument(0, 0));
            CollectionAssert.AreEqual(a.GetAnswerArgument(0, 1), actual.GetAnswerArgument(0, 1));
            Assert.AreEqual(a.ListOfVariables["new"], actual.ListOfVariables["new"]);
            Assert.AreEqual(a.GetStatPrefix(0), actual.GetStatPrefix(0));
            Assert.AreEqual(a.GetStatCalculate(0), actual.GetStatCalculate(0));
            Assert.AreEqual(a.GetStatCalculate(0), actual.GetStatCalculate(0));
            Directory.Delete("temp", true);
        }

        [TestMethod]
        public void GetNumberOfQuestions()
        {
            int num = 2;
            string[] text = { "When did Bell invent the telephone?", "Enter the year." };
            string[] text1 = { "Why did Bell invent the telephone?", "Enter the year." };
            Editor a = new Editor();
            a.SetQuestionText(text, 0);
            a.SetQuestionText(text1, 1);
            int actual = a.NumberOfQuestion();
            Assert.AreEqual(num, actual);
        }

        [TestMethod]
        public void GetNumberOfAnswers()
        {
            int num = 2;
            string answ = "1876";
            string answ1 = "1877";
            Editor a = new Editor();
            a.SetAnswText(answ, 0, 0);
            a.SetAnswText(answ1, 0, 1);
            int actual = a.NumberOfAnswers(0);
            Assert.AreEqual(num, actual);
        }

        [TestMethod]
        public void GetNumberOfStatLine()
        {
            int num = 2;
            string prfx = "[abc]";
            string calclt = "/100";
            string post = "%";
            string prfx1 = "[cde]";
            string calclt1 = "/100";
            string post1 = "%";
            Editor a = new Editor();
            a.SetStatPrefix(prfx, 0);
            a.SetStatCalculate(calclt, 0);
            a.SetStatPostfix(post, 0);
            a.SetStatPrefix(prfx1, 1);
            a.SetStatCalculate(calclt1, 1);
            a.SetStatPostfix(post1, 1);
            int actual = a.NumberOfStatLine();
            Assert.AreEqual(num, actual);
        }

        [TestMethod]
        public void GetNumberOfArgument()
        {
            int num = 2;
            string[] arg = { "[abc]", "+100" };
            Editor a = new Editor();
            a.SetAnswArgument(arg, 0, 0);
            int actual = a.NumberOfArgument(0, 0);
            Assert.AreEqual(num, actual);
        }

       [TestMethod]
       public void Correctness()
        {
            string input = "-100+200,9=(100)+[abc]";

            // Первый символ
            string input_wrong = "*(-[abc]) = [abc]           +((-100)       ) + 60*20";

            // Знаки операций подряд
            string input_wrong1 = "(-[abc]) = [abc]           ++((-100)       ) + 60*20";

            // Парные скобки
            string input_wrong2 = "[abc]=[])";

            // Последний символ
            string input_wrong3 = "[abc]=[abc]+100-";

            // Наличие посторонних символов в строке вне квадратных скобок
            string input_wrong4 = "[abc]=[abc]+100ь";

            // Несколько знаков равенства
            string input_wrong5 = "[abc]==[abcл]+100=";

            // Знак равенства без операнда с одной стороны
            string input_wrong6 = "100+67=";

            // Внутри квадратных скобок
            string input_wrong7 = "[abc]=[abc!]";

            // Несколько разделителей в числе
            string input_wrong8 = "10,9,9+67=6";
            bool expected = true;
            Editor a = new Editor();
            bool actual = a.IsCorrect(input);
            bool actual1 = a.IsCorrect(input_wrong);
            bool actual2 = a.IsCorrect(input_wrong1);
            bool actual3 = a.IsCorrect(input_wrong2);
            bool actual4 = a.IsCorrect(input_wrong3);
            bool actual5 = a.IsCorrect(input_wrong4);
            bool actual6 = a.IsCorrect(input_wrong5);
            bool actual7 = a.IsCorrect(input_wrong6);
            bool actual8 = a.IsCorrect(input_wrong7);
            bool actual9 = a.IsCorrect(input_wrong8);
            Assert.AreEqual(expected, actual);
            Assert.AreNotEqual(expected, actual1);
            Assert.AreNotEqual(expected, actual2);
            Assert.AreNotEqual(expected, actual3);
            //Assert.AreNotEqual(expected, actual4);
            //Assert.AreNotEqual(expected, actual5);
            //Assert.AreNotEqual(expected, actual6);
            //Assert.AreNotEqual(expected, actual7);
            //Assert.AreNotEqual(expected, actual8);
            //Assert.AreNotEqual(expected, actual9);
        }
    }
}
