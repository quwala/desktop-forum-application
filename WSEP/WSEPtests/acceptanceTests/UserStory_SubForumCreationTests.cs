using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;
using WSEP.forumManagement.forumHandler;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_SubForumCreationTests
    {

        private IForumSystem fs;
        private Forum f;

        [TestInitialize()]
        public void Initialize()
        {
            fs = new ForumSystem("superAdmin");
            f = new Forum("Test Forum");
        }

        [TestMethod]
        public void Test_SubForumCreation_GoodInput()
        {
            Assert.IsTrue(f.addSubForum("Test Sub Forum"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
            "A Sub Forum with that name already exists")]
        public void Test_SubForumCreation_ExistingSubForumAdd()
        {
            f.addSubForum("test");
            f.addSubForum("test");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException),
            "Name of the Sub Forum contains illegal character")]
        public void Test_SubForumCreation_BadCharacter()
        {
            f.addSubForum("@#$%^&*");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException),
            "Name of the Sub Forum cannot be empty")]
        public void Test_SubForumCreation_EmptyName()
        {
            f.addSubForum("");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException),
            "Name of the Sub Forum cannot be null")]
        public void Test_SubForumCreation_CatastophicInput()
        {
            f.addSubForum(null);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException),
            "Name of the Sub Forum cannot begin with a space character")]
        public void Test_SubForumCreation_SpaceFirstCharacter()
        {
            f.addSubForum(" test");
        }
    }
}
