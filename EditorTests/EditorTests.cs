using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using QuizRunner.Editor;
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
            string[] desc = { "This is a new test!" , "It's good."};
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
            a.SetQuestText(text, numOfQuestion);
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
            bool actual = a.GetAnswType(numOfQuestion);
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
            string actual = a.GetAnswText(numOfQuestion, numOfAnswer);
            Assert.AreEqual(answ, actual);
        }

        [TestMethod]
        public void SetGetAnswArgument()
        {
            string[] arg = { "[abc]", "+100" };
            int numOfQuestion = 4;
            int numOfAnswer = 2;
            Editor a = new Editor();
            a.SetAnswArgument(arg, numOfQuestion, numOfAnswer);
            string[] actual = a.GetAnswArgument(numOfQuestion, numOfAnswer);
            CollectionAssert.AreEqual(arg, actual);
        }
    }
}
