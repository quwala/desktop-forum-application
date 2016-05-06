using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain;
using WSEP_service.forumManagement;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class AssignAdminTests
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
        public void AssignAdminGoodInput()
        {
            Assert.AreEqual("true", forumSystem.assignAdmin("TestingForum", "username1", "superAdmin"));
        }

        [TestMethod]
        public void AssignAdminNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.assignAdmin(null, "username1", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.assignAdmin("TestingForum", null, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.assignAdmin("TestingForum", "username1", null));
            Assert.AreNotEqual("true", forumSystem.assignAdmin(null, null, null));
        }

        [TestMethod]
        public void AssignAdminEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.assignAdmin("", "username1", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.assignAdmin("TestingForum", "", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.assignAdmin("TestingForum", "username1", ""));
            Assert.AreNotEqual("true", forumSystem.assignAdmin("", "", ""));
        }

        [TestMethod]
        public void AssignAdminUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.assignAdmin("TestingForum", "UnexistedUser", "superAdmin"));
        }

        [TestMethod]
        public void AssignAdminUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.assignAdmin("UnexistedForum", "username1", "superAdmin"));
        }

        [TestMethod]
        public void AssignAdminUnpermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.assignAdmin("TestingForum", "username1", "username1"));
        }
    }
}
