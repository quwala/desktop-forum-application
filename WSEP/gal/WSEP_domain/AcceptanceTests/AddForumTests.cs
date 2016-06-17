using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumSystem;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class AddForumTests
    {
        private IForumSystem forumSystem;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
            //forumSystem.registerMemberToForum("TestingForum", "Magal", "1234", "example@gmail.com");
        }

        [TestMethod]
        public void AddForumGoodInput()
        {
            Assert.AreEqual("true", forumSystem.addForum("newForum", "superAdmin"));
        }

        [TestMethod]
        public void AddForumNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.addForum(null, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.addForum("TestingForum", null));
            Assert.AreNotEqual("true", forumSystem.addForum(null, null));
        }

        [TestMethod]
        public void AddForumEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.addForum("", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.addForum("TestingForum", ""));
            Assert.AreNotEqual("true", forumSystem.addForum("", ""));
        }

        [TestMethod]
        public void AddForumUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.addForum("TestingForum", "unexistedUser"));
        }

        [TestMethod]
        public void AddForumExistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.addForum("TestingForum", "superAdmin"));
        }
    }
}
