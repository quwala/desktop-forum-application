using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.userManagementService;
using System.Collections.Generic;

namespace WSEP_tests.userManagementTests
{
    [TestClass]
    public class UserManagerUnitTests
    {

        private IUserManager um;

        [TestInitialize()]
        public void Initialize()
        {
            um = new UserManager();
        }

        [TestMethod]
        public void UnitTest_addForum()
        {
            // start conditions
            // none

            // invalid inputs
            Assert.IsTrue(um.addForum(null).Contains("Invalid"));
            Assert.IsTrue(um.addForum("null").Contains("Invalid"));
            Assert.IsTrue(um.addForum("").Contains("Invalid"));

            // valid inputs that should succeed
            Assert.IsTrue(um.addForum("forum1").Equals("true"));
            Assert.IsTrue(um.addForum("forum2").Equals("true"));

            // valid inputs that should fail
            Assert.IsFalse(um.addForum("forum1").Equals("true"));
        }

        [TestMethod]
        public void UnitTest_addSubForum()
        {
            // start conditions:
            // there is a forum
            // that forum has a member
            um.addForum("forum1");
            um.registerMemberToForum("forum1", "user1", "pass1", "eMail1@gmail.com");
            List<string> moderators = new List<string>();
            moderators.Add("user1");
            List<string> invalidList1 = new List<string>();
            invalidList1.Add("null");
            List<string> invalidList2 = new List<string>();
            invalidList2.Add("");
            List<string> wrongUsername = new List<string>();
            wrongUsername.Add("user2");

            // invalid input
            Assert.IsTrue(um.addSubForum(null, "subforum1", moderators).Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("null", "subforum1", moderators).Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("", "subforum1", moderators).Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("forum1", null, moderators).Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("forum1", "null", moderators).Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("forum1", "", moderators).Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("forum1", "subforum1", null).Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("forum1", "subforum1", invalidList1).Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("forum1", "subforum1", invalidList2).Contains("Invalid"));

            // valid inputs that should succeed
            Assert.IsTrue(um.addSubForum("forum1", "subforum1", moderators).Equals("true"));
            Assert.IsTrue(um.addSubForum("forum1", "subforum2", moderators).Equals("true"));

            // valid inputs that should fail
            // sub forum exists in forum
            Assert.IsFalse(um.addSubForum("forum1", "subforum1", moderators).Equals("true"));
            // forum does not exist
            Assert.IsFalse(um.addSubForum("forum2", "subforum3", moderators).Equals("true"));
            // user does not exist
            Assert.IsFalse(um.addSubForum("forum1", "subforum4", wrongUsername).Equals("true"));
            // not enough moderators
            Assert.IsFalse(um.addSubForum("forum1", "subforum4", moderators).Equals("true"));
            // too much moderators
            moderators.Add("user3");
            Assert.IsFalse(um.addSubForum("forum1", "subforum4", moderators).Equals("true"));
        }

