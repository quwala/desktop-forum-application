using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumManagementDomain.forumHandler;
using WSEP_service.forumManagementService;
using WSEP_service.userManagementService;

namespace WSEP_tests.acceptanceTests
{
    [TestClass]
    public class UserStory_PolicyActionsTests
    {

        private Forum f;
        private UserManager um;


        [TestInitialize()]
        public void Initialize()
        {
            um = new UserManager();

            ForumSystem fs = new ForumSystem("superAdmin",um);
            fs.addForum("Test Forum");
            um.addForum("Test Forum");
            f = fs.getForum("Test Forum");
        }

        [TestMethod]
        public void Test_PolicyActions_SetNewPolicy()
        {
            ForumPolicy nPolicy = new ForumPolicy("New Policy");
            nPolicy.MinAdmins = 1;
            nPolicy.MaxAdmins = 5;
            nPolicy.ForumRules = "Don't have any fun please";
            Assert.IsTrue(f.setPolicy(nPolicy));

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_PolicyActions_BadData()
        {
            ForumPolicy nPolicy = new ForumPolicy("Bad Policy");
            nPolicy.MinAdmins = 0;
            nPolicy.MaxAdmins = 5;
            nPolicy.ForumRules = "Don't have any fun please";
            f.setPolicy(nPolicy);
        }


        [TestMethod]
        public void Test_PolicyActions_NewPolicyDoesNotComplyWithForum()
        {
            um.registerMemberToForum("Test Forum", "user1", "user1", "user1@gmail.com");
            ForumPolicy nPolicy = new ForumPolicy("Bad Policy");
            nPolicy.MinAdmins = 1;
            nPolicy.MaxAdmins = 1;
            nPolicy.ForumRules = "Don't have any fun please";

            Assert.AreNotEqual(um.checkForumPolicy(nPolicy.Name, nPolicy.MinAdmins, nPolicy.MaxAdmins,
                nPolicy.MinModerators, nPolicy.MaxModerators), "true");
        }
    }
}
