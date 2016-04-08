using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.forumManagement.threadsHandler
{
    public class Post
    {
        private string _id;
        public string Id
        {
            get
            {
                return _id;
            }
        }

        private string _title;
        private string _content;
        private string _userName;
        private DateTime _date;
        private List<Post> _replies;
        private Post _replyTo;

        //thread constructor
        public Post(string title, string content, string userName)
        {
            if (content.Equals("") && title.Equals(""))
                throw new Exception("Post could not be created; title and content both are empty");

            _replyTo = null;
            _title = title;
            _content = content;
            _userName = userName;
            _date = DateTime.Now;
            _replies = new List<Post>();

            Guid guid = Guid.NewGuid();
            _id = guid.ToString();//generate code
        }

        internal Post findPost(string id)
        {
            if (this.Id == id)
                return this;

            foreach (Post p in _replies)
            {
                Post found = p.findPost(id);
                if (found != null)
                    return found;
            }
            return null;
        }

        //reply constructor
        public Post(string title, string content, string userName, Post replyTo)
        {
            this._replyTo = replyTo;
            _title = title;
            _content = content;
            _userName = userName;
            _date = DateTime.Now;
            _replies = new List<Post>();

            Guid guid = Guid.NewGuid();
            _id = guid.ToString();
        }

        internal string addReply(Post reply)
        {
            _replies.Add(reply);
            return reply.Id;
        }

        //delete this post and all of it's replies 
        public bool delete()
        {
            if (_replyTo != null)
                _replyTo._replies.Remove(this);
            return true;
        }
    }
}
