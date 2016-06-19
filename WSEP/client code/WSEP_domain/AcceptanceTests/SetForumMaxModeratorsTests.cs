using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.forumManagement;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class SetForumMaxModeratorsTests
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
        public void SetForumMaxModeratorsGoodInput()
        {
            Assert.AreEqual("true", forumSystem.setForumMaxModerators("TestingForum", 5, "superAdmin"));
        }

        [TestMethod]
        public void SetForumMaxModeratorsNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMaxModerators(null, 5, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumMaxModerators("TestingForum", 5, null));
            Assert.AreNotEqual("true", forumSystem.setForumMaxModerators(null, 5, null));
        }

        [TestMethod]
        public void SetForumMaxModeratorsEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMaxModerators("", 5, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumMaxModerators("TestingForum", 5, ""));
            Assert.AreNotEqual("true", forumSystem.setForumMaxModerators("", 5, ""));
        }

        [TestMethod]
        public void SetForumMaxModeratorsUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMaxModerators("UnexistedForum", 5, "superAdmin"));
        }

        [TestMethod]
        public void SetForumMaxModeratorsUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMaxModerators("TestingForum", 5, "UnexistedUser"));
        }

        [TestMethod]
        public void SetForumMaxModeratorsUnpermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumMaxModerators("TestingForum", 5, "username1"));
        }
    }
}
