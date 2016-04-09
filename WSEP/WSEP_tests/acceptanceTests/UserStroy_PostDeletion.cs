using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WSEP_tests.adapter;

namespace WSEP_tests.acceptanceTests
{
    [TestClass]
    public class UserStroy_PostDeletion
    {
        private IAdapter adapter;


        [TestInitialize()]
        public void Initialize()
        {

            adapter = new Adapter();
            adapter.addForum("forumName");                                               //adding the new forum..      
            adapter.registerToForum("forumName", "UserName", "UserPassword1", "ccc@gmail.com");             //register to forum new user
            adapter.registerToForum("forumName", "UserName2", "UserPassword2", "cdd@gmail.com");
            adapter.registerToForum("forumName", "UserName3", "UserPassword3", "cee@gmail.com");

            List<string> mods = new List<string>();
            mods.Add("UserName");
            adapter.addSubForum("forumName", "subForumName", mods);                 //open new subforum in the forum

        }


        [TestMethod]
        public void Test_PostDeletion_GoodInput() //12.1
        {
            string ID = adapter.createThreadAndGetID("forumName", "subForumName", "title", "content", "UserName");
            Assert.IsTrue(adapter.deletePost("forumName", "subForumName", ID)); //TID 82
        }

        [TestMethod]
        public void Test_PostDeletion_BadInput() //12.2
        {
            string ID = adapter.createThreadAndGetID("forumName", "subForumName", "title1", "content", "UserName");
            Assert.IsFalse(adapter.deletePost("forumName", "subForumName", "fff")); //TID 83
            Assert.IsFalse(adapter.deletePost("forumName", "subForumName", ""));//TID 84
        }

        [TestMethod]
        public void Test_PostDeletion_NullInput() //12.2
        {
            Assert.IsFalse(adapter.deletePost(null, "subForumName", "fff")); //TID 85
            Assert.IsFalse(adapter.deletePost("forumName", null, "fff"));//TID 86
            Assert.IsFalse(adapter.deletePost("forumName", "subForumName", null));//TID 87
        }
    }
}
