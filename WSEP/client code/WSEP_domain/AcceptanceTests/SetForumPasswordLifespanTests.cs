using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain;
using WSEP_domain.userManagement;
using WSEP_service.forumManagement;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class SetForumPasswordLifespanTests
    {
        private IForumSystem forumSystem;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
            forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "email1@gmail.com");
        }

        [TestMethod]
        public void SetForumPasswordLifespanGoodInput()
        {
            Assert.AreEqual("true", forumSystem.setForumPasswordLifespan("TestingForum", 100, "superAdmin"));
        }

        [TestMethod]
        public void SetForumPasswordLifespanNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumPasswordLifespan(null, 100, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumPasswordLifespan("TestingForum", 100, null));
            Assert.AreNotEqual("true", forumSystem.setForumPasswordLifespan(null, 100, null));
        }

        [TestMethod]
        public void SetForumPasswordLifespanEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumPasswordLifespan("", 100, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumPasswordLifespan("TestingForum", 100, ""));
            Assert.AreNotEqual("true", forumSystem.setForumPasswordLifespan("", 100, ""));
        }

        [TestMethod]
        public void SetForumPasswordLifespanUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.setForumPasswordLifespan("UnexistedForum", 100, "superAdmin"));
        }

        [TestMethod]
        public void SetForumPasswordLifespanUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumPasswordLifespan("TestingForum", 100, "UnexistedUser"));
        }

        [TestMethod]
        public void SetForumPasswordLifespanUnpermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumPasswordLifespan("TestingForum", 100, "username1"));
        }

        [TestMethod]
        public void SetForumPasswordLifespanInvalidNumber()
        {
            Assert.AreNotEqual("true", forumSystem.setForumPasswordLifespan("TestingForum", -1, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumPasswordLifespan("TestingForum", -4634147, "superAdmin"));
        }
    }
}
