using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP.forumManagement.forumHandler;

namespace WSEP.forumManagement
{
    public class ForumSystem : IForumSystem
    {

        private string _superAdmin;
        private List<Forum> _forums;

        public ForumSystem(string superAdmin)
        {
            _superAdmin = superAdmin;
            _forums = new List<Forum>();
        }

       public Forum getForum(string name)
        {
            foreach (Forum f in _forums)
                if (f.getName().Equals(name))
                    return f;

            throw new Exception("A Forum with that name does not exist");
        }

        public bool addForum(string name)
        {

            
            // verify there is no forum with that name
            foreach (Forum f in _forums)
                if (f.getName().Equals(name))
                    throw new Exception("A Forum with that name already exists");
            

            Forum nForum = new Forum(name);
            _forums.Add(nForum);
            if (!_forums.Contains(nForum))
                throw new Exception("Failed to add forum to DB");

            return true;

        }


    }
}
