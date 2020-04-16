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
            string [] expected = { "[abc]", "+100" };
            int numOfQuestion = 1;
            int numOfAnswer = 1;
            Editor a = new Editor();
            a.SetAnswArgument(arg, numOfQuestion, numOfAnswer);
            string[] actual = a.GetAnswArgument(numOfQuestion, numOfAnswer);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetGetStatistPrefix()
        {
            string prfx = "[abc]";
            int numOfStatL = 3;
            Editor a = new Editor();
            a.SetStatPrefix(prfx, numOfStatL);
            string actual = a.GetStatistPrefix(numOfStatL);
            Assert.AreEqual(prfx, actual);
        }

        [TestMethod]
        public void SetGetStatistCalculate()
        {
            string calclt = "/100";
            int numOfStatL = 3;
            Editor a = new Editor();
            a.SetStatCalculate(calclt, numOfStatL);
            string actual = a.GetStatistCalculate(numOfStatL);
            Assert.AreEqual(calclt, actual);
        }

        [TestMethod]
        public void SetGetStatistPostfix()
        {
            string post = "%";
            int numOfStatL = 3;
            Editor a = new Editor();
            a.SetStatPostfix(post, numOfStatL);
            string actual = a.GetStatistPostfix(numOfStatL);
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
            string answ2 = "1878";
            bool answtp = true;
            string[] arg = { "[abc]", "+100" };
            string[] arg1 = { "[abc]", "+0" };
            string[] arg2 = { "[abc]", "+0" };
            string prfx = "[abc]";
            string calclt = "/100";
            string post = "%";
            string path = @"c:\temp\2.txt";
            // act
            Editor a = new Editor();
            a.SetName(name);
            a.SetDescrip(desc);
            a.SetQuestionText(text, 0);
            a.SetAnswType(answtp, 0);
            
            a.SetAnswText(answ1, 0, 1);
            a.SetAnswText(answ, 0, 0);
            a.SetAnswArgument(arg, 0, 0);
            a.SetAnswArgument(arg1, 0, 1);
            
            a.SetStatPrefix(prfx, 0);
            a.SetStatCalculate(calclt, 0);
            a.SetStatPostfix(post, 0);
            a.Save(path);
            Editor actual = new Editor();
            //Debug.Write("\n" + a.GetAnswText(0, 0) + " ++++");
            //Debug.Write("\n" + a.GetAnswText(0, 0) + " ++++");
            //Debug.Write("\n" + a.GetAnswText(0, 1) + "  ++++");
            actual.Open(path);
            // assert
            Assert.AreEqual(a, actual);
            //CollectionAssert.AreEqual(arg, actual);
        }
    }
}
