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


            //admin should be loged in as a pre condition, not sure if to check it here or not..

        }

        public void Test_Policy_GoodInput()                         //create new policy or change exiting one.
        {
            //Assert.isFalse(fs.AddPolicy("forumName",3,1,1,3,"Forum rules"));           //min num of admins is bigger then max

            //Assert.isTrue(fs.AddPolicy("forumName",1,3,1,3,"Forum rules"));
            
            //settind the new policy..

            //Assert.isTrue(fs.SetPolicy("forumName",1,4,1,5,""));                                 //changing max num of admins..
            //Assert.isTrue(fs.SetPolicy("forumName",,,,,"Different rules"));                                 //changing max num of admins..
            //Assert.isFalse(fs.SetPolicy("forumName",5,4,2,4,"Forum rules"));                     //min num of admins is bigger then max in set.
            


        }

        [TestMethod]
        public void Test_Policy_BadInput()
        {

            //Assert.isFalse(fs.AddPolicy(" forumName",1,3,1,3,"Forum rules"));
            //Assert.isFalse(fs.AddPolicy("",1,3,1,3,"Forum rules"));
            //Assert.isFalse(fs.AddPolicy("forumName",,3,1,3,"Forum rules"));
            //Assert.isFalse(fs.AddPolicy("forumName",1,,,,"Forum rules"));

            //Assert.isTrue(fs.AddPolicy("forumName",1,3,1,3,"Forum rules"));           //just for init new policy


            //Assert.isFalse(fs.SetPolicy(" forumName",1,3,1,4,"Forum rules"));          //is set empty field is ok because there is allready old policy that we can use
            //Assert.isFalse(fs.SetPolicy("forumName",0,3,1,3,"Forum rules"));          //must have at least one admin
            //Assert.isFalse(fs.SetPolicy("",1,3,1,4,"Forum rules"));
            
        }

        [TestMethod]
        public void Test_Policy_CatastophicInput()
        {
            //Assert.isFalse(fs.AddPolicy("forumName",1,3,1,4,NULL));
            //Assert.isFalse(fs.AddPolicy("forumName",1,3,,4,"Forum rules"));
            //Assert.isFalse(fs.AddPolicy("forumName",1,,1,4,"Forum rules"));
            //Assert.isFalse(fs.AddPolicy("forumName",,,2,4,"Forum rules"));
            //Assert.isFalse(fs.AddPolicy("forumName",,,1,4,"Forum rules"));
            //Assert.isFalse(fs.AddPolicy(NULL,1,2,1,4,"Forum rules"));

        }

        [TestMethod]
        public void Test_AddingAdminsAccordingToPolicy()                //check if the forum policy is been checked(num of admins allowed..)
        {
           

            //Assert.isTrue(fs.AddPolicy("forumName",1,3,1,2,"Forum rules"));        //setting max admin num to be 3
            // Assert.isTrue(fs.addForumAdmin("forumName","AdminName"));
            // Assert.isFalse(fs.addForumAdmin("forumName","AdminName"));               //still have place for another admin, but not the same one;
            // Assert.isTrue(fs.addForumAdmin("forumName","AdminName2"));
            // Assert.isFalse(fs.addForumAdmin("forumName","AdminName3"));              //already have 3 admins
        }
    }
}
