using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumManagement.forumHandler;
using System.Collections.Generic;
using WSEP_domain.userManagement;
using WSEP_domain.forumManagement;
using WSEP_domain.forumManagement.threadsHandler;

namespace DomainUnitTests
{
    [TestClass]
    public class UnitTestsForum
    {
        [TestInitialize]
        public void SetupTests()
        {
            Post.reset();
        }

        [TestMethod]
        public void UnitTestForumInvalidInput()
        {
            Assert.IsTrue(Forum.create(null) == null);
            Assert.IsTrue(Forum.create("null") == null);
            Assert.IsTrue(Forum.create("") == null);
            Assert.IsTrue(Forum.create(" forum") == null);
            ForumPolicy fp = new ForumPolicy();
            Assert.IsTrue(Forum.create("forum\nimportant", fp, new List<SubForum>()) == null);
            Assert.IsTrue(Forum.create("forum", null, new List<SubForum>()) == null);
        }

        [TestMethod]
        public void UnitTestForumCreateForum()
        {
            ForumPolicy fp = new ForumPolicy();
            Assert.IsTrue(Forum.create("forum", fp, new List<SubForum>() { SubForum.create("forum", "subforum") }) != null);
        }

        [TestMethod]
        public void UnitTestForumCreateSubForum()
        {
            ForumPolicy fp = new ForumPolicy();
            Forum f = Forum.create("forum", fp, new List<SubForum>());
            Assert.IsTrue(f != null);
            Assert.IsTrue(f.addSubForum("subForum").Equals("true"));
            Assert.IsTrue(!f.addSubForum("subForum").Equals("true"));
            Assert.IsTrue(!f.addSubForum(null).Equals("true"));
        }

