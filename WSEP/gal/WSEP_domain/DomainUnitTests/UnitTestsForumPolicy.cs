using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumManagement.forumHandler;

namespace DomainUnitTests
{
    [TestClass]
    public class UnitTestsForumPolicy
    {
        [TestMethod]
        public void UnitTestForumPolicyCreate()
        {
            Assert.IsTrue(ForumPolicy.create(10, 1, 10, 1, postDeletionPermission.WRITER, 365, 0, modUnassignmentPermission.ADMIN) != null);
        }

        [TestMethod]
        public void UnitTestForumPolicyInvalidPolicy()
        {
            Assert.IsTrue(ForumPolicy.create(10, 11, 10, 1, postDeletionPermission.WRITER, 365, 0, modUnassignmentPermission.ADMIN) == null);
            Assert.IsTrue(ForumPolicy.create(10, 1, 10, 11, postDeletionPermission.WRITER, 365, 0, modUnassignmentPermission.ADMIN) == null);
            Assert.IsTrue(ForumPolicy.create(0, -1, 10, 1, postDeletionPermission.WRITER, 365, 0, modUnassignmentPermission.ADMIN) == null);
            Assert.IsTrue(ForumPolicy.create(10, 1, 0, -1, postDeletionPermission.WRITER, 365, 0, modUnassignmentPermission.ADMIN) == null);
            Assert.IsTrue(ForumPolicy.create(10, 1, 10, 1, postDeletionPermission.WRITER, 0, 0, modUnassignmentPermission.ADMIN) == null);
            Assert.IsTrue(ForumPolicy.create(10, 1, 10, 1, postDeletionPermission.WRITER, 365, -5, modUnassignmentPermission.ADMIN) == null);
            Assert.IsTrue(ForumPolicy.create(10, 1, 10, 1, postDeletionPermission.INVALID, 365, 0, modUnassignmentPermission.ADMIN) == null);
            Assert.IsTrue(ForumPolicy.create(10, 1, 10, 1, postDeletionPermission.WRITER, 365, 0, modUnassignmentPermission.INVALID) == null);
        }

        [TestMethod]
        public void UnitTestForumPolicyAdminsGettersSetters()
        {
            ForumPolicy fp = new ForumPolicy();
            fp.setMaxAdmins(0);
            Assert.IsTrue(fp.getMaxAdmins() != 0);
            Assert.IsTrue(fp.getMaxAdmins() == 10);
            fp.setMaxAdmins(11);
            Assert.IsTrue(fp.getMaxAdmins() == 11);
            fp.setMinAdmins(12);
            Assert.IsTrue(fp.getMinAdmins() != 12);
            Assert.IsTrue(fp.getMinAdmins() == 1);
            fp.setMinAdmins(2);
            Assert.IsTrue(fp.getMinAdmins() == 2);
        }

        [TestMethod]
        public void UnitTestForumPolicyModeratorsGettersSetters()
        {
            ForumPolicy fp = new ForumPolicy();
            fp.setMaxModerators(0);
            Assert.IsTrue(fp.getMaxModerators() != 0);
            Assert.IsTrue(fp.getMaxModerators() == 10);
            fp.setMaxModerators(11);
            Assert.IsTrue(fp.getMaxModerators() == 11);
            fp.setMinModerators(12);
            Assert.IsTrue(fp.getMinModerators() != 12);
            Assert.IsTrue(fp.getMinModerators() == 1);
            fp.setMinModerators(2);
            Assert.IsTrue(fp.getMinModerators() == 2);
        }

        [TestMethod]
        public void UnitTestForumPolicyPermissionsGettersSetters()
        {
            ForumPolicy fp = new ForumPolicy();
            fp.setPostDeletionPermissions(postDeletionPermission.INVALID);
            Assert.IsTrue(fp.getPostDeletionPermissions() != postDeletionPermission.INVALID);
            Assert.IsTrue(fp.getPostDeletionPermissions() == postDeletionPermission.WRITER);
            fp.setPostDeletionPermissions(postDeletionPermission.SUPER_ADMIN);
            Assert.IsTrue(fp.getPostDeletionPermissions() == postDeletionPermission.SUPER_ADMIN);
            fp.setModUnassignmentPermission(modUnassignmentPermission.INVALID);
            Assert.IsTrue(fp.getModUnassignmentPermissions() != modUnassignmentPermission.INVALID);
            Assert.IsTrue(fp.getModUnassignmentPermissions() == modUnassignmentPermission.ADMIN);
            fp.setModUnassignmentPermission(modUnassignmentPermission.SUPER_ADMIN);
            Assert.IsTrue(fp.getModUnassignmentPermissions() == modUnassignmentPermission.SUPER_ADMIN);
        }

        [TestMethod]
        public void UnitTestForumPolicyPasswordLifespanGettersSetters()
        {
            ForumPolicy fp = new ForumPolicy();
            fp.setPasswordLifespan(0);
            Assert.IsTrue(fp.getPasswordLifespan() != 0);
            Assert.IsTrue(fp.getPasswordLifespan() == 365);
            fp.setPasswordLifespan(7);
            Assert.IsTrue(fp.getPasswordLifespan() == 7);
        }

        [TestMethod]
        public void UnitTestForumPolicyModeratorsSeniorityGettersSetters()
        {
            ForumPolicy fp = new ForumPolicy();
            fp.setModeratorsSeniority(-1);
            Assert.IsTrue(fp.getModeratorsSeniority() != -1);
            Assert.IsTrue(fp.getModeratorsSeniority() == 0);
            fp.setModeratorsSeniority(7);
            Assert.IsTrue(fp.getModeratorsSeniority() == 7);
        }
    }
}
