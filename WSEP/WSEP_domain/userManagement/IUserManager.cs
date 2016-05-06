using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP_domain.forumManagement.forumHandler;

namespace WSEP_domain.userManagement
{
    public enum permission { INVALID = 1, GUEST = 2, MEMBER = 3, MODERATOR = 4, ADMIN = 5, SUPER_ADMIN = 6 };
    public enum loginStatus { TRUE, FALSE, UPDATE_PASSWORD };

    public interface IUserManager
    {
        string addForum(string forumName, string requestingUser);
        string addSubForum(string forumName, string subForumName, List<Tuple<string, string, int>> moderators, string requestingUser, int seniority);
        string registerMemberToForum(string forumName, string username, string password, string eMail);
        string assignAdmin(string forumName, string username, string requestingUser);
        string unassignAdmin(string forumName, string username, string requestingUser);
        string assignModerator(string forumName, string subForumName, string username, string requestingUser, int days);
        string unassignModerator(string forumName, string subForumName, string username, string requestingUser, bool onlyAssigningAdmin);
        permission getUserPermissionsForForum(string forumName, string username);
        permission getUserPermissionsForSubForum(string forumName, string subForumName, string username);
        string sendPM(string forumName, string from, string to, string msg);
        loginStatus login(string forumname, string username, string password, int lifespan);
        int getNumOfAdmins(string forumName);
        int getNumOfModerators(string forumName, string subForumName);
        int getMaxModerators(string forumName);
        int getMinModerators(string forumName);
        int getMinModeratorSeniority(string forumName);
        User getUserFromForum(string forumName, string username);
        string setModeratorTrialTime(string forumName, string subForumName, string moderator, int newTime, string requestingUser);
        List<Tuple<string, string, DateTime, string>> getListOfForummoderators(string forumName);
        bool isSuperAdmin(string username);
        void notifyUsersThatNeedToUpdatePassword(string forumName, int lifespan);
    }
}
