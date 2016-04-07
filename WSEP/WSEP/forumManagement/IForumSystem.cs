using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP.userManagement;
using WSEP.notificationManagement;
using WSEP.forumManagement.forumHandler;

namespace WSEP.forumManagement
{
    public interface IForumSystem
    {
        bool addForum(string name);
        Forum getForum(string name);
        bool addSubForum(string forumName, string subForumName);
        bool changeForumPolicy(string forumName, int minAdmins, int maxAdmins,
            int minModerators, int maxModerators, string forumRules);


    }
}
