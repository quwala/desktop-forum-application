using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;
using System.Collections.Generic;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_SubForumCreationTests
    {
        private IForumSystem fs;
        
        [TestInitialize()]
        public void Initialize()
        {
            fs = new ForumSystem("superAdmin");
            //fs.addForum("forumName");                                              //adding the new forum..      
            //fs.addForum("forumName2");                                              //adding the new forum..      
            //fs.RegisterToFurom("forumName","UserName","UserPassword1");             //register to forum new user1
            //fs.RegisterToFurom("forumName","UserName2","UserPassword2");             //register to forum another user2
            //fs.RegisterToFurom("forumName2","UserName3","UserPassword3");            //register to another forum a new user3

            List<String> EmptyList = new List<string>();

            List<String> List = new List<string>();
            List.Add("UserName");
            List.Add("UserName2");

            List<String> List2 = new List<string>();
            List.Add("UserName");

            List<String> List3 = new List<string>();
            List.Add("UserName");
            List.Add("UserName3");
        }

        [TestMethod]
        public void Test_SubForumCreation_GoodInput()
        {
            //Assert.isFalse(fs.ContainSubForum("forumName","subForumName"));                       //check if new subforum add to the subforum list  
            //Assert.isTrue(fs.addSubForum("forumName","subForumName",List));                       //creation of new subforum with 2 moderators from the forum members  
            //Assert.isTrue(fs.ContainSubForum("forumName","subForumName"));                        //check if new subforum add to the subforum list  
            //Assert.isFalse(fs.addSubForum("forumName","subForumName",List));                      //try again with the same subforum name
            //Assert.isFalse(fs.addSubForum("forumName","subForumName2",List3));                    //try to create subforum with moderator not from this forum
            

        }

        [TestMethod]
        public void Test_SubForumCreation_BadInput()
        {
            //Assert.isFalse(fs.addSubForum("forumName","subForumName2",EmptyList));                          //try to create subforum without moderators
            //Assert.isFalse(fs.addSubForum("forumName"," subForumName",List));                               //subforum name start wih space
            //Assert.isFalse(fs.addSubForum("forumName","subForum\name",List2));                              //subforum name contain \n in a smart way that maybe gal mor is gonna fail in this test??
            //Assert.isFalse(fs.addSubForum("forumName","",List));                                            //subforum name is empty string  
            //Assert.isFalse(fs.addSubForum("","subForumName","UserName","UserName2"));                       //empty forum name
        }

        [TestMethod]
        public void Test_SubForumCreation_CatastophicInput()
        {
            //Assert.isFalse(fs.addSubForum(NULL,"subForumName2", List2));                      //NULL everywhere
            //Assert.isFalse(fs.addSubForum("forumName",NULL, List2));                      
            //Assert.isFalse(fs.addSubForum("forumName","subForumName",NULL));                 
            //Assert.isFalse(fs.addSubForum(NULL,NULL,NULL));                  
        }


        //addSubForumModerator("forumName","subForumName","ModeratorName")
    }
}
