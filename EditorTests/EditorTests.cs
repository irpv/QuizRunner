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
            Editor a = new Editor();
            a.SetQuestText(text, 4);
            string[] actual = a.GetQuestionText(4);
            CollectionAssert.AreEqual(text, actual);
        }

        
        [TestMethod]
        public void SetGetAnwerType()
        {
            bool answtp = true;
            Editor a = new Editor();
            a.SetAnswType(answtp, 3);
            bool actual = a.GetAnswType(3);
            Assert.AreEqual(answtp, actual);
        }

        [TestMethod]
        public void SetGetAnswTxt()
        {
            string[] text = { "When did Bell invent the telephone?", "Enter the year." };
            string answ = "1876";
            Editor a = new Editor();
            a.SetQuestText(text, 4);
            a.SetAnswText(answ, 4, 2);
            string actual = a.GetAnswText(4, 2);
            Assert.AreEqual(answ, actual);
        }
    }
}
