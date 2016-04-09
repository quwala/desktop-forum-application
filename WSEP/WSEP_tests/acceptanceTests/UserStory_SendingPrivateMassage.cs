using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.forumManagementService;
using WSEP_tests.adapter;

namespace WSEP_tests.acceptanceTests
{
    [TestClass]
    public class UserStory_SendingPrivateMassage
    {
        //private IForumSystem fs;
        IAdapter adapter;

        [TestInitialize()]
        public void Initialize()
        {
            adapter = new Adapter();
            adapter.addForum("forumName");
            adapter.addForum("forumName2");
            adapter.registerToForum("forumName", "UserName", "UserPassword1", "abc@gmail.com");
            adapter.registerToForum("forumName", "UserName2", "UserPassword1", "avc@gmail.com");
            adapter.registerToForum("forumName2", "UserName3", "UserPassword1", "aaac@gmail.com");

        }

        [TestMethod]
        public void Test_SendingPrivateMassage_GoodInput() //9.1
        {
            Assert.IsTrue(adapter.sendingPrivateMassage("forumName", "UserName", "UserName2", "Very important content")); //TID 66         //sending from user to user2 
            Assert.IsFalse(adapter.sendingPrivateMassage("forumName", "UserName", "UserName3", "Very important content"));   //TID 67       //sending from user to user3- user 3 is not member in forum 

        }

        [TestMethod]
        public void Test_SendingPrivateMassage_BadInput() //9.2
        {
            Assert.IsFalse(adapter.sendingPrivateMassage("forumName", "", "UserName2", "Very important content"));    //TID 68     //bad inputs
            Assert.IsFalse(adapter.sendingPrivateMassage("", "UserName", "UserName2", "Very important content")); //TID 69
            Assert.IsFalse(adapter.sendingPrivateMassage("forumName", " UserName", " UserName2", "Very important content"));  //TID 70
            Assert.IsFalse(adapter.sendingPrivateMassage(" forumName", "UserName", "UserName2", "Very important content"));  //TID 71
            Assert.IsFalse(adapter.sendingPrivateMassage("forumName", "UserName", "", "Very important content"));  //TID 72
            Assert.IsFalse(adapter.sendingPrivateMassage("forumName", "UserName", "UserName2", ""));  //TID 73


        }

        [TestMethod]
        public void Test_SendingPrivateMassage_CatastophicInput() //9.3
        {
            Assert.IsFalse(adapter.sendingPrivateMassage(null, "UserName", "UserName3", "Very important content"));   //TID 74       //NULL everyWhere
            Assert.IsFalse(adapter.sendingPrivateMassage("forumName", null, "UserName3", "Very important content"));   //TID 75       
            Assert.IsFalse(adapter.sendingPrivateMassage("forumName", "UserName", "UserName3", null));         //TID 76
            Assert.IsFalse(adapter.sendingPrivateMassage("forumName", "UserName", null, "Very important content"));   //TID 77       

        }
    }
}
