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
    public class AssignModeratorTests
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
        public void AssignModeratorGoodInput()
        {
            Assert.AreEqual("true", forumSystem.assignModerator("TestingForum", "TestingSubForum", "username1", "superAdmin", 7));
        }

        [TestMethod]
        public void AssignModeratorNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.assignModerator(null, "TestingSubForum", "username1", "superAdmin", 7));
            Assert.AreNotEqual("true", forumSystem.assignModerator("TestingForum", null, "username1", "superAdmin", 7));
            Assert.AreNotEqual("true", forumSystem.assignModerator("TestingForum", "TestingSubForum", null, "superAdmin", 7));
            Assert.AreNotEqual("true", forumSystem.assignModerator("TestingForum", "TestingSubForum", "username1", null, 7));
            Assert.AreNotEqual("true", forumSystem.assignModerator(null, null, null, null, 7));
        }

        [TestMethod]
        public void AssignModeratorEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.assignModerator("", "TestingSubForum", "username1", "superAdmin", 7));
            Assert.AreNotEqual("true", forumSystem.assignModerator("TestingForum", "", "username1", "superAdmin", 7));
            Assert.AreNotEqual("true", forumSystem.assignModerator("TestingForum", "TestingSubForum", "", "superAdmin", 7));
            Assert.AreNotEqual("true", forumSystem.assignModerator("TestingForum", "TestingSubForum", "username1", "", 7));
            Assert.AreNotEqual("true", forumSystem.assignModerator("", "", "", "", 7));
        }

        [TestMethod]
        public void AssignModeratorUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.assignModerator("UnexistedForum", "TestingSubForum", "username1", "superAdmin", 7));
        }

        [TestMethod]
        public void AssignModeratorUnexistedSubForum()
        {
            Assert.AreNotEqual("true", forumSystem.assignModerator("TestingForum", "UnexistedSubForum", "username1", "superAdmin", 7));
        }

        [TestMethod]
        public void AssignModeratorUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.assignModerator("TestingForum", "TestingSubForum", "UnexistedUser", "superAdmin", 7));
        }

        [TestMethod]
        public void AssignModeratorUnpermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.assignModerator("TestingForum", "TestingSubForum", "username1", "username1", 7));
        }
    }
}
