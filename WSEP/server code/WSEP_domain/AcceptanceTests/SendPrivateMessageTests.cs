using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumSystem;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class SendPrivateMessageTests
    {
        private IForumSystem forumSystem;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
            forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "email1@gmail.com", "q", "a");
            forumSystem.registerMemberToForum("TestingForum", "username2", "pass2", "email2@gmail.com", "q", "a");
        }

        [TestMethod]
        public void SendPrivateMessageGoodInput()
        {
            Assert.AreEqual("true", forumSystem.sendPM("TestingForum", "username1", "username2", "Hello"));
        }

        [TestMethod]
        public void SendPrivateMessageNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.sendPM(null, "username1", "username2", "Hello"));
            Assert.AreNotEqual("true", forumSystem.sendPM("TestingForum", null, "username2", "Hello"));
            Assert.AreNotEqual("true", forumSystem.sendPM("TestingForum", "username1", null, "Hello"));
            Assert.AreNotEqual("true", forumSystem.sendPM("TestingForum", "username1", "username2", null));
            Assert.AreNotEqual("true", forumSystem.sendPM(null, null, null, null));
        }

        [TestMethod]
        public void SendPrivateMessageEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.sendPM("", "username1", "username2", "Hello"));
            Assert.AreNotEqual("true", forumSystem.sendPM("TestingForum", "", "username2", "Hello"));
            Assert.AreNotEqual("true", forumSystem.sendPM("TestingForum", "username1", "", "Hello"));
            Assert.AreNotEqual("true", forumSystem.sendPM("TestingForum", "username1", "username2", ""));
            Assert.AreNotEqual("true", forumSystem.sendPM("", "", "", ""));
        }

        [TestMethod]
        public void SendPrivateMessageUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.sendPM("UnexistedForum", "username1", "username2", "Hello"));
        }

        [TestMethod]
        public void SendPrivateMessageUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.sendPM("TestingForum", "UnexistedUser", "username2", "Hello"));
            Assert.AreNotEqual("true", forumSystem.sendPM("TestingForum", "username1", "UnexistedUser", "Hello"));
        }
    }
}
