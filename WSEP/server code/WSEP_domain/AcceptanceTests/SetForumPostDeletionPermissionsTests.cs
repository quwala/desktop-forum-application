using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumManagement.forumHandler;
using WSEP_domain.forumSystem;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class SetForumPostDeletionPermissionsTests
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
        public void SetForumPostDeletionPermissionsGoodInput()
        {
            Assert.AreEqual("true", forumSystem.setForumPostDeletionPermissions("TestingForum", postDeletionPermission.ADMIN, "superAdmin"));
        }

        [TestMethod]
        public void SetForumPostDeletionPermissionsNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumPostDeletionPermissions(null, postDeletionPermission.ADMIN, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumPostDeletionPermissions("TestingForum", postDeletionPermission.ADMIN, null));
            Assert.AreNotEqual("true", forumSystem.setForumPostDeletionPermissions(null, postDeletionPermission.ADMIN, null));
        }

        [TestMethod]
        public void SetForumPostDeletionPermissionsEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumPostDeletionPermissions("", postDeletionPermission.ADMIN, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumPostDeletionPermissions("TestingForum", postDeletionPermission.ADMIN, ""));
            Assert.AreNotEqual("true", forumSystem.setForumPostDeletionPermissions("", postDeletionPermission.ADMIN, ""));
        }

        [TestMethod]
        public void SetForumPostDeletionPermissionsUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.setForumPostDeletionPermissions("UnexistedForum", postDeletionPermission.ADMIN, "superAdmin"));
        }

        [TestMethod]
        public void SetForumPostDeletionPermissionsUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumPostDeletionPermissions("TestingForum", postDeletionPermission.ADMIN, "UnexistedUser"));
        }

        [TestMethod]
        public void SetForumPostDeletionPermissionsUnpermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumPostDeletionPermissions("TestingForum", postDeletionPermission.ADMIN, "username1"));
        }
    }
}
