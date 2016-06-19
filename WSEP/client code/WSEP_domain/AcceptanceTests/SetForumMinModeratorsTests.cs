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
    public class SetForumMinModeratorsTests
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
        public void SetForumMinModeratorsGoodInput()
        {
            Assert.AreEqual("true", forumSystem.setForumMinModerators("TestingForum", 5, "superAdmin"));
        }

        [TestMethod]
        public void SetForumMinModeratorsNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMinModerators(null, 5, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumMinModerators("TestingForum", 5, null));
            Assert.AreNotEqual("true", forumSystem.setForumMinModerators(null, 5, null));
        }

        [TestMethod]
        public void SetForumMinModeratorsEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMinModerators("", 5, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumMinModerators("TestingForum", 5, ""));
            Assert.AreNotEqual("true", forumSystem.setForumMinModerators("", 5, ""));
        }

        [TestMethod]
        public void SetForumMinModeratorsUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMinModerators("UnexistedForum", 5, "superAdmin"));
        }

        [TestMethod]
        public void SetForumMinModeratorsUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMinModerators("TestingForum", 5, "UnexistedUser"));
        }

        [TestMethod]
        public void SetForumMinModeratorsUnpermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMinModerators("TestingForum", 5, "username1"));
        }
    }
}
