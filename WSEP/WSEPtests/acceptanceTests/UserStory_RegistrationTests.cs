using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_RegistrationTests
    {
        private IForumSystem fs;

        [TestInitialize()]
        public void Initialize()
        {
            fs = new ForumSystem("superAdmin");
            //fs.addForum("forumName");                //adding the new forum..      
           

        }

        [TestMethod]
        public void Test_Registration_GoodInput()
        {
            //Assert.isTrue(fs.RegisterToFurom("forumName","UserName","UserPassword1");             //register to forum new user
            //Assert.isFalse(fs.RegisterToFuromr("forumName","UserName","UserPassword1"));          //cant have the same username in the forum
            //Assert.isFalse(fs.RegisterToFurom("invalidForumName","UserName2","UserPassword2"));       //invalid furom name

        }

        [TestMethod]
        public void Test_Registration_BadInput()
        {

            //Assert.isFalse(fs.RegisterToFurom("forumName","","UserPassword1");                   //empty
            //Assert.isFalse(fs.RegisterToFurom("forumName"," UserName","UserPassword1");         //with space
            //Assert.isFalse(fs.RegisterToFurom("forumName","UserName","simplepassword");         //to simple password
            //Assert.isFalse(fs.RegisterToFurom("forumName","UserName","ONLYCAPITAL"));             //only capital letters
            //Assert.isFalse(fs.RegisterToFurom("forumName","UserName","Pass With5pace"));          //password contain space
            //Assert.isFalse(fs.RegisterToFurom("forumName","UserName\n","Pass4"));          //try with newlines in the strings
            //Assert.isFalse(fs.RegisterToFurom("forumName","UserName","Pass4\n"));          
            //Assert.isFalse(fs.RegisterToFurom("forumName","UserName\nName","Pass"));

        }

        [TestMethod]
        public void Test_Registration_CatastophicInput()
        {
            //Assert.isFalse(fs.RegisterToFurom("forumName",NULL,"Password1");                 //NULL everywhere  
            //Assert.isFalse(fs.RegisterToFurom(NULL," UserName","Password1");         
            //Assert.isFalse(fs.RegisterToFurom("forumName","UserName",NULL);         
            //Assert.isFalse(fs.RegisterToFurom(NULL,NULL,NULL);         


        }
    }
}
