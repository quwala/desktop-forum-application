using System;
using System.Collections.Generic;
using WSEP_domain.forumSystem;
using WSEP_domain.userManagement;

namespace WSEP_domain.forumManagement.threadsHandler
{
    public class Post
    {
        private static int sn = 1;

        private string _title;
        private string _content;
        private User _user;
        private DateTime _date;
        private List<Post> _replies;
        private Post _repliedTo;
        private int _sn;
        private List<string> _followChanges;
        private List<string> _interactiveNotifications;

        public static Post create(string title, string content, User user, Post repliedTo)
        {
            if (user == null)
            {
                return null;
            }
            List<string> input = new List<string>() { user.getUsername() };
            if (repliedTo != null)
            {
                if (!Constants.isValidPost(repliedTo.getTitle(), repliedTo.getContent())) 
                {
                    return null; // cannot cover condition
                }
            }
            if (!Constants.isValidInput(input) || !Constants.isValidPost(title, content)) // cannot cover first condition
            {
                return null;
            }
            return new Post(title, content, user, repliedTo);
        }

        public static Post create(string title, string content, User user, Post repliedTo, int serialNumber, DateTime creationDate, List<string> followers)
        {
            if (user == null || followers == null)
            {
                return null;
            }
            List<string> input = new List<string>() { user.getUsername() };
            foreach (string str in followers)
            {
                input.Add(str);
            }
            if (repliedTo != null)
            {
                if (!Constants.isValidPost(repliedTo.getTitle(), repliedTo.getContent()))
                {
                    return null; // cannot cover condition
                }
            }
            if (!Constants.isValidInput(input) || !Constants.isValidPost(title, content)) // cannot cover first condition
            {
                return null;
            }
            sn = Math.Max(sn, serialNumber);
            return new Post(title, content, user, repliedTo, serialNumber, creationDate, followers);
        }

        private Post(string title, string content, User user, Post repliedTo)
        {
            _repliedTo = repliedTo;
            _title = title;
            _content = content;
            _user = user;
            _date = DateTime.Now;
            _replies = new List<Post>();
            _sn = sn++;
            _followChanges = new List<string>();
            _interactiveNotifications = new List<string>();
        }

        private Post(string title, string content, User user, Post repliedTo, int serialNumber, DateTime creationDate, List<string> followers)
        {
            _repliedTo = repliedTo;
            _title = title;
            _content = content;
            _user = user;
            _date = creationDate;
            _replies = new List<Post>();
            _sn = serialNumber;
            _followChanges = followers;
            _interactiveNotifications = new List<string>();
        }

        public void readReplies(List<Post> replies)
        {
            // verify entire list and inner lists does not contain this post
            _replies = replies;
        }

        public string getTitle()
        {
            return _title;
        }

        public string getContent()
        {
            return _content;
        }

        public string getWriter()
        {
            return _user.getUsername();
        }

        public string getWriterEMail()
        {
            return _user.getEMail();
        }

        public Post getAncestor()
        {
            return _repliedTo;
        }

        public int getSN()
        {
            return _sn;
        }

        public DateTime getCreationDate()
        {
            return _date;
        }

        public void setContent(string content)
        {
            if (content != null && !content.Equals("null") && !(_title.Equals("") && content.Equals(""))){
                _content = content;
            }
        }

        public List<Post> getReplies()
        {
            return _replies;
        }

        public bool addReply(Post reply, string forumName)
        {
            // verify entire tree of reply does not contain this post
            _replies.Add(reply);
            if (_replies.Contains(reply))
            {
                observe("follow", reply.getWriter());
                if (!ForumSystem._testFlag)
                {
                    if (ForumSystem._db.addPostFollower(_sn, reply.getWriter()))
                    {
                        ForumSystem.notify("A new reply has been added to the thread you are currently in.", _interactiveNotifications, forumName);
                        return true;
                    }
                    return false;
                }
                ForumSystem.notify("A new reply has been added to the thread you are currently in.", _interactiveNotifications, forumName);
                return true;
            }
            return false;
        }

        public Tuple<bool, List<string>> delete()
        {
            _repliedTo._replies.Remove(this);
            if (!_repliedTo._replies.Contains(this))
            {
                List<string> followers = new List<string>();
                getInnerFollowers(followers);
                foreach (string s in _followChanges)
                {
                    if (!followers.Contains(s))
                    {
                        followers.Add(s);
                    }
                }
                return new Tuple<bool, List<string>>(true, followers);
            }
            return new Tuple<bool, List<string>>(false, null); // cannot cover this case
        }

        public void getAllTree(List<Tuple<string, string, DateTime, int, int, string, DateTime>> list)
        {
            int rsn = _repliedTo == null ? 0 : _repliedTo.getSN();
            list.Add(new Tuple<string, string, DateTime, int, int, string, DateTime>(_title, _content, _date, _sn, rsn, _user.getUsername(), _user.getRegistrationDate()));
            foreach (Post p in _replies)
            {
                p.getAllTree(list);
            }
        }

        public List<string> getFollowers()
        {
            return _followChanges;
        }

        public List<string> getAllFollowers()
        {
            List<string> followers = new List<string>();
            getInnerFollowers(followers);
            foreach (string s in _followChanges)
            {
                if (!followers.Contains(s))
                {
                    followers.Add(s);
                }
            }
            return followers;
        }

        public List<string> getInteractiveFollowers()
        {
            return _interactiveNotifications;
        }

        public static void reset()
        {
            sn = 1;
        }

        public void observe(string list, string username)
        {
            switch (list)
            {
                case "interactive":
                    _interactiveNotifications.Add(username);
                    break;
                case "follow":
                    _followChanges.Add(username);
                    break;
            }
        }

        public void unobserve(string username)
        {
            _interactiveNotifications.Remove(username);
        }

        private void getInnerFollowers(List<string> followers)
        {
            foreach (Post p in _replies)
            {
                foreach (string s in p._followChanges)
                {
                    if (!followers.Contains(s))
                    {
                        followers.Add(s);
                    }
                }
                foreach (string s in p._followChanges)
                {
                    p.getInnerFollowers(followers);
                }
            }
        }
    }
}
