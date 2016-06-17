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
    public class SetModeratorTrialTimeTests
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
        public void SetModeratorTrialTimeGoodInput()
        {
            Assert.AreEqual("true", forumSystem.setModeratorTrialTime("TestingForum", "TestingSubForum", "username1", 10, "superAdmin"));
        }

        [TestMethod]
        public void SetModeratorTrialTimeNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime(null, "TestingSubForum", "username1", 10, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("TestingForum", null, "username1", 10, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("TestingForum", "TestingSubForum", null, 10, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("TestingForum", "TestingSubForum", "username1", 10, null));
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime(null, null, null, 10, null));
        }

        [TestMethod]
        public void SetModeratorTrialTimeEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("", "TestingSubForum", "username1", 10, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("TestingForum", "", "username1", 10, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("TestingForum", "TestingSubForum", "", 10, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("TestingForum", "TestingSubForum", "username1", 10, ""));
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("", "", "", 10, ""));
        }

        [TestMethod]
        public void SetModeratorTrialTimeUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("UnexistedForum", "TestingSubForum", "username1", 10, "superAdmin"));
        }

        [TestMethod]
        public void SetModeratorTrialTimeUnexistedSubForum()
        {
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("TestingForum", "UnexistedSubForum", "username1", 10, "superAdmin"));
        }

        [TestMethod]
        public void SetModeratorTrialTimeUnexistedModerator()
        {
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("TestingForum", "TestingSubForum", "UnexistedModerator", 10, "superAdmin"));
        }

        [TestMethod]
        public void SetModeratorTrialTimeUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("TestingForum", "TestingSubForum", "username1", 10, "UnexistedUser"));
        }

        [TestMethod]
        public void SetModeratorTrialTimeUnpermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("TestingForum", "TestingSubForum", "username1", 10, "username1"));
        }

        [TestMethod]
        public void SetModeratorTrialTimeInvalidNumber()
        {
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("TestingForum", "TestingSubForum", "username1", -1, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setModeratorTrialTime("TestingForum", "TestingSubForum", "username1", -543643, "superAdmin"));
        }
    }
}
