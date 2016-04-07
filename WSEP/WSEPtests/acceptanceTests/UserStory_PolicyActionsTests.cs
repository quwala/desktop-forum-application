using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement.forumHandler;
using WSEP.forumManagement;
using WSEP.userManagement;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class UserStory_PolicyActionsTests
    {

        private Forum f;
        private UserManager um;


        [TestInitialize()]
        public void Initialize()
        {
            ForumSystem fs = new ForumSystem("superAdmin");
            um = new UserManager();
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
        [ExpectedException(typeof(Exception),
            "Minimum number of admins cannot be smaller than 1.")]
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

            //Assert.IsFalse(um.checkForumPolicy(nPolicy.Name,nPolicy.MinAdmins, nPolicy.MaxAdmins,
              //  nPolicy.MinModerators,nPolicy.MaxModerators));
        }
    }
}
