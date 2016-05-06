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
    public class RegisterMemberToForumTests
    {
        private IForumSystem forumSystem;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
        }

        [TestMethod]
        public void RegisterMemberToForumGoodInput()
        {
            Assert.AreEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "email1@gmail.com"));
        }

        [TestMethod]
        public void RegisterMemberToForumNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum(null, "username1", "pass1", "email1@gmail.com"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", null, "pass1", "email1@gmail.com"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", null, "email1@gmail.com"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", null));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum(null, null, null, null));
        }

        [TestMethod]
        public void RegisterMemberToForumEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("", "username1", "pass1", "email1@gmail.com"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "", "pass1", "email1@gmail.com"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "", "email1@gmail.com"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", ""));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("", "", "", ""));
        }

        [TestMethod]
        public void RegisterMemberToForumUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("UnexistedForum", "username1", "pass1", "email1@gmail.com"));
     
        }

        [TestMethod]
        public void RegisterMemberToForumInvalidEmail()
        {
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "InvalidEmail"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "@$%&#*!$^ $^"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "124234vd    ghdn$%&@  $&a sdg "));

        }
    }
}
