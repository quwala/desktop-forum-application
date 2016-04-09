using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.forumManagementService;
using WSEP_tests.adapter;

namespace WSEP_tests.acceptanceTests
{
    [TestClass]
    public class UserStory_LogInTests
    {
      
        private IAdapter adapter;

        [TestInitialize()]
        public void Initialize()
        {
            
            adapter = new Adapter();
            adapter.addForum("forumName");
            adapter.registerToForum("forumName", "UserName", "UserPassword", "aaa@gmail.com");
        }

        [TestMethod] // test 3.1
        public void Test_LogIn_GoodInput()
        {
            Assert.IsTrue(adapter.forumLogIn("forumName", "UserName", "UserPassword"));     //TID 14             //login with good valid input 
            Assert.IsFalse(adapter.forumLogIn("forumName", "UserName", "badPass"));   //TID 15                   //try to login with good invalid pass
            Assert.IsFalse(adapter.forumLogIn("invalidForumName", "UserName", "UserPassword"));    //TID 16      //try to login with good invalid furomname
            Assert.IsFalse(adapter.forumLogIn("forumName", "invalidUserName", "UserPassword"));//TID 17           //try to login with good invalid username

        }

        [TestMethod] // test 3.2
        public void Test_LogIn_BadInput()
        {
            Assert.IsFalse(adapter.forumLogIn("", "UserName", "UserPassword"));  //TID 18                       //try to login with empty forumname
            Assert.IsFalse(adapter.forumLogIn("forumName", "", "UserPassword"));   //TID 19                      //try to login with empty username
            Assert.IsFalse(adapter.forumLogIn("forumName", "UserName", ""));     //TID 20                        //try to login with empty pass

        }

        [TestMethod] // test 3.3
        public void Test_LogIn_CatastophicInput()
        {
            Assert.IsFalse(adapter.forumLogIn(null, "UserName", "UserPassword"));   //TID 21                 //NULL everywhere...
            Assert.IsFalse(adapter.forumLogIn("forumName", null, "UserPassword"));//TID 22
            Assert.IsFalse(adapter.forumLogIn("furomName", "UserName", null));//TID 23
            Assert.IsFalse(adapter.forumLogIn(null, null, null));//TID 24




        }
    }
}
