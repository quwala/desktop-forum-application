using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.forumManagementService;

namespace WSEP_tests.acceptanceTests
{
    [TestClass]
    public class UserStory_ModeratorAppointment
    {
        

        [TestInitialize()]
        public void Initialize()
        {
           // fs = new ForumSystem("superAdmin", new WSEP.userManagement.UserManager());
            //fs.addForum("forumName");                                               //adding the new forum..      
            //fs.RegisterToFurom("forumName","UserName","UserPassword");             //register to forum new user
            //fs.addSubForum("forumName","subForumName","UserName");                 //open new subforum in the forum
            // int time = 10;
            //int time2 = 0;

        }

        [TestMethod]
        public void Test_ModeratorAppointment_GoodInput()
        {

            //Assert.isTrue(fs.ModeratorAppointment("forumName","subForumName","UserName",time);                //Moderator Appointment to subforum with time = 10
            //Assert.isFalse(fs.ModeratorAppointment("forumName","subForumName","UserName",time2);              //Moderator Appointment to subforum with time = 0
            //Assert.isFalse(fs.ModeratorAppointment("invalidForumName","subForumName","UserName",time2);       //Moderator Appointment bad forum name
            //Assert.isFalse(fs.ModeratorAppointment("ForumName","invalidSubForumName","UserName",time2);       //Moderator Appointment bad sub forum name


        }

        [TestMethod]
        public void Test_ModeratorAppointment_BadInput()
        {
            //Assert.isFalse(fs.ModeratorAppointment("forumName","","UserName",time);               //bad input- empty, spaces..       
            //Assert.isFalse(fs.ModeratorAppointment("forumName"," subForumName","UserName",time);  //bad input- empty, spaces..       
            //Assert.isFalse(fs.ModeratorAppointment("forumName","subForumName","",time);                 //bad input- empty, spaces..       
            //Assert.isFalse(fs.ModeratorAppointment("","SubForumName","UserName",time);                      //bad input- empty, spaces..       

        }

        [TestMethod]
        public void Test_ModeratorAppointment_CatastophicInput()
        {
            //Assert.isFalse(fs.ModeratorAppointment("forumName",NULL,"UserName",time);               // Null everywhereee      
            //Assert.isFalse(fs.ModeratorAppointment(NULL,"subForumName","UserName",time);         
            //Assert.isFalse(fs.ModeratorAppointment("forumName","subForumName",NULL,time);                     
            //Assert.isFalse(fs.ModeratorAppointment(NULL,NULL,NULL,NULL);       
            //Assert.isFalse(fs.ModeratorAppointment("forumName","subForumName","UserName,-1);       //time is negative              
            //Assert.isFalse(fs.ModeratorAppointment("forumName","subForumName","UserName,NULL);       //time is null           


        }
    }
}
