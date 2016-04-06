using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_PolicyEditTests
    {
        private IForumSystem fs;

        [TestInitialize()]
        public void Initialize()
        {
            fs = new ForumSystem("superAdmin");
            //fs.addForum("forumName");                              //adding the new forum.. superadmin is the new forum admin           



        }

        public void Test_Policy_GoodInput()                        
        {
            //Assert.isTrue(fs.SetPolicy("forumName",1,3,1,3,"Forum rules"));
            //Assert.isFalse(fs.SetPolicy("forumName",1,3,1,3,""));
            //Assert.isFalse(fs.SetPolicy("forumName",3,1,1,3,"Forum rules"));           //min num of admins is bigger then max
            //Assert.isFalse(fs.SetPolicy("forumName",5,4,2,4,"Forum rules"));           //min num of admins is bigger then max 
            


        }

        [TestMethod]
        public void Test_Policy_BadInput()
        {

            //Assert.isFalse(fs.SetPolicy(" forumName",1,3,1,3,"Forum rules"));
            //Assert.isFalse(fs.SetPolicy("",1,3,1,3,"Forum rules"));
            //Assert.isFalse(fs.SetPolicy("forumName",,3,1,3,"Forum rules"));

            //Assert.isFalse(fs.SetPolicy(" forumName",1,3,1,4,"Forum rules"));          //is set empty field is ok because there is allready old policy that we can use
            //Assert.isFalse(fs.SetPolicy("forumName",0,3,1,3,"Forum rules"));          //must have at least one admin
            //Assert.isFalse(fs.SetPolicy("",1,3,1,4,"Forum rules"));
            
        }

        [TestMethod]
        public void Test_Policy_CatastophicInput()
        {
            //Assert.isFalse(fs.SetPolicy("forumName",1,3,1,4,NULL));                   //Nulls and empty
            //Assert.isFalse(fs.SetPolicy("forumName",1,3,,4,"Forum rules"));
            //Assert.isFalse(fs.SetPolicy("forumName",1,,,,"Forum rules"));
            //Assert.isFalse(fs.SetPolicy("forumName",1,,1,4,"Forum rules"));
            //Assert.isFalse(fs.SetPolicy("forumName",,,2,4,"Forum rules"));
            //Assert.isFalse(fs.SetPolicy("forumName",,,1,4,"Forum rules"));
            //Assert.isFalse(fs.SetPolicy(NULL,1,2,1,4,"Forum rules"));

        }

        [TestMethod]
        public void Test_AddingAdminsAccordingToPolicy()                //check if the forum policy is been checked(num of admins allowed..)
        {
           

            //Assert.isTrue(fs.SetPolicy("forumName",1,3,1,2,"Forum rules"));        //setting max admin num to be 2
            // Assert.isTrue(fs.addForumAdmin("forumName","AdminName"));
            // Assert.isFalse(fs.addForumAdmin("forumName","AdminName"));               //still have place for another admin, but not the same one;
            // Assert.isTrue(fs.addForumAdmin("forumName","AdminName2"));
            // Assert.isFalse(fs.addForumAdmin("forumName","AdminName3"));              //already have 3 admins
        }
    }
}
