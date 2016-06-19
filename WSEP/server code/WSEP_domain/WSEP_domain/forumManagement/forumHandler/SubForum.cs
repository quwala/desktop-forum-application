using System;
using System.Collections.Generic;
using WSEP_domain.forumManagement.threadsHandler;
using WSEP_domain.forumSystem;
using WSEP_domain.userManagement;

namespace WSEP_domain.forumManagement.forumHandler
{
    public class SubForum
    {
        private const string SUCCESS = "true";

        private string _forumName;
        private string _name;
        private List<Post> _threads;
        private int _numOfPosts;
        private List<string> _interativeNotifications;

        public static SubForum create(string forumName, string name)
        {
            List<string> input = new List<string>() { forumName, name };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            return new SubForum(forumName, name);
        }

        public static SubForum create(string forumName, string name, List<Post> threads)
        {
            List<string> input = new List<string>() { forumName, name };
            if (!Constants.isValidInput(input) || threads == null)
            {
                return null;
            }
            return new SubForum(forumName, name, threads);
        }

        private SubForum(string forumName, string name)
        {
            _forumName = forumName;
            _name = name;
            _threads = new List<Post>();
            _numOfPosts = 0;
            _interativeNotifications = new List<string>();
        }
        
        private SubForum(string forumName, string name, List<Post> threads)
        {
            _forumName = forumName;
            _name = name;
            _threads = threads;
            _numOfPosts = countPosts(threads);
            _interativeNotifications = new List<string>();
        }

        private int countPosts(List<Post> posts)
        {
            int ans = posts.Count;
            foreach (Post p in posts)
            {
                ans += countPosts(p.getReplies());
            }
            return ans;
        }

        public string getName()
        {
            return _name;
        }

        public int getNumOfPOsts()
        {
            return _numOfPosts;
        }

        // writePost incomplete - need to synchronize a part inside
        public string writePost(int parentPostNo, User user, string title, string content)
        {
            Post repliedTo = searchPost(_threads, parentPostNo);
            Post post = Post.create(title, content, user, repliedTo);
            if (post == null)
            {
                return Constants.INVALID_INPUT;
            }
            if (!ForumSystem._testFlag)
            {
                if (!ForumSystem._db.addSubForumPost(title, content, _forumName, _name, user.getUsername(), post.getCreationDate(), post.getSN(), parentPostNo))
                {
                    return Constants.DB_ERROR;
                }
            }
            string deletedParent = "The post you are trying to reply to has been deleted.";
            if (repliedTo == null)
            {
                if (parentPostNo > 0)
                {
                    return deletedParent;
                }
                _threads.Add(post);
                if (!_threads.Contains(post))
                {
                    return "Could not write post."; // cannot cover this case
                }
                _numOfPosts++;
                ForumSystem.notify("A new thread has been opened in the sub forum you are currently in.", _interativeNotifications, _forumName);
                return SUCCESS;
            }
            // start synchronization
            if (isLiveThread(repliedTo))
            {
                if (!repliedTo.addReply(post, _forumName))
                {
                    return "Could not reply."; // cannot cover this case
                }
                _numOfPosts++;
                string msg = "Hello " + repliedTo.getWriter() + ",\n" + user.getUsername() + " has replied to your post in forum " + _forumName + "," +
                    " sub forum " + _name + ", on the topic " + getThreadName(post);
                Constants.sendMail(repliedTo.getWriterEMail(), "Your post have been replied to", msg);
                return SUCCESS;
            }
            return deletedParent; // cannot cover this case
        }
        // deletePost need to be synchronized
        public string deletePost(int postNo, postDeletionPermission pdp, permission p, string requestingUser)
        {
            Post post = searchPost(_threads, postNo);
            if (post == null)
            {
                return "Post doesn't exist";
            }
            switch (pdp)
            {
                case postDeletionPermission.WRITER:
                    if (p < permission.MODERATOR && !post.getWriter().Equals(requestingUser))
                    {
                        return Constants.noPermissionToDeletePost(requestingUser);
                    }
                    break;
                case postDeletionPermission.MODERATOR:
                    if (p < permission.MODERATOR)
                    {
                        return Constants.noPermissionToDeletePost(requestingUser);
                    }
                    break;
                case postDeletionPermission.ADMIN:
                    if (p < permission.ADMIN)
                    {
                        return Constants.noPermissionToDeletePost(requestingUser);
                    }
                    break;
                case postDeletionPermission.SUPER_ADMIN:
                    if (p < permission.SUPER_ADMIN)
                    {
                        return Constants.noPermissionToDeletePost(requestingUser);
                    }
                    break;
                default:
                    if (p != permission.SUPER_ADMIN)
                    {
                        return Constants.noPermissionToDeletePost(requestingUser);
                    }
                    break;
            }
            if (!ForumSystem._testFlag)
            {
                if (!ForumSystem._db.removeSubForumPost(post.getSN()))
                {
                    return Constants.DB_ERROR;
                }
            }
            if (_threads.Contains(post))
            {
                _threads.Remove(post);
                if (_threads.Contains(post))
                {
                    throw new ShouldNotHappenException(); // cannot cover this case
                }
                List<Tuple<string, string, DateTime, int, int, string, DateTime>> list = new List<Tuple<string, string, DateTime, int, int, string, DateTime>>();
                post.getAllTree(list);
                _numOfPosts -= list.Count;
                ForumSystem.notify("A thread you are following has been deleted.", post.getAllFollowers(), _forumName);
                ForumSystem.notify("A thread you are currently watching has been deleted.", post.getInteractiveFollowers(), _forumName);
                return Constants.SUCCESS;
            }
            Tuple<bool, List<string>> deletion = post.delete();
            if (deletion.Item1)
            {
                List<Tuple<string, string, DateTime, int, int, string, DateTime>> list = new List<Tuple<string, string, DateTime, int, int, string, DateTime>>();
                post.getAllTree(list);
                _numOfPosts -= list.Count;
                ForumSystem.notify("A post you are following has been deleted.", post.getAllFollowers(), _forumName);
                ForumSystem.notify("A post in a thread you are currently watching has been deleted.", getAncestor(post).getInteractiveFollowers(), _forumName);
                return Constants.SUCCESS;
            }
            return "Could not delete post"; // cannot cover this case
        }

