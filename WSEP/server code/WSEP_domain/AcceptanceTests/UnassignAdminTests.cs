using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumSystem;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class UnassignAdminTests
    {
        private IForumSystem forumSystem;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
            forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "email1@gmail.com", "q", "a");
            forumSystem.registerMemberToForum("TestingForum", "username2", "pass2", "email2@gmail.com", "q", "a");
            forumSystem.assignAdmin("TestingForum", "username1", "superAdmin");
        }

        [TestMethod]
        public void UnassignAdminGoodInput()
        {
            Assert.AreEqual("true", forumSystem.unassignAdmin("TestingForum", "username1", "superAdmin"));
        }

        [TestMethod]
        public void UnassignAdminNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.unassignAdmin(null, "username1", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.unassignAdmin("TestingForum", null, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.unassignAdmin("TestingForum", "username1", null));
            Assert.AreNotEqual("true", forumSystem.unassignAdmin(null, null, null));
        }

        [TestMethod]
        public void UnassignAdminEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.unassignAdmin("", "username1", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.unassignAdmin("TestingForum", "", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.unassignAdmin("TestingForum", "username1", ""));
            Assert.AreNotEqual("true", forumSystem.unassignAdmin("", "", ""));
        }

        [TestMethod]
        public void UnassignAdminUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.unassignAdmin("UnexistedForum", "username1", "superAdmin"));
        }

        [TestMethod]
        public void UnassignAdminUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.unassignAdmin("TestingForum", "UnexistedUser", "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.unassignAdmin("TestingForum", "username1", "UnexistedUser"));
        }

        [TestMethod]
        public void UnassignAdminUnpermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.unassignAdmin("TestingForum", "username1", "username2"));
        }
    }
}
