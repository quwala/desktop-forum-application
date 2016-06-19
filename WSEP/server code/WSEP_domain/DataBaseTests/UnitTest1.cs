using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ForumsDataBase;

namespace DataBaseTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DB db = new DB();
            Assert.IsTrue(db.addForum("forumNames12"));
        }
    }
}
