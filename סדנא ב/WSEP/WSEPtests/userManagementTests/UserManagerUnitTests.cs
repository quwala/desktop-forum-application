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
            Assert.IsTrue(um.addForum("forum1"));
            Assert.IsFalse(um.addForum("forum1"));
            Assert.IsTrue(um.addForum("forum2"));
            Assert.IsFalse(um.addForum(""));
            Assert.IsFalse(um.addForum(null));
        }

        [TestMethod]
        public void Test_addSubForum()
        {
            um.addForum("forum1");
            Assert.IsTrue(um.addSubForum("forum1", "subforum1", "superAdmin"));
            Assert.IsTrue(um.addSubForum("forum1", "subforum2", "superAdmin"));
            // sub forum exists in forum
            Assert.IsFalse(um.addSubForum("forum1", "subforum1", "superAdmin"));
            // forum does not exist
            Assert.IsFalse(um.addSubForum("forum2", "subforum3", "superAdmin"));
            // admin does not exist
            Assert.IsFalse(um.addSubForum("forum1", "subforum4", "admin"));
            // empty forum name
            Assert.IsFalse(um.addSubForum("", "subforum4", "admin"));
            // empty sub forum name
            Assert.IsFalse(um.addSubForum("forum1", "", "admin"));
            // empty admin username
            Assert.IsFalse(um.addSubForum("forum1", "subforum4", ""));
            Assert.IsFalse(um.addSubForum(null, "subforum5", "superAdmin"));
            Assert.IsFalse(um.addSubForum("forum1", null, "superAdmin"));
            Assert.IsFalse(um.addSubForum("forum1", "subforum5", null));
        }

        [TestMethod]
        public void Test_registerMemberToForum()
        {
            um.addForum("forum1");
            Assert.IsTrue(um.registerMemberToForum("forum1", "user1", "pass1", "email1@gmail.com"));
            Assert.IsTrue(um.registerMemberToForum("forum1", "user2", "pass1", "email2@gmail.com"));
            // should be true - for now
            Assert.IsTrue(um.registerMemberToForum("forum1", "user3", "pass1", "email2@gmail.com"));
            // username exists in this forum
            Assert.IsFalse(um.registerMemberToForum("forum1", "user2", "pass1", "email4@gmail.com"));
            // invalid email - no "@"
            Assert.IsFalse(um.registerMemberToForum("forum1", "user4", "pass1", "email.com"));
            // invalid email - no "." after "@"
            Assert.IsFalse(um.registerMemberToForum("forum1", "user5", "pass1", "email@gmail"));
            // invalid email - "." right after "@"
            Assert.IsFalse(um.registerMemberToForum("forum1", "user5", "pass1", "email@.com"));
            // invalid email - "." as last character
            Assert.IsFalse(um.registerMemberToForum("forum1", "user5", "pass1", "email@gmail."));
            // invalid email - "@" as first character
            Assert.IsFalse(um.registerMemberToForum("forum1", "user5", "pass1", "@gmail."));
            // empty username
            Assert.IsFalse(um.registerMemberToForum("forum1", "", "pass1", "email1@gmail.com"));
            // empty password
            Assert.IsFalse(um.registerMemberToForum("forum1", "user6", "", "email1@gmail.com"));
            // empty email
            Assert.IsFalse(um.registerMemberToForum("forum1", "user6", "pass1", ""));
            // invalid username - begins with " "
            Assert.IsFalse(um.registerMemberToForum("forum1", " user7", "pass1", "email5@gmail.com"));
            // invalid password - contains " "
            Assert.IsFalse(um.registerMemberToForum("forum1", "user7", " pass1", "email5@gmail.com"));
            // invalid password - contains " "
            Assert.IsFalse(um.registerMemberToForum("forum1", "user7", "pas s1", "email5@gmail.com"));
            // invalid password - contains " "
            Assert.IsFalse(um.registerMemberToForum("forum1", "user7", "pass1 ", "email5@gmail.com"));
            Assert.IsFalse(um.registerMemberToForum(null, "user8", "pass2", "email6@gmail.com"));
            Assert.IsFalse(um.registerMemberToForum("forum1", null, "pass2", "email6@gmail.com"));
            Assert.IsFalse(um.registerMemberToForum("forum1", "user8", null, "email6@gmail.com"));
            Assert.IsFalse(um.registerMemberToForum("forum1", "user8", "pass2", null));
        }
    }
}
