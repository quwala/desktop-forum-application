using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.userManagement
{

    public enum permission { SUPER_ADMIN, ADMIN, MODERATOR, MEMBER, GUEST, INVALID };

    public interface IUserManager
    {
        string addForum(string forumName); // has tests
        string addSubForum(string forumName, string subForumName, List<string> moderators, int minNumOfModerators, int maxNumOfModerators); // changed - no tests
        string registerMemberToForum(string forumName, string username, string password, string eMail); // has tests
        string assignAdmin(string forumName, string username, int maxNumOfAdmins); // has tests
        string unassignAdmin(string forumName, string username,  int minNumOfAdmins); // has tests
        string assignModerator(string forumName, string subForumName, string username, int maxNumOfModerators); // has tests
        string unassignModerator(string forumName, string subForumName, string username, int minNumOfModerators); // has tests
        permission getUserPermissionsForForum(string forumName, string username); // implemented - no tests
        permission getUserPermissionsForSubForum(string forumName, string subForumName, string username); // implemented - no tests
        string sendPM(string forumName, string from, string to, string msg); // has tests
        string checkForumPolicy(string forumName, int minNumOfAdmins, int maxNumOfAdmins); // has tests
    }
}
