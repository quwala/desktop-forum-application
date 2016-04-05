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

        private const string SUCCESS = "true";
        private const string FUNCTION_ERRROR = "An error has occured with C# internal function.";
        private const string INVALID_FORUM_NAME = "Invalid forum name. Forum name cannot be null, \"null\" or empty.";
        private const string INVALID_SUB_FORUM_NAME = "Invalid sub forum name. Sub forum name cannot be null, \"null\" or empty.";
        private const string INVALID_USERNAME = "Invalid username. Username cannot be null, \"null\" or empty.";
        private const string WRONG_FORUM_NAME = "Wrong forum name. No forum found with such a name.";
        private const string WRONG_USERNAME = "Wrong username. There is no member of this forum with that username.";
        private const string WRONG_SUB_FORUM_NAME = "Wrong sub forum name. No sub forum found with such a name found in this forum.";

        public UserManager()
        {
            _forumsMembers = new List<Tuple<string, List<User>>>();
            _forumsAdmins = new List<Tuple<string, List<User>>>();
            _subForumsModerators = new List<Tuple<string, string, List<User>>>();
            // WTF should I do with the super admin?
            _superAdmin = new User("superAdmin", "superAdmin", "superAdmin@gmail.com", this);
        }

        public string addForum(string forumName)
        {
            string forumNameTaken = "This forum name is already in use. Please select another name.";
            if (forumName == null || forumName.Equals("null") || forumName.Equals(""))
            {
                return INVALID_FORUM_NAME;
            }
            // verify there is no forum with that name
            foreach (Tuple<string, List<User>> list in _forumsMembers)
            {
                if (list.Item1.Equals(forumName))
                {
                    return forumNameTaken;
                }
            }
            foreach (Tuple<string, List<User>> list in _forumsAdmins)
            {
                if (list.Item1.Equals(forumName))
                {
                    return forumNameTaken;
                }
            }
            // add new forum lists to the DB
            List<User> admins = new List<User>();
            admins.Add(_superAdmin);
            if (!admins.Contains(_superAdmin))
            {
                return FUNCTION_ERRROR;
            }
            Tuple<string, List<User>> newForumAdmins = new Tuple<string, List<User>>(forumName, admins);
            _forumsAdmins.Add(newForumAdmins);
            if (!_forumsAdmins.Contains(newForumAdmins))
            {
                return FUNCTION_ERRROR;
            }
            Tuple<string, List<User>> newForumMembers = new Tuple<string, List<User>>(forumName, new List<User>());
            _forumsMembers.Add(newForumMembers);
            if (!_forumsMembers.Contains(newForumMembers))
            {
                return FUNCTION_ERRROR;
            }
            return SUCCESS;
        }

        public string addSubForum(string forumName, string subForumName, string adminUsername)
        {
            string subForumNameTaken = "This sub forum name is already in use in that forum. Please select another name.";
            string wrongUsername = "Wrong admin username. No admin was found with that username in that forum.";
            if (forumName == null || forumName.Equals("null") || forumName.Equals(""))
            {
                return INVALID_FORUM_NAME;
            }
            if (subForumName == null || subForumName.Equals("null") || subForumName.Equals(""))
            {
                return INVALID_SUB_FORUM_NAME;
            }
            if (adminUsername == null || adminUsername.Equals("null") || adminUsername.Equals(""))
            {
                return INVALID_USERNAME;
            }
            // verify there is a forum with that name
            // if list not found return false (wrong forum name)
            if (getForumMembers(forumName) == null)
            {
                return WRONG_FORUM_NAME;
            }
            List<User> admins = getForumAdmins(forumName);
            // if list not found return false (wrong forum name)
            // should never be true as it should fail in the previous forumFound test
            if (admins == null)
            {
                return WRONG_FORUM_NAME;
            }
            // verify there is no sub forum with that name under a forum with that name
            foreach (Tuple<string, string, List<User>> subForum in _subForumsModerators)
            {
                if (subForum.Item1.Equals(forumName) && subForum.Item2.Equals(subForumName))
                {
                    return subForumNameTaken;
                }
            }
            // add new sub forum list to the DB
            // get admin user to set as a moderator
            User moderator = getAdmin(admins, adminUsername);
            // wrong admin username
            if (moderator == null)
            {
                return wrongUsername;
            }
            List<User> moderators = new List<User>();
            moderators.Add(moderator);
            if (!moderators.Contains(moderator))
            {
                return FUNCTION_ERRROR;
            }
            Tuple<string, string, List<User>> newSubForumModerators = new Tuple<string,string,List<User>>(forumName, subForumName, moderators);
            _subForumsModerators.Add(newSubForumModerators);
            if (!_subForumsModerators.Contains(newSubForumModerators))
            {
                return FUNCTION_ERRROR;
            }
            return SUCCESS;
        }

        // should be synchronized
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string registerMemberToForum(string forumName, string username, string password, string eMail)
        {
            string invalidPassword = "Invalid password. Password cannot be null, \"null\" or empty, or contain any of the following:\nSpace";
            string invalidEMail = "Invalid eMail. eMail cannot be null, \"null\" or empty.";
            string usernameTaken = "This username is already in use in that forum. Please select another name.";
            string wrongEMail = "Illegal eMail address. Please enter a corrent eMail address.";
            string eMailTaken = "This eMail address is already in use in that forum. Please enter another eMail address.";
            if (forumName == null || forumName.Equals("null") || forumName.Equals(""))
            {
                return INVALID_FORUM_NAME;
            }
            if (username == null || username.Equals("null") || username.Equals(""))
            {
                return INVALID_USERNAME;
            }
            if (password == null || password.Equals("null") || password.Equals("") || password.Contains(" "))
            {
                return invalidPassword;
            }
            if (eMail == null || eMail.Equals("null") || eMail.Equals(""))
            {
                return invalidEMail;
            }
            #region valid username (also verifies valid forum name)
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            // if list not found return false (wrong forum name)
            if (admins == null || members == null)
            {
                return WRONG_FORUM_NAME;
            }
            // check if there is an admin with that username
            foreach (User admin in admins)
            {
                if (username.Equals(admin.getUsername()))
                {
                    return usernameTaken;
                }
                if (eMail.Equals(admin.getEMail()))
                {
                    return eMailTaken;
                }
            }
            // check if there is a member with that username
            foreach (User member in members)
            {
                if (username.Equals(member.getUsername()))
                {
                    return usernameTaken;
                }
                if (eMail.Equals(member.getEMail()))
                {
                    return eMailTaken;
                }
            }
            #endregion
            // verify password match the forum's policy
            #region valid eMail
            if (!eMail.Contains("@"))
            {
                return wrongEMail;
            }
            if (eMail.IndexOf('@') == 0)
            {
                return wrongEMail;
            }
            string eMailSuffix = eMail.Substring(eMail.IndexOf('@') + 1);
            if (eMailSuffix.Contains("@") || !eMailSuffix.Contains("."))
            {
                return wrongEMail;
            }
            if (eMailSuffix.IndexOf('.') == 0)
            {
                return wrongEMail;
            }
            if (eMailSuffix.IndexOf('.') == eMailSuffix.Length - 1)
            {
                return wrongEMail;
            }
            #endregion
            User newMember = new User(username, password, eMail, this);
            members.Add(newMember);
            if (!members.Contains(newMember))
            {
                return FUNCTION_ERRROR;
            }
            return SUCCESS;
        }

        public string assignAdmin(string forumName, string username)
        {
            string inputStatus = adminsAssignmeentInputValidation(forumName, username);
            if (!inputStatus.Equals(SUCCESS))
            {
                return inputStatus;
            }
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null || members == null)
            {
                return WRONG_FORUM_NAME;
            }
            User admin = getAdmin(admins, username);
            // if found return true
            if (admin != null)
            {
                return SUCCESS;
            }
            User member = getMember(members, username);
            // if not found return false
            if (member == null)
            {
                return WRONG_USERNAME;
            }
            // add user to list of admins
            admins.Add(member);
            // if not added return false
            if (!admins.Contains(member))
            {
                return FUNCTION_ERRROR;
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
            return SUCCESS;
        }

        public string unassignAdmin(string forumName, string username)
        {
            string inputStatus = adminsAssignmeentInputValidation(forumName, username);
            if (!inputStatus.Equals(SUCCESS))
            {
                return inputStatus;
            }
            if (username.Equals(_superAdmin.getUsername()))
            {
                return "Cannot unassign the super admin.";
            }
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null || members == null)
            {
                return WRONG_FORUM_NAME;
            }
            User admin = getAdmin(admins, username);
            // if not found return true
            if (admin == null)
            {
                return SUCCESS;
            }
            // add user to regular members list
            members.Add(admin);
            // if not added return false
            if (!members.Contains(admin))
            {
                return FUNCTION_ERRROR;
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
            return SUCCESS;
        }

        public string assignModerator(string forumName, string subForumName, string username)
        {
            string inputStatus = moderatorsAssignmentInputValidation(forumName, subForumName, username);
            if (!inputStatus.Equals(SUCCESS))
            {
                return inputStatus;
            }
            // verify correct forum name
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null || members == null)
            {
                return WRONG_FORUM_NAME;
            }
            // verify user exists in this forum
            User moderator = getAdmin(admins, username);
            if (moderator == null)
            {
                moderator = getMember(members, username);
            }
            if (moderator == null)
            {
                return WRONG_USERNAME;
            }
            // verify correct sub forum name
            List<User> moderators = getSubForumModerators(forumName, subForumName);
            if (moderators == null)
            {
                return WRONG_SUB_FORUM_NAME;
            }
            // if user is a moderator return success
            if (moderators.Contains(moderator))
            {
                return SUCCESS;
            }
            // set user as moderator
            moderators.Add(moderator);
            if (!moderators.Contains(moderator))
            {
                return FUNCTION_ERRROR;
            }
            return SUCCESS;
        }

        public string unassignModerator(string forumName, string subForumName, string username)
        {
            string inputStatus = moderatorsAssignmentInputValidation(forumName, subForumName, username);
            if (!inputStatus.Equals(SUCCESS))
            {
                return inputStatus;
            }
            // set user as regular member
            // if succedded return true
            // else return false
            return SUCCESS;
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

        public bool checkForumPolicy(string forumName, int minAdmins, int maxAdmins)
        {
            return false;
            //Thanks Gal, glhf 
            //only need to check stuff like if the minAdmins is 2 and there is 
            //currently only one, shit like that. 
            //also accordring to UC3, 2.1.2 the user needs to be presented with
            //which attribute of the new policy creates a problem, so i know you
            //won't like it, but could you just throw an exception such as:
            //throw new Exception("Cannot set new policy - Conflicting minimum number of Moderators");
            //and if everything is okay return true?
            //Thanks!
            //Roy
        }

        private List<User> getForumAdmins(string forumName)
        {
            List<User> admins = null;
            foreach (Tuple<string, List<User>> t in _forumsAdmins)
            {
                if (forumName.Equals(t.Item1))
                {
                    admins = t.Item2;
                    break;
                }
            }
            return admins;
        }

        private List<User> getForumMembers(string forumName)
        {
            List<User> members = null;
            foreach (Tuple<string, List<User>> t in _forumsMembers)
            {
                if (forumName.Equals(t.Item1))
                {
                    members = t.Item2;
                    break;
                }
            }
            return members;
        }

        private User getAdmin(List<User> admins, string username)
        {
            User admin = null;
            foreach (User u in admins)
            {
                if (username.Equals(u.getUsername()))
                {
                    admin = u;
                    break;
                }
            }
            return admin;
        }

        private User getMember(List<User> members, string username)
        {
            User member = null;
            foreach (User u in members)
            {
                if (username.Equals(u.getUsername()))
                {
                    member = u;
                    break;
                }
            }
            return member;
        }

        private List<User> getSubForumModerators(string forumName, string subForumName)
        {
            List<User> moderators = null;
            foreach (Tuple<string, string, List<User>> subForumModerators in _subForumsModerators)
            {
                if (forumName.Equals(subForumModerators.Item1) && subForumName.Equals(subForumModerators.Item2))
                {
                    moderators = subForumModerators.Item3;
                    break;
                }
            }
            return moderators;
        }

        private string adminsAssignmeentInputValidation(string forumName, string username)
        {
            if (forumName == null || forumName.Equals("null") || forumName.Equals(""))
            {
                return INVALID_FORUM_NAME;
            }
            if (username == null || username.Equals("null") || username.Equals(""))
            {
                return INVALID_USERNAME;
            }
            return SUCCESS;
        }

        private string moderatorsAssignmentInputValidation(string forumName, string subForumName, string username)
        {
            if (forumName == null || forumName.Equals("null") || forumName.Equals(""))
            {
                return INVALID_FORUM_NAME;
            }
            if (subForumName == null || subForumName.Equals("null") || subForumName.Equals(""))
            {
                return INVALID_SUB_FORUM_NAME;
            }
            if (username == null || username.Equals("null") || username.Equals(""))
            {
                return INVALID_USERNAME;
            }
            return SUCCESS;
        }
    }
}
