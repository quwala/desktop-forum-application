using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.userManagement;
using System.Collections.Generic;

namespace DomainUnitTests
{
    [TestClass]
    public class UnitTestsUser
    {
        [TestMethod]
        public void UnitTestUserCreate()
        {
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", "q", "a") != null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") != null);
        }

        [TestMethod]
        public void UnitTestUserCreateInvalidUser()
        {
            Assert.IsTrue(User.create(null, "password", "eMail@gmail.com", "q", "a") == null);
            Assert.IsTrue(User.create("null", "password", "eMail@gmail.com", "q", "a") == null);
            Assert.IsTrue(User.create("", "password", "eMail@gmail.com", "q", "a") == null);
            Assert.IsTrue(User.create("user\n", "password", "eMail@gmail.com", "q", "a") == null);
            Assert.IsTrue(User.create(" user", "password", "eMail@gmail.com", "q", "a") == null);
            Assert.IsTrue(User.create("user", null, "eMail@gmail.com", "q", "a") == null);
            Assert.IsTrue(User.create("user", "null", "eMail@gmail.com", "q", "a") == null);
            Assert.IsTrue(User.create("user", "", "eMail@gmail.com", "q", "a") == null);
            Assert.IsTrue(User.create("user", "password\n", "eMail@gmail.com", "q", "a") == null);
            Assert.IsTrue(User.create("user", " password", "eMail@gmail.com", "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", null, "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "null", "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "", "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail\n.com", "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", " eMail@gmail.com", "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMailgmail.com", "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "@eMailgmail.com", "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@g@mail.com", "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmailcom", "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@.gmail.com", "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmailcom.", "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com.", "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", null, "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", "null", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", "", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", "\nq", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", " q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", "q", null) == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", "q", "null") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", "q", "") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", "q", "a\n") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", "q", " ") == null);
            Assert.IsTrue(User.create(null, "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("null", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user\n", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create(" user", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", null, "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "null", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password\n", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", " password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", null, DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "null", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail\n.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", " eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMailgmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "@eMailgmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@g@mail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmailcom", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@.gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmailcom.", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com.", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, null, "q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), null, "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "null", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q\n", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), " q", "a") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", null) == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "null") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", "\na") == null);
            Assert.IsTrue(User.create("user", "password", "eMail@gmail.com", DateTime.Now, DateTime.Now, new List<Tuple<string, List<PrivateMessage>>>(), "q", " a") == null);
        }

        [TestMethod]
        public void UnitTestUserSendMessage()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Assert.IsTrue(u.sendMessage("user2", "hello").Equals("true"));
            Assert.IsTrue(u.sendMessage("user2", "how are you?").Equals("true"));
            Assert.IsTrue(u.sendMessage("user3", "hi!").Equals("true"));
            Assert.IsTrue(u.sendMessage("user3", "whats up?").Equals("true"));
        }

        [TestMethod]
        public void UnitTestUserSendInvalidMessage()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Assert.IsTrue(!u.sendMessage(null, "hello").Equals("true"));
            Assert.IsTrue(!u.sendMessage("null", "hello").Equals("true"));
            Assert.IsTrue(!u.sendMessage("", "hello").Equals("true"));
            Assert.IsTrue(!u.sendMessage("user2\n", "hello").Equals("true"));
            Assert.IsTrue(!u.sendMessage(" user2", "hello").Equals("true"));
            Assert.IsTrue(!u.sendMessage("user2", null).Equals("true"));
            Assert.IsTrue(!u.sendMessage("user2", "null").Equals("true"));
            Assert.IsTrue(!u.sendMessage("user2", "").Equals("true"));
        }

        [TestMethod]
        public void UnitTestUserGetMessage()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Assert.IsTrue(u.getMessage("user2", "hello").Equals("true"));
            Assert.IsTrue(u.getMessage("user2", "how are you?").Equals("true"));
            Assert.IsTrue(u.getMessage("user3", "hi!").Equals("true"));
            Assert.IsTrue(u.getMessage("user3", "whats up?").Equals("true"));
        }

        [TestMethod]
        public void UnitTestUserGetInvalidMessage()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Assert.IsTrue(!u.getMessage(null, "hello").Equals("true"));
            Assert.IsTrue(!u.getMessage("null", "hello").Equals("true"));
            Assert.IsTrue(!u.getMessage("", "hello").Equals("true"));
            Assert.IsTrue(!u.getMessage("user2\n", "hello").Equals("true"));
            Assert.IsTrue(!u.getMessage(" user2", "hello").Equals("true"));
            Assert.IsTrue(!u.getMessage("user2", null).Equals("true"));
            Assert.IsTrue(!u.getMessage("user2", "null").Equals("true"));
            Assert.IsTrue(!u.getMessage("user2", "").Equals("true"));
        }

        [TestMethod]
        public void UnitTestUserPrivateConversation()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Assert.IsTrue(u.sendMessage("user2", "hello").Equals("true"));
            Assert.IsTrue(u.getMessage("user2", "hi!").Equals("true"));
            Assert.IsTrue(u.sendMessage("user2", "how are you?").Equals("true"));
            Assert.IsTrue(u.getMessage("user2", "good, and you?").Equals("true"));
            Assert.IsTrue(u.sendMessage("user2", "all good").Equals("true"));
        }

        // get security question, get answer, set password (inc invalid)
        [TestMethod]
        public void UnitTestUserGettersSetters()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Assert.IsTrue(u.getUsername().Equals("user"));
            Assert.IsTrue(u.getPassword().Equals("password"));
            Assert.IsTrue(u.getEMail().Equals("eMail@gmail.com"));
            Assert.IsTrue(u.getSecurityQuestion().Equals("q"));
            Assert.IsTrue(u.getAnswer().Equals("a"));
            u.setPassword("new_password");
            Assert.IsTrue(u.getPassword().Equals("new_password"));
        }

        [TestMethod]
        public void UnitTestUserSetInvalidPassword()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            u.setPassword(null);
            Assert.IsTrue(u.getPassword().Equals("password"));
            u.setPassword("null");
            Assert.IsTrue(!u.getPassword().Equals("null"));
            Assert.IsTrue(u.getPassword().Equals("password"));
            u.setPassword("");
            Assert.IsTrue(!u.getPassword().Equals(""));
            Assert.IsTrue(u.getPassword().Equals("password"));
            u.setPassword("pass\nword");
            Assert.IsTrue(!u.getPassword().Equals("pass\nword"));
            Assert.IsTrue(u.getPassword().Equals("password"));
            u.setPassword(" password");
            Assert.IsTrue(!u.getPassword().Equals(" password"));
            Assert.IsTrue(u.getPassword().Equals("password"));
        }
    }
}
