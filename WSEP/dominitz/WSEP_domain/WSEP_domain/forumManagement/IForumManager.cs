using ForumsDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP_domain.forumManagement.forumHandler;
using WSEP_domain.forumManagement.threadsHandler;
using WSEP_domain.userManagement;

namespace WSEP_domain.forumManagement
{
    public interface IForumManager
    {
        void readForum(Forum forum);
        string addForum(string forumName);
        string addSubForum(string forumName, string subForumName);
        int getForumMaxAdmins(string forumName);
        int getForumMinAdmins(string forumName);
        int getForumMaxModerators(string forumName);
        int getForumMinModerators(string forumName);
        int getForumSeniorityLimit(string forumName);
        modUnassignmentPermission getForumModUnassignmentPermissions(string forumName);
        postDeletionPermission getForumPostDeletionPermission(string forumName);
        bool setForumMaxAdmins(string forumName, int maxAdmins);
        bool setForumMinAdmins(string forumName, int minAdmins);
        bool setForumMaxModerators(string forumName, int maxModerators);
        bool setForumPostDeletionPermissions(string forumName, postDeletionPermission permission);
        bool setForumPasswordLifespan(string forumName, int lifespan);
        bool setForumModeratorsSeniority(string forumName, int seniority);
        bool setForumModUnassignmentPermissions(string forumName, modUnassignmentPermission permission);
        string writePost(string forumName, string subForumName, int parentPostNo, User user, string title, string content);
        string deletePost(string forumName, string subForumName, int postNo, postDeletionPermission pdp, permission p, string requestingUser);
        string editPost(string forumName, string subForumName, int postNo, string requestingUser, permission p, string content);
        int getNumOfPostsInSubForum(string forumName, string subForumName);
        List<Tuple<string, string, DateTime, int>> getListOfMemberMessages(string forumName, string memberName);
        int getNumOfForums();
        List<string> getForums();
        List<string> getSubForums(string forumName);
        List<Tuple<string, DateTime, int>> getThreads(string forumName, string subForumName);
        List<Tuple<string, string, DateTime, int, int, string, DateTime>> getThread(string forumName, string subForumName, int openPostNo);
        int getForumPasswordLifespan(string forumName);
    }
}