        [TestMethod]
        public void UnitTestForumWritePost()
        {
            Forum f = Forum.create("forum");
            f.addSubForum("subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Assert.IsTrue(f.writePost("subforum", 0, u, "title", "content").Equals("true"));
            Assert.IsTrue(f.writePost("subforum", 1, u, "title1", "content1").Equals("true"));
            Assert.IsTrue(!f.writePost("sub_forum", 0, u, "title", "content").Equals("true"));
        }

        [TestMethod]
        public void UnitTestForumDeletePost()
        {
            Forum f = Forum.create("forum");
            f.addSubForum("subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            f.writePost("subforum", 0, u, "title", "content");
            Assert.IsTrue(f.deletePost("subforum", 1, postDeletionPermission.SUPER_ADMIN, permission.SUPER_ADMIN, "superAdmin").Equals("true"));
            f.writePost("subforum", 0, u, "title", "content");
            Assert.IsTrue(f.deletePost("subforum", 2, postDeletionPermission.SUPER_ADMIN, permission.SUPER_ADMIN, "superAdmin").Equals("true"));
            f.writePost("subforum", 0, u, "title", "content");
            Assert.IsTrue(!f.deletePost("sub_forum", 2, postDeletionPermission.SUPER_ADMIN, permission.SUPER_ADMIN, "superAdmin").Equals("true"));
        }

        [TestMethod]
        public void UnitTestForumEditPost()
        {
            Forum f = Forum.create("forum");
            f.addSubForum("subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            f.writePost("subforum", 0, u, "title", "content");
            Assert.IsTrue(f.editPost("subforum", 1, "user", permission.MODERATOR, "new content").Equals("true"));
            Assert.IsTrue(f.editPost("subforum", 1, "superAdmin", permission.SUPER_ADMIN, "new content1").Equals("true"));
            Assert.IsTrue(!f.editPost("subforum", 1, "user1", permission.MEMBER, "new content2").Equals("true"));
            Assert.IsTrue(!f.editPost("sub_forum", 1, "superAdmin", permission.SUPER_ADMIN, "new content3").Equals("true"));
        }

        [TestMethod]
        public void UnitTestForumMemberPosts()
        {
            Forum f = Forum.create("forum");
            f.addSubForum("subforum");
            User u1 = User.create("user1", "password", "eMail1@gmail.com", "q", "a");
            User u2 = User.create("user2", "password", "eMail2@gmail.com", "q", "a");
            User u3 = User.create("user3", "password", "eMail3@gmail.com", "q", "a");
            f.writePost("subforum", 0, u1, "hello", "to user2");
            f.writePost("subforum", 1, u2, "hello", "to user1");
            f.writePost("subforum", 2, u1, "how are you", "user2?");
            f.writePost("subforum", 3, u2, "good", "how are you?");
            f.writePost("subforum", 4, u1, "fine", "thanks");
            f.writePost("subforum", 0, u1, "is there", "anyone else here?");
            f.writePost("subforum", 6, u3, "yes", "user3");
            Assert.IsTrue(f.getListOfMemberMessages(new List<Tuple<string, string, DateTime, int>>(), "user1").Count == 4);
            Assert.IsTrue(f.getListOfMemberMessages(new List<Tuple<string, string, DateTime, int>>(), "user2").Count == 2);
            Assert.IsTrue(f.getListOfMemberMessages(new List<Tuple<string, string, DateTime, int>>(), "user3").Count == 1);
        }

        [TestMethod]
        public void UnitTestForumGetSubForumsList()
        {
            Forum f = Forum.create("forum");
            List<string> l = new List<string>();
            f.getSubForumsList(l);
            Assert.IsTrue(l.Count == 0);
            f.addSubForum("subforum1");
            f.addSubForum("subforum2");
            f.addSubForum("subforum3");
            f.getSubForumsList(l);
            Assert.IsTrue(l.Count == 3);
            Assert.IsTrue(l.Contains("subforum1"));
            Assert.IsTrue(l.Contains("subforum2"));
            Assert.IsTrue(l.Contains("subforum3"));
        }

        [TestMethod]
        public void UnitTestForumGetThread()
        {
            Forum f = Forum.create("forum");
            f.addSubForum("subforum");
            User u1 = User.create("user1", "password", "eMail1@gmail.com", "q", "a");
            User u2 = User.create("user2", "password", "eMail2@gmail.com", "q", "a");
            User u3 = User.create("user3", "password", "eMail3@gmail.com", "q", "a");
            f.writePost("subforum", 0, u1, "hello", "to user2");
            f.writePost("subforum", 1, u2, "hello", "to user1");
            f.writePost("subforum", 2, u1, "how are you", "user2?");
            f.writePost("subforum", 3, u2, "good", "how are you?");
            f.writePost("subforum", 4, u1, "fine", "thanks");
            List<Tuple<string, string, DateTime, int, int, string, DateTime>> l = new List<Tuple<string, string, DateTime, int, int, string, DateTime>>();
            f.addPostsToList(l, "subforum", 1, "user1");
            Assert.IsTrue(l.Count == 5);
            l.Clear();
            f.addPostsToList(l, "subforum", 3, "user1");
            Assert.IsTrue(l.Count != 3);
            Assert.IsTrue(l.Count == 0);
            l.Clear();
            f.addPostsToList(l, "sub_forum", 1, "user1");
            Assert.IsTrue(l.Count == 0);
        }

        [TestMethod]
        public void UnitTestForumGetThreads()
        {
            Forum f = Forum.create("forum");
            f.addSubForum("subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            f.writePost("subforum", 0, u, "cars", "");
            f.writePost("subforum", 0, u, "sports", "");
            f.writePost("subforum", 0, u, "food", "");
            List<Tuple<string, DateTime, int>> l = new List<Tuple<string, DateTime, int>>();
            f.addThreadsToList(l, "subforum", "user");
            Assert.IsTrue(l.Count == 3);
            l.Clear();
            f.writePost("subforum", 1, u, "bmw", "");
            f.addThreadsToList(l, "subforum", "user");
            Assert.IsTrue(l.Count == 3);
            l.Clear();
            f.addThreadsToList(l, "sub_forum", "user");
            Assert.IsTrue(l.Count == 0);
        }
    }
}
