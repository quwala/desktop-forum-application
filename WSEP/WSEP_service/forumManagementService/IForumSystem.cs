using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP_service.userManagementService;
using WSEP_service.notificationManagementService;
using WSEP_domain.forumManagementDomain.forumHandler;

namespace WSEP_service.forumManagementService
{
    public interface IForumSystem
    {
        bool addForum(string name);

        Forum getForum(string name);//adds it in the usermanager as well.

        bool addSubForum(string forumName, string subForumName, List<string> mods);

        bool setForumPolicy(string forumName, string policyName, int minAdmins, int maxAdmins,
            int minModerators, int maxModerators, string forumRules);

        bool hasForum(string name);

        string createThread(string forumName, string subForumName, string title, string content,
            string userName);

        string createReply(string forumName, string subForumName, string title, string content,
            string userName, string postToReplyToID);

        List<string> getThreadIDSFromSubForum(string forumName, string subForumName);

        bool deletePost(string forumName, string subForumName, string postId);


    }
}
