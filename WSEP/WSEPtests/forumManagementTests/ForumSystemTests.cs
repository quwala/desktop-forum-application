using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.forumManagementService;
using WSEP_service.userManagementService;
using System.Collections.Generic;
using WSEP_domain.forumManagementDomain.forumHandler;
using WSEP_doamin.forumManagementDomain;

namespace WSEP_tests.forumManagementTests
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


        //test the createThread() with accepted standard input.
        [TestMethod]
        public void Test_createThread_StandardInput()
        {
            fs.createThread("Test Forum", "Test Sub Forum", "Test Thread", "content", "Avi");
            Assert.AreNotEqual(0,sf.getThreadIDS().Count);
            Assert.IsTrue(fs.getLogger().getLog().Contains(
               "Successfully created Thread Test Thread in Sub Forum Test Sub Forum"));
        }

        //test the createThread() with empty content and title.
        [TestMethod]
        [ExpectedException(typeof(Exception),
           "Post could not be created; title and content both are empty")]
        public void Test_createThread_NoContentNoTitle()
        {
            fs.createThread("Test Forum", "Test Sub Forum", "", "", "Avi");
        }

        //test the createThread() with non existant Sub Forum.
        [TestMethod]
        [ExpectedException(typeof(Exception),
          "Cannot create Thread - Forum was not found")]
        public void Test_createThread_NonExistingSubForum()
        {
            fs.createThread("Test Forum", "dummy", "title", "", "Avi");
        }

        //test the createReply() with accepted standard input.
        [TestMethod]
        public void Test_createReplyStandardInput()
        {
            string postId=fs.createThread("Test Forum", "Test Sub Forum", "Test Thread", "content", "Avi");
            fs.createReply("Test Forum", "Test Sub Forum", "title", "content", "Shlomo", postId);
            Assert.IsNotNull(sf.getPostById(postId));
            Assert.IsTrue(fs.getLogger().getLog().Contains(
"Successfully created Reply title in Sub Forum Test Sub Forum"));
        }

        //test the createReply() with non existing post to reply to
        [TestMethod]
        [ExpectedException(typeof(Exception),
           "Cannot create Reply - original Post was not found")]
        public void Test_createReply_NoPostToReplyTo()
        {
            fs.createReply("Test Forum", "Test Sub Forum", "title", "content", "Shlomo", "made_up_id");
        }

        //test the getThreadIDSFromSubForum() with accepted standard input.
        [TestMethod]
        public void Test_getThreadIDSFromSubForum_StandardInput()
        {
            string postId1 = fs.createThread("Test Forum", "Test Sub Forum", "Test Thread1", "content", "Avi");
            string postId2 = fs.createThread("Test Forum", "Test Sub Forum", "Test Thread2", "content", "Avi");
            string postId3 = fs.createThread("Test Forum", "Test Sub Forum", "Test Thread3", "content", "Shlomo");

            string postId4 = fs.createReply("Test Forum", "Test Sub Forum", "title", "content", "Shlomo", postId1);
            Assert.IsNotNull(sf.getPostById(postId1));
            Assert.IsNotNull(sf.getPostById(postId2));
            Assert.IsNotNull(sf.getPostById(postId3));
            Assert.IsNotNull(sf.getPostById(postId4));

            List<string> l = new List<string>();
            bool res = fs.getThreadIDSFromSubForum("Test Forum", "Test Sub Forum").Contains(postId1)
                && fs.getThreadIDSFromSubForum("Test Forum", "Test Sub Forum").Contains(postId2)
                && fs.getThreadIDSFromSubForum("Test Forum", "Test Sub Forum").Contains(postId3)
                && !fs.getThreadIDSFromSubForum("Test Forum", "Test Sub Forum").Contains(postId4);

            Assert.IsTrue(res);
        }

        //test the deletePost() with accepted standard input.
        [TestMethod]
        public void Test_deletePost_StandardInput()
        {
            string postId1 = fs.createThread("Test Forum", "Test Sub Forum", "Test Thread1", "content", "Avi");
           

            string postId4 = fs.createReply("Test Forum", "Test Sub Forum", "title", "content", "Shlomo", postId1);

            Assert.IsTrue(fs.deletePost("Test Forum", "Test Sub Forum", postId4));
            Assert.IsNull(sf.getPostById(postId4));
            Assert.IsNotNull(sf.getPostById(postId1));
        }

        //test the deletePost() with non existing post.
        [TestMethod]
        [ExpectedException(typeof(Exception),
           "Cannot delete post - no such post was found")]
        public void Test_deletePost_NoPost()
        {
            fs.deletePost("Test Forum", "Test SubForum", "made_up_post");    
        }

        //test the deletePost() with a thread deletion - all replies should be erased.
        [TestMethod]
        public void Test_deletePost_DeleteEnitreThread()
        {
            string postId1 = fs.createThread("Test Forum", "Test Sub Forum", "Test Thread1", "content", "Avi");
            string postId2 = fs.createThread("Test Forum", "Test Sub Forum", "Test Thread2", "content", "Shlomo");
            string postId3 = fs.createThread("Test Forum", "Test Sub Forum", "Test Thread3", "content", "Avi");


            string postId4 = fs.createReply("Test Forum", "Test Sub Forum", "title", "content", "Shlomo", postId1);
            string postId5 = fs.createReply("Test Forum", "Test Sub Forum", "title2", "content", "Shlomo", postId4);
            string postId6 = fs.createReply("Test Forum", "Test Sub Forum", "title3", "content", "Shlomo", postId4);
            string postId7 = fs.createReply("Test Forum", "Test Sub Forum", "title3", "content", "Avi", postId4);
            string postId8 = fs.createReply("Test Forum", "Test Sub Forum", "title3", "content", "Avi", postId1);
            string postId9 = fs.createReply("Test Forum", "Test Sub Forum", "title3", "content", "Avi", postId6);

            Assert.IsTrue(fs.deletePost("Test Forum", "Test Sub Forum", postId1));
            Assert.IsNull(sf.getPostById(postId1));
            Assert.IsNotNull(sf.getPostById(postId2));
            Assert.IsNotNull(sf.getPostById(postId3));
            Assert.IsNull(sf.getPostById(postId4));
            Assert.IsNull(sf.getPostById(postId5));
            Assert.IsNull(sf.getPostById(postId6));
            Assert.IsNull(sf.getPostById(postId7));
            Assert.IsNull(sf.getPostById(postId8));
            Assert.IsNull(sf.getPostById(postId9));

        }






    }
}
