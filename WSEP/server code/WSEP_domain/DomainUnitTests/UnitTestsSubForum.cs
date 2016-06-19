using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSEP_domain.forumManagement.forumHandler;
using System.Collections.Generic;
using WSEP_domain.forumManagement.threadsHandler;
using WSEP_domain.userManagement;

namespace DomainUnitTests
{
    [TestClass]
    public class UnitTestsSubForum
    {
        [TestInitialize]
        public void SetupTests()
        {
            Post.reset();
        }

        [TestMethod]
        public void UnitTestSubForumCreate()
        {
            Assert.IsTrue(SubForum.create("forum", "subforum", new List<Post>()) != null);
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Post t1 = Post.create("title1", "content1", u, null);
            Post t2 = Post.create("title2", "content2", u, null);
            Post t3 = Post.create("title3", "content3", u, null);
            Post p11 = Post.create("title11", "content11", u, t1);
            Post p21 = Post.create("title21", "content21", u, t2);
            Post p12 = Post.create("title12", "content12", u, p11);
            Post p31 = Post.create("title31", "content31", u, t3);
            p11.readReplies(new List<Post> { p12 });
            t1.readReplies(new List<Post> { p11 });
            t2.readReplies(new List<Post> { p21 });
            t3.readReplies(new List<Post> { p31 });
            Assert.IsTrue(SubForum.create("forum", "sub_forum", new List<Post> { t1, t2, t3 }) != null);
        }

