using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumSystem;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class WritePostTests
    {
        private IForumSystem forumSystem;
        private List<Tuple<string, string, int>> mods;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
            forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "email1@gmail.com", "q", "a");
            forumSystem.registerMemberToForum("TestingForum", "username2", "pass2", "email2@gmail.com", "q", "a");
            forumSystem.registerMemberToForum("TestingForum", "username3", "pass3", "email3@gmail.com", "q", "a");
            forumSystem.assignAdmin("TestingForum", "username1", "superAdmin");
            mods = new List<Tuple<string, string, int>>();
            mods.Add(new Tuple<string, string, int>("username1", "superAdmin", 7));
            mods.Add(new Tuple<string, string, int>("username2", "superAdmin", 7));
            forumSystem.addSubForum("TestingForum", "TestingSubForum", mods, "superAdmin");
        }

        [TestMethod]
        public void WritePostGoodInput()
        {
            Assert.AreEqual("true", forumSystem.writePost("TestingForum", "TestingSubForum", 0, "username3", "title", "content"));
        }

        [TestMethod]
        public void WritePostNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.writePost(null, "TestingSubForum", 0, "username3", "title", "content"));
            Assert.AreNotEqual("true", forumSystem.writePost("TestingForum", null, 0, "username3", "title", "content"));
            Assert.AreNotEqual("true", forumSystem.writePost("TestingForum", "TestingSubForum", 0, null, "title", "content"));
            Assert.AreNotEqual("true", forumSystem.writePost("TestingForum", "TestingSubForum", 0, "username3", null, "content"));
            Assert.AreNotEqual("true", forumSystem.writePost("TestingForum", "TestingSubForum", 0, "username3", "title", null));
            Assert.AreNotEqual("true", forumSystem.writePost(null, null, 0, null, null, null));
        }

        [TestMethod]
        public void WritePostEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.writePost("", "TestingSubForum", 0, "username3", "title", "content"));
            Assert.AreNotEqual("true", forumSystem.writePost("TestingForum", "", 0, "username3", "title", "content"));
            Assert.AreNotEqual("true", forumSystem.writePost("TestingForum", "TestingSubForum", 0, "", "title", "content"));
            Assert.AreNotEqual("true", forumSystem.writePost("TestingForum", "TestingSubForum", 0, "username3", "", "content"));
            Assert.AreNotEqual("true", forumSystem.writePost("TestingForum", "TestingSubForum", 0, "username3", "title", ""));
            Assert.AreNotEqual("true", forumSystem.writePost("", "", 0, "", "", ""));
        }

        [TestMethod]
        public void WritePostUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.writePost("UnexistedForum", "TestingSubForum", 0, "username3", "title", "content"));
        }

        [TestMethod]
        public void WritePostUnexistedSubForum()
        {
            Assert.AreNotEqual("true", forumSystem.writePost("TestingForum", "UnexistedSubForum", 0, "username3", "title", "content"));
        }

        [TestMethod]
        public void WritePostUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.writePost("TestingForum", "TestingSubForum", 0, "UnexistedUser", "title", "content"));
        }
    }
}
