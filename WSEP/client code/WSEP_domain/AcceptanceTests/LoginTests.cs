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
    public class LoginTests
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
        public void LoginGoodInput()
        {
            Assert.AreEqual(loginStatus.TRUE, forumSystem.login("TestingForum", "username1", "pass1"));
        }

        [TestMethod]
        public void LoginNullArguments()
        {
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login(null, "username1", "pass1"));
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("TestingForum", null, "pass1"));
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("TestingForum", "username1", null));
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login(null, null, null));
        }

        [TestMethod]
        public void LoginEmptyArguments()
        {
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("", "username1", "pass1"));
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("TestingForum", "", "pass1"));
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("TestingForum", "username1", ""));
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("", "", ""));
        }

        [TestMethod]
        public void LoginUnexistedForum()
        {
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("UnexistedForum", "username1", "pass1"));
        }

        [TestMethod]
        public void LoginUnexistedUser()
        {
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("TestingForum", "UnexistedUser", "pass1"));
        }

        [TestMethod]
        public void LoginWrongPassword()
        {
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("TestingForum", "username1", "WrongPassword"));
        }
    }
}
