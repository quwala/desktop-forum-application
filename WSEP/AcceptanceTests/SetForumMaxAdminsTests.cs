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
    public class SetForumMaxAdminsTests
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
        public void SetForumMaxAdminsGoodInput()
        {
            Assert.AreEqual("true", forumSystem.setForumMaxAdmins("TestingForum", 3, "superAdmin"));
        }

        [TestMethod]
        public void SetForumMaxAdminsNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMaxAdmins(null, 3, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumMaxAdmins("TestingForum", 3, null));
            Assert.AreNotEqual("true", forumSystem.setForumMaxAdmins(null, 3, null));
        }

        [TestMethod]
        public void SetForumMaxAdminsEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMaxAdmins("", 3, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumMaxAdmins("TestingForum", 3, ""));
            Assert.AreNotEqual("true", forumSystem.setForumMaxAdmins("", 3, ""));
        }

        [TestMethod]
        public void SetForumMaxAdminsInvalidNumber()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMaxAdmins("TestingForum", -1, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumMaxAdmins("TestingForum", -135235, "superAdmin"));
        }

        [TestMethod]
        public void SetForumMaxAdminsUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMaxAdmins("UnexistedForum", 3, "superAdmin"));
        }

        [TestMethod]
        public void SetForumMaxAdminsUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMaxAdmins("TestingForum", 3, "UnexistedUser"));
        }

        [TestMethod]
        public void SetForumMaxAdminsUnpermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMaxAdmins("TestingForum", 3, "username1"));
        }
    }
}
