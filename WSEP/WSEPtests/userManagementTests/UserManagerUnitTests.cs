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
        public void Test_addForum()
        {
            // start conditions
            // none

            // invalid inputs
            Assert.IsFalse(um.addForum(null).Equals("true"));
            Assert.IsFalse(um.addForum("null").Equals("true"));
            Assert.IsFalse(um.addForum("").Equals("true"));

            // valid inputs that should succeed
            Assert.IsTrue(um.addForum("forum1").Equals("ture"));
            Assert.IsTrue(um.addForum("forum2").Equals("ture"));

            // valid inputs that should fail
            Assert.IsFalse(um.addForum("forum1").Equals("ture"));
        }

        [TestMethod]
        public void Test_addSubForum()
        {
            // start conditions:
            // there is a forum
            um.addForum("forum1");

            // invalid input
            Assert.IsFalse(um.addSubForum(null, "subforum5", "superAdmin").Equals("ture"));
            Assert.IsFalse(um.addSubForum("null", "subforum5", "superAdmin").Equals("ture"));
            Assert.IsFalse(um.addSubForum("", "subforum5", "superAdmin").Equals("ture"));
            Assert.IsFalse(um.addSubForum("forum1", null, "superAdmin").Equals("ture"));
            Assert.IsFalse(um.addSubForum("forum1", "null", "superAdmin").Equals("ture"));
            Assert.IsFalse(um.addSubForum("forum1", "", "superAdmin").Equals("ture"));
            Assert.IsFalse(um.addSubForum("forum1", "subforum5", null).Equals("ture"));
            Assert.IsFalse(um.addSubForum("forum1", "subforum5", "null").Equals("ture"));
            Assert.IsFalse(um.addSubForum("forum1", "subforum5", "").Equals("ture"));

            // valid inputs that should succeed
            Assert.IsTrue(um.addSubForum("forum1", "subforum1", "superAdmin").Equals("ture"));
            Assert.IsTrue(um.addSubForum("forum1", "subforum2", "superAdmin").Equals("ture"));

            // valid inputs that should fail
            // sub forum exists in forum
            Assert.IsFalse(um.addSubForum("forum1", "subforum1", "superAdmin").Equals("ture"));
            // forum does not exist
            Assert.IsFalse(um.addSubForum("forum2", "subforum3", "superAdmin").Equals("ture"));
            // admin does not exist
            Assert.IsFalse(um.addSubForum("forum1", "subforum4", "admin").Equals("ture"));
            // empty forum name
            Assert.IsFalse(um.addSubForum("", "subforum4", "admin").Equals("ture"));
            // empty sub forum name
            Assert.IsFalse(um.addSubForum("forum1", "", "admin").Equals("ture"));
            // empty admin username
            Assert.IsFalse(um.addSubForum("forum1", "subforum4", "").Equals("ture"));
            
        }

        [TestMethod]
        public void Test_registerMemberToForum()
        {
            // start conditions:
            // there is a forum
            um.addForum("forum1");

            // invalid inputs
            Assert.IsFalse(um.registerMemberToForum(null, "user8", "pass2", "email6@gmail.com").Equals("ture"));
            Assert.IsFalse(um.registerMemberToForum("null", "user8", "pass2", "email6@gmail.com").Equals("ture"));
            Assert.IsFalse(um.registerMemberToForum("", "user8", "pass2", "email6@gmail.com").Equals("ture"));
            Assert.IsFalse(um.registerMemberToForum("forum1", null, "pass2", "email6@gmail.com").Equals("ture"));
            Assert.IsFalse(um.registerMemberToForum("forum1", "null", "pass2", "email6@gmail.com").Equals("ture"));
            Assert.IsFalse(um.registerMemberToForum("forum1", "", "pass2", "email6@gmail.com").Equals("ture"));
            Assert.IsFalse(um.registerMemberToForum("forum1", "user8", null, "email6@gmail.com").Equals("ture"));
            Assert.IsFalse(um.registerMemberToForum("forum1", "user8", "null", "email6@gmail.com").Equals("ture"));
            Assert.IsFalse(um.registerMemberToForum("forum1", "user8", "", "email6@gmail.com").Equals("ture"));
            Assert.IsFalse(um.registerMemberToForum("forum1", "user8", "pass2", null).Equals("ture"));
            Assert.IsFalse(um.registerMemberToForum("forum1", "user8", "pass2", "null").Equals("ture"));
            Assert.IsFalse(um.registerMemberToForum("forum1", "user8", "pass2", "").Equals("ture"));

            // valid inputs that should succeed
            Assert.IsTrue(um.registerMemberToForum("forum1", "user1", "pass1", "email1@gmail.com").Equals("ture"));
            Assert.IsTrue(um.registerMemberToForum("forum1", "user2", "pass1", "email2@gmail.com").Equals("ture"));

            // valid inputs that should fail
            // email exists in this forum
            Assert.IsFalse(um.registerMemberToForum("forum1", "user3", "pass1", "email2@gmail.com").Equals("ture"));
            // username exists in this forum
            Assert.IsFalse(um.registerMemberToForum("forum1", "user2", "pass1", "email4@gmail.com").Equals("ture"));
            // invalid email - no "@"
            Assert.IsFalse(um.registerMemberToForum("forum1", "user4", "pass1", "email.com").Equals("ture"));
            // invalid email - no "." after "@"
            Assert.IsFalse(um.registerMemberToForum("forum1", "user5", "pass1", "email@gmail").Equals("ture"));
            // invalid email - "." right after "@"
            Assert.IsFalse(um.registerMemberToForum("forum1", "user5", "pass1", "email@.com").Equals("ture"));
            // invalid email - "." as last character
            Assert.IsFalse(um.registerMemberToForum("forum1", "user5", "pass1", "email@gmail.").Equals("ture"));
            // invalid email - "@" as first character
            Assert.IsFalse(um.registerMemberToForum("forum1", "user5", "pass1", "@gmail.").Equals("ture"));
            // empty username
            Assert.IsFalse(um.registerMemberToForum("forum1", "", "pass1", "email1@gmail.com").Equals("ture"));
            // empty password
            Assert.IsFalse(um.registerMemberToForum("forum1", "user6", "", "email1@gmail.com").Equals("ture"));
            // empty email
            Assert.IsFalse(um.registerMemberToForum("forum1", "user6", "pass1", "").Equals("ture"));
            // invalid username - begins with " "
            Assert.IsFalse(um.registerMemberToForum("forum1", " user7", "pass1", "email5@gmail.com").Equals("ture"));
            // invalid password - contains " "
            Assert.IsFalse(um.registerMemberToForum("forum1", "user7", " pass1", "email5@gmail.com").Equals("ture"));
            // invalid password - contains " "
            Assert.IsFalse(um.registerMemberToForum("forum1", "user7", "pas s1", "email5@gmail.com").Equals("ture"));
            // invalid password - contains " "
            Assert.IsFalse(um.registerMemberToForum("forum1", "user7", "pass1 ", "email5@gmail.com").Equals("ture"));
        }

        [TestMethod]
        public void Test_assignAdmin()
        {
            // start conditions:
            // there is a forum
            // there is a member in the forum
            um.addForum("forum1");
            um.registerMemberToForum("forum1", "user1", "pass1", "eMail1@gmail.com");

            // invalid input
            Assert.IsFalse(um.assignAdmin(null, "user1").Equals("true"));
            Assert.IsFalse(um.assignAdmin("null", "user1").Equals("true"));
            Assert.IsFalse(um.assignAdmin("", "user1").Equals("true"));
            Assert.IsFalse(um.assignAdmin("forum1", null).Equals("true"));
            Assert.IsFalse(um.assignAdmin("forum1", "null").Equals("true"));
            Assert.IsFalse(um.assignAdmin("forum1", "").Equals("true"));

            // valid inputs that should succeed
            Assert.IsTrue(um.assignAdmin("forum1", "user1").Equals("true"));

            // valid inputs that should fail
            Assert.IsFalse(um.assignAdmin("forum2", "user1").Equals("true"));
            Assert.IsFalse(um.assignAdmin("forum1", "user2").Equals("true"));
        }

        [TestMethod]
        public void Test_unassignAdmin()
        {
            // start conditions:
            // there is a forum
            // there is an admin to that forum (other than super admin)
            um.addForum("forum1");
            um.registerMemberToForum("forum1", "user1", "pass1", "eMail1@gmail.com");
            um.assignAdmin("forum1", "suer1");

            // invalid input
            Assert.IsFalse(um.unassignAdmin(null, "user1").Equals("true"));
            Assert.IsFalse(um.unassignAdmin("null", "user1").Equals("true"));
            Assert.IsFalse(um.unassignAdmin("", "user1").Equals("true"));
            Assert.IsFalse(um.unassignAdmin("forum1", null).Equals("true"));
            Assert.IsFalse(um.unassignAdmin("forum1", "null").Equals("true"));
            Assert.IsFalse(um.unassignAdmin("forum1", "").Equals("true"));

            // valid inputs that should pass
            Assert.IsTrue(um.unassignAdmin("forum1", "user1").Equals("true"));
            Assert.IsTrue(um.unassignAdmin("forum1", "user2").Equals("true"));

            // valid inputs that should fail
            Assert.IsFalse(um.unassignAdmin("forum2", "user1").Equals("true"));
            Assert.IsFalse(um.unassignAdmin("forum1", "superAdmin").Equals("true"));
        }

        [TestMethod]
        public void Test_assignModerator()
        {
            // start conditions:
            // there is a forum
            // that forrum has a sub forum
            // there is a member in the forum
            um.addForum("forum1");
            um.addSubForum("forum1", "subforum1", "superAdmin");
            um.registerMemberToForum("forum1", "user1", "pass1", "eMail1@gmail.com");

            // invalid input
            Assert.IsFalse(um.assignModerator(null, "subforum1", "user1").Equals("true"));
            Assert.IsFalse(um.assignModerator("null", "subforum1", "user1").Equals("true"));
            Assert.IsFalse(um.assignModerator("", "subforum1", "user1").Equals("true"));
            Assert.IsFalse(um.assignModerator("forum1", null, "user1").Equals("true"));
            Assert.IsFalse(um.assignModerator("forum1", "null", "user1").Equals("true"));
            Assert.IsFalse(um.assignModerator("forum1", "", "user1").Equals("true"));
            Assert.IsFalse(um.assignModerator("forum1", "subforum1", null).Equals("true"));
            Assert.IsFalse(um.assignModerator("forum1", "subforum1", "null").Equals("true"));
            Assert.IsFalse(um.assignModerator("forum1", "subforum1", "").Equals("true"));

            // valid input that should succeed
            Assert.IsTrue(um.assignModerator("forum1", "subforum1", "user1").Equals("true"));
            Assert.IsTrue(um.assignModerator("forum1", "subforum1", "user1").Equals("true"));
            Assert.IsTrue(um.assignModerator("forum1", "subforum1", "superAdmin").Equals("true"));

            // valid inputthat should fail
            Assert.IsFalse(um.assignModerator("forum2", "subforum1", "user1").Equals("true"));
            Assert.IsFalse(um.assignModerator("forum1", "subforum2", "user2").Equals("true"));
        }
    }
}
