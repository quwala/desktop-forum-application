using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;
using System.Collections.Generic;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_ReplyCreationTests
    {
        private IForumSystem fs;

        [TestInitialize()]
        public void Initialize()
        {
            
            fs = new ForumSystem("superAdmin");
            //fs.addForum("forumName");                                                           //adding the new forum..      
            //fs.RegisterToForum("forumName","UserName","UserPassword1","galMor1007@gmail.com")             //register to forum new user
            //fs.addSubForum("forumName","subForumName","UserName");                       //open new subforum in the forum
            //fs.CreateThread("forumName","subForumName","thread title","someContent")



            //List<Post> list = fs.getThreadFromSubForum("forumName", "subForumName");
            
        }

        [TestMethod]
        public void Test_ReplyCreation_GoodInput()
        {
            //Assert.isTrue(list.Count == 1);                                                                      //check list size
            //Assert.isTrue(fs.CreateReply("forumName","subForumName",list.first(),"someContent"));                   //creation of new reply in Thread number 1
            //Assert.isFalse(fs.CreateReply("invalidForumName","subForumName",list.first(),"someContent"));          //good input bad validation       


        }

        [TestMethod]
        public void Test_ReplyCreation_BadInput()              
        {
            //Assert.isFalse(fs.CreateReply("forumName","subForumName",list.first(),""));             //try to create of new reply without content  
            //Assert.isFalse(fs.CreateReply("","SubForumName",list.first(),"someContent"));               //bad input- empty, spaces..       
            //Assert.isFalse(fs.CreateReply("forumName","",list.first(),"someContent"));                  //bad input- empty, spaces..       

        }

        [TestMethod]
        public void Test_ReplyCreation_CatastophicInput()            
        {
            //Assert.isFalse(fs.CreateReply(NULL,"subForumName",list.first(),"someContent"));             //NUll everywhereee  
            //Assert.isFalse(fs.CreateReply("forumName",NULL,list.first(),"someContent"));                     
            //Assert.isFalse(fs.CreateReply("forumName","subForumName",NULL,"someContent"));             
            //Assert.isFalse(fs.CreateReply("forumName","subForumName",NULL,NULL));                         

        }
    }
}
