using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP_doamin.forumManagementDomain.threadsHandler;

namespace WSEP_tests.adapter
{
    interface IAdapter
    {
        bool addForum(string forumName);//via ForumSystem

        bool containForum(string forumName);//via ForumSystem

        bool registerToForum(string forumName, string userName, string userPassword, string userMail);//via UserManager

        bool forumLogIn(string forumName, string userName, string userPassword);//via UserManager

        bool setPolicy(string forumName, int minNumAdmins, int maxNumAdmins, int minNummoderator, int maxNumModerator, string freeText);//via ForumSystem

        bool addSubForum(string forumName, string subForumName, List<string> moderators);//via ForumSystem

        bool addForumAdmin(string forumName, string adminName);//via UserManager

        bool addSubForumModerator(string forumName, string subForumName, string moderatorName);//via UserManager

        //note: ForumSystem has a function that generates and returns ID's for the thread and the opening post,
        //you can just call that function and if you got codes just return "true"
        bool createThread(string forumName, string subForumName, string threadTitle, string content
            ,string userName);//via ForumSystem 

        //same as above
        bool createReply(string forumName, string subForumName, string title, string content
            ,string userName, string postIdToReplyTo);//via ForumSystem


        bool deletePost(string forumName, string subForumName, string postId);//via ForumSystem


        bool moderatorAppointment(string forumName, string subForumName, string moderatorName, int howMuchTime);//via UserManager

        bool editModeratorTime(string ForumName, string SubForumName, string UserName, int time);//via UserManager

        bool sendingPrivateMassage(string forumName, string sender, string receiver, string content);//via UserManager

        bool containSubForum(string forumName, string subForum);//via ForumSystem

        List<string> getThreadsFromSubForum(string forumName, string subForumName);//via ForumSystem


    }
}
