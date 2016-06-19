using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain;
using WSEP_service.forumManagement;

namespace AcceptanceTests.ServerTests
{
    [TestClass]
    public class GetNumOfPostsInSubForumTests
    {
        private IForumSystem forumSystem;
        private List<Tuple<string, string, int>> mods;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
            forumSystem.registerMemberToForum("TestingForum", "username1", "pass1", "email1@gmail.com");
            mods = new List<Tuple<string, string, int>>();
            mods.Add(new Tuple<string, string, int>("username1", "superAdmin", 7));
            forumSystem.addSubForum("TestingForum", "TestingSubForum", mods, "superAdmin");
        }

        [TestMethod]
        public void GetNumOfPostsInSubForumGoodInput()
        {
            Assert.AreEqual(0, forumSystem.getNumOfPostsInSubForum("TestingForum", "TestingSubForum", "superAdmin"));
        }

        [TestMethod]
        public void GetNumOfPostsInSubForumNullArguments()
        {
            Assert.AreEqual(-1, forumSystem.getNumOfPostsInSubForum(null, "TestingSubForum", "superAdmin"));
            Assert.AreEqual(-1, forumSystem.getNumOfPostsInSubForum("TestingForum", null, "superAdmin"));
            Assert.AreEqual(-1, forumSystem.getNumOfPostsInSubForum("TestingForum", "TestingSubForum", null));
            Assert.AreEqual(-1, forumSystem.getNumOfPostsInSubForum(null, null, null));
        }

        [TestMethod]
        public void GetNumOfPostsInSubForumEmptyArguments()
        {
            Assert.AreEqual(-1, forumSystem.getNumOfPostsInSubForum("", "TestingSubForum", "superAdmin"));
            Assert.AreEqual(-1, forumSystem.getNumOfPostsInSubForum("TestingForum", "", "superAdmin"));
            Assert.AreEqual(-1, forumSystem.getNumOfPostsInSubForum("TestingForum", "TestingSubForum", ""));
            Assert.AreEqual(-1, forumSystem.getNumOfPostsInSubForum("", "", ""));
        }

        [TestMethod]
        public void GetNumOfPostsInSubForumUnexistedForum()
        {
            Assert.AreEqual(-1, forumSystem.getNumOfPostsInSubForum("UnexistedForum", "TestingSubForum", "superAdmin"));
        }

        [TestMethod]
        public void GetNumOfPostsInSubForumUnexistedSubForum()
        {
            Assert.AreEqual(-1, forumSystem.getNumOfPostsInSubForum("TestingForum", "UnexistedSubForum", "superAdmin"));
        }

        [TestMethod]
        public void GetNumOfPostsInSubForumUnexistedUser()
        {
            Assert.AreEqual(-1, forumSystem.getNumOfPostsInSubForum("TestingForum", "UnexistedSubForum", "UnexistedUser"));
        }

        [TestMethod]
        public void GetNumOfPostsInSubForumUnpermissionedUser()
        {
            Assert.AreEqual(-1, forumSystem.getNumOfPostsInSubForum("TestingForum", "UnexistedSubForum", "username1"));
        }
    }
}
