using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;
using WSEP.forumManagement.forumHandler;
using WSEP.forumManagement.threadsHandler;
using WSEP.userManagement;
using System.Collections.Generic;

namespace WSEPtests.forumManagementTests
{
    [TestClass]
    public class ForumSystemTests
    {
        private UserManager um;
        private ForumSystem fs;
        private Forum f;
        private SubForum sf;
        private List<string> mods;

        /*
        Initialize a user manager, a ForumSystem, a forum,
        and a sub forum that we can play with
        */
        [TestInitialize()]
        public void Initialize()
        {
            um = new UserManager();

            fs = new ForumSystem("superAdmin", um);
            fs.addForum("Test Forum");
            um.addForum("Test Forum");
            f = fs.getForum("Test Forum");

            um.registerMemberToForum("Test Forum", "Avi", "Avi", "Avi@hotmail.com");
            um.registerMemberToForum("Test Forum", "Shlomo", "Shlomo", "Shlomo@hotmail.com");

            mods = new List<string>();
            mods.Add("Avi");
            mods.Add("Shlomo");

            fs.addSubForum("Test Forum", "Test Sub Forum",mods);
            sf = fs.getForum("Test Forum").getSubForum("Test Sub Forum");
            
        }

        //test the getForum() method in ForumSystem
        [TestMethod]
        public void Test_getForum()
        {
            Assert.AreEqual("Test Forum",fs.getForum("Test Forum").getName());
        }

        //test the hasForum() method in ForumSystem
        [TestMethod]
        public void Test_hasForum()
        {
            Assert.IsTrue(fs.hasForum("Test Forum"));
        }


        //test the addForum() with accepted input
        [TestMethod]
        public void Test_addForum_StandardInput()
        {
            fs.addForum("test");
            Assert.IsTrue(fs.hasForum("test"));
        }

        //try giving the addForum() method a forum that already exists
        [TestMethod]
        [ExpectedException(typeof(Exception),
            "A Forum with that name already exists")]
        public void Test_addForum_ExistingForum()
        {
            fs.addForum("test");
            fs.addForum("test");
        }

        //try giving the addForum() method a forum with an illegal name
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException),
            "Name of the forum cannot be empty")]
        public void Test_addForum_EmptyName()
        {
            fs.addForum("");
        }

        //try giving the addForum() method a forum with an illegal name
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException),
            "Name of the forum contains illegal character")]
        public void Test_addForum_IllegalCharacter()
        {
            fs.addForum("roman&shmil");
        }

        //try giving the addForum() method a forum with an illegal name
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException),
            "Name of the forum cannot be null")]
        public void Test_addForum_NullName()
        {
            fs.addForum(null);
        }

            //try giving the addForum() method a forum with an illegal name
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException),
            "Name of the forum cannot begin with a space character")]
        public void Test_addForum_Space()
        {
            fs.addForum(" Sports");
        }


        //test the addSubForum() with accepted input
        [TestMethod]
        public void Test_addSubForum_StandardInput()
        {
            fs.addSubForum("Test Forum","test",mods);
            Assert.AreEqual("test",f.getSubForum("test").getName());
        }

        //test the addSubForum() with an already existing name
        [TestMethod]
        [ExpectedException(typeof(Exception),
           "A Sub Forum with that name already exists")]
        public void Test_addSubForum_ExistingSubForum()
        {
            fs.addSubForum("Test Forum","Test Sub Forum",mods);
        }

        //test the addSubForum() with non-existing forum
        [TestMethod]
        [ExpectedException(typeof(Exception),
           "Cannot add Sub Forum - Forum was not found")]
        public void Test_addSubForum_WrongForumName()
        {
            fs.addSubForum("Test Forum1", "Test Sub Forum", mods);
        }

        //test the setForumPolicy() with accepted standard input.
        [TestMethod]
        public void Test_setForumPolicy_StandardInput()
        {
            fs.setForumPolicy("Test Forum","Default Policy", 1, 10, 1, 10, "Test");
            Assert.AreEqual("Default Policy", f.getPolicy().Name);
        }

        //test the setForumPolicy() with 0 minadmins.
        [TestMethod]
        [ExpectedException(typeof(Exception),
          "Minimum number of admins cannot be smaller than 1")]
        public void Test_setForumPolicy_ZeroAdmins()
        {
            fs.setForumPolicy("Test Forum","Test Policy", 0, 0,0,0,"");
        }

        //test the setForumPolicy() with maxmods < minmods.
        [TestMethod]
        [ExpectedException(typeof(Exception),
          "Maximum number of moderators cannot be smaller than minimal number of moderators")]
        public void Test_setForumPolicy_MoreMaxModsThanMinMods()
        {
            fs.setForumPolicy("Test Forum", "Test Policy", 1, 10, 2, 1, "");
        }

        //test the setForumPolicy() with bad policy name.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException),
          "Name of the policy cannot be empty")]
        public void Test_setForumPolicy_EmptyPolicyName()
        {
            fs.setForumPolicy("Test Forum", "", 1, 10, 2, 10, "");
        }

        //test the setForumPolicy() with a policy that conflicts with
        //the current number of admins in the forum.
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_setForumPolicy_ConliftingMaxAdminNumber()
        {
            um.registerMemberToForum("Test Forum", "Adminos1", "Adminos1", "Adminos1@hotmail.com");
            um.registerMemberToForum("Test Forum", "Adminos2", "Adminos2", "Adminos2@hotmail.com");
            um.assignAdmin("Test Forum", "Adminos1", 10);
            um.assignAdmin("Test Forum", "Adminos2", 10);

            fs.setForumPolicy("Test Forum", "Test Policy", 1, 1, 2, 10, "");
        }

        //test the setForumPolicy() with a policy that conflicts with
        //the current number of admins in the forum.
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_setForumPolicy_ConliftingMinAdminNumber()
        {
            fs.setForumPolicy("Test Forum", "Test Policy", 2, 10, 2, 10, "");
        }

        //test the setForumPolicy() with a policy that conflicts with
        //the current number of moderators in the forum.
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_setForumPolicy_ConliftingMaxModNumber()
        {
            fs.setForumPolicy("Test Forum", "Test Policy", 1, 10, 1, 1, "");
        }

    }
}
