using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.userManagement;
using System.Threading;

namespace DomainUnitTests
{
    [TestClass]
    public class UnitTestsPrivateMessage
    {
        [TestMethod]
        public void UnitTestPrivateMessageCreate()
        {
            Assert.IsTrue(PrivateMessage.create("user", "msg") != null);
            Assert.IsTrue(PrivateMessage.create("user", "msg", DateTime.Now) != null);
        }

        [TestMethod]
        public void UnitTestPrivateMessageCreateInvalidPM()
        {
            Assert.IsTrue(PrivateMessage.create(null, "msg") == null);
            Assert.IsTrue(PrivateMessage.create("null", "msg") == null);
            Assert.IsTrue(PrivateMessage.create("", "msg") == null);
            Assert.IsTrue(PrivateMessage.create("user\n", "msg") == null);
            Assert.IsTrue(PrivateMessage.create(" user", "msg") == null);
            Assert.IsTrue(PrivateMessage.create("user", null) == null);
            Assert.IsTrue(PrivateMessage.create("user", "null") == null);
            Assert.IsTrue(PrivateMessage.create(null, "msg", DateTime.Now) == null);
            Assert.IsTrue(PrivateMessage.create("null", "msg", DateTime.Now) == null);
            Assert.IsTrue(PrivateMessage.create("", "msg", DateTime.Now) == null);
            Assert.IsTrue(PrivateMessage.create("user\n", "msg", DateTime.Now) == null);
            Assert.IsTrue(PrivateMessage.create(" user", "msg", DateTime.Now) == null);
            Assert.IsTrue(PrivateMessage.create("user", null, DateTime.Now) == null);
            Assert.IsTrue(PrivateMessage.create("user", "null", DateTime.Now) == null);
        }

        [TestMethod]
        public void UnitTestPrivateMessageGetters()
        {
            DateTime dt = DateTime.Now;
            PrivateMessage pm = PrivateMessage.create("user", "msg", dt);
            Assert.IsTrue(pm.getCreationDate().Equals(dt));
        }
    }
}
