using Microsoft.VisualStudio.TestTools.UnitTesting;
using Observer;
using System.Net;
using System.Net.Sockets;
using WSEP_domain.forumSystem;
using WSEP_domain.userManagement;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class LoginTests
    {
        private IForumSystem forumSystem;
        IObserverable observerable;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
            forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "email1@gmail.com", "q", "a");
            observerable = new Observerable();
        }

        [TestMethod]
        public void LoginGoodInput()
        {
            Assert.AreEqual(loginStatus.TRUE, forumSystem.login("TestingForum", "username1", "pass1", observerable));
        }

        [TestMethod]
        public void LoginNullArguments()
        {
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login(null, "username1", "pass1", observerable));
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("TestingForum", null, "pass1", observerable));
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("TestingForum", "username1", null, observerable));
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login(null, null, null, observerable));
        }

        [TestMethod]
        public void LoginEmptyArguments()
        {
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("", "username1", "pass1", observerable));
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("TestingForum", "", "pass1", observerable));
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("TestingForum", "username1", "", observerable));
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("", "", "", observerable));
        }

        [TestMethod]
        public void LoginUnexistedForum()
        {
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("UnexistedForum", "username1", "pass1", observerable));
        }

        [TestMethod]
        public void LoginUnexistedUser()
        {
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("TestingForum", "UnexistedUser", "pass1", observerable));
        }

        [TestMethod]
        public void LoginWrongPassword()
        {
            Assert.AreEqual(loginStatus.FALSE, forumSystem.login("TestingForum", "username1", "WrongPassword", observerable));
        }
    }
}
