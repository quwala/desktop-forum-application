using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.forumManagementService;
using WSEP_tests.adapter;
using System.Collections.Generic;

namespace WSEP_tests.acceptanceTests
{
    [TestClass]
    public class UserStory_PolicyEditTests
    {
        //private IForumSystem fs;
        private IAdapter adapter;

        [TestInitialize()]
        public void Initialize()
        {
            //fs = new ForumSystem("superAdmin",new WSEP.userManagement.UserManager());
            //fs.addForum("forumName");                              //adding the new forum.. superadmin is the new forum admin           
            adapter = new Adapter();
            adapter.addForum("forumName");
            adapter.addForum("forum2");
            adapter.registerToForum("forum2", "admin1", "Aa123", "aaa@gmail.com");
            adapter.addForumAdmin("forum2", "admin1");
            //admin should be loged in as a pre condition, not sure if to check it here or not..

        }
        [TestMethod] // test 2.1
        public void Test_Policy_GoodInput()                         //set Policy for forum
        {
            Assert.IsTrue(adapter.setPolicy("forumName", 1, 3, 1, 3, "Forum rules")); //TID 6





        }

        [TestMethod] // test 2.2
        public void Test_Policy_BadInput()
        {

            // Assert.IsFalse(adapter.setPolicy(" forumName", 1, 3, 1, 3, "Forum rules")); 
            // Assert.IsFalse(adapter.setPolicy("", 1, 3, 1, 3, "Forum rules")); 
            Assert.IsFalse(adapter.setPolicy("forumName", -4, 3, 1, 3, "Forum rules")); //TID 7
            Assert.IsFalse(adapter.setPolicy("forumName", 5, 3, 1, 3, "Forum rules")); //TID 8
            Assert.IsFalse(adapter.setPolicy("forumName", 1, 3, 6, 3, "Forum rules")); //TID 9
            Assert.IsFalse(adapter.setPolicy("forumName", 1, 3, 0, 1, "Forum rules"));         //TID 10
            // Assert.IsFalse(adapter.setPolicy("forumName", 0, 3, 1, 3, "Forum rules"));          //must have at least one admin
            // Assert.IsFalse(adapter.setPolicy("", 1, 3, 1, 4, "Forum rules")); 

        }

        [TestMethod] // test 2.3
        public void Test_Policy_CatastophicInput()
        {
            Assert.IsFalse(adapter.setPolicy(null, 1, 2, 1, 4, "Forum rules")); //TID 11
            adapter.registerToForum("forumName", "user1", "Ac123", "abc@gmail.com");
            adapter.registerToForum("forumName", "user2", "Ac123", "abe@gmail.com");
            adapter.registerToForum("forumName", "user3", "Ac123", "abd@gmail.com");
            List<string> moderators = new List<string>();
            moderators.Add("user1");
            moderators.Add("user2");
            moderators.Add("user3");
            adapter.addSubForum("forumName", "subFor", moderators);
            Assert.IsFalse(adapter.setPolicy("forumName", 1, 2, 1, 4, "Forum rules")); //TID 12
            Assert.IsFalse(adapter.setPolicy("forum2", 1, 2, 1, 1, "Forum rules")); //TID 13

        }

        /*  [TestMethod] // test 2.4
         public void Test_AddingAdminsAccordingToPolicy()                //check if the forum policy is been checked(num of admins allowed..)
         {
           

             //Assert.isTrue(fs.SetPolicy("forumName",1,3,1,2,"Forum rules"));        //setting max admin num to be 2
             // Assert.isTrue(fs.addForumAdmin("forumName","AdminName"));
             // Assert.isFalse(fs.addForumAdmin("forumName","AdminName"));               //still have place for another admin, but not the same one;
             // Assert.isTrue(fs.addForumAdmin("forumName","AdminName2"));
             // Assert.isFalse(fs.addForumAdmin("forumName","AdminName3"));              //already have 3 admins
         }*/
    }
}
