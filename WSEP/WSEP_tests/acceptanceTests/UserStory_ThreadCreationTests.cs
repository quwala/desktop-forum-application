using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.forumManagementService;
using System.Collections.Generic;
using WSEP_tests.adapter;

namespace WSEP_tests.acceptanceTests
{
    [TestClass]
    public class UserStory_ThreadCreationTests
    {
        //private IForumSystem fs;
        IAdapter adapter;

        [TestInitialize()]
        public void Initialize()
        {
            adapter = new Adapter();
            adapter.addForum("forumName");                                               //adding the new forum..      
            adapter.registerToForum("forumName", "UserName", "UserPassword1", "ccc@gmail.com");             //register to forum new user
            List<string> mods = new List<string>();
            mods.Add("UserName");
            adapter.addSubForum("forumName", "subForumName", mods);                 //open new subforum in the forum

        }

        [TestMethod]
        public void Test_ThreadCreation_GoodInput() //6.1
        {

            Assert.IsTrue(adapter.createThread("forumName", "subForumName", "thread title", "someContent", "UserName")); //TID 55          //creation of new thread in subforum

            Assert.IsFalse(adapter.createThread("invalidForumName", "subForumName", "title", "someContent", "UserName"));  //TID 56        //good input bad validation       
            Assert.IsFalse(adapter.createThread("forumName", "invalidSubForumName", "title", "someContent", "UserName"));  //TID 57        //good input bad validation       


        }

        [TestMethod]
        public void Test_ThreadCreation_BadInput() //6.2
        {

            Assert.IsFalse(adapter.createThread("", "SubForumName", "title", "someContent", "UserName"));   //TID 58            //bad input- empty, spaces..       
            Assert.IsFalse(adapter.createThread("forumName", "", "title", "someContent", "UserName"));     //TID 59             //bad input- empty, spaces..       
            Assert.IsFalse(adapter.createThread("forumName", "SubForumName", "", "", "UserName"));        //TID 60             //bad input- empty, spaces..       
            Assert.IsFalse(adapter.createThread("forumName", "SubForumName", "title", "someContent", "UserName1"));     //TID 61           
        }

        [TestMethod]
        public void Test_ThreadCreation_CatastophicInput() //6.3
        {
            Assert.IsFalse(adapter.createThread(null, "subForumName", "title", "someContent", "UserName"));    //TID 62         //NUll everywhereee  
            Assert.IsFalse(adapter.createThread("forumName", null, "title", "someContent", "UserName"));    //TID 63                 
            Assert.IsFalse(adapter.createThread("forumName", "subForumName", null, "someContent", "UserName"));     //TID 64        
            Assert.IsFalse(adapter.createThread("forumName", "subForumName", null, null, "UserName"));        //TID 65                 

        }
    }
}
