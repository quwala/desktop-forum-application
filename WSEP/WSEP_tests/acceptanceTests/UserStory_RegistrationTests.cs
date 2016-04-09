using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.forumManagementService;
using WSEP_tests.adapter;

namespace WSEP_tests.acceptanceTests
{
    [TestClass]
    public class UserStory_RegistrationTests
    {
       
        private IAdapter adapter;

        [TestInitialize()]
        public void Initialize()
        {
            //fs = new ForumSystem("superAdmin", new WSEP.userManagement.UserManager());
            //fs.addForum("forumName");                //adding the new forum..      
            //fs.addForum("forumName2");                //adding the new forum..      
            adapter = new Adapter();
            adapter.addForum("forumName");
            adapter.addForum("forumName2");
        }

        [TestMethod] // test 4.1
        public void Test_Registration_GoodInput()                   //Register gets an email to.. fix it..
        {
            Assert.IsTrue(adapter.registerToForum("forumName", "UserName", "UserPassword1", "galMor1007@gmail.com")); //TID 25           //register to forum new user
            Assert.IsFalse(adapter.registerToForum("forumName", "UserName", "UserPassword1", "galMor@gmail.com"));      //TID 26         //cant have the same username in the forum
            Assert.IsFalse(adapter.registerToForum("forumName", "UserName2", "UserPassword2", "galMor1007@gmail.com")); //TID 27         //cant have the same mail in the forum
            Assert.IsTrue(adapter.registerToForum("forumName2", "UserName", "UserPassword1", "galMor1007@gmail.com"));  //TID 28         //same mail should work in different forums


        }

        [TestMethod] // test 4.2
        public void Test_Registration_BadInput()
        {
            Assert.IsFalse(adapter.registerToForum("forumName", "UserName2", "UserPassword2", "galMor1007&gmail.com")); //TID 29          //test on invalid Mail addresses
            Assert.IsFalse(adapter.registerToForum("forumName", "UserName2", "UserPassword2", "galMor1007@gmail")); //TID 30         //test on invalid Mail addresses
            Assert.IsFalse(adapter.registerToForum("forumName", "", "UserPassword1", "galMor1007@gmail.com"));          //TID 31         //empty
            Assert.IsFalse(adapter.registerToForum("forumName", " UserName", "UserPassword1", "galMor1007@gmail.com"));   //TID 32       //with space
            //Assert.IsFalse(adapter.registerToForum("forumName", "UserName", "simplepassword", "galMor1007@gmail.com"));  //TID 33       //too simple password
            //Assert.IsFalse(adapter.registerToForum("forumName", "UserName", "ONLYCAPITAL", "galMor1007@gmail.com"));   //TID 34          //only capital letters
            Assert.IsFalse(adapter.registerToForum("forumName", "UserName", "Pass With5pace", "galMor1007@gmail.com")); //TID 35         //password contain space
            Assert.IsFalse(adapter.registerToForum("forumName", "UserName\n", "Pass4", "galMor1007@gmail.com")); //TID 36                //try with newlines in the strings
            Assert.IsFalse(adapter.registerToForum("forumName", "UserName", "Pass4\n", "galMor1007@gmail.com"));       //TID 37   
            Assert.IsFalse(adapter.registerToForum("forumName", "UserName\nName", "Pass", "galMor1007@gmail.com"));//TID 38
            Assert.IsFalse(adapter.registerToForum("forumName", "UserName", "UserPassword1", "")); //TID 39
        }

        [TestMethod] // test 4.3
        public void Test_Registration_CatastophicInput()
        {

            Assert.IsFalse(adapter.registerToForum("forumName", null, "Password1", ""));    //TID 40             //null everywhere  
            Assert.IsFalse(adapter.registerToForum(null, " UserName", "Password1", ""));     //TID 41    
            Assert.IsFalse(adapter.registerToForum("forumName", "UserName", null, null));    //TID 42     
            Assert.IsFalse(adapter.registerToForum(null, null, null, null));         //TID 43


        }
    }
}
