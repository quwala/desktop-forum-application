﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.userManagement;

namespace WSEPtests.userManagementTests
{
    [TestClass]
    public class UserManagerUnitTests
    {

        /// <summary>
        //fffggg
        /// </summary>

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
        }
    }
}