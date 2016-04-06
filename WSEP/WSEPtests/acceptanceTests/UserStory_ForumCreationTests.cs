using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_ForumCreationTests
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
             //Assert.isTrue(fs.addForum("forumName"));             //add new forum
             //Assert.istrue(fs.ContainForum("forumName"));         //check if new forum exist in the forum list
             // Assert.isFalse(fs.addForum("forumName"));           //try to add the same forum name
        }

        [TestMethod]
        public void Test_ForumCreation_BadInput()
        {
            // Assert.isFalse(fs.addForum("invalidForumName"));         //bad inputs
            // Assert.isFalse(fs.addForum(""));
            // Assert.isFalse(fs.addForum("  forumname");
        }

        [TestMethod]
        public void Test_ForumCreation_CatastophicInput()
        {
            // Assert.isFalse(fs.addForum(NULL));
            // Assert.isFalse(fs.addForum("NULL"));

        }
    }
}
