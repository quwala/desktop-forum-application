using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.forumManagementService;
using WSEP_doamin.forumManagementDomain;

namespace WSEP_tests.acceptanceTests
{
    [TestClass]
    public class UserStory_ForumCreationTests
    {

        private IForumSystem fs;

        [TestInitialize()]
        public void Initialize()
        {
            fs = new ForumSystem("superAdmin",new WSEP_service.userManagementService.UserManager());
        }

        [TestMethod]
        public void Test_ForumCreation_GoodInput()
        {
            Assert.IsTrue(fs.addForum("forumName"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_ForumCreation_ExistingForumAdd()
        {
            fs.addForum("test");
            fs.addForum("test");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test_ForumCreation_BadCharacter()
        {
            fs.addForum("@#$%^&*");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test_ForumCreation_EmptyName()
        {
            fs.addForum("");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test_ForumCreation_CatastophicInput()
        {
            fs.addForum(null);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test_ForumCreation_SpaceFirstCharacter()
        {
            fs.addForum(" test");
        }
    }
}
