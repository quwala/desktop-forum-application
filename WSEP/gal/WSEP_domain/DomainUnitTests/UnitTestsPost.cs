using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumManagement.threadsHandler;
using WSEP_domain.userManagement;
using System.Collections.Generic;

namespace DomainUnitTests
{
    [TestClass]
    public class UnitTestsPost
    {
        [TestInitialize]
        public void SetupTests()
        {
            Post.reset();
        }

        [TestMethod]
        public void UnitTestPostCreatePost()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Assert.IsTrue(Post.create("title", "content", u, null) != null);
            Assert.IsTrue(Post.create("title", "content", u, null, 4, DateTime.Now, new List<string>() { "user1", "user2" }) != null);
        }

        [TestMethod]
        public void UnitTestPostNullUser()
        {
            Assert.IsTrue(Post.create("title", "content", null, null) == null);
            Assert.IsTrue(Post.create("title", "content", null, null, 4, DateTime.Now, new List<string>()) == null);
        }

        [TestMethod]
        public void UnitTestPostSetPostAsInvalid()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Post p = Post.create("", "content", u, null);
            p.setContent("");
            Assert.IsTrue(p.getContent().Equals("content"));
            p.setContent("null");
            Assert.IsTrue(p.getContent().Equals("content"));
            p.setContent(null);
            Assert.IsTrue(p.getContent().Equals("content"));
        }

        [TestMethod]
        public void UnitTestPostInvalidPost()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Assert.IsTrue(Post.create("", "", u, null) == null);
            Assert.IsTrue(Post.create(null, "content", u, null) == null);
            Assert.IsTrue(Post.create("title", null, u, null) == null);
            Assert.IsTrue(Post.create("null", "content", u, null) == null);
            Assert.IsTrue(Post.create("title", "null", u, null) == null);
            Assert.IsTrue(Post.create("", "", u, null, 4, DateTime.Now, new List<string>()) == null);
            Assert.IsTrue(Post.create(null, "content", u, null, 4, DateTime.Now, new List<string>()) == null);
            Assert.IsTrue(Post.create("title", null, u, null, 4, DateTime.Now, new List<string>()) == null);
            Assert.IsTrue(Post.create("null", "content", u, null, 4, DateTime.Now, new List<string>()) == null);
            Assert.IsTrue(Post.create("title", "null", u, null, 4, DateTime.Now, new List<string>()) == null);
            Assert.IsTrue(Post.create("title", "content", u, null, 4, DateTime.Now, null) == null);
        }

        [TestMethod]
        public void UnitTestPostReplyToPost()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Post p = Post.create("title", "content", u, null);
            Assert.IsTrue(Post.create("reply title", "reply content", u, p) != null);
            Assert.IsTrue(Post.create("title", "content", u, p, 4, DateTime.Now, new List<string>()) != null);
        }

        [TestMethod]
        public void UnitTestPostGettersSetters()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Post p1 = Post.create("title1", "content1", u, null);
            Post p2 = Post.create("title2", "content2", u, p1);
            p1.readReplies(new List<Post>() { p2 });
            Post p3 = Post.create("title3", "content3", u, p2);
            p2.readReplies(new List<Post>() { p3 });
            Assert.IsTrue(p2.getTitle().Equals("title2"));
            Assert.IsTrue(p2.getContent().Equals("content2"));
            Assert.IsTrue(p2.getWriter().Equals("user"));
            Assert.IsTrue(p2.getWriterEMail().Equals("eMail@gmail.com"));
            Assert.IsTrue(p2.getAncestor().Equals(p1));
            int sn1 = p1.getSN();
            int sn2 = p2.getSN();
            int sn3 = p3.getSN();
            Assert.IsTrue(sn1 >= 1);
            Assert.IsTrue(sn2 >= 1);
            Assert.IsTrue(sn3 >= 1);
            Assert.IsTrue(sn3 > sn2);
            Assert.IsTrue(sn2 > sn1);
            DateTime dt = DateTime.Now;
            Assert.IsTrue(p2.getCreationDate().CompareTo(dt) <= 0); // on fast runs it is actually 0
            p2.setContent("new content2");
            Assert.IsTrue(p2.getContent().Equals("new content2"));
            Assert.IsTrue(p2.getReplies().Contains(p3));
        }

        [TestMethod]
        public void UnitTestPostAddReply()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Post p1 = Post.create("title1", "content1", u, null);
            Post p2 = Post.create("title2", "content2", u, p1);
            Assert.IsTrue(p1.addReply(p2, "forumName"));
        }

        [TestMethod]
        public void UnitTestPostDelete()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Post p1 = Post.create("title1", "content1", u, null);
            Post p2 = Post.create("title2", "content2", u, p1);
            p1.readReplies(new List<Post>() { p2 });
            Assert.IsTrue(p2.delete().Item1);
        }

        [TestMethod]
        public void UnitTestPostGetRepliesTree()
        {
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Post p1 = Post.create("title1", "content1", u, null);
            Post p2 = Post.create("title2", "content2", u, p1);
            Post p3 = Post.create("title3", "content3", u, p1);
            p1.readReplies(new List<Post>() { p2, p3 });
            Post p4 = Post.create("title4", "content4", u, p2);
            p2.readReplies(new List<Post>() { p4 });
            List<Tuple<string, string, DateTime, int, int, string, DateTime>> list = new List<Tuple<string, string, DateTime, int, int, string, DateTime>>();
            p1.getAllTree(list);
            Assert.IsTrue(list.Count == 4);
        }

        [TestMethod]
        public void UnitTestPostGetFollowers()
        {
            User u1 = User.create("user1", "password", "eMail1@gmail.com", "q", "a");
            User u2 = User.create("user2", "password", "eMail2@gmail.com", "q", "a");
            User u3 = User.create("user3", "password", "eMail3@gmail.com", "q", "a");
            User u4 = User.create("user4", "password", "eMail4@gmail.com", "q", "a");
            Post p1 = Post.create("title1", "content1", u1, null);
            Post p2 = Post.create("title2", "content2", u2, p1);
            Post p3 = Post.create("title3", "content3", u3, p1);
            p1.addReply(p2, "forumName");
            p1.addReply(p3, "forumName");
            Post p4 = Post.create("title3", "content3", u4, p2);
            p2.addReply(p4, "forumName");
            Assert.IsTrue(p1.getFollowers().Count == 2);
        }
    }
}
