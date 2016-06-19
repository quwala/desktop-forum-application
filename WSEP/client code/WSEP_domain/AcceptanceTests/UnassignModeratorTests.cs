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
    public class UnassignModeratorTests
    {
        private IForumSystem forumSystem;
        private List<Tuple<string, string, int>> mods;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
            forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "email1@gmail.com");
            forumSystem.registerMemberToForum("TestingForum", "username2", "pass2", "email2@gmail.com");
            forumSystem.registerMemberToForum("TestingForum", "username3", "pass3", "email3@gmail.com");
            forumSystem.assignAdmin("TestingForum", "username1", "superAdmin");
            mods = new List<Tuple<string, string, int>>();
            mods.Add(new Tuple<string, string, int>("username1", "superAdmin", 7));
            mods.Add(new Tuple<string, string, int>("username2", "superAdmin", 7));
            forumSystem.addSubForum("TestingForum", "TestingSubForum", mods, "superAdmin");
        }

        [TestMethod]
        public void UnassignModeratorGoodInput()
        {
            Assert.AreEqual("true", forumSystem.unassignModerator("TestingForum", "TestingSubForum", "username1", "superAdmin"));
        }

        [TestMethod]
        public void UnassignModeratorNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.unassignModerator(null, "TestingSubForum", "username1", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.unassignModerator("TestingForum", null, "username1", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.unassignModerator("TestingForum", "TestingSubForum", null, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.unassignModerator("TestingForum", "TestingSubForum", "username1", null));
            Assert.AreNotEqual("true", forumSystem.unassignModerator(null, null, null, null));
        }

        [TestMethod]
        public void UnassignModeratorEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.unassignModerator("", "TestingSubForum", "username1", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.unassignModerator("TestingForum", "", "username1", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.unassignModerator("TestingForum", "TestingSubForum", "", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.unassignModerator("TestingForum", "TestingSubForum", "username1", ""));
            Assert.AreNotEqual("true", forumSystem.unassignModerator("", "", "", ""));
        }

        [TestMethod]
        public void UnassignModeratorUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.unassignModerator("UnexistedForum", "TestingSubForum", "username1", "superAdmin"));
        }

        [TestMethod]
        public void UnassignModeratorUnexistedSubForum()
        {
            Assert.AreNotEqual("true", forumSystem.unassignModerator("TestingForum", "UnexistedSubForum", "username1", "superAdmin"));
        }

        [TestMethod]
        public void UnassignModeratorUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.unassignModerator("TestingForum", "TestingSubForum", "UnexistedModerator", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.unassignModerator("TestingForum", "TestingSubForum", "username1", "UnexistedUser"));
        }

        [TestMethod]
        public void UnassignModeratorUnpermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.unassignModerator("TestingForum", "TestingSubForum", "username1", "username3"));
            Assert.AreNotEqual("true", forumSystem.unassignModerator("TestingForum", "TestingSubForum", "username3", "username1"));
        }
    }
}
