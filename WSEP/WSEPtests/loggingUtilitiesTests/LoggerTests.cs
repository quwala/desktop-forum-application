using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP.forumManagement.forumHandler;
using WSEP.forumManagement;
using WSEP.userManagement;
using WSEP.loggingUtilities;
using System.IO;

namespace WSEPtests.acceptanceTests
{
    [TestClass]
    public class LoggerTests
    {

        private ForumSystem fs;
        private UserManager um;
        private Forum f;
        private ForumLogger fl;

        [TestInitialize()]
        public void Initialize()
        {
            um = new UserManager();
            fs = new ForumSystem("superAdmin", um);
            fs.addForum("Test Forum");
            um.addForum("Test Forum");
            f = fs.getForum("Test Forum");
            fl = fs.getLogger();
        }

        [TestCleanup]
        public void TearDown()
        {
        }


        [TestMethod]
        public void Test_BasicLogging()
        {
            string expected1 = "Forum System was created.";
            string expected2 = "Successfully added forum Test Forum.";

            fl.log("Testing Logger successfully completed.");
            Assert.IsTrue(fl.getLog().Contains(expected1));
            Assert.IsTrue(fl.getLog().Contains(expected2));

        }

        [TestMethod]
        public void Test_ExceptionLogging()
        {
            string expected1 = "Name of the Sub Forum cannot be empty";
            try
            {
                f.addSubForum("");
            }
            catch (Exception e)
            {
                fl.logException(e);
            }
               
            Assert.IsTrue(fl.getLog().Contains(expected1));

        }

    }
}
