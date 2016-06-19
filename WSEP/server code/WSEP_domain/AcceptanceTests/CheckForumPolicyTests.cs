using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumManagement.forumHandler;
using WSEP_domain.forumSystem;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class CheckForumPolicyTests
    {
        private IForumSystem forumSystem;
        ForumPolicy forumPolicy;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
            forumPolicy = new ForumPolicy();
        }

        [TestMethod]
        public void CheckForumPolicyGoodInput()
        {
            Assert.AreEqual("true", forumSystem.checkForumPolicy("TestingForum", forumPolicy));
        }

        [TestMethod]
        public void CheckForumPolicyNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.checkForumPolicy(null, forumPolicy));
            Assert.AreNotEqual("true", forumSystem.checkForumPolicy("TestingForum", null));
            Assert.AreNotEqual("true", forumSystem.checkForumPolicy(null, null));
        }

        [TestMethod]
        public void CheckForumPolicyEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.checkForumPolicy("", forumPolicy));
        }

        [TestMethod]
        public void CheckForumPolicyUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.checkForumPolicy("UnexistedForum", forumPolicy));
        }
    }
}
