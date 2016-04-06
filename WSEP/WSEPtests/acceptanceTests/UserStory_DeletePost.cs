using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WSEP.forumManagement;


namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_DeletePost
    {
        
        private IForumSystem fs;

        [TestInitialize()]
        public void Initialize()
        {

            fs = new ForumSystem("superAdmin");
            //fs.addForum("forumName");                                               //adding the new forum..      
            //fs.RegisterToFurom("forumName","UserName","UserPassword");             //register to forum new user
            //fs.addSubForum("forumName","subForumName","UserName");                 //open new subforum in the forum
            //fs.addSubForum("forumName","subForumName2","UserName");                 //open new subforum in the forum
            //fs.CreateThread("forumName","subForumName","thread title","someContent")
            //fs.CreateThread("forumName","subForumName2","thread title2","someContent")

            //List<Post> list = fs.getThreadFromSubForum("forumName", "subForumName");

        }

        [TestMethod]
        public void Test_DeletePost_GoodInput()
        {
            //Assert.isTrue(fs.DeletePost("forumName","subForumName",list.first());                       //delete the post
            //Assert.isFalse(fs.DeletePost("invalidForumName","subForumName",list.first(),"someContent");          //good input bad validation       
            //Assert.isfalse(fs.DeletePost("forumName","subForumName2",list.first());                            //try to delete post that this subforum doesn't contain
    
            //list= fs.getThreadFromSubForum("forumName", "subForumName");
            //Assert.AreEqual(list.Count ,1);                                                                           //check if list doesn't contain this post.

        }

        [TestMethod]
        public void Test_DeletePost_BadInput()
        {

            //Assert.AreEqual(list.Count ,1);                                                        //check list size
            //Assert.isFalse(fs.DeletePost(" forumName","subForumName",list.first());               //bad input- empty, spaces..       
            //Assert.isFalse(fs.DeletePost("forumName"," subForumName",list.first());               //bad input- empty, spaces..       
            //Assert.AreEqual(list.Count ,1);                                                     //check list size

        }

        [TestMethod]
        public void Test_DeletePost_CatastophicInput()
        {
            //Assert.isFalse(fs.CreateReply(NULL,"subForumName",list.first(),"someContent");             //NUll everywhereee  
            //Assert.isFalse(fs.CreateReply("forumName",NULL,list.first(),"someContent");                     
            //Assert.isFalse(fs.CreateReply("forumName","subForumName",NULL,"someContent");             
            //Assert.isFalse(fs.CreateReply("forumName","subForumName",NULL,NULL);                         
            //Assert.AreEqual(list.Count ,1);                                                        //check list size

        }
    }
}
