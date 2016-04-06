using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP.forumManagement.forumHandler;

namespace WSEP.adapter
{
    interface IAdapter
    {
        bool addForum(string forumName);

        bool containForum(string forumName);

        bool RegisterToForum(string forumName, string userName, string userPassword, string userMail);

        bool ForumLogIn(string forumName, string userName, string userPassword);

        bool SetPolicy(string forumName, int minNumAdmins, int maxNumAdmins, int minNummoderator, int maxNumModerator, string freeText);

        bool addSubForum(string forumName, string subForumName, List<string> moderatorsNames);

        bool addForumAdmin(string forumName, string adminName);

        bool addSubForumModerator(string forumName, string subForumName, string moderatorName);

        bool CreateThread(string forumName, string subForumName, string threadTitle, string content);

        List<Post> getThreadsFromSubForum(string forumName, string subForumName);



    }
}
