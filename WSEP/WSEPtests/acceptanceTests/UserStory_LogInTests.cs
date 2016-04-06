using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_LogInTests
    {
        private IForumSystem fs;

        [TestInitialize()]
        public void Initialize()
        {
            fs = new ForumSystem("superAdmin");
            //fs.addForum("forumName");                //adding the new forum..      
            //fs.RegisterToFurom("forumName","UserName","UserPassword");   //register new user for this forum
            
        }

        [TestMethod]
        public void Test_LogIn_GoodInput()
        {
            //Assert.isTrue(fs.ForumLogIn("forumName","UserName","UserPassword"));                  //login with good valid input 
            //Assert.isFalse(fs.ForumLogIn("forumName","UserName","badPass"));                      //try to login with good invalid pass
            //Assert.isFalse(fs.ForumLogIn("invalidForumName","UserName","UserPassword"));          //try to login with good invalid furomname
            //Assert.isFalse(fs.ForumLogIn("forumName","invalidUserName","UserPassword"));           //try to login with good invalid username

        }

        [TestMethod]
        public void Test_LogIn_BadInput()
        {
            //Assert.isFalse(fs.ForumLogIn("","UserName","UserPassword"));                          //try to login with empty forumname
            //Assert.isFalse(fs.ForumLogIn("forumName","","UserPassword"));                         //try to login with empty username
            // Assert.isFalse(fs.ForumLogIn("forumName,"UserName",""));                             //try to login with empty pass

        }

        [TestMethod]
        public void Test_LogIn_CatastophicInput()
        {
            //Assert.isFalse(fs.ForumLogIn(NULL,"UserName","UserPassword"));                    //NULL everywhere...
            //Assert.isFalse(fs.ForumLogIn("forumName",NULL,"UserPassword"));                  
            // Assert.isFalse(fs.ForumLogIn("furomName","UserName",NULL));                                          
            // Assert.isFalse(fs.ForumLogIn(NULL,NULL,NULL));
            // Assert.isFalse(fs.ForumLogIn(,,));



        }
    }
}