        [TestMethod]
        public void UnitTest_registerMemberToForum()
        {
            // start conditions:
            // there is a forum
            um.addForum("forum1");

            // invalid inputs
            Assert.IsTrue(um.registerMemberToForum(null, "user8", "pass2", "email6@gmail.com").Contains("Invalid"));
            Assert.IsTrue(um.registerMemberToForum("null", "user8", "pass2", "email6@gmail.com").Contains("Invalid"));
            Assert.IsTrue(um.registerMemberToForum("", "user8", "pass2", "email6@gmail.com").Contains("Invalid"));
            Assert.IsTrue(um.registerMemberToForum("forum1", null, "pass2", "email6@gmail.com").Contains("Invalid"));
            Assert.IsTrue(um.registerMemberToForum("forum1", "null", "pass2", "email6@gmail.com").Contains("Invalid"));
            Assert.IsTrue(um.registerMemberToForum("forum1", "", "pass2", "email6@gmail.com").Contains("Invalid"));
            Assert.IsTrue(um.registerMemberToForum("forum1", "user8", null, "email6@gmail.com").Contains("Invalid"));
            Assert.IsTrue(um.registerMemberToForum("forum1", "user8", "null", "email6@gmail.com").Contains("Invalid"));
            Assert.IsTrue(um.registerMemberToForum("forum1", "user8", "", "email6@gmail.com").Contains("Invalid"));
            Assert.IsTrue(um.registerMemberToForum("forum1", "user8", "pass2", null).Contains("Invalid"));
            Assert.IsTrue(um.registerMemberToForum("forum1", "user8", "pass2", "null").Contains("Invalid"));
            Assert.IsTrue(um.registerMemberToForum("forum1", "user8", "pass2", "").Contains("Invalid"));

            // valid inputs that should succeed
            Assert.IsTrue(um.registerMemberToForum("forum1", "user1", "pass1", "email1@gmail.com").Equals("true"));
            Assert.IsTrue(um.registerMemberToForum("forum1", "user2", "pass1", "email2@gmail.com").Equals("true"));
            
            // valid inputs that should fail
            // email exists in this forum
            Assert.IsFalse(um.registerMemberToForum("forum1", "user3", "pass1", "email2@gmail.com").Equals("true"));
            // username exists in this forum
            Assert.IsFalse(um.registerMemberToForum("forum1", "user2", "pass1", "email4@gmail.com").Equals("true"));
            // invalid email - no "@"
            Assert.IsFalse(um.registerMemberToForum("forum1", "user4", "pass1", "email.com").Equals("true"));
            // invalid email - no "." after "@"
            Assert.IsFalse(um.registerMemberToForum("forum1", "user5", "pass1", "email@gmail").Equals("true"));
            // invalid email - "." right after "@"
            Assert.IsFalse(um.registerMemberToForum("forum1", "user5", "pass1", "email@.com").Equals("true"));
            // invalid email - "." as last character
            Assert.IsFalse(um.registerMemberToForum("forum1", "user5", "pass1", "email@gmail.").Equals("true"));
            // invalid email - "@" as first character
            Assert.IsFalse(um.registerMemberToForum("forum1", "user5", "pass1", "@gmail.").Equals("true"));
            // empty username
            Assert.IsFalse(um.registerMemberToForum("forum1", "", "pass1", "email1@gmail.com").Equals("true"));
            // empty password
            Assert.IsFalse(um.registerMemberToForum("forum1", "user6", "", "email1@gmail.com").Equals("true"));
            // empty email
            Assert.IsFalse(um.registerMemberToForum("forum1", "user6", "pass1", "").Equals("true"));
            // invalid username - begins with " "
            Assert.IsFalse(um.registerMemberToForum("forum1", " user7", "pass1", "email5@gmail.com").Equals("true"));
            // invalid password - contains " "
            Assert.IsFalse(um.registerMemberToForum("forum1", "user7", " pass1", "email5@gmail.com").Equals("true"));
            // invalid password - contains " "
            Assert.IsFalse(um.registerMemberToForum("forum1", "user7", "pas s1", "email5@gmail.com").Equals("true"));
            // invalid password - contains " "
            Assert.IsFalse(um.registerMemberToForum("forum1", "user7", "pass1 ", "email5@gmail.com").Equals("true"));
        }

        [TestMethod]
        public void UnitTest_assignAdmin()
        {
            // start conditions:
            // there is a forum
            // there are 2 members in the forum
            um.addForum("forum1");
            um.registerMemberToForum("forum1", "user1", "pass1", "eMail1@gmail.com");
            um.registerMemberToForum("forum1", "user2", "pass2", "eMail2@gmail.com");

            // invalid input
            Assert.IsTrue(um.assignAdmin(null, "user1", 2).Contains("Invalid"));
            Assert.IsTrue(um.assignAdmin("null", "user1", 2).Contains("Invalid"));
            Assert.IsTrue(um.assignAdmin("", "user1", 2).Contains("Invalid"));
            Assert.IsTrue(um.assignAdmin("forum1", null, 2).Contains("Invalid"));
            Assert.IsTrue(um.assignAdmin("forum1", "null", 2).Contains("Invalid"));
            Assert.IsTrue(um.assignAdmin("forum1", "", 2).Contains("Invalid"));

            // valid inputs that should succeed
            Assert.IsTrue(um.assignAdmin("forum1", "superAdmin", 2).Equals("true"));
            Assert.IsTrue(um.assignAdmin("forum1", "user1", 2).Equals("true"));
            Assert.IsTrue(um.assignAdmin("forum1", "user1", 2).Equals("true"));

            // valid inputs that should fail
            Assert.IsFalse(um.assignAdmin("forum2", "user1", 2).Equals("true"));
            Assert.IsFalse(um.assignAdmin("forum1", "user2", 2).Equals("true"));
            Assert.IsFalse(um.assignAdmin("forum1", "user3", 2).Equals("true"));

            // valid inputs that should succeed
            Assert.IsTrue(um.assignAdmin("forum1", "user2", 3).Equals("true"));
        }

