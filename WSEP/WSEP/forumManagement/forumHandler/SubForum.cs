using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP.forumManagement.threadsHandler;

namespace WSEP.forumManagement.forumHandler
{
    public class SubForum
    {
        private string _name;
        private List<Post> _threads;

        public SubForum(string name)
        {
            checkForumName(name);
            this._name = name;
            this._threads = new List<Post>();
        }

       public Post getPostById(string id)
        {
            foreach(Post p in _threads)
            {
                Post found = p.findPost(id);
                if (found != null)
                    return found;
            }
            return null;
        }

        private void checkForumName(string name)
        {
            if (name == null)
                throw new InvalidNameException("Name of the Sub Forum cannot be null");


            if (name.Equals(""))
                throw new InvalidNameException("Name of the Sub Forum cannot be empty");

            if (name.Contains("%") || name.Contains("&") || name.Contains("@"))
                throw new InvalidNameException("Name of the Sub Forum contains illegal character");

            if (name[0].Equals(' '))
                throw new InvalidNameException("Name of the Sub Forum cannot begin with a space character");

        }


        public string getName()
        {
            return this._name;
        }

        public bool createThread(Post thread)
        {
            foreach (Post t in _threads)
                if (t.Id.Equals(thread.Id))
                    throw new Exception("Cannot create thread, a thread ID duplication was detected. <br> Please try again");

            _threads.Add(thread);
            return true;
        }

        internal List<string> getThreadIDS()
        {
            List<string> ids = new List<string>();
            foreach (Post t in _threads)
                ids.Add(t.Id);

            return ids;
        }
    }
}
