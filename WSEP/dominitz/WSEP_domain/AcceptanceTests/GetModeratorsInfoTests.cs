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
    public class GetModeratorsInfoTests
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
            mods.Add(new Tuple<string, string, int>("username2", "superAdmin", 7));
            forumSystem.addSubForum("TestingForum", "TestingSubForum", mods, "superAdmin");
            forumSystem.writePost("TestingForum", "TestingSubForum", 0, "username1", "title", "content");
        }
        [TestMethod]
        public void GetModeratorsInfoGoodInput()
        {
            List<Tuple<string, string, string, DateTime, List<Post>>> list = forumSystem.getModeratorsData("TestingForum", "superAdmin");
            Assert.AreEqual(2, list.Count);
            foreach (Tuple<string, string, string, DateTime, List<Post>> data in list)
            {
                Assert.AreEqual(data.Item2, "TestingSubForum");
                Assert.AreEqual(data.Item3, "superAdmin");
            }
        }

        [TestMethod]
        public void GetModeratorsInfonullInput()
        {
            List<Tuple<string, string, string, DateTime, List<Post>>> list = forumSystem.getModeratorsData("TestingForum", null);
            Assert.AreEqual(0, list.Count);
            list = forumSystem.getModeratorsData(null, "superAdmin");
            Assert.AreEqual(0, list.Count); 
        }

        [TestMethod]
        public void GetModeratorsInfoBadInput()
        {
            List<Tuple<string, string, string, DateTime, List<Post>>> list = forumSystem.getModeratorsData("TestingForum", "username1");
            Assert.AreEqual(0, list.Count);
            list = forumSystem.getModeratorsData("TestingForum123", "superAdmin");
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void GetModeratorsInfoEmptyInput()
        {
            List<Tuple<string, string, string, DateTime, List<Post>>> list = forumSystem.getModeratorsData("TestingForum", "");
            Assert.AreEqual(0, list.Count);
            list = forumSystem.getModeratorsData("", "superAdmin");
            Assert.AreEqual(0, list.Count);
        }
    }
}
