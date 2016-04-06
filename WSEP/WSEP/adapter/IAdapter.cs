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

        bool DeletePost(string forumName, string subForumName, List<Post> posts);

        bool ModeratorAppointment(string forumName, string subForumName, string moderatorName, int howMuchTime);
        bool EditModeratorTime(string ForumName, string SubForumName, string UserName, int time);


        bool CreateReply(string forumName, string subForumName, Post post, string content);

        bool SendingPrivateMassage(string forumName, string sender, string receiver, string content);

        bool ContainSubForum(string forumName, string subForum);

        List<Post> getThreadFromSubForum(string forumName, string subForumName);


    }
}
