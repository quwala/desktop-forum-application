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
    public class getNumOfForumsTests
    {

        private IForumSystem forumSystem;
        private List<Tuple<string, string, int>> mods;

        [TestInitialize]
        public void SetupTests()
        {
            forumSystem = new ForumSystem(true);
            forumSystem.addForum("TestingForum", "superAdmin");
        }
        [TestMethod]
        public void getNumOfForumsGoodInput()
        {
            Assert.AreEqual(1, forumSystem.getNumOfForums("superAdmin"));
            forumSystem.addForum("TestingForum2", "superAdmin");
            forumSystem.addForum("TestingForum3", "superAdmin");
            Assert.AreEqual(3, forumSystem.getNumOfForums("superAdmin"));
        }

        [TestMethod]
        public void getNumOfForumsNullInput()
        {
            Assert.AreEqual(-1, forumSystem.getNumOfForums(null));
        }

        [TestMethod]
        public void getNumOfForumsEmptyInput()
        {
            Assert.AreEqual(-1, forumSystem.getNumOfForums(""));
        }

        [TestMethod]
        public void getNumOfForumsIncorrectInput()
        {
            Assert.AreEqual(-1, forumSystem.getNumOfForums("something"));
        }
    }
}
