using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_ModeratorAppointment
    {
        private IForumSystem fs;

        [TestInitialize()]
        public void Initialize()
        {
            fs = new ForumSystem("superAdmin");
            //fs.addForum("forumName");                                               //adding the new forum..      
            //fs.RegisterToFurom("forumName","UserName","UserPassword");             //register to forum new user
            //fs.addSubForum("forumName","subForumName","UserName");                 //open new subforum in the forum
           
            
        }

        [TestMethod]
        public void Test_ModeratorAppointment_GoodInput()
        {
            
            //Assert.isTrue(fs.ModeratorAppointment("forumName","subForumName","UserName",10);                //Moderator Appointment to subforum with time = 10
            //Assert.isFalse(fs.ModeratorAppointment("invalidForumName","subForumName","UserName",2);       //Moderator Appointment bad forum name
            //Assert.isFalse(fs.ModeratorAppointment("ForumName","invalidSubForumName","UserName",2);       //Moderator Appointment bad sub forum name


        }

        [TestMethod]
        public void Test_ModeratorAppointment_BadInput()
        {
            //Assert.isFalse(fs.ModeratorAppointment("forumName","","UserName",10);               //bad input- empty, spaces..       
            //Assert.isFalse(fs.ModeratorAppointment("forumName"," subForumName","UserName",10);  //bad input- empty, spaces..       
            //Assert.isFalse(fs.ModeratorAppointment("forumName","subForumName","",10);                 //bad input- empty, spaces..       
            //Assert.isFalse(fs.ModeratorAppointment("","SubForumName","UserName",10);                      //bad input- empty, spaces..       

        }

        [TestMethod]
        public void Test_ModeratorAppointment_CatastophicInput()
        {
            //Assert.isFalse(fs.ModeratorAppointment("forumName",NULL,"UserName",10);               // Null everywhereee      
            //Assert.isFalse(fs.ModeratorAppointment(NULL,"subForumName","UserName",10);         
            //Assert.isFalse(fs.ModeratorAppointment("forumName","subForumName",NULL,10);                     
            //Assert.isFalse(fs.ModeratorAppointment(NULL,NULL,NULL,NULL);       
            //Assert.isFalse(fs.ModeratorAppointment("forumName","subForumName","UserName,-1);       //time is negative              
            //Assert.isFalse(fs.ModeratorAppointment("forumName","subForumName","UserName,NULL);       //time is null           
            //Assert.isFalse(fs.ModeratorAppointment("forumName","subForumName","UserName",0);              //Moderator Appointment to subforum with time = 0 its useless and need to return false on time like this


        }
    }
}
