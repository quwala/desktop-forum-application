using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.userManagement
{
    public interface IUserManager
    {
        string addForum(string forumName); // has tests
        string addSubForum(string forumName, string subForumName, string adminUsername); // has tests
        string registerMemberToForum(string forumName, string username, string password, string eMail); // has tests
        string assignAdmin(string forumName, string username, int maxNumOfAdmins); // has tests
        string unassignAdmin(string forumName, string username,  int minNumOfAdmins); // has tests
        string assignModerator(string forumName, string subForumName, string username, int maxNumOfModerators); // has tests
        string unassignModerator(string forumName, string subForumName, string username, int minNumOfModerators); // implemented - no tests
        void getUserPermissionsForForum(string forumName, string username); // need to figure out enums
        void getUserPermissionsForSubForum(string forumName, string subForumName, string username); // need to figure out enums
        string sendPM(string forumName, string from, string to, string msg); // implemented - no tests
        string checkForumPolicy(string forumName, int minNumOfAdmins, int maxNumOfAdmins); // implemented - no tests
    }
}
