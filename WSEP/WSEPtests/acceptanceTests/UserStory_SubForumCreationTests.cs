using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;

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
            //fs.RegisterToFurom("forumName","UserName","UserPassword1");             //register to forum new user1
            //fs.RegisterToFurom("forumName","UserName2","UserPassword2");             //register to forum another user2
            //fs.RegisterToFurom("forumName2","UserName3","UserPassword3");            //register to another forum a new user3


            //admin should be loged in as a pre condition, not sure if to check it here or not..
            //
            //fs.ForumLogIn("forumName","AdminName","AdminPassword"))
            //if so, we need to add test to check validation with admin loged in and out..
        }

        [TestMethod]
        public void Test_SubForumCreation_GoodInput()
        {
            //Assert.isFalse(fs.ContainSubForum("forumName","subForumName"));                        //check if new subforum add to the subforum list  
            //Assert.isTrue(fs.addSubForum("forumName","subForumName","UserName","UserName2"));     //creation of new subforum with 2 moderators from the forum members  
            //Assert.isTrue(fs.ContainSubForum("forumName","subForumName"));                        //check if new subforum add to the subforum list  
            //Assert.isFalse(fs.addSubForum("forumName","subForumName","UserName","UserName2"));    //try again with the same subforum name
            //Assert.isFalse(fs.addSubForum("forumName","subForumName2","UserName","UserName3"));   //try to create subforum with moderator not from this forum
            

        }

        [TestMethod]
        public void Test_SubForumCreation_BadInput()
        {
            //Assert.isFalse(fs.addSubForum("forumName","subForumName2"));                           //try to create subforum without moderators
            //Assert.isFalse(fs.addSubForum("forumName"," subForumName","UserName","UserName2"));    //subforum name start wih space
            //Assert.isFalse(fs.addSubForum("forumName","subForum\name","UserName","UserName2"));    //subforum name contain \n in a smart way that maybe gal mor is gonna fail in this test??
            //Assert.isFalse(fs.addSubForum("forumName","","UserName","UserName2"));                 //subforum name is empty string  
            //Assert.isFalse(fs.addSubForum("","subForumName","UserName","UserName2"));              //empty forum name
        }

        [TestMethod]
        public void Test_SubForumCreation_CatastophicInput()
        {
            //Assert.isFalse(fs.addSubForum(NULL,"subForumName2", "userName"));                      //NULL everywhere
            //Assert.isFalse(fs.addSubForum("forumName",NULL, "userName"));                      
            //Assert.isFalse(fs.addSubForum("forumName","subForumName",NULL));                 
            //Assert.isFalse(fs.addSubForum(NULL,NULL,NULL));                  
        }
    }
}
