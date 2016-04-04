using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;
using WSEP.userManagement;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_ForumCreationTest
    {

        private IForumSystem fs;

        [TestInitialize()]
        public void Initialize()
        {
            fs = new ForumSystem("superAdmin");
        }

        [TestMethod]
        public void Test_ForumCreation_GoodInput()
        {
            Assert.IsTrue(fs.addForum("forumName"));
            Assert.IsFalse(fs.addForum("forumName"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test_ForumCreation_BadInput()
        {
            fs.addForum("@#$%^&*");
            fs.addForum("");
        }

        [TestMethod]
        public void Test_ForumCreation_CatastophicInput()
        {
            Assert.IsFalse(fs.addForum(null));
        }
    }
}
