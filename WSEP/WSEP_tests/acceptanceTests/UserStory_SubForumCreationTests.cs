using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.forumManagementService;
using System.Collections.Generic;
using WSEP_tests.adapter;

namespace WSEP_tests.acceptanceTests
{
    [TestClass]
    public class UserStory_SubForumCreationTests
    {
        //private IForumSystem fs;
        IAdapter adapter;
        List<String> list;
        List<String> list2;
        List<String> list3;
        List<String> list4;
        List<String> EmptyList;
        [TestInitialize()]
        public void Initialize()
        {
            adapter = new Adapter();

            adapter.addForum("forumName");                                              //adding the new forum..      
            adapter.addForum("forumName2");                                              //adding the new forum.. 
            adapter.addForum("forumName3");
            adapter.registerToForum("forumName", "UserName1", "UserPassword1", "galMor1007@gmail.com");             //register to forum new user1
            adapter.registerToForum("forumName", "UserName2", "UserPassword2", "abd@gmail.com");             //register to forum another user2
            adapter.registerToForum("forumName2", "UserName3", "UserPassword3", "abe@gmail.com");            //register to another forum a new user3
            adapter.setPolicy("forumName3", 2, 3, 2, 3, "sss");
            adapter.registerToForum("forumName3", "UserName1", "UserPassword3", "abe@gmail.com");
            adapter.registerToForum("forumName3", "UserName2", "UserPassword3", "aba@gmail.com");
            adapter.registerToForum("forumName3", "UserName3", "UserPassword3", "abs@gmail.com");
            adapter.registerToForum("forumName3", "UserName4", "UserPassword3", "abv@gmail.com");
            adapter.registerToForum("forumName3", "UserName", "UserPassword3", "abv@gmail.com");

            EmptyList = new List<string>();

            list = new List<string>();
            list.Add("UserName1");
            list.Add("UserName2");

            list2 = new List<string>();
            list2.Add("UserName");

            list3 = new List<string>();
            list3.Add("UserName");
            list3.Add("UserName3");

            list4 = new List<string>();
            list4.Add("UserName1");
            list4.Add("UserName2");
            list4.Add("UserName3");
            list4.Add("UserName4");
        }

        [TestMethod]
        public void Test_SubForumCreation_GoodInput() //5.1
        {
            Assert.IsTrue(adapter.addSubForum("forumName", "subForumName", list));  //TID 44                     //creation of new subforum with 2 moderators from the forum members  
            Assert.IsFalse(adapter.addSubForum("forumName", "subForumName", list));    //TID 45                  //try again with the same subforum name
            Assert.IsFalse(adapter.addSubForum("forumName", "subForumName2", list3));  //TID 46                  //try to create subforum with moderator not from this forum


        }

        [TestMethod]
        public void Test_SubForumCreation_BadInput() //5.2
        {
            Assert.IsFalse(adapter.addSubForum("forumName", "subForumName2", EmptyList));   //TID 47                       //try to create subforum without moderators
            Assert.IsFalse(adapter.addSubForum("forumName", " subForumName", list));          //TID 48                     //subforum name start wih space
            Assert.IsFalse(adapter.addSubForum("forumName", "subForum\name", list2));       //TID 49                       //subforum name contain \n in a smart way that maybe gal mor is gonna fail in this test??
            Assert.IsFalse(adapter.addSubForum("forumName", "", list));                     //TID 50                       //subforum name is empty string  
            Assert.IsFalse(adapter.addSubForum("forumName3", "name", list4));                //TID 51
            Assert.IsFalse(adapter.addSubForum("forumName3", "name", list2));                //TID 52
        }

        [TestMethod]
        public void Test_SubForumCreation_CatastophicInput() //5.3
        {
            Assert.IsFalse(adapter.addSubForum(null, "subForumName2", list2));    //TID 53                  //NULL everywhere
            Assert.IsFalse(adapter.addSubForum("forumName", null, list2));        //TID 54              

        }



    }
}