        [TestMethod]
        public void UnitTestSubForumInvalidSubForum()
        {
            Assert.IsTrue(SubForum.create(null, "subforum", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("null", "subforum", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("", "subforum", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum\n", "subforum", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create(" forum", "subforum", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum", null, new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum", "null", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum", "", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum", "subforum\n", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum", " subforum", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum", "subforum", null) == null);
            Assert.IsTrue(SubForum.create(null, "subforum", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("null", "subforum", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("", "subforum", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum\n", "subforum", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create(" forum", "subforum", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum", null, new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum", "null", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum", "", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum", "subforum\n", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum", " subforum", new List<Post>()) == null);
            Assert.IsTrue(SubForum.create("forum", "subforum", null) == null);
        }

        [TestMethod]
        public void UnitTestSubForumWritePost()
        {
            SubForum sf = SubForum.create("forum", "subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Assert.IsTrue(sf.writePost(0, u, "title", "content").Equals("true"));
        }

        [TestMethod]
        public void UnitTestSubForumWriteInvalidPost()
        {
            SubForum sf = SubForum.create("forum", "subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Assert.IsTrue(!sf.writePost(0, u, null, "content").Equals("true"));
            Assert.IsTrue(!sf.writePost(0, u, "null", "content").Equals("true"));
            Assert.IsTrue(!sf.writePost(0, u, "title", null).Equals("true"));
            Assert.IsTrue(!sf.writePost(0, u, "title", "null").Equals("true"));
            Assert.IsTrue(!sf.writePost(0, u, "", "").Equals("true"));
        }

        [TestMethod]
        public void UnitTestSubForumWriteReply()
        {
            SubForum sf = SubForum.create("forum", "subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            sf.writePost(0, u, "title", "content");
            Assert.IsTrue(sf.writePost(1, u, "reply_title", "reply_content").Equals("true"));
        }

        [TestMethod]
        public void UnitTestSubForumDeletePost()
        {
            SubForum sf = SubForum.create("forum", "subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            sf.writePost(0, u, "title1", "content1");
            sf.writePost(0, u, "title2", "content2");
            sf.writePost(0, u, "title3", "content3");
            sf.writePost(0, u, "title4", "content4");
            sf.writePost(0, u, "title5", "content5");
            sf.writePost(0, u, "title6", "content6");
            Assert.IsTrue(sf.deletePost(1, postDeletionPermission.WRITER, permission.MEMBER, "user").Equals("true"));
            Assert.IsTrue(sf.deletePost(2, postDeletionPermission.WRITER, permission.ADMIN, "user").Equals("true"));
            Assert.IsTrue(sf.deletePost(3, postDeletionPermission.MODERATOR, permission.ADMIN, "user").Equals("true"));
            Assert.IsTrue(sf.deletePost(4, postDeletionPermission.ADMIN, permission.ADMIN, "user").Equals("true"));
            Assert.IsTrue(sf.deletePost(5, postDeletionPermission.SUPER_ADMIN, permission.SUPER_ADMIN, "superAdmin").Equals("true"));
            Assert.IsTrue(sf.deletePost(6, postDeletionPermission.INVALID, permission.SUPER_ADMIN, "superAdmin").Equals("true"));
        }

        [TestMethod]
        public void UnitTestSubForumDeletePostFail()
        {
            SubForum sf = SubForum.create("forum", "subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            sf.writePost(0, u, "title1", "content1");
            Assert.IsTrue(!sf.deletePost(0, postDeletionPermission.WRITER, permission.MEMBER, "user").Equals("true"));
            Assert.IsTrue(!sf.deletePost(1, postDeletionPermission.WRITER, permission.MEMBER, "user1").Equals("true"));
            Assert.IsTrue(!sf.deletePost(1, postDeletionPermission.MODERATOR, permission.MEMBER, "user").Equals("true"));
            Assert.IsTrue(!sf.deletePost(1, postDeletionPermission.ADMIN, permission.MEMBER, "user").Equals("true"));
            Assert.IsTrue(!sf.deletePost(1, postDeletionPermission.SUPER_ADMIN, permission.MEMBER, "user").Equals("true"));
            Assert.IsTrue(!sf.deletePost(1, postDeletionPermission.INVALID, permission.MEMBER, "user").Equals("true"));
        }

        [TestMethod]
        public void UnitTestSubForumDeleteReply()
        {
            SubForum sf = SubForum.create("forum", "subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            sf.writePost(0, u, "title1", "content1");
            sf.writePost(1, u, "title2", "content2");
            sf.writePost(2, u, "title3", "content3");
            Assert.IsTrue(sf.deletePost(2, postDeletionPermission.WRITER, permission.ADMIN, "user").Equals("true"));
            Assert.IsTrue(sf.getNumOfPOsts() == 1);
        }

        [TestMethod]
        public void UnitTestSubForumDeleteReplyFail()
        {
            SubForum sf = SubForum.create("forum", "subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            sf.writePost(0, u, "title1", "content1");
            sf.writePost(1, u, "title2", "content2");
            sf.writePost(2, u, "title3", "content3");
            Assert.IsTrue(!sf.deletePost(2, postDeletionPermission.WRITER, permission.MEMBER, "user1").Equals("true"));
            Assert.IsTrue(!sf.deletePost(2, postDeletionPermission.MODERATOR, permission.MEMBER, "user").Equals("true"));
            Assert.IsTrue(!sf.deletePost(2, postDeletionPermission.ADMIN, permission.MEMBER, "user").Equals("true"));
            Assert.IsTrue(!sf.deletePost(2, postDeletionPermission.SUPER_ADMIN, permission.MEMBER, "user").Equals("true"));
            Assert.IsTrue(!sf.deletePost(2, postDeletionPermission.INVALID, permission.MEMBER, "user").Equals("true"));
            Assert.IsTrue(sf.getNumOfPOsts() != 1);
            Assert.IsTrue(sf.getNumOfPOsts() == 3);
        }

        [TestMethod]
        public void UnitTestSubForumCountPosts()
        {
            SubForum sf = SubForum.create("forum", "subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            sf.writePost(0, u, "title1", "content1");
            sf.writePost(0, u, "title2", "content2");
            sf.writePost(0, u, "title3", "content3");
            sf.writePost(1, u, "title11", "content11");
            sf.writePost(2, u, "title21", "content21");
            sf.writePost(4, u, "title12", "content12");
            sf.writePost(3, u, "title31", "content31");
            Assert.IsTrue(sf.getNumOfPOsts() == 7);
            sf.deletePost(4, postDeletionPermission.WRITER, permission.MODERATOR, "user");
            Assert.IsTrue(sf.getNumOfPOsts() == 5);
            sf.deletePost(2, postDeletionPermission.MODERATOR, permission.MODERATOR, "moderator");
            Assert.IsTrue(sf.getNumOfPOsts() == 3);
        }

        [TestMethod]
        public void UnitTestSubForumReplyToDeletedThread()
        {
            SubForum sf = SubForum.create("forum", "subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            sf.writePost(0, u, "title1", "content1");
            sf.deletePost(1, postDeletionPermission.WRITER, permission.MODERATOR, "user");
            Assert.IsTrue(!sf.writePost(1, u, "title2", "content2").Equals("true"));
        }

        [TestMethod]
        public void UnitTestSubForumReplyToDeletedReply()
        {
            SubForum sf = SubForum.create("forum", "subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            sf.writePost(0, u, "title1", "content1");
            sf.writePost(1, u, "title2", "content2");
            sf.writePost(2, u, "title3", "content3");
            sf.deletePost(2, postDeletionPermission.WRITER, permission.MODERATOR, "user");
            Assert.IsTrue(!sf.writePost(2, u, "title4", "content4").Equals("true"));
        }

        [TestMethod]
        public void UnitTestSubForumEditPostFail()
        {
            SubForum sf = SubForum.create("forum", "subforum");
            User u = User.create("user", "password", "eMail@gmail.com", "q", "a");
            Assert.IsTrue(!sf.editPost(1, "superAdmin", permission.SUPER_ADMIN, "new content").Equals("true"));
            sf.writePost(0, u, "title1", "content1");
            Assert.IsTrue(!sf.editPost(2, "superAdmin", permission.SUPER_ADMIN, "new content").Equals("true"));
            sf.writePost(1, u, "title2", "content2");
            sf.deletePost(2, postDeletionPermission.MODERATOR, permission.SUPER_ADMIN, "superAdmin");
            Assert.IsTrue(!sf.editPost(2, "superAdmin", permission.SUPER_ADMIN, "new content").Equals("true"));
            sf.deletePost(1, postDeletionPermission.MODERATOR, permission.SUPER_ADMIN, "superAdmin");
            Assert.IsTrue(!sf.editPost(1, "superAdmin", permission.SUPER_ADMIN, "new content").Equals("true"));
        }
    }
}
