using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ForumsDataBase.Tests
{
    [TestClass]
    public class dbTest1
    {

        DBI db = new DB();

        [TestMethod]
        public void TestAddAndRemoveForumUser()
        {
            DateTime d = db.ReturnSubforumModerators("forumName1", "subForumName1").First().Item4;
            int count = db.ReturnForumUsers("forumName1").Count();                                          //count before for forumName1 users
            Assert.IsTrue(db.addForumUser("userName1","password1","email@gmail.com",d,"forumName1",d));     //good adding
            Assert.IsFalse(db.addForumUser("userName1","password1","email@gmail.com",d,"forumName1",d));    //bad adding
            Assert.AreEqual(count + 1, db.ReturnForumUsers("forumName1").Count());                          //count+1 = new count
            Assert.IsTrue(db.removeForumUser("userName1","forumName1"));                                    //good removing
            Assert.AreEqual(count , db.ReturnForumUsers("forumName1").Count());                             //count = new count

        }

        [TestMethod]
        public void TestAddAndRemoveSubForum()
        {
            int count = db.ReturnSubForumList("forumName1").Count();                                          //count before for forumName1 users
            Assert.IsTrue(db.addSubForum("forumName1","subForumName4"));                                      //good adding
            Assert.IsFalse(db.addSubForum("forumName1", "subForumName4"));                                    //bad adding
            Assert.AreEqual(count + 1, db.ReturnSubForumList("forumName1").Count());                          //count+1 = new count
            Assert.IsTrue(db.removeSubForum("forumName1", "subForumName4"));                                  //good removing
            Assert.AreEqual(count, db.ReturnSubForumList("forumName1").Count());                              //count = new count

        }

        [TestMethod]
        public void TestAddAndRemovePrivateMessage()
        {
            DateTime d = db.ReturnSubforumModerators("forumName1", "subForumName1").First().Item4;
            int count = db.ReturforumMessages("forumName1").Count();                                          //count before for forumName1 users
            Assert.IsTrue(db.addPrivateMessage("dominitz","shayday",d,"forumName1","some bla bla things"));   //good adding
            Assert.IsFalse(db.addPrivateMessage("dominitz","shayday",d,"forumName1","some bla bla things"));  //bad adding
            Assert.AreEqual(count + 1, db.ReturforumMessages("forumName1").Count());                          //count+1 = new count
            Assert.IsTrue(db.removePrivateMessage("dominitz",d,"ForumName1"));                                //good removing
            Assert.AreEqual(count, db.ReturforumMessages("forumName1").Count());                              //count = new count

        }

        [TestMethod]
        public void TestAddAndRemoveSubForumPost()
        {
            DateTime d = db.ReturnSubforumModerators("forumName1", "subForumName1").First().Item4;
            int count = db.ReturnSubforumPosts("forumName1","subForumName1").Count();                                             //count before for forumName1 users
            Assert.IsTrue(db.addSubForumPost("postTitle", "post content", "forumName1","subForumName1","domonitz",d,-1,0));       //good adding
            Assert.IsFalse(db.addSubForumPost("postTitle", "post content", "forumName1", "subForumName1", "domonitz", d, -1, 0)); //bad adding
            Assert.AreEqual(count + 1, db.ReturnSubforumPosts("forumName1", "subForumName1").Count());                            //count+1 = new count
            Assert.IsTrue(db.removeSubForumPost(-1));                                                                             //good removing
            Assert.AreEqual(count, db.ReturnSubforumPosts("forumName1","subForumName1").Count());                                 //count = new count

        }

        [TestMethod]
        public void TestAddAndRemoveSubForumModerator()
        {
            DateTime d = db.ReturnSubforumModerators("forumName1", "subForumName1").First().Item4;
            int count = db.ReturnSubforumModerators("forumName1", "subForumName1").Count();                      //count before for forumName1 users
            Assert.IsTrue(db.addSubForumModerator("forumName1","subForumName1","shayday","galmor",45,d));        //good adding
            Assert.IsFalse(db.addSubForumModerator("forumName1", "subForumName1", "shayday", "galmor", 45, d));  //bad adding
            Assert.AreEqual(count + 1, db.ReturnSubforumModerators("forumName1", "subForumName1").Count());      //count+1 = new count
            Assert.IsTrue(db.removeSubForumModerator("forumName1","subForumName1","dominitz"));                  //good removing
            Assert.AreEqual(count, db.ReturnSubforumModerators("forumName1", "subForumName1").Count());          //count = new count

        }

        [TestMethod]
        public void TestAddAndRemoveSubForumMember()
        {
            DateTime d = db.ReturnSubforumModerators("forumName1", "subForumName1").First().Item4;
            int count = db.ReturnforumMembers("forumName1").Count();                                     //count before for forumName1 users
            Assert.IsTrue(db.addForumMember("forumName1", "shayday"));                                   //good adding
            Assert.IsFalse(db.addForumMember("forumName1", "shayday"));                                  //bad adding
            Assert.AreEqual(count + 1, db.ReturnforumMembers("forumName1").Count());                     //count+1 = new count
            Assert.IsTrue(db.removeForumMember("forumName1", "shayday"));                                //good removing
            Assert.AreEqual(count, db.ReturnforumMembers("forumName1").Count());                         //count = new count

        }

    }
}
