using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumSystem;
using WSEP_domain.userManagement;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class GetUserPermissionsForForumTests
    {
        private IForumSystem forumSystem;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
            forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "email1@gmail.com", "q", "a");
        }

        [TestMethod]
        public void GetUserPermissionsForForumGoodInput()
        {
            Assert.AreEqual(permission.SUPER_ADMIN, forumSystem.getUserPermissionsForForum("TestingForum", "superAdmin"));
            Assert.AreEqual(permission.MEMBER, forumSystem.getUserPermissionsForForum("TestingForum", "username1"));
        }

        [TestMethod]
        public void GetUserPermissionsForForumNullArguments()
        {
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForForum(null, "superAdmin"));
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForForum("TestingForum", null));
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForForum(null, null));
        }

        [TestMethod]
        public void GetUserPermissionsForForumEmptyArguments()
        {
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForForum("", "superAdmin"));
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForForum("TestingForum", ""));
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForForum("", ""));
        }

        [TestMethod]
        public void GetUserPermissionsForForumUnexistedForum()
        {
            Assert.AreEqual(permission.INVALID, forumSystem.getUserPermissionsForForum("UnexistedForum", "superAdmin"));
        }

        [TestMethod]
        public void GetUserPermissionsForForumUnexistedUser()
        {
            Assert.AreEqual(permission.GUEST, forumSystem.getUserPermissionsForForum("TestingForum", "UnexistedUser"));
        }
    }
}