        public string editPost(int postNo, string requestingUser, permission p, string content)
        {
            Post post = searchPost(_threads, postNo);
            if (post == null)
            {
                return Constants.INVALID_INPUT;
            }
            if (!ForumSystem._testFlag)
            {
                if (!ForumSystem._db.changeForumPost(post.getTitle(), content, _forumName, _name, postNo))
                {
                    return Constants.DB_ERROR;
                }
            }
            if (p < permission.MODERATOR && !post.getWriter().Equals(requestingUser))
            {
                return Constants.UNAUTHORIZED;
            }
            post.setContent(content);
            if (content.Equals(post.getContent()))
            {
                ForumSystem.notify("A post you are following has been changed.", post.getAllFollowers(), _forumName);
                ForumSystem.notify("A post in a thread you are currently watching has been changed.", getAncestor(post).getInteractiveFollowers(), _forumName);
                return Constants.SUCCESS;
            }
            return "Could not edit post."; // cannot cover this case
        }

        public void addUserMessages(List<Tuple<string, string, DateTime, int>> list, string username)
        {
            addMessagesFromList(list, username, _threads);
        }

        public void addThreadsToList(List<Tuple<string, DateTime, int>> list)
        {
            foreach (Post thread in _threads)
            {
                list.Add(new Tuple<string, DateTime, int>(thread.getTitle(), thread.getCreationDate(), thread.getSN()));
            }
        }

        public void addPostsToList(List<Tuple<string, string, DateTime, int, int, string, DateTime>> list, int openPostNo, string requsetingUser)
        {
            Post p = null;
            foreach (Post post in _threads)
            {
                if (post.getSN() == openPostNo)
                {
                    p = post;
                    break;
                }
            }
            if (p == null)
            {
                return;
            }
            p.getAllTree(list);
            p.observe("interactive", requsetingUser);
        }

        public void observe(string username)
        {
            _interativeNotifications.Add(username);
            foreach (Post p in _threads)
            {
                p.unobserve(username);
            }
        }

        public void unobserve(string username)
        {
            _interativeNotifications.Remove(username);
        }

        private Post getAncestor(Post post)
        {
            Post main = post;
            while (main.getAncestor() != null)
            {
                main = main.getAncestor();
            }
            return _threads.Contains(main) ? main : null;
        }

        private bool isLiveThread(Post post)
        {
            Post main = post;
            while (main.getAncestor() != null)
            {
                main = main.getAncestor();
            }
            return _threads.Contains(main);
        }

        private Post searchPost(List<Post> list, int postNo)
        {
            foreach (Post p in list)
            {
                if (p.getSN() == postNo)
                {
                    return p;
                }
                Post post = searchPost(p.getReplies(), postNo);
                if (post != null)
                {
                    return post;
                }
            }
            return null;
        }

        private void addMessagesFromList(List<Tuple<string, string, DateTime, int>> list, string username, List<Post> posts)
        {
            foreach (Post p in posts)
            {
                if (p.getWriter().Equals(username))
                {
                    list.Add(new Tuple<string, string, DateTime, int>(p.getTitle(), p.getContent(), p.getCreationDate(), p.getSN()));
                }
                addMessagesFromList(list, username, p.getReplies());
            }
        }

        private string getThreadName(Post post)
        {
            Post main = post;
            while (main.getAncestor() != null)
            {
                main = main.getAncestor();
            }
            return main.getTitle();
        }
    }
}
