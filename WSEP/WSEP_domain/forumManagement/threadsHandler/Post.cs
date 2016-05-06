using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP_domain.userManagement;

namespace WSEP_domain.forumManagement.threadsHandler
{
    public class Post
    {
        static int sn = 1;

        private string _title;
        private string _content;
        private User _user;
        private DateTime _date;
        private List<Post> _replies;
        private Post _repliedTo;
        private int _sn;

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
                    return null;
                }
            }
            if (!Constants.isValidInput(input) || !Constants.isValidPost(title, content))
            {
                return null;
            }
            return new Post(title, content, user, repliedTo);
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
            _content = content;
        }

        public List<Post> getReplies()
        {
            return _replies;
        }

        public bool addReply(Post reply)
        {
            _replies.Add(reply);
            return (_replies.Contains(reply));
        }

        public bool delete()
        {
            _repliedTo._replies.Remove(this);
            return _repliedTo._replies.Contains(this);
        }

        public void getAllTree(List<Tuple<string, string, DateTime, int, int, string, DateTime>> list)
        {
            list.Add(new Tuple<string, string, DateTime, int, int, string, DateTime>(_title, _content, _date, _sn, _repliedTo.getSN(), _user.getUsername(), _user.getRegistrationDate()));
            foreach (Post p in _replies)
            {
                p.getAllTree(list);
            }
        }
    }
}
