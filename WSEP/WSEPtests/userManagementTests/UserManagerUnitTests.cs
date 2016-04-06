using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.userManagement;

namespace WSEPtests.userManagementTests
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
            um.addForum("forum1");

            // invalid input
            Assert.IsTrue(um.addSubForum(null, "subforum5", "superAdmin").Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("null", "subforum5", "superAdmin").Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("", "subforum5", "superAdmin").Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("forum1", null, "superAdmin").Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("forum1", "null", "superAdmin").Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("forum1", "", "superAdmin").Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("forum1", "subforum5", null).Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("forum1", "subforum5", "null").Contains("Invalid"));
            Assert.IsTrue(um.addSubForum("forum1", "subforum5", "").Contains("Invalid"));

            // valid inputs that should succeed
            Assert.IsTrue(um.addSubForum("forum1", "subforum1", "superAdmin").Equals("true"));
            Assert.IsTrue(um.addSubForum("forum1", "subforum2", "superAdmin").Equals("true"));

            // valid inputs that should fail
            // sub forum exists in forum
            Assert.IsFalse(um.addSubForum("forum1", "subforum1", "superAdmin").Equals("true"));
            // forum does not exist
            Assert.IsFalse(um.addSubForum("forum2", "subforum3", "superAdmin").Equals("true"));
            // admin does not exist
            Assert.IsFalse(um.addSubForum("forum1", "subforum4", "admin").Equals("true"));
            // empty forum name
            Assert.IsFalse(um.addSubForum("", "subforum4", "admin").Equals("true"));
            // empty sub forum name
            Assert.IsFalse(um.addSubForum("forum1", "", "admin").Equals("true"));
            // empty admin username
            Assert.IsFalse(um.addSubForum("forum1", "subforum4", "").Equals("true"));
            
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
            Assert.IsTrue(um.assignAdmin("forum1", "user1", 2).Equals("true"));

            // valid inputs that should fail
            Assert.IsFalse(um.assignAdmin("forum2", "user1", 2).Equals("true"));
            Assert.IsFalse(um.assignAdmin("forum1", "user2", 2).Equals("true"));

            // valid inputs that should succeed
            Assert.IsTrue(um.assignAdmin("forum1", "user2", 3).Equals("true"));
        }

        [TestMethod]
        public void UnitTest_unassignAdmin()
        {
            // start conditions:
            // there is a forum
            // there is an admin to that forum (other than super admin)
            um.addForum("forum1");
            um.registerMemberToForum("forum1", "user1", "pass1", "eMail1@gmail.com");
            um.assignAdmin("forum1", "suer1", 2);

            // invalid input
            Assert.IsTrue(um.unassignAdmin(null, "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignAdmin("null", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignAdmin("", "user1", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignAdmin("forum1", null, 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignAdmin("forum1", "null", 1).Contains("Invalid"));
            Assert.IsTrue(um.unassignAdmin("forum1", "", 1).Contains("Invalid"));
            
            // valid inputs that should pass
            Assert.IsTrue(um.unassignAdmin("forum1", "user1", 1).Equals("true"));
            Assert.IsTrue(um.unassignAdmin("forum1", "user2", 1).Equals("true"));

            // valid inputs that should fail
            Assert.IsFalse(um.unassignAdmin("forum2", "user1", 2).Equals("true"));
            Assert.IsFalse(um.unassignAdmin("forum1", "superAdmin", 0).Equals("true"));
        }

        [TestMethod]
        public void UnitTest_assignModerator()
        {
            // start conditions:
            // there is a forum
            // that forrum has a sub forum
            // there are 2 members in the forum
            um.addForum("forum1");
            um.addSubForum("forum1", "subforum1", "superAdmin");
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
        }

        [TestMethod]
        public void UnitTest_unassignModerator()
        {
            // start conditions:
            // there is a forum
            // that forrum has a sub forum
            // that sub forum has a moderator
            um.addForum("forum1");
            um.addSubForum("forum1", "subforum1", "superAdmin");
            
            // invalid input

        }
    }
}
