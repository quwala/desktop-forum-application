using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;
using WSEP.forumManagement.forumHandler;
using WSEP.forumManagement.threadsHandler;

namespace WSEPtests.forumManagementTests
{
    [TestClass]
    public class ForumSystemTests
    {

        private ForumSystem fs;
        private Forum f;
        private SubForum sf;


        [TestInitialize()]
        public void Initialize()
        {
            fs = new ForumSystem("superAdmin", new WSEP.userManagement.UserManager());
            

        }

        [TestMethod]
        public void Test_getForum()
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
