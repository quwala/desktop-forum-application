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
    public class SetForumMinAdminsTests
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
        public void SetForumMinAdminsGoodInput()
        {
            forumSystem.assignAdmin("TestingForum", "username1", "superAdmin");
            Assert.AreEqual("true", forumSystem.setForumMinAdmins("TestingForum", 2, "superAdmin"));
        }

        [TestMethod]
        public void SetForumMinAdminsNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMinAdmins(null, 3, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumMinAdmins("TestingForum", 3, null));
            Assert.AreNotEqual("true", forumSystem.setForumMinAdmins(null, 3, null));
        }

        [TestMethod]
        public void SetForumMinAdminsEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMinAdmins("", 3, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumMinAdmins("TestingForum", 3, ""));
            Assert.AreNotEqual("true", forumSystem.setForumMinAdmins("", 3, ""));
        }

        [TestMethod]
        public void SetForumMinAdminsInvalidNumber()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMinAdmins("TestingForum", 3, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumMinAdmins("TestingForum", -1, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumMinAdmins("TestingForum", -135235, "superAdmin"));
        }

        [TestMethod]
        public void SetForumMinAdminsUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMinAdmins("UnexistedForum", 3, "superAdmin"));
        }

        [TestMethod]
        public void SetForumMinAdminsUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMinAdmins("TestingForum", 3, "UnexistedUser"));
        }

        [TestMethod]
        public void SetForumMinAdminsUnpermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMinAdmins("TestingForum", 3, "username1"));
        }
    }
}
