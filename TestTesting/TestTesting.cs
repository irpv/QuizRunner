using NUnit.Framework;
using QuizRunner.Testing;
using System.Collections.Generic;

namespace TestTesting
{
    public class Tests
    {
        [Test]
        public void ToSimplifyArg()
        {
            string arg = "[test] = [test] + 1";
            string actual = "[test]=[test]+1";

            Testing t = new Testing();

            Assert.AreEqual(t.SimplifyArg(arg), actual);
            Assert.AreEqual(t.SimplifyArg("[] = [] + 1"), "[]=[]+1");
        }

        [Test]
        public void getArg()
        {
            string arg = "[test] = [test] + 1";
            string actual = "test ";

            Testing t = new Testing();

            Assert.AreEqual(t.GetArgumentName(arg), actual); 
            Assert.AreEqual(t.GetArgumentName("[] = [] + 1"), " ");
        }

        [Test]
        public void GetCmpte()
        {
            string arg = "[test] = [test] + 1";
            var dict = new Dictionary<string, double>();
            dict.Add("test", 1);

            object actual = 2;

            Testing t = new Testing();

            Assert.AreEqual(t.GetCompute(arg, dict), actual);
        }
    }
}
