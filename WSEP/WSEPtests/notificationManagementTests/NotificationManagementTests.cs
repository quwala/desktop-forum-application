using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_service.notificationManagementService;

namespace WSEP_tests.notificationManagementTests
{
    [TestClass]
    public class NotificationManagementTests
    {
        private INotificationsManager nm;

        [TestInitialize()]
        public void Initialize()
        {
            nm = new NotificationManager();
        }

        [TestMethod]
        public void NotifyPostTestsGoodInput()
        {
            // good input
            Assert.IsTrue(nm.NotifyPost("responser's name", "post owner's name", "dominitz@post.bgu.ac.il")); 
        }

        [TestMethod]
        public void NotifyPostTestsNullArgument()
        {
            // null argument
            Assert.IsFalse(nm.NotifyPost("responser's name", null, "magal.w1@gmail.com"));
        }

        [TestMethod]
        public void NotifyPostTestsEmptyArgument()
        {
            // empty argument
            Assert.IsFalse(nm.NotifyPost("", "post owner's name", "magal.w1@gmail.com"));
        }

        [TestMethod]
        public void NotifyPostTestsWrongEmail()
        {
            // wrong email address
            Assert.IsFalse(nm.NotifyPost("responser's name", "post owner's name", "aaaaaaaaaa"));
        }

    }
}
