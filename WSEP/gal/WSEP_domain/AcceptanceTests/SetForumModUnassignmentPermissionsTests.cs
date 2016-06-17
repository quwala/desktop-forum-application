using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumManagement.forumHandler;
using WSEP_domain.forumSystem;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class SetForumModUnassignmentPermissionsTests
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
        public void SetForumModUnassignmentPermissionsGoodInput()
        {
            Assert.AreEqual("true", forumSystem.setForumModUnassignmentPermissions("TestingForum", modUnassignmentPermission.ADMIN, "superAdmin"));
        }

        [TestMethod]
        public void SetForumModUnassignmentPermissionsNullArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumModUnassignmentPermissions(null, modUnassignmentPermission.ADMIN, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumModUnassignmentPermissions("TestingForum", modUnassignmentPermission.ADMIN, null));
            Assert.AreNotEqual("true", forumSystem.setForumModUnassignmentPermissions(null, modUnassignmentPermission.ADMIN, null));
        }

        [TestMethod]
        public void SetForumModUnassignmentPermissionsEmptyArguments()
        {
            Assert.AreNotEqual("true", forumSystem.setForumModUnassignmentPermissions("", modUnassignmentPermission.ADMIN, "superAdmin"));
            Assert.AreNotEqual("true", forumSystem.setForumModUnassignmentPermissions("TestingForum", modUnassignmentPermission.ADMIN, ""));
            Assert.AreNotEqual("true", forumSystem.setForumModUnassignmentPermissions("", modUnassignmentPermission.ADMIN, ""));
        }

        [TestMethod]
        public void SetForumModUnassignmentPermissionsUnexistedForum()
        {
            Assert.AreNotEqual("true", forumSystem.setForumModUnassignmentPermissions("UnexistedForum", modUnassignmentPermission.ADMIN, "superAdmin"));
        }

        [TestMethod]
        public void SetForumModUnassignmentPermissionsUnexistedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumModUnassignmentPermissions("TestingForum", modUnassignmentPermission.ADMIN, "UnexistedUser"));
        }

        [TestMethod]
        public void SetForumModUnassignmentPermissionsUnpermissionedUser()
        {
            Assert.AreNotEqual("true", forumSystem.setForumModUnassignmentPermissions("TestingForum", modUnassignmentPermission.ADMIN, "username1"));
        }
    }
}
