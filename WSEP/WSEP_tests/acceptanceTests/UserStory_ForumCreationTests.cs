using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.forumManagementService;
using WSEP_doamin.forumManagementDomain;
using WSEP_tests.adapter;

namespace WSEP_tests.acceptanceTests
{
    [TestClass]
    public class UserStory_ForumCreationTests
    {

        // private IForumSystem fs; // roie added
        private IAdapter adapter;


        [TestInitialize()]
        public void Initialize()
        {
            // fs = new ForumSystem("superAdmin",new WSEP.userManagement.UserManager());
            adapter = new Adapter();
        }

        [TestMethod]  // test 1.1
        public void Test_ForumCreation_GoodInput()
        {
            Assert.IsTrue(adapter.addForum("forumName")); // TID 1
        }

        [TestMethod] // test 1.2
        [ExpectedException(typeof(Exception))]
        public void Test_ForumCreation_ExistingForumAdd()
        {
            adapter.addForum("test");
            adapter.addForum("test"); // TID 2
        }

        [TestMethod] // test 1.3
        [ExpectedException(typeof(InvalidNameException))]
        public void Test_ForumCreation_BadCharacter()
        {
            adapter.addForum("@#$%^&*");
        }

        [TestMethod] // test 1.4
        [ExpectedException(typeof(InvalidNameException))]
        public void Test_ForumCreation_EmptyName()
        {
            adapter.addForum("");  // TID 3
        }

        [TestMethod] // test 1.5
        [ExpectedException(typeof(InvalidNameException))]
        public void Test_ForumCreation_CatastophicInput()
        {
            adapter.addForum(null); // TID 4
        }


        [TestMethod] // test 1.6
        [ExpectedException(typeof(InvalidNameException))]
        public void Test_ForumCreation_SpaceFirstCharacter()
        {
            adapter.addForum(" test"); // TID 5
        }
    }
}
