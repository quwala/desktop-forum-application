using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.forumManagementService;

namespace WSEP_tests.acceptanceTests
{
    [TestClass]
    public class UserStory_ThreadCreationTests
    {
        private IForumSystem fs;

        [TestInitialize()]
        public void Initialize()
        {
            fs = new ForumSystem("superAdmin", new WSEP_service.userManagementService.UserManager());
            //fs.addForum("forumName");                                               //adding the new forum..      
            //fs.RegisterToFurom("forumName","UserName","UserPassword");             //register to forum new user
            //fs.addSubForum("forumName","subForumName","UserName");                 //open new subforum in the forum

            //user should be loged in as a pre condition, not sure if to check it here or not..
            //
            //fs.ForumLogIn("forumName","UserName","UserPassword"))
            //if so, we need to add test to check validation with user loged in and out..
        }

        [TestMethod]
        public void Test_ThreadCreation_GoodInput()
        {
            //Assert.AreEqual(fs.NumOfThreads("forumName","subForumName"),0);                                   //check number of threads in the subforum
            //Assert.isTrue(fs.CreateThread("forumName","subForumName","thread title","someContent");           //creation of new thread in subforum
            //Assert.AreEqual(fs.NumOfThreads("forumName","subForumName"),1);                                   //check number of threads in the subforum
            //Assert.isFalse(fs.CreateThread("invalidForumName","subForumName","title","someContent");          //good input bad validation       
            //Assert.isFalse(fs.CreateThread("forumName","invalidSubForumName","title","someContent");          //good input bad validation       


        }

        [TestMethod]
        public void Test_ThreadCreation_BadInput()
        {
            //Assert.isFalse(fs.CreateThread("forumName","subForumName","","someContent");             //try to create of new thread without title  
            //Assert.isFalse(fs.CreateThread("","SubForumName","title","someContent");               //bad input- empty, spaces..       
            //Assert.isFalse(fs.CreateThread("forumName","","title","someContent");                  //bad input- empty, spaces..       
            //Assert.isFalse(fs.CreateThread("forumName","SubForumName","","");                     //bad input- empty, spaces..       

        }

        [TestMethod]
        public void Test_ThreadCreation_CatastophicInput()
        {
            //Assert.isFalse(fs.CreateThread(NULL,"subForumName","title","someContent");             //NUll everywhereee  
            //Assert.isFalse(fs.CreateThread("forumName",NULL,"title","someContent");                     
            //Assert.isFalse(fs.CreateThread("forumName","subForumName",NULL,"someContent");             
            //Assert.isFalse(fs.CreateThread("forumName","subForumName",NULL,NULL);                         
       
        }
    }
}
