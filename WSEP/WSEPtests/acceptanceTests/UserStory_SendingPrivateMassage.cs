using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_SendingPrivateMassage
    {
        private IForumSystem fs;

        [TestInitialize()]
        public void Initialize()
        {
            fs = new ForumSystem("superAdmin", new WSEP.userManagement.UserManager());
            //fs.addForum("forumName");                                              //adding the new forum..      
            //fs.addForum("forumName2");                                              //adding the new forum..      
            //fs.SendingPrivateMassage("forumName","UserName","UserPassword1");             //register to forum new user1
            //fs.SendingPrivateMassage("forumName","UserName2","UserPassword2");             //register to forum another user2
            //fs.SendingPrivateMassage("forumName2","UserName3","UserPassword3");            //register to another forum a new user3

        }

        [TestMethod]
        public void Test_SendingPrivateMassage_GoodInput()
        {
            //Assert.isTrue(fs.SendingPrivateMassage("forumName","UserName","UserName2","Very important content"));          //sending from user to user2 
            //Assert.isFalse(fs.SendingPrivateMassage("forumName","UserName","UserName3","Very important content"));          //sending from user to user3- user 3 is not member in forum 

        }

        [TestMethod]
        public void Test_SendingPrivateMassage_BadInput()
        {
            //Assert.isFalse(fs.SendingPrivateMassage("forumName","","UserName2","Very important content"));          //bad inputs
            //Assert.isFalse(fs.SendingPrivateMassage("","UserName","UserName2","Very important content"));  
            //Assert.isFalse(fs.SendingPrivateMassage("forumName"," UserName"," UserName2","Very important content"));  
            //Assert.isFalse(fs.SendingPrivateMassage(" forumName","UserName","UserName2","Very important content"));  
            //Assert.isFalse(fs.SendingPrivateMassage("forumName","UserName","","Very important content"));  
            //Assert.isFalse(fs.SendingPrivateMassage("forumName","UserName","UserName2",""));  


        }

        [TestMethod]
        public void Test_SendingPrivateMassage_CatastophicInput()
        {
            //Assert.isFalse(fs.SendingPrivateMassage(NULL,"UserName","UserName3","Very important content"));          //NULL everyWhere
            //Assert.isFalse(fs.SendingPrivateMassage("forumName",NULL,"UserName3","Very important content"));          
            //Assert.isFalse(fs.SendingPrivateMassage("forumName","UserName","UserName3",NULL));         
            //Assert.isFalse(fs.SendingPrivateMassage("forumName","UserName",NULL,"Very important content"));          
                 
        }
    }
}
