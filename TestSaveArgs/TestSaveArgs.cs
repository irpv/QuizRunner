using NUnit.Framework;
using QuizRunner.SaveArgs;
using System.IO;

namespace TestSaveArgs
{
    public class Tests
    {

        [Test]
        public void isSave()
        {
            Directory.CreateDirectory("temp");
            string path = @"temp\1.txt";

            SaveArgs s = new SaveArgs();
            s.Save(path);

            Assert.IsTrue(File.Exists(path));
        }
    }
}