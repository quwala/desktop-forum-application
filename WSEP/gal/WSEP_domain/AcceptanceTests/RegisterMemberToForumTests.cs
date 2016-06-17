using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumSystem;

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
            Assert.AreEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "email1@gmail.com", "q", "a"));
        }

        [TestMethod]
        public void RegisterMemberToForumNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum(null, "username1", "pass1", "email1@gmail.com", "q", "a"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", null, "pass1", "email1@gmail.com", "q", "a"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", null, "email1@gmail.com", "q", "a"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", null, "q", "a"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum(null, null, null, null, "q", "a"));
        }

        [TestMethod]
        public void RegisterMemberToForumEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("", "username1", "pass1", "email1@gmail.com", "q", "a"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "", "pass1", "email1@gmail.com", "q", "a"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "", "email1@gmail.com", "q", "a"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "", "q", "a"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("", "", "", "", "q", "a"));
        }

        [TestMethod]
        public void RegisterMemberToForumUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("UnexistedForum", "username1", "pass1", "email1@gmail.com", "q", "a"));
     
        }

        [TestMethod]
        public void RegisterMemberToForumInvalidEmail()
        {
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "InvalidEmail", "q", "a"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "@$%&#*!$^ $^", "q", "a"));
            Assert.AreNotEqual("true", forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "124234vd    ghdn$%&@  $&a sdg ", "q", "a"));

        }
    }
}
