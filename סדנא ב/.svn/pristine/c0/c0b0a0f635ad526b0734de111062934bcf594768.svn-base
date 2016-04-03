using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.userManagement
{
    public class UserManager : IUserManager
    {

        // add logger

        private User _superAdmin;
        private List<Tuple<string, List<User>>> _forumsMembers; // forum name and a list of members
        private List<Tuple<string, List<User>>> _forumsAdmins; // forum name and a list of admins
        private List<Tuple<string, string, List<User>>> _subForumsModerators; // forum name, sub forum name and a list of moderators

        public UserManager()
        {
            _forumsMembers = new List<Tuple<string, List<User>>>();
            _forumsAdmins = new List<Tuple<string, List<User>>>();
            _subForumsModerators = new List<Tuple<string, string, List<User>>>();
            // WTF should I do with the super admin?
            _superAdmin = new User("superAdmin", "superAdmin", "superAdmin@gmail.com", this);
        }

        public bool addForum(string forumName)
        {
            if (forumName == null || forumName.Equals(""))
            {
                return false;
            }
            // verify there is no forum with that name
            foreach (Tuple<string, List<User>> list in _forumsMembers)
            {
                if (list.Item1.Equals(forumName))
                {
                    return false;
                }
            }
            foreach (Tuple<string, List<User>> list in _forumsAdmins)
            {
                if (list.Item1.Equals(forumName))
                {
                    return false;
                }
            }
            // add new forum lists to the DB
            List<User> admins = new List<User>();
            admins.Add(_superAdmin);
            if (!admins.Contains(_superAdmin))
            {
                return false;
            }
            Tuple<string, List<User>> newForumAdmins = new Tuple<string, List<User>>(forumName, admins);
            _forumsAdmins.Add(newForumAdmins);
            if (!_forumsAdmins.Contains(newForumAdmins))
            {
                return false;
            }
            Tuple<string, List<User>> newForumMembers = new Tuple<string, List<User>>(forumName, new List<User>());
            _forumsMembers.Add(newForumMembers);
            if (!_forumsMembers.Contains(newForumMembers))
            {
                return false;
            }
            return true;
        }

        public bool addSubForum(string forumName, string subForumName, string adminUsername)
        {
            if (forumName == null || subForumName == null || adminUsername == null)
            {
                return false;
            }
            if (forumName.Equals("") || subForumName.Equals("") || adminUsername.Equals(""))
            {
                return false;
            }
            // verify there is a forum with that name
            bool forumFound = false;
            foreach (Tuple<string, List<User>> list in _forumsMembers)
            {
                if (list.Item1.Equals(forumName))
                {
                    forumFound = true;
                    break;
                }
            }
            // if list not found return false (wrong forum name)
            if (!forumFound)
            {
                return false;
            }
            forumFound = false;
            List<User> admins = null;
            foreach (Tuple<string, List<User>> list in _forumsAdmins)
            {
                if (list.Item1.Equals(forumName))
                {
                    forumFound = true;
                    admins = list.Item2;
                    break;
                }
            }
            // if list not found return false (wrong forum name)
            // should never be true as it should fail in the previous forumFound test
            if (!forumFound || admins == null)
            {
                return false;
            }
            // verify there is no sub forum with that name under a forum with that name
            foreach (Tuple<string, string, List<User>> subForum in _subForumsModerators)
            {
                if (subForum.Item1.Equals(forumName) && subForum.Item2.Equals(subForumName))
                {
                    return false;
                }
            }
            // add new sub forum list to the DB
            // get admin user to set as a moderator
            User moderator = null;
            foreach (User admin in admins)
            {
                if (admin.getUsername().Equals(adminUsername))
                {
                    moderator = admin;
                    break;
                }
            }
            // wrong admin username
            if (moderator == null)
            {
                return false;
            }
            List<User> moderators = new List<User>();
            moderators.Add(moderator);
            if (!moderators.Contains(moderator))
            {
                return false;
            }
            Tuple<string, string, List<User>> newSubForumModerators = new Tuple<string,string,List<User>>(forumName, subForumName, moderators);
            _subForumsModerators.Add(newSubForumModerators);
            if (!_subForumsModerators.Contains(newSubForumModerators))
            {
                return false;
            }
            return true;
        }

        // should be synchronized
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool registerMemberToForum(string forumName, string username, string password, string eMail)
        {
            if (forumName == null || username == null || password == null || eMail == null)
            {
                return false;
            }
            if (username.Equals("") || password.Equals("") || eMail.Equals(""))
            {
                return false;
            }
            if (username.IndexOf(' ') == 0 || password.Contains(" "))
            {
                return false;
            }
            #region valid username (also verifies valid forum name)
            List<User> admins = null;
            List<User> members = null;
            // get list of forum admins
            foreach (Tuple<string, List<User>> t in _forumsAdmins)
            {
                if (forumName.Equals(t.Item1))
                {
                    admins = t.Item2;
                    break;
                }
            }
            // if list not found return false (wrong forum name)
            if (admins == null)
            {
                return false;
            }
            // check if there is an admin with that username
            foreach (User admin in admins)
            {
                if (username.Equals(admin.getUsername()))
                {
                    return false;
                }
            }
            // get list of forum members
            foreach (Tuple<string, List<User>> t in _forumsMembers)
            {
                if (forumName.Equals(t.Item1))
                {
                    members = t.Item2;
                    break;
                }
            }
            // if list not found return false (wrong forum name)
            // should never be true as it should fail in the test admins == null before
            if (members == null)
            {
                return false;
            }
            // check if there is a member with that username
            foreach (User member in members)
            {
                if (username.Equals(member.getUsername()))
                {
                    return false;
                }
            }
            #endregion
            // verify password match the forum's policy
            #region valid eMail
            if (!eMail.Contains("@"))
            {
                return false;
            }
            if (eMail.IndexOf('@') == 0)
            {
                return false;
            }
            string eMailSuffix = eMail.Substring(eMail.IndexOf('@') + 1);
            if (eMailSuffix.Contains("@") || !eMailSuffix.Contains("."))
            {
                return false;
            }
            if (eMailSuffix.IndexOf('.') == 0)
            {
                return false;
            }
            if (eMailSuffix.IndexOf('.') == eMailSuffix.Length - 1)
            {
                return false;
            }
            #endregion
            User newMember = new User(username, password, eMail, this);
            members.Add(newMember);
            if (!members.Contains(newMember))
            {
                return false;
            }
            return true;
        }

        public bool aggainAdmin(string forumName, string username)
        {
            List<User> admins = null;
            List<User> members = null;
            // get list of admins
            foreach (Tuple<string, List<User>> t in _forumsAdmins)
            {
                if (forumName.Equals(t.Item1))
                {
                    admins = t.Item2;
                    break;
                }
            }
            // if list not found return false (wrong forum name)
            if (admins == null)
            {
                return false;
            }
            User admin = null;
            // search user in admins list
            foreach (User user in admins)
            {
                if (username.Equals(user.getUsername()))
                {
                    admin = user;
                    break;
                }
            }
            // if found return true
            if (admin != null)
            {
                return true;
            }
            // get regular members list
            foreach (Tuple<string, List<User>> t in _forumsMembers)
            {
                if (forumName.Equals(t.Item1))
                {
                    members = t.Item2;
                    break;
                }
            }
            // if list not found return false (wrong forum name)
            // should never be true as it should fail in the test admins == null before
            if (members == null)
            {
                return false;
            }
            User member = null;
            // search user in members list
            foreach (User user in members)
            {
                if (username.Equals(user.getUsername()))
                {
                    member = user;
                    break;
                }
            }
            // if not found return false
            if (member == null)
            {
                return false;
            }
            // add user to list of admins
            admins.Add(member);
            // if not added return false
            if (!admins.Contains(member))
            {
                return false;
            }
            // remove user from regular members list
            members.Remove(member);
            // if not removed throw exception - fatal error
            if (members.Contains(member))
            {
                string str1 = "User " + username + " was added as an admin to forum " + forumName + " but could not be removed from the regular members list.\n";
                string str2 = "This created an illegal situation where the same user was in both data bases where a user can only be in one.\n";
                string str3 = "Hence a fatal error occured.";
                throw new Exception (str1 + str2 + str3);
            }
            // success
            return true;
        }

        public bool unassignAdmin(string forumName, string username)
        {
            // if username is super admins username return false
            List<User> admins = null;
            List<User> members = null;
            // get list of admins
            foreach (Tuple<string, List<User>> t in _forumsAdmins)
            {
                if (forumName.Equals(t.Item1))
                {
                    admins = t.Item2;
                    break;
                }
            }
            // if list not found return false (wrong forum name)
            if (admins == null)
            {
                return false;
            }
            User admin = null;
            // search user in admins list
            foreach (User user in admins)
            {
                if (username.Equals(user.getUsername()))
                {
                    admin = user;
                    break;
                }
            }
            // if not found return true
            if (admin == null)
            {
                return true;
            }
            // get regular members list
            foreach (Tuple<string, List<User>> t in _forumsMembers)
            {
                if (forumName.Equals(t.Item1))
                {
                    members = t.Item2;
                    break;
                }
            }
            // if list not found return false (wrong forum name)
            // should never be true as it should fail in the test admins == null before
            if (members == null)
            {
                return false;
            }
            // add user to regular members list
            members.Add(admin);
            // if not added return false
            if (!members.Contains(admin))
            {
                return false;
            }
            // remove user from admins list
            admins.Remove(admin);
            // if not removed throw exception - fatal error
            if (admins.Contains(admin))
            {
                string str1 = "User " + username + " is no longer an admin to forum " + forumName + " and added to the regular members list, but could not be removed from the admins list.\n";
                string str2 = "This created an illegal situation where the same user was in both data bases where a user can only be in one.\n";
                string str3 = "Hence a fatal error occured.";
                throw new Exception(str1 + str2 + str3);
            }
            // success
            return true;
        }

        public bool assignModerator(string forumName, string subForumName, string username)
        {
            // if username is super admins username return false
            // set user as moderator
            // if succedded return true
            // else return false
            return false;
        }

        public bool unassignModerator(string forumName, string subForumName, string username)
        {
            // set user as regular member
            // if succedded return true
            // else return false
            return false;
        }

        public void getUserPermissionsForForum(string forumName, string username)
        {
            // check if user is a regular member, an admin or the super admin
        }

        public void getUserPermissionsForSubForum(string forumName, string subForumName, string username)
        {
            // check if user us a regular member, a moderator, an admin or the super admin
        }

        public bool sendPM(string forumName, string from, string to, string msg)
        {
            // get user with that username from that forum
            // activate user method getPM
            return false;
        }
    }
}
