using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_ForumCreationTest
    {

        private IForumSystem fs;

        [TestInitialize()]
        public void Initialize()
        {
            fs = new ForumSystem();
        }

        [TestMethod]
        public void Test_ForumCreation_GoodInput()
        {
            // Assert.isTrue(fs.addForum("forumName"));
        }
    }
}
