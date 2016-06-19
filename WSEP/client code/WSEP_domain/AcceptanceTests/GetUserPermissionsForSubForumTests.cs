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
    public class GetUserPermissionsForSubForumTests
    {
        private IForumSystem forumSystem;
        private List<Tuple<string, string, int>> mods;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
            forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "email1@gmail.com");
            mods = new List<Tuple<string, string, int>>();
            mods.Add(new Tuple<string, string, int>("username1", "superAdmin", 7));
            forumSystem.addSubForum("TestingForum", "TestingSubForum", mods, "superAdmin");
        }

        [TestMethod]
        public void GetUserPermissionsForSubForumGoodInput()
        {
            Assert.AreEqual(permission.SUPER_ADMIN, forumSystem.getUserPermissionsForSubForum("TestingForum", "TestingSubForum", "superAdmin"));
            Assert.AreEqual(permission.MODERATOR, forumSystem.getUserPermissionsForSubForum("TestingForum", "TestingSubForum", "username1"));
        }

        [TestMethod]
        public void GetUserPermissionsForSubForumNullArguments()
        {
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForSubForum(null, "TestingSubForum", "superAdmin"));
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForSubForum("TestingForum", null, "superAdmin"));
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForSubForum("TestingForum", "TestingSubForum", null));
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForSubForum(null, null, null));
        }

        [TestMethod]
        public void GetUserPermissionsForSubForumEmptyArguments()
        {
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForSubForum("", "TestingSubForum", "superAdmin"));
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForSubForum("TestingForum", "", "superAdmin"));
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForSubForum("TestingForum", "TestingSubForum", ""));
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForSubForum("", "", ""));
        }

        [TestMethod]
        public void GetUserPermissionsForSubForumUnexistedForum()
        {
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForSubForum("UnexistedForum", "TestingSubForum", "superAdmin"));
        }

        [TestMethod]
        public void GetUserPermissionsForSubForumUnexistedUser()
        {
            Assert.AreEqual(permission.GUEST, forumSystem.getUserPermissionsForSubForum("TestingForum", "TestingSubForum", "UnexistedUser"));
        }

        [TestMethod]
        public void GetUserPermissionsForSubForumUnexistedSubForum()
        {
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForSubForum("TestingForum", "UnexistedSubForum", "superAdmin"));
        }
    }
}
