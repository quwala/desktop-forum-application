using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP.forumManagement.forumHandler;

namespace WSEP.forumManagement
{
    public interface IForumSystem
    {
        bool addForum(String name);

        Forum getForum(string name);
    }


}
