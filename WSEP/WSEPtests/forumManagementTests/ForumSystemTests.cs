using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;
using WSEP.forumManagement.forumHandler;
using WSEP.forumManagement.threadsHandler;
using WSEP.userManagement;
using System.Collections.Generic;

namespace WSEPtests.forumManagementTests
{
    [TestClass]
    public class ForumSystemTests
    {
        private UserManager um;
        private ForumSystem fs;
        private Forum f;
        private SubForum sf;


        [TestInitialize()]
        public void Initialize()
        {
            um = new UserManager();

            fs = new ForumSystem("superAdmin", um);
            fs.addForum("Test Forum");
            um.addForum("Test Forum");
            f = fs.getForum("Test Forum");

            um.registerMemberToForum("Test Forum", "Avi", "Avi", "Avi@hotmail.com");
            um.registerMemberToForum("Test Forum", "Shlomo", "Shlomo", "Shlomo@hotmail.com");

            List<string> mods = new List<string>();
            mods.Add("Avi");
            mods.Add("Shlomo");

            fs.addSubForum("Test Forum", "Test Sub Forum",mods);
            sf = fs.getForum("Test Forum").getSubForum("Test Sub Forum");
            
        }

        [TestMethod]
        public void Test_getForum()
        {
            Assert.AreEqual("Test Forum",fs.getForum("Test Forum").getName());
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
