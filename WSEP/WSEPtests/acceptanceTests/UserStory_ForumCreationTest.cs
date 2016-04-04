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
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
            "A Forum with that name already exists")]
        public void Test_ForumCreation_ExistingForumAdd()
        {
            fs.addForum("test");
            fs.addForum("test");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test_ForumCreation_BadInput()
        {
            fs.addForum("@#$%^&*");
            fs.addForum("");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test_ForumCreation_CatastophicInput()
        {
            fs.addForum(null);
        }
    }
}
