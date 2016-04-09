using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP_service.userManagementService
{

    public enum permission { SUPER_ADMIN, ADMIN, MODERATOR, MEMBER, GUEST, INVALID };

    public interface IUserManager
    {
        string addForum(string forumName); // has tests with perfect coverage & log
        string addSubForum(string forumName, string subForumName, List<string> moderators, int minNumOfModerators, int maxNumOfModerators); // has tests with perfect coverage & log
        string registerMemberToForum(string forumName, string username, string password, string eMail); // has tests with perfect coverage & log
        string assignAdmin(string forumName, string username, int maxNumOfAdmins); // has tests with perfect coverage & log
        string unassignAdmin(string forumName, string username, int minNumOfAdmins); // has tests with perfect coverage & log
        string assignModerator(string forumName, string subForumName, string username, int maxNumOfModerators); // has tests with perfect coverage & log
        string unassignModerator(string forumName, string subForumName, string username, int minNumOfModerators); // has tests with perfect coverage & log
        permission getUserPermissionsForForum(string forumName, string username); // has tests with perfect coverage & log
        permission getUserPermissionsForSubForum(string forumName, string subForumName, string username); // has tests with perfect coverage & log
        string sendPM(string forumName, string from, string to, string msg); // has tests with perfect coverage & log
        string checkForumPolicy(string forumName, int minNumOfAdmins, int maxNumOfAdminsm, int minModerators, int maxModerators); // has tests with perfect coverage & log
        bool login(string forumname, string username, string password); // no tests, has log
    }
}