        [TestMethod]
        public void UnitTest_unassignAdmin()
        {
            // start conditions:
            // there is a forum
            // there is are 2 admins to that forum (other than super admin)
            um.addForum("forum1");
            um.registerMemberToForum("forum1", "user1", "pass1", "eMail1@gmail.com");
            um.registerMemberToForum("forum1", "user2", "pass2", "eMail2@gmail.com");
            um.assignAdmin("forum1", "user1", 2);
            um.assignAdmin("forum1", "user2", 3);

            // invalid input
            Assert.IsTrue(um.unassignAdmin(null, "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignAdmin("null", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignAdmin("", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignAdmin("forum1", null, 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignAdmin("forum1", "null", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignAdmin("forum1", "", 1).Contains("Invalid"));

            // valid inputs that should pass
            Assert.IsTrue(um.unassignAdmin("forum1", "user1", 1).Equals("true"));
            Assert.IsTrue(um.unassignAdmin("forum1", "user3", 1).Equals("true"));

            // valid inputs that should fail
            Assert.IsFalse(um.unassignAdmin("forum2", "user1", 2).Equals("true"));
            Assert.IsFalse(um.unassignAdmin("forum1", "superAdmin", 0).Equals("true"));
            Assert.IsFalse(um.unassignAdmin("forum1", "user2", 2).Equals("true"));
        }

        [TestMethod]
        public void UnitTest_assignModerator()
        {
            // start conditions:
            // there is a forum
            // that forrum has a sub forum
            // there are 2 members in the forum
            um.addForum("forum1");
            List<string> moderators = new List<string>();
            moderators.Add("superAdmin");
            um.addSubForum("forum1", "subforum1", moderators);
            um.registerMemberToForum("forum1", "user1", "pass1", "eMail1@gmail.com");
            um.registerMemberToForum("forum1", "user2", "pass2", "eMail2@gmail.com");

            // invalid input
            Assert.IsTrue(um.assignModerator(null, "subforum1", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.assignModerator("null", "subforum1", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.assignModerator("", "subforum1", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.assignModerator("forum1", null, "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.assignModerator("forum1", "null", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.assignModerator("forum1", "", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.assignModerator("forum1", "subforum1", null, 1).Contains("Invalid"));
            Assert.IsTrue(um.assignModerator("forum1", "subforum1", "null", 1).Contains("Invalid"));
            Assert.IsTrue(um.assignModerator("forum1", "subforum1", "", 1).Contains("Invalid"));

            // valid input that should succeed
            Assert.IsTrue(um.assignModerator("forum1", "subforum1", "user1", 2).Equals("true"));
            Assert.IsTrue(um.assignModerator("forum1", "subforum1", "user1", 2).Equals("true"));
            Assert.IsTrue(um.assignModerator("forum1", "subforum1", "superAdmin", 2).Equals("true"));

            // valid inputthat should fail
            Assert.IsFalse(um.assignModerator("forum2", "subforum1", "user1", 2).Equals("true"));
            Assert.IsFalse(um.assignModerator("forum1", "subforum2", "user2", 2).Equals("true"));
            Assert.IsFalse(um.assignModerator("forum1", "subforum1", "user2", 2).Equals("true"));
            Assert.IsFalse(um.assignModerator("forum1", "subforum1", "user3", 5).Equals("true"));
        }

        [TestMethod]
        public void UnitTest_getUserPermissionsForForum()
        {
            // start conditions:
            // there is a forum
            // there is a member
            // there is a user
            // there is an admin
            um.addForum("forum1");
            um.registerMemberToForum("forum1", "user1", "pass1", "eMail1@gmail.com");
            um.registerMemberToForum("forum1", "user2", "pass2", "eMail2@gmail.com");
            um.assignAdmin("forum1", "user1", 2);

            // invalid input
            Assert.IsTrue(um.getUserPermissionsForForum(null, "user1").Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForForum("null", "user1").Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForForum("", "user1").Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForForum("forum1", null).Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForForum("forum1", "null").Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForForum("forum1", "").Equals(permission.INVALID));

            // valid inputs that should succeed
            Assert.IsTrue(um.getUserPermissionsForForum("forum1", "superAdmin").Equals(permission.SUPER_ADMIN));
            Assert.IsTrue(um.getUserPermissionsForForum("forum1", "user1").Equals(permission.ADMIN));
            Assert.IsTrue(um.getUserPermissionsForForum("forum1", "user2").Equals(permission.MEMBER));
            Assert.IsTrue(um.getUserPermissionsForForum("forum1", "user3").Equals(permission.GUEST));

            // valid inputs that should succeed
            Assert.IsTrue(um.getUserPermissionsForForum("forum2", "user1").Equals(permission.INVALID));
        }

        [TestMethod]
        public void UnitTest_getUserPermissionsForSubForum()
        {
            // start conditions"
            // there is a forum
            // there is an admin
            // there is a sub forum
            // there is a member
            um.addForum("forum1");
            um.registerMemberToForum("forum1", "user1", "pass1", "eMail1@gmail.com");
            um.assignAdmin("forum1", "user1", 2);
            um.registerMemberToForum("forum1", "user2", "pass2", "eMail2@gmail.com");
            List<string> moderators = new List<string>();
            moderators.Add("user2");
            um.addSubForum("forum1", "subforum1", moderators);
            um.registerMemberToForum("forum1", "user3", "pass3", "eMail3@gmail.com");

            // invalid input
            Assert.IsTrue(um.getUserPermissionsForSubForum(null, "subforum1", "user1").Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForSubForum("null", "subforum1", "user1").Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForSubForum("", "subforum1", "user1").Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForSubForum("forum1", null, "user1").Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForSubForum("forum1", "null", "user1").Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForSubForum("forum1", "", "user1").Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForSubForum("forum1", "subforum1", null).Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForSubForum("forum1", "subforum1", "null").Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForSubForum("forum1", "subforum1", "").Equals(permission.INVALID));

            // valid inputs that should succeed
            Assert.IsTrue(um.getUserPermissionsForSubForum("forum1", "subforum1", "superAdmin").Equals(permission.SUPER_ADMIN));
            Assert.IsTrue(um.getUserPermissionsForSubForum("forum1", "subforum1", "user1").Equals(permission.ADMIN));
            Assert.IsTrue(um.getUserPermissionsForSubForum("forum1", "subforum1", "user2").Equals(permission.MODERATOR));
            Assert.IsTrue(um.getUserPermissionsForSubForum("forum1", "subforum1", "user3").Equals(permission.MEMBER));
            Assert.IsTrue(um.getUserPermissionsForSubForum("forum1", "subforum1", "user4").Equals(permission.GUEST));

            // valid inputs that should fail
            Assert.IsTrue(um.getUserPermissionsForSubForum("forum2", "subforum1", "user1").Equals(permission.INVALID));
            Assert.IsTrue(um.getUserPermissionsForSubForum("forum1", "subforum2", "user1").Equals(permission.INVALID));
        }

        [TestMethod]
        public void UnitTest_unassignModerator()
        {
            // start conditions:
            // there is a forum
            // that forum has a sub forum
            // that forum has a member
            // that sub forum has a moderator
            um.addForum("forum1");
            List<string> moderators = new List<string>();
            moderators.Add("superAdmin");
            um.addSubForum("forum1", "subforum1", moderators);
            um.registerMemberToForum("forum1", "user1", "pass1", "eMail1@gmail.com");
            um.assignModerator("forum1", "subforum1", "user1", 2);

            // invalid input
            Assert.IsTrue(um.unassignModerator(null, "subforum1", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignModerator("null", "subforum1", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignModerator("", "subforum1", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignModerator("forum1", null, "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignModerator("forum1", "null", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignModerator("forum1", "", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignModerator("forum1", "subforum1", null, 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignModerator("forum1", "subforum1", "null", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignModerator("forum1", "subforum1", "", 1).Contains("Invalid"));

            // valid inputs that should succeed
            Assert.IsTrue(um.unassignModerator("forum1", "subforum1", "user1", 1).Equals("true"));
            Assert.IsTrue(um.unassignModerator("forum1", "subforum1", "user2", 1).Equals("true"));
            Assert.IsTrue(um.unassignModerator("forum1", "subforum1", "superAdmin", 0).Equals("true"));

            um.assignModerator("forum1", "subforum1", "user1", 2);
            um.assignModerator("forum1", "subforum1", "superAdmin", 2);

            // valid inputs that should fail
            Assert.IsFalse(um.unassignModerator("forum2", "subforum1", "user1", 2).Equals("true"));
            Assert.IsFalse(um.unassignModerator("forum1", "subforum2", "user1", 2).Equals("true"));
            Assert.IsFalse(um.unassignModerator("forum1", "subforum1", "user1", 2).Equals("true"));
        }

        [TestMethod]
        public void UnitTest_sendPM()
        {
            // start conditions:
            // there is a forum
            // that forum has two members
            um.addForum("forum1");
            um.registerMemberToForum("forum1", "user1", "pass1", "eMail1@gmail.com");
            um.registerMemberToForum("forum1", "user2", "pass1", "eMail2@gmail.com");

            // invalid inputs
            Assert.IsTrue(um.sendPM(null, "user1", "user2", "msg").Contains("Invalid"));
            Assert.IsTrue(um.sendPM("null", "user1", "user2", "msg").Contains("Invalid"));
            Assert.IsTrue(um.sendPM("", "user1", "user2", "msg").Contains("Invalid"));
            Assert.IsTrue(um.sendPM("forum1", null, "user2", "msg").Contains("Invalid"));
            Assert.IsTrue(um.sendPM("forum1", "null", "user2", "msg").Contains("Invalid"));
            Assert.IsTrue(um.sendPM("forum1", "", "user2", "msg").Contains("Invalid"));
            Assert.IsTrue(um.sendPM("forum1", "user1", null, "msg").Contains("Invalid"));
            Assert.IsTrue(um.sendPM("forum1", "user1", "null", "msg").Contains("Invalid"));
            Assert.IsTrue(um.sendPM("forum1", "user1", "", "msg").Contains("Invalid"));
            Assert.IsTrue(um.sendPM("forum1", "user1", "user2", null).Contains("Invalid"));
            Assert.IsTrue(um.sendPM("forum1", "user1", "user2", "null").Contains("Invalid"));
            Assert.IsTrue(um.sendPM("forum1", "user1", "user2", "").Contains("Invalid"));

            // valid inputs that should succeed
            Assert.IsTrue(um.sendPM("forum1", "user1", "user2", "msg").Equals("true"));

            // valid inputs that should fail
            Assert.IsFalse(um.sendPM("forum2", "user1", "user2", "msg").Equals("true"));
            Assert.IsFalse(um.sendPM("forum1", "user3", "user2", "msg").Equals("true"));
            Assert.IsFalse(um.sendPM("forum1", "user1", "user3", "msg").Equals("true"));
        }

        [TestMethod]
        public void UnitTest_checkForumPolicy()
        {
            // start conditions:
            // there is a forum
            // that forum has a member
            um.addForum("forum1");
            um.registerMemberToForum("forum1", "user1", "pass1", "eMail1@gmail.com");

            // invalid inputs
            Assert.IsTrue(um.checkForumPolicy(null, 1, 2, 1, 2).Contains("Invalid"));
            Assert.IsTrue(um.checkForumPolicy("null", 1, 2, 1, 2).Contains("Invalid"));
            Assert.IsTrue(um.checkForumPolicy("", 1, 2, 1, 2).Contains("Invalid"));

            // valid inputs that should succeed
            Assert.IsTrue(um.checkForumPolicy("forum1", 1, 1, 1, 2).Equals("true"));
            Assert.IsTrue(um.checkForumPolicy("forum1", 1, 2, 1, 2).Equals("true"));

            um.assignAdmin("forum1", "user1", 2);

            Assert.IsTrue(um.checkForumPolicy("forum1", 2, 2, 1, 2).Equals("true"));

            // valid inputs that should fail
            Assert.IsFalse(um.checkForumPolicy("forum2", 1, 1, 1, 2).Equals("true"));
            Assert.IsFalse(um.checkForumPolicy("forum1", 3, 5, 1, 2).Equals("true"));
            Assert.IsFalse(um.checkForumPolicy("forum1", 7, 5, 1, 2).Equals("true"));
            Assert.IsFalse(um.checkForumPolicy("forum1", 0, 0, 1, 2).Equals("true"));

            List<string> moderators = new List<string>();
            moderators.Add("user1");
            um.addSubForum("forum1", "subforum1", moderators);

            Assert.IsFalse(um.checkForumPolicy("forum1", 2, 2, 0, 0).Equals("true"));
            Assert.IsFalse(um.checkForumPolicy("forum1", 2, 2, 2, 2).Equals("true"));

            // valid inputs that should succeed
            Assert.IsTrue(um.checkForumPolicy("forum1", 2, 2, 1, 1).Equals("true"));
        }
    }
}
