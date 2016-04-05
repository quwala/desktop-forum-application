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
        string assignAdmin(string forumName, string username); // has tests
        string unassignAdmin(string forumName, string username); // has tests
        string assignModerator(string forumName, string subForumName, string username); // has tests
        string unassignModerator(string forumName, string subForumName, string username);
        void getUserPermissionsForForum(string forumName, string username);
        void getUserPermissionsForSubForum(string forumName, string subForumName, string username);
    }
}
