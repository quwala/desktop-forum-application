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
        [ExpectedException(typeof(InvalidNameException),
            "Name of the forum contains illegal character")]
        public void Test_ForumCreation_BadCharacter()
        {
            fs.addForum("@#$%^&*");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException),
            "Name of the forum cannot be empty")]
        public void Test_ForumCreation_EmptyName()
        {
            fs.addForum("");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException),
            "Name of the forum cannot be null")]
        public void Test_ForumCreation_CatastophicInput()
        {
            fs.addForum(null);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException), 
            "Name of the forum cannot begin with a space character")]
        public void Test_ForumCreation_SpaceFirstCharacter()
        {
            fs.addForum(" test");
        }
    }
}
