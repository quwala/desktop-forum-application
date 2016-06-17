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
    public class SetForumModeratorsSeniorityTests
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
        public void SetForumModeratorsSeniorityGoodInput()
        {
            Assert.AreEqual("true", forumSystem.setForumModeratorsSeniority("TestingForum", 7, "superAdmin"));
        }

        [TestMethod]
        public void SetForumModeratorsSeniorityNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumModeratorsSeniority(null, 7, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumModeratorsSeniority("TestingForum", 7, null));
            Assert.AreNotEqual("true", forumSystem.setForumModeratorsSeniority(null, 7, null));
        }

        [TestMethod]
        public void SetForumModeratorsSeniorityEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumModeratorsSeniority("", 7, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumModeratorsSeniority("TestingForum", 7, ""));
            Assert.AreNotEqual("true", forumSystem.setForumModeratorsSeniority("", 7, ""));
        }

        [TestMethod]
        public void SetForumModeratorsSeniorityUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.setForumModeratorsSeniority("UnexistedForum", 7, "superAdmin"));
        }

        [TestMethod]
        public void SetForumModeratorsSeniorityUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumModeratorsSeniority("TestingForum", 7, "UnexistedUser"));
        }

        [TestMethod]
        public void SetForumModeratorsSeniorityUnpermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumModeratorsSeniority("TestingForum", 7, "username1"));
        }

        [TestMethod]
        public void SetForumModeratorsSeniorityInvalidNumber()
        {
            Assert.AreNotEqual("true", forumSystem.setForumModeratorsSeniority("TestingForum", -1, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumModeratorsSeniority("TestingForum", -23523, "superAdmin"));
        }
    }
}
