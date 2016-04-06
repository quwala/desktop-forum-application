using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;

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
            //fs.addForum("forumName");                                               //adding the new forum..      
            //fs.RegisterToFurom("forumName","UserName","UserPassword");             //register to forum new user
            //fs.addSubForum("forumName","subForumName","UserName");                 //open new subforum in the forum
            //fs.CreateThread("forumName","subForumName","thread title","someContent")
            
            //user should be loged in as a pre condition, not sure if to check it here or not..
            //
            //fs.ForumLogIn("forumName","UserName","UserPassword"))
            //if so, we need to add test to check validation with user loged in and out..
        }

        [TestMethod]
        public void Test_ReplyCreation_GoodInput()
        {
            //Assert.AreEqual(fs.NumOfReply("forumName","subForumName",1),0);                             //check number of Reply in Thread number 1
            //Assert.isTrue(fs.CreateReply("forumName","subForumName",1,"someContent");                   //creation of new reply in Thread number 1
            //Assert.AreEqual(fs.NumOfReply("forumName","subForumName",1),1);                             //check number of Reply in Thread number 1
            //Assert.isFalse(fs.CreateReply("invalidForumName","subForumName",1,"someContent");          //good input bad validation       
            //Assert.isFalse(fs.CreateReply("forumName","invalidSubForumName","title","someContent");          //good input bad validation       


        }

        [TestMethod]
        public void Test_ReplyCreation_BadInput()
        {
            //Assert.isFalse(fs.CreateThread("forumName","subForumName","","someContent");             //try to create of new thread without title  
            //Assert.isFalse(fs.CreateThread("","SubForumName","title","someContent");               //bad input- empty, spaces..       
            //Assert.isFalse(fs.CreateThread("forumName","","title","someContent");                  //bad input- empty, spaces..       
            //Assert.isFalse(fs.CreateThread("forumName","SubForumName","","");                     //bad input- empty, spaces..       

        }

        [TestMethod]
        public void Test_ReplyCreation_CatastophicInput()
        {
            //Assert.isFalse(fs.CreateThread(NULL,"subForumName","title","someContent");             //NUll everywhereee  
            //Assert.isFalse(fs.CreateThread("forumName",NULL,"title","someContent");                     
            //Assert.isFalse(fs.CreateThread("forumName","subForumName",NULL,"someContent");             
            //Assert.isFalse(fs.CreateThread("forumName","subForumName",NULL,NULL);                         

        }
    }
}
