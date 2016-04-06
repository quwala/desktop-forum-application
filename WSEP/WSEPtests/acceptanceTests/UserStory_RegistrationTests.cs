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
            //fs.addForum("forumName2");                //adding the new forum..      


        }

        [TestMethod]
        public void Test_Registration_GoodInput()                   //Register gets an email to.. fix it..
        {
            //Assert.isTrue(fs.RegisterToForum("forumName","UserName","UserPassword1","galMor1007@gmail.com"));            //register to forum new user
            //Assert.isFalse(fs.RegisterToForum("forumName","UserName","UserPassword1","galMor@gmail.com"));               //cant have the same username in the forum
            //Assert.isFalse(fs.RegisterToForum("forumName","UserName2","UserPassword2","galMor1007@gmail.com"));          //cant have the same mail in the forum
            //Assert.isTrue(fs.RegisterToForum("forumName2","UserName","UserPassword1","galMor1007@gmail.com"));           //same mail should work in different forums
            //Assert.isFalse(fs.RegisterToForum("invalidForumName","UserName2","UserPassword2","galMor1007@gmail.com"));   //invalid furom name

        }

        [TestMethod]
        public void Test_Registration_BadInput()
        {
            //Assert.isFalse(fs.RegisterToForum("forumName","UserName2","UserPassword2","galMor1007&gmail.com"));          //test on invalid Mail addresses
            //Assert.isFalse(fs.RegisterToForum("forumName","UserName2","UserPassword2","galMor1007@gmail.com"));          //test on invalid Mail addresses
            //Assert.isFalse(fs.RegisterToForum("forumName","UserName2","UserPassword2","galMor1007&gmail.com"));          //test on invalid Mail addresses            
            //Assert.isFalse(fs.RegisterToForum("forumName","","UserPassword1","galMor1007@gmail.com"));                   //empty
            //Assert.isFalse(fs.RegisterToForum("forumName"," UserName","UserPassword1","galMor1007@gmail.com"));          //with space
            //Assert.isFalse(fs.RegisterToForum("forumName","UserName","simplepassword","galMor1007@gmail.com"));          //to simple password
            //Assert.isFalse(fs.RegisterToForum("forumName","UserName","ONLYCAPITAL","galMor1007@gmail.com"));             //only capital letters
            //Assert.isFalse(fs.RegisterToForum("forumName","UserName","Pass With5pace","galMor1007@gmail.com"));          //password contain space
            //Assert.isFalse(fs.RegisterToForum("forumName","UserName\n","Pass4","galMor1007@gmail.com"));                 //try with newlines in the strings
            //Assert.isFalse(fs.RegisterToForum("forumName","UserName","Pass4\n","galMor1007@gmail.com"));          
            //Assert.isFalse(fs.RegisterToForum("forumName","UserName\nName","Pass","galMor1007@gmail.com"));

        }

        [TestMethod]
        public void Test_Registration_CatastophicInput()
        {
            //Assert.isFalse(fs.RegisterToForum("forumName","UserName","UserPassword1",""));
            //Assert.isFalse(fs.RegisterToForum("forumName",NULL,"Password1",""));                 //NULL everywhere  
            //Assert.isFalse(fs.RegisterToForum(NULL," UserName","Password1",""));         
            //Assert.isFalse(fs.RegisterToForum("forumName","UserName",NULL,NULL));         
            //Assert.isFalse(fs.RegisterToForum(NULL,NULL,NULL,NULL));         


        }
    }
}
