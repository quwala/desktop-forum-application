using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain;
using WSEP_service.forumManagement;
using WSEP_domain.forumManagement.threadsHandler;
namespace AcceptanceTests
{
    [TestClass]
    public class GetPostsByUserTests
    {
        private IForumSystem forumSystem;
        private List<Tuple<string, string, int>> mods;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
            forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "email1@gmail.com");
            forumSystem.registerMemberToForum("TestingForum", "username2", "pass2", "email2@gmail.com");
            mods = new List<Tuple<string, string, int>>();
            mods.Add(new Tuple<string, string, int>("username1", "superAdmin", 7));
            forumSystem.addSubForum("TestingForum", "TestingSubForum", mods, "superAdmin");
            forumSystem.writePost("TestingForum", "TestingSubForum", 0, "username1", "title", "content");
        }

        [TestMethod]
        public void GetPostsByUserGoodInput()
        {
            List<Post> list = forumSystem.getPostsByUser("TestingForum", "username1", "superAdmin");
            Assert.AreEqual(1, list.Count);
            forumSystem.writePost("TestingForum", "TestingSubForum", 0, "username1", "other title", " other content");
            list = forumSystem.getPostsByUser("TestingForum", "username1", "superAdmin");
            Assert.AreEqual(2, list.Count);       
        }

        [TestMethod]
        public void GetPostsByUsernullInput()
        {
            List<Post> list = forumSystem.getPostsByUser(null, "username1", "superAdmin");
            Assert.AreEqual(0, list.Count);
             list = forumSystem.getPostsByUser("TestingForum", null, "superAdmin");
            Assert.AreEqual(0, list.Count);
             list = forumSystem.getPostsByUser("TestingForum", "username1", null);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void GetPostsByUserUnexistingUser()
        {
            
            List<Post> list = forumSystem.getPostsByUser("TestingForum", "username14354", "superAdmin");
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void GetPostsByUserNotAdmin()
        {
            List<Post> list = forumSystem.getPostsByUser("TestingForum", "username1", "username2");
            Assert.AreEqual(0, list.Count); 
        }

        [TestMethod]
        public void GetPostsByUserUnexistingForum()
        {

            List<Post> list = forumSystem.getPostsByUser("TestingForum3", "username1", "superAdmin4");
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void GetPostsByUserUnexistingAdmin()
        {

            List<Post> list = forumSystem.getPostsByUser("TestingForum", "username1", "superAdmin4");
            Assert.AreEqual(0, list.Count);
        }

        public void GetPostsByUserEmptyInputs()
        {

            List<Post> list = forumSystem.getPostsByUser("TestingForum", "username1", "");
            Assert.AreEqual(0, list.Count);
             list = forumSystem.getPostsByUser("TestingForum", "", "superAdmin4");
            Assert.AreEqual(0, list.Count);
             list = forumSystem.getPostsByUser("", "username1", "superAdmin4");
            Assert.AreEqual(0, list.Count);
        }
    }
}
