using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumSystem;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class AddSubForumTests
    {
        private IForumSystem forumSystem;
        private List<Tuple<string, string, int>> mods;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
            forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "email1@gmail.com", "q", "a");
            mods = new List<Tuple<string, string, int>>();
            mods.Add(new Tuple<string, string, int>("username1", "superAdmin", 7));
        }

        [TestMethod]
        public void AddSubForumGoodInput()
        {
            Assert.AreEqual("true", forumSystem.addSubForum("TestingForum", "TestingSubForum", mods, "superAdmin"));
        }

        [TestMethod]
        public void AddSubForumNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.addSubForum(null, "TestingSubForum", mods, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.addSubForum("TestingForum", null, mods, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.addSubForum("TestingForum", "TestingSubForum", null, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.addSubForum("TestingForum", "TestingSubForum", mods, null));
            Assert.AreNotEqual("true", forumSystem.addSubForum(null, null, null, null));
        }

        [TestMethod]
        public void AddSubForumEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.addSubForum("", "TestingSubForum", mods, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.addSubForum("TestingForum", "", mods, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.addSubForum("TestingForum", "TestingSubForum", mods, ""));
            Assert.AreNotEqual("true", forumSystem.addSubForum("", "", mods, ""));
        }

        [TestMethod]
        public void AddSubForumUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.addSubForum("UnexistedForum", "TestingSubForum", mods, "superAdmin"));
        }

        [TestMethod]
        public void AddSubForumNonPermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.addSubForum("TestingForum", "TestingSubForum", mods, "username1"));
        }
    }
}
