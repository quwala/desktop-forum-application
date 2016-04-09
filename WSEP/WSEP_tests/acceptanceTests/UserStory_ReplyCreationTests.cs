using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.forumManagementService;
using System.Collections.Generic;
using WSEP_tests.adapter;

namespace WSEP_tests.acceptanceTests
{
    [TestClass]
    public class UserStory_ReplyCreationTests
    {
        //private IForumSystem fs;
        IAdapter adapter;
        string threadID;

        [TestInitialize()]
        public void Initialize()
        {

            adapter = new Adapter();
            adapter.addForum("forumName");                                               //adding the new forum..      
            adapter.registerToForum("forumName", "UserName", "UserPassword1", "aaa@gmail.com");             //register to forum new user
            List<string> mods = new List<string>();
            mods.Add("UserName");
            adapter.addSubForum("forumName", "subForumName", mods);                 //open new subforum in the forum
            threadID = adapter.createThreadAndGetID("forumName", "subForumName", "thread title", "someContent", "UserName");
            


        }

        [TestMethod]
        public void Test_ReplyCreation_GoodInput() //7.1
        {
            Assert.IsTrue(adapter.createReply("forumName", "subForumName", "title", "cont", "UserName", threadID));  //TID 74                 //creation of new reply in Thread number 1
            Assert.IsFalse(adapter.createReply("invalidForumName", "subForumName", "title", "someContent", "UserName", threadID));  //TID 75        //good input bad validation       


        }

        [TestMethod]
        public void Test_ReplyCreation_BadInput()   //7.2           
        {
            Assert.IsFalse(adapter.createReply("invalidForumName", "subForumName", "", "", "UserName", threadID));  //TID 76 //try to create of new reply without content         
            Assert.IsFalse(adapter.createReply("", "subForumName", "sad", "dfsf", "UserName", threadID));//TID 77
            Assert.IsFalse(adapter.createReply("forumName", "", "sss", "fff", "UserName", threadID));//TID 78


        }

        [TestMethod]
        public void Test_ReplyCreation_CatastophicInput() //7.3           
        {
            Assert.IsFalse(adapter.createReply(null, "subForumName", "fff", "fff", "UserName", threadID));             //NUll everywhereee  
            Assert.IsFalse(adapter.createReply("forumName", null, "fff", "fff", "UserName", threadID));
            Assert.IsFalse(adapter.createReply("forumName", "subForumName", "fff", "fff", null, threadID));
        }
    }
}
