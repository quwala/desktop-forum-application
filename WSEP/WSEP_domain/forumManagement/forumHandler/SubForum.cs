using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP_domain.forumManagement.threadsHandler;
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

        public static SubForum create(string forumName, string name)
        {
            List<string> input = new List<string>() { forumName, name };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            return new SubForum(forumName, name);
        }

        private SubForum(string forumName, string name)
        {
            _forumName = forumName;
            _name = name;
            _threads = new List<Post>();
            _numOfPosts = 0;
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
            if (repliedTo == null)
            {
                _threads.Add(post);
                if (!_threads.Contains(post))
                {
                    return "Could not write post.";
                }
                _numOfPosts++;
                return SUCCESS;
            }
            // start synchronization
            if (isLiveThread(repliedTo))
            {
                if (!repliedTo.addReply(post))
                {
                    return "Could not reply.";
                }
                _numOfPosts++;
                string msg = "Hello " + repliedTo.getWriter() + ",\n" + user.getUsername() + " has replied to your post in forum " + _forumName + "," +
                    " sub forum " + _name + ", on the topic " + getThreadName(post);
                Constants.sendMail(repliedTo.getWriterEMail(), "Your post have been replied to", msg);
                return SUCCESS;
            }
            return "The post you are trying to reply to has been deleted.";
        }
        // deletePost need to be synchronized
        public string deletePost(int postNo, postDeletionPermission pdp, permission p, string requestingUser)
        {
            Post post = searchPost(_threads, postNo);
            if (post == null)
            {
                return Constants.SUCCESS;
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
                    return Constants.noPermissionToDeletePost(requestingUser);
            }
            if (post.delete())
            {
                _numOfPosts--;
                return Constants.SUCCESS;
            }
            return "Could not delete post";
        }

        public string editPost(int postNo, string requestingUser, permission p, string content)
        {
            Post post = searchPost(_threads, postNo);
            if (post == null || (post.getTitle().Equals("") && content.Equals("")))
            {
                return Constants.INVALID_INPUT;
            }
            post.setContent(content);
            if (content.Equals(post.getContent()))
            {
                return Constants.SUCCESS;
            }
            return "Could not edit post.";
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

        public void addPostsToList(List<Tuple<string, string, DateTime, int, int, string, DateTime>> list, int openPostNo)
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
                    return p;
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
