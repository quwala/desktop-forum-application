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

       

        public bool addForum(string name)
        {

            if (name == null || name.Equals(""))
            {
                return false;
            }

            // verify there is no forum with that name
            foreach (Forum f in _forums)
            {
                if (f.getName().Equals(name))
                {
                    return false;
                }
            }

            Forum nForum = new Forum(name);
            _forums.Add(nForum);
            if (!_forums.Contains(nForum))
                return false;

            return true;

        }


    }
}
