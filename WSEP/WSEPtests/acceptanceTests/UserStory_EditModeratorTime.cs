using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_EditModeratorTime
    {
        private IForumSystem fs;

        [TestInitialize()]
        public void Initialize()
        {
            fs = new ForumSystem("superAdmin");
            //fs.addForum("forumName");                                               //adding the new forum..      
            //fs.RegisterToFurom("forumName","UserName","UserPassword");             //register to forum new user
            //fs.addSubForum("forumName","subForumName","UserName");                 //open new subforum in the forum
            //fs.ModeratorAppointment("forumName","subForumName","UserName",10)       //Moderator Appointment of user to subforum with time = 10
        }

        [TestMethod]
        public void Test_EditModeratorTime_GoodInput()
        {

            //Assert.isTrue(fs.EditModeratorTime("forumName","subForumName","UserName",5);                    //Moderator time change to 5
            //Assert.isTrue(fs.EditModeratorTime("forumName","subForumName","UserName",0);                    //Moderator time change to 0
            //Assert.isFalse(fs.EditModeratorTime("invalidForumName","subForumName","UserName",5);       //invalid details
            //Assert.isFalse(fs.EditModeratorTime("ForumName","invalidSubForumName","UserName",5);       
            //Assert.isFalse(fs.EditModeratorTime("ForumName","SubForumName","UserName5",5);
            //Assert.isFalse(fs.EditModeratorTime("forumName","subForumName","UserName",500000);                    //Moderator time change to 500000


        }

        [TestMethod]
        public void Test_EditModeratorTime_BadInput()
        {
            //Assert.isFalse(fs.EditModeratorTime(" ForumName","SubForumName","UserName",5);                 //bad input- empty, spaces..       
            //Assert.isFalse(fs.EditModeratorTime("ForumName","SubForumName"," UserName",5);                 //bad input- empty, spaces..       
            //Assert.isFalse(fs.EditModeratorTime("ForumName"," SubForumName","UserName",5);                 //bad input- empty, spaces..       
            //Assert.isFalse(fs.EditModeratorTime("ForumName","Sub ForumName","UserName",5);                 //bad input- empty, spaces..       

        }

        [TestMethod]
        public void Test_EditModeratorTime_CatastophicInput()
        {
            int n;
            //Assert.isFalse(fs.EditModeratorTime(" ForumName","SubForumName","UserName",n);          //uninit int n;           
            //Assert.isFalse(fs.EditModeratorTime(NULL,"SubForumName","UserName",5);                    //NULL everywhere
            //Assert.isFalse(fs.EditModeratorTime("ForumName",NULL,"UserName",5);               
            //Assert.isFalse(fs.EditModeratorTime("ForumName","SubForumName",NULL,5);               
            //Assert.isFalse(fs.EditModeratorTime(NULL,NULL,NULL,NULL);       
            //Assert.isFalse(fs.ModeratorAppointment("forumName","subForumName","UserName,-1);       //time is negative              
            //Assert.isFalse(fs.ModeratorAppointment("forumName","subForumName","UserName,NULL);       //time is null           


        }
    }
}
