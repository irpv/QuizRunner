using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizRunner.Editor;
namespace EditorTests
{
    [TestClass]
    public class EditorTests
    {
        [TestMethod]
        public void SetGetName_New_Newreturned()
        {
            // arrange
            string name = "New";
            string expected = "New";
            // act
            Editor a = new Editor();
            a.SetName(name);
            string actual = a.GetName();
            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}
