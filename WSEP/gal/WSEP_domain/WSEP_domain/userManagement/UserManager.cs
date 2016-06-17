using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WSEP_domain.forumSystem;

namespace WSEP_domain.userManagement
{

    public enum permission { INVALID = 1, GUEST = 2, MEMBER = 3, MODERATOR = 4, ADMIN = 5, SUPER_ADMIN = 6 };
    public enum loginStatus { TRUE, FALSE, UPDATE_PASSWORD };

    public class UserManager
    {

        private User _superAdmin;
        private List<Tuple<string, List<User>>> _forumsMembers; // forum name and a list of members
        private List<Tuple<string, List<User>>> _forumsAdmins; // forum name and a list of admins
        private List<Tuple<string, string, List<Tuple<User, string, DateTime, int>>>> _subForumsModerators; // forum name, sub forum name and a list of moderators, assigning admin, assignment time and duration (days)

        private const string WRONG_FORUM_NAME = "Wrong forum name. No forum found with such a name.";
        private const string WRONG_USERNAME = "Wrong username. There is no member of this forum with that username.";
        private const string WRONG_SUB_FORUM_NAME = "Wrong sub forum name. No sub forum found with such a name found in this forum.";

        public UserManager()
        {
            _forumsMembers = new List<Tuple<string, List<User>>>();
            _forumsAdmins = new List<Tuple<string, List<User>>>();
            _subForumsModerators = new List<Tuple<string, string, List<Tuple<User, string, DateTime, int>>>>();
            // WTF should I do with the super admin?
            _superAdmin = User.create("superAdmin", "superAdmin", "superAdmin@gmail.com", "q", "a");
        }

        public void readForumMembers(string forumName, List<User> members)
        {
            _forumsMembers.Add(new Tuple<string, List<User>>(forumName, members));
        }

        public void readForumAdmins(string forumName, List<User> admins)
        {
            _forumsAdmins.Add(new Tuple<string, List<User>>(forumName, admins));
        }

        public void readSubForumModerators(string forumName, string subForumName, List<Tuple<User, string, DateTime, int>> moderators)
        {
            _subForumsModerators.Add(new Tuple<string, string, List<Tuple<User, string, DateTime, int>>>(forumName, subForumName, moderators));
        }

        public string addForum(string forumName, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            if (!requestingUser.Equals(_superAdmin.getUsername()))
            {
                return Constants.UNAUTHORIZED;
            }
            string forumNameTaken = "This forum name is already in use. Please select another name.";
            // verify there is no forum with that name
            List<User> admins = getForumAdmins(forumName);
            List<User> member = getForumMembers(forumName);
            if (admins != null || member != null)
            {
                return forumNameTaken;
            }
            // add new forum lists to the DB
            admins = new List<User>();
            admins.Add(_superAdmin);
            if (!admins.Contains(_superAdmin)) // cannot deliberatly cover this case
            {
                return Constants.FUNCTION_ERRROR;
            }
            Tuple<string, List<User>> newForumAdmins = new Tuple<string, List<User>>(forumName, admins);
            _forumsAdmins.Add(newForumAdmins);
            if (!_forumsAdmins.Contains(newForumAdmins)) // cannot deliberatly cover this case
            {
                return Constants.FUNCTION_ERRROR;
            }
            Tuple<string, List<User>> newForumMembers = new Tuple<string, List<User>>(forumName, new List<User>());
            _forumsMembers.Add(newForumMembers);
            if (!_forumsMembers.Contains(newForumMembers)) // cannot deliberatly cover this case
            {
                return Constants.FUNCTION_ERRROR;
            }
            return Constants.SUCCESS;
        }
        
        public string addSubForum(string forumName, string subForumName, List<Tuple<string, string, int>> setModerators, string requestingUser, int seniority)
        {
            List<string> input = new List<string>() { forumName, subForumName, requestingUser };
            foreach (Tuple<string, string, int> t in setModerators)
            {
                input.Add(t.Item1);
                //input.Add(t.Item2);
            }
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            if (getUserPermissionsForForum(forumName, requestingUser) != permission.SUPER_ADMIN && getUserPermissionsForForum(forumName, requestingUser) != permission.ADMIN)
            {
                return Constants.UNAUTHORIZED;
            }
            string subForumNameTaken = "This sub forum name is already in use in that forum. Please select another name.";
            // verify there is a forum with that name
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null || members == null)
            {
                return WRONG_FORUM_NAME;
            }
            if (getUserPermissionsForForum(forumName, requestingUser) != permission.SUPER_ADMIN && getUserPermissionsForForum(forumName, requestingUser) != permission.ADMIN)
            {
                return "Unauthorized user";
            }
            // verify there is no sub forum with that name under a forum with that name
            foreach (Tuple<string, string, List<Tuple<User, string, DateTime, int>>> subForum in _subForumsModerators)
            {
                if (subForum.Item1.Equals(forumName) && subForum.Item2.Equals(subForumName))
                {
                    return subForumNameTaken;
                }
            }
            // verify all new moderators are registered to the forum and add to moderators list
            List<Tuple<User, string, DateTime, int>> moderators = new List<Tuple<User, string, DateTime, int>>();
            foreach (Tuple<string, string, int> newModerator in setModerators)
            {
                User user = getUser(admins, newModerator.Item1);
                if (user == null)
                {
                    user = getUser(members, newModerator.Item1);
                }
                if (user == null)
                {
                    return "Moderators list contains a non existing user: " + newModerator;
                }
                if (DateTime.Now.Date.Subtract(user.getRegistrationDate()).Days < seniority)
                {
                    return "Cannot add " + newModerator.Item1 + " as a moderator as the user does not have the required seniority.";
                }
                Tuple<User, string, DateTime, int> mod = new Tuple<User, string, DateTime, int>(user, requestingUser, DateTime.Now.Date, newModerator.Item3);
                if (!ForumSystem._testFlag)
                {
                    if (!ForumSystem._db.addSubForumModerator(forumName, subForumName, newModerator.Item1, requestingUser, newModerator.Item3, DateTime.Now.Date))
                    {
                        // remove all written moderators
                        return Constants.DB_ERROR;
                    }
                }
                moderators.Add(mod);
                if (!moderators.Contains(mod)) // cannot deliberatly cover this case
                {
                    return Constants.FUNCTION_ERRROR;
                }
            }
            // add new sub forum list to the DB
            Tuple<string, string, List<Tuple<User, string, DateTime, int>>> newSubForumModerators = new Tuple<string, string, List<Tuple<User, string, DateTime, int>>>(forumName, subForumName, moderators);
            _subForumsModerators.Add(newSubForumModerators);
            if (!_subForumsModerators.Contains(newSubForumModerators)) // cannot deliberatly cover this case
            {
                return Constants.FUNCTION_ERRROR;
            }
            return Constants.SUCCESS;
        }

        // should be synchronized
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string registerMemberToForum(string forumName, string username, string password, string eMail, string securityQuestion, string answer)
        {
            List<string> input = new List<string>() { forumName, username, password, eMail, securityQuestion, answer };
            if (!Constants.isValidInput(input) || !Constants.isValidEmail(eMail))
            {
                return Constants.INVALID_INPUT;
            }
            string usernameTaken = "This username is already in use in that forum. Please select another name.";
            string eMailTaken = "This eMail address is already in use in that forum. Please enter another eMail address.";
            if (username.IndexOf(' ') == 0)
            {
                return "Username cannot begin with a space.";
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
            User newMember = User.create(username, password, eMail, securityQuestion, answer);
            if (!ForumSystem._testFlag)
            {
                if (ForumSystem._db.addForumUser(username, password, eMail, newMember.getRegistrationDate(), forumName, newMember.getDateOfLastPasswordChange(), securityQuestion, answer))
                {
                    if (!ForumSystem._db.addForumMember(forumName, username))
                    {
                        if (ForumSystem._db.removeForumUser(username, forumName))
                        {
                            return Constants.DB_ERROR;
                        }
                        throw new ShouldNotHappenException();
                    }
                }
                else
                {
                    return Constants.DB_ERROR;
                }
            }
            members.Add(newMember);
            if (!members.Contains(newMember)) // cannot deliberatly cover this case
            {
                return Constants.FUNCTION_ERRROR;
            }
            string msg = "Welcome, " + username + ", to forum " + forumName + " of our forum system.\nWe hope you will enjoy our system.\n\nGMRRS team.";
            Constants.sendMail(eMail, "Forum registration confirmation", msg);
            return Constants.SUCCESS;
        }

        public string assignAdmin(string forumName, string username, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, username, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null || members == null)
            {
                return WRONG_FORUM_NAME;
            }
            if (getUserPermissionsForForum(forumName, requestingUser) != permission.SUPER_ADMIN && getUserPermissionsForForum(forumName, requestingUser) != permission.ADMIN)
            {
                return Constants.UNAUTHORIZED;
            }
            User admin = getUser(admins, username);
            // if found return true
            if (admin != null)
            {
                return Constants.SUCCESS;
            }
            User member = getUser(members, username);
            // if not found return false
            if (member == null)
            {
                return WRONG_USERNAME;
            }
            // add user to list of admins
            if (!ForumSystem._testFlag)
            {
                if (ForumSystem._db.addForumAdmin(forumName, username))
                {
                    if (!ForumSystem._db.removeForumMember(forumName, username))
                    {
                        if (ForumSystem._db.removeForumAdmin(forumName, username))
                        {
                            return Constants.DB_ERROR;
                        }
                        throw new ShouldNotHappenException();
                    }
                }
                else
                {
                    return Constants.DB_ERROR;
                }
            }
            admins.Add(member);
            // if not added return false
            if (!admins.Contains(member)) // cannot deliberatly cover this case
            {
                return Constants.FUNCTION_ERRROR;
            }
            // remove user from regular members list
            members.Remove(member);
            // if not removed throw exception - fatal error
            if (members.Contains(member)) // cannot deliberatly cover this case
            {
                string str1 = "User " + username + " was added as an admin to forum " + forumName + " but could not be removed from the regular members list.\n";
                string str2 = "This created an illegal situation where the same user was in both data bases where a user can only be in one.\n";
                string str3 = "Hence a fatal error occured.";
                throw new Exception(str1 + str2 + str3);
            }
            // success
            return Constants.SUCCESS;
        }

        public string unassignAdmin(string forumName, string username, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, username, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
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
            if (getUserPermissionsForForum(forumName, requestingUser) != permission.SUPER_ADMIN && getUserPermissionsForForum(forumName, requestingUser) != permission.ADMIN)
            {
                return Constants.UNAUTHORIZED;
            }
            User admin = getUser(admins, username);
            // if not found return true
            if (admin == null)
            {
                if (getUser(members, username) != null)
                {
                    return Constants.SUCCESS;
                }
                return WRONG_USERNAME;
            }
            // add user to regular members list
            if (!ForumSystem._testFlag)
            {
                if (ForumSystem._db.addForumMember(forumName, username))
                {
                    if (!ForumSystem._db.removeForumAdmin(forumName, username))
                    {
                        if (ForumSystem._db.removeForumMember(forumName, username))
                        {
                            return Constants.DB_ERROR;
                        }
                        throw new ShouldNotHappenException();
                    }
                }
                else
                {
                    return Constants.DB_ERROR;
                }
            }
            members.Add(admin);
            // if not added return false
            if (!members.Contains(admin)) // cannot deliberatly cover this case
            {
                return Constants.FUNCTION_ERRROR;
            }
            // remove user from admins list
            admins.Remove(admin);
            // if not removed throw exception - fatal error
            if (admins.Contains(admin)) // cannot deliberatly cover this case
            {
                string str1 = "User " + username + " is no inter an admin to forum " + forumName + " and added to the regular members list, but could not be removed from the admins list.\n";
                string str2 = "This created an illegal situation where the same user was in both data bases where a user can only be in one.\n";
                string str3 = "Hence a fatal error occured.";
                throw new Exception(str1 + str2 + str3);
            }
            // success
            return Constants.SUCCESS;
        }

        public string assignModerator(string forumName, string subForumName, string username, string requestingUser, int days)
        {
            List<string> input = new List<string>() { forumName, username, username, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            // verify correct forum name
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null || members == null)
            {
                return WRONG_FORUM_NAME;
            }
            if (getUserPermissionsForForum(forumName, requestingUser) != permission.SUPER_ADMIN && getUserPermissionsForForum(forumName, requestingUser) != permission.ADMIN)
            {
                return Constants.UNAUTHORIZED;
            }
            // verify user exists in this forum
            User moderator = getUser(admins, username);
            if (moderator == null)
            {
                moderator = getUser(members, username);
            }
            if (moderator == null)
            {
                return WRONG_USERNAME;
            }
            // verify correct sub forum name
            List<Tuple<User, string, DateTime, int>> moderators = getSubForumModerators(forumName, subForumName);
            if (moderators == null)
            {
                return WRONG_SUB_FORUM_NAME;
            }
            // if user is a moderator return success
            foreach (Tuple<User, string, DateTime, int> t in moderators)
            {
                if (t.Item1.Equals(moderator.getUsername()))
                {
                    return Constants.SUCCESS;
                }
            }
            // set user as moderator
            DateTime dt = DateTime.Now;
            Tuple<User, string, DateTime, int> mod = new Tuple<User, string, DateTime, int>(moderator, requestingUser, dt, days);
            if (!ForumSystem._testFlag)
            {
                if (!ForumSystem._db.addSubForumModerator(forumName, subForumName, username, requestingUser, days, dt))
                {
                    return Constants.DB_ERROR;
                }
            }
            moderators.Add(mod);
            if (!moderators.Contains(mod)) // cannot deliberatly cover this case
            {
                return Constants.FUNCTION_ERRROR;
            }
            return Constants.SUCCESS;
        }

        public string unassignModerator(string forumName, string subForumName, string username, string requestingUser, bool onlyAssigningAdmin)
        {
            List<string> input = new List<string>() { forumName, subForumName, username, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            if (getUserPermissionsForForum(forumName, requestingUser) != permission.SUPER_ADMIN && getUserPermissionsForForum(forumName, requestingUser) != permission.ADMIN)
            {
                return Constants.UNAUTHORIZED;
            }
            // verify correct forum name
            List<User> members = getForumMembers(forumName);
            if (members == null)
            {
                return WRONG_FORUM_NAME;
            }
            // verify correct sub forum name
            List<Tuple<User, string, DateTime, int>> moderators = getSubForumModerators(forumName, subForumName);
            if (moderators == null)
            {
                return WRONG_SUB_FORUM_NAME;
            }
            // if username is not a moderator than it is a success
            Tuple<User, string, DateTime, int> moderator = null;
            foreach (Tuple<User, string, DateTime, int> t in moderators)
            {
                if (t.Item1.getUsername().Equals(username))
                {
                    moderator = t;
                    break;
                }
            }
            if (moderator == null)
            {
                if (getUser(getForumAdmins(forumName), username) != null && getUser(members, username) != null)
                {
                    return Constants.SUCCESS;
                }
                return WRONG_USERNAME;
            }
            if (onlyAssigningAdmin && (!(getUserPermissionsForForum(forumName, requestingUser) == permission.SUPER_ADMIN) || !(moderator.Item2.Equals(requestingUser))))
            {
                return "error";
            }
            // remove user from moderators list
            if (!ForumSystem._testFlag)
            {
                if (!ForumSystem._db.removeSubForumModerator(forumName, subForumName, username))
                {
                    return Constants.DB_ERROR;
                }
            }
            moderators.Remove(moderator);
            if (moderators.Contains(moderator)) // cannot deliberatly cover this case
            {
                return Constants.FUNCTION_ERRROR;
            }
            return Constants.SUCCESS;
        }

        public permission getUserPermissionsForForum(string forumName, string username)
        {
            List<string> input = new List<string>() { forumName, username };
            if (!Constants.isValidInput(input))
            {
                return permission.INVALID;
            }
            // verify correct forum name
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null || members == null)
            {
                return permission.INVALID;
            }
            // check if the user is the super admin
            if (username.Equals(_superAdmin.getUsername()))
            {
                return permission.SUPER_ADMIN;
            }
            // check if the user is an admin
            User user = getUser(admins, username);
            if (user != null)
            {
                return permission.ADMIN;
            }
            // check if the user is a member
            user = getUser(members, username);
            if (user != null)
            {
                return permission.MEMBER;
            }
            return permission.GUEST;
        }

        public permission getUserPermissionsForSubForum(string forumName, string subForumName, string username)
        {
            List<string> input = new List<string>() { forumName, subForumName, username };
            if (!Constants.isValidInput(input))
            {
                return permission.INVALID;
            }
            // verify correct forum name
            List<User> admins = getForumAdmins(forumName);
            List<Tuple<User, string, DateTime, int>> moderators = getSubForumModerators(forumName, subForumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null || members == null || moderators == null)
            {
                return permission.INVALID;
            }
            // check if the user is the super admin
            if (username.Equals(_superAdmin.getUsername()))
            {
                return permission.SUPER_ADMIN;
            }
            // check if the user is an admin
            User user = getUser(admins, username);
            if (user != null)
            {
                return permission.ADMIN;
            }
            // check if the user is a moderator
            foreach (Tuple<User, string, DateTime, int> t in moderators)
            {
                if (t.Item1.getUsername().Equals(username))
                {
                    user = t.Item1;
                    break;
                }
            }
            if (user != null)
            {
                return permission.MODERATOR;
            }
            // check if the user is a member
            user = getUser(members, username);
            if (user != null)
            {
                return permission.MEMBER;
            }
            return permission.GUEST;
        }

        public string sendPM(string forumName, string from, string to, string msg)
        {
            List<string> input = new List<string>() { forumName, from, to };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            if (msg == null || msg.Equals("null") || msg.Equals(""))
            {
                return Constants.INVALID_INPUT;
            }
            // get both users
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null || members == null)
            {
                return WRONG_FORUM_NAME;
            }
            User sender = getUser(admins, from);
            if (sender == null)
            {
                sender = getUser(members, from);
            }
            if (sender == null)
            {
                return from + " - " + WRONG_USERNAME;
            }
            User receiver = getUser(admins, to);
            if (receiver == null)
            {
                receiver = getUser(members, to);
            }
            if (receiver == null)
            {
                return to + " - " + WRONG_USERNAME;
            }
            // add msg to both users PM collections
            if (!ForumSystem._testFlag)
            {
                if (!ForumSystem._db.addPrivateMessage(from, to, DateTime.Now, forumName, msg))
                {
                    return Constants.DB_ERROR;
                }
            }
            if (!receiver.getMessage(from, msg).Equals(Constants.SUCCESS)) // cannot deliberatly cover this case
            {
                return Constants.FUNCTION_ERRROR;
            }
            if (!sender.sendMessage(to, msg).Equals(Constants.SUCCESS)) // cannot deliberatly cover this case
            {
                string str1 = "Private Message was added only to one user collection of private messages.\n";
                string str2 = "This  caused a critical error.\n";
                throw new Exception(str1 + str2);
            }
            return Constants.SUCCESS;
        }

        public loginStatus login(string forumName, string username, string password, int lifespan)
        {
            List<string> input = new List<string>() { forumName, username, password };
            if (!Constants.isValidInput(input))
            {
                return loginStatus.FALSE;
            }
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null || members == null)
            {
                return loginStatus.FALSE;
            }
            User user = getUser(admins, username);
            if (user == null)
            {
                user = getUser(members, username);
            }
            if (user == null)
            {
                return loginStatus.FALSE;
            }
            if (!user.getPassword().Equals(password))
            {
                return loginStatus.FALSE;
            }
            if (DateTime.Now.Date.Subtract(user.getDateOfLastPasswordChange()).Days > lifespan)
            {
                return loginStatus.UPDATE_PASSWORD;
            }
            return loginStatus.TRUE;
        }

        public int getNumOfAdmins(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            List<User> admins = getForumAdmins(forumName);
            if (admins == null)
            {
                return -1;
            }
            return admins.Count;
        }

        public int getNumOfModerators(string forumName, string subForumName)
        {
            List<string> input = new List<string>() { forumName, subForumName };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            List<Tuple<User, string, DateTime, int>> moderators = getSubForumModerators(forumName, subForumName);
            if (moderators == null)
            {
                return -1;
            }
            return moderators.Count;
        }

        public int getMaxModerators(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            if (getForumAdmins(forumName) == null)
            {
                return -1;
            }
            int ans = 0;
            foreach (Tuple<string, string, List<Tuple<User, string, DateTime, int>>> moderators in _subForumsModerators)
            {
                if (moderators.Item1.Equals(forumName))
                {
                    ans = Math.Max(ans, moderators.Item3.Count);
                }
            }
            return ans;
        }

        public int getMinModerators(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            if (getForumAdmins(forumName) == null)
            {
                return -1;
            }
            int ans = int.MaxValue;
            foreach (Tuple<string, string, List<Tuple<User, string, DateTime, int>>> moderators in _subForumsModerators)
            {
                if (moderators.Item1.Equals(forumName))
                {
                    ans = Math.Min(ans, moderators.Item3.Count);
                }
            }
            return ans;
        }

        public int getMinModeratorSeniority(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            if (getForumAdmins(forumName) == null)
            {
                return -1;
            }
            int ans = int.MaxValue;
            foreach (Tuple<string, string, List<Tuple<User, string, DateTime, int>>> moderators in _subForumsModerators)
            {
                if (moderators.Item1.Equals(forumName))
                {
                    foreach (Tuple<User, string, DateTime, int> moderator in moderators.Item3)
                    {
                        ans = Math.Min(ans, DateTime.Now.Date.Subtract(moderator.Item1.getRegistrationDate()).Days);
                    }
                }
            }
            return ans;
        }

        public User getUserFromForum(string forumName, string username)
        {
            List<string> input = new List<string>() { forumName, username };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null || members == null)
            {
                return null;
            }
            User user = getUser(admins, username);
            if (user == null)
            {
                user = getUser(members, username);
            }
            return user;
        }

        public string setModeratorTrialTime(string forumName, string subForumName, string moderator, int newTime, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, subForumName, moderator, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            permission p = getUserPermissionsForForum(forumName, requestingUser);
            if (p != permission.SUPER_ADMIN && p != permission.ADMIN)
            {
                return Constants.UNAUTHORIZED;
            }
            List<User> admins = getForumAdmins(forumName);
            if (admins == null)
            {
                return WRONG_FORUM_NAME;
            }
            List<Tuple<User, string, DateTime, int>> moderators = getSubForumModerators(forumName, subForumName);
            if (moderators == null)
            {
                return WRONG_SUB_FORUM_NAME;
            }
            Tuple<User, string, DateTime, int> mod = null;
            foreach (Tuple<User, string, DateTime, int> t in moderators)
            {
                if (t.Item1.getUsername().Equals(moderator))
                {
                    mod = t;
                    break;
                }
            }
            if (mod == null)
            {
                return WRONG_USERNAME;
            }
            if (p != permission.SUPER_ADMIN && !mod.Item2.Equals(requestingUser))
            {
                return Constants.UNAUTHORIZED;
            }
            if (DateTime.Now.Date.Subtract(mod.Item3).Days > newTime)
            {
                return "Cannot set trrial time to less time than the user is a moderator.";
            }
            if (!ForumSystem._testFlag)
            {
                if (!ForumSystem._db.changeModeratorTrialTime(forumName, subForumName, moderator, newTime))
                {
                    return Constants.DB_ERROR;
                }
            }
            Tuple<User, string, DateTime, int> m = new Tuple<User, string, DateTime, int>(mod.Item1, mod.Item2, mod.Item3, newTime);
            moderators.Remove(mod);
            if (moderators.Contains(mod))
            {
                return Constants.FUNCTION_ERRROR;
            }
            moderators.Add(m);
            if (!moderators.Contains(m))
            {
                throw new Exception("Critical failure - moderator was unassigned without requst while modifying trial time.");
            }
            return Constants.SUCCESS;
        }

        public List<Tuple<string, string, DateTime, string>> getListOfForumModerators(string forumName)
        {
            List<Tuple<string, string, DateTime, string>> ans = new List<Tuple<string, string, DateTime, string>>();
            foreach (Tuple<string, string, List<Tuple<User, string, DateTime, int>>> sfm in _subForumsModerators)
            {
                if (sfm.Item1.Equals(forumName))
                {
                    foreach (Tuple<User, string, DateTime, int> mod in sfm.Item3)
                    {
                        ans.Add(new Tuple<string, string, DateTime, string>(mod.Item1.getUsername(), mod.Item2, mod.Item3, sfm.Item2));
                    }
                }
            }
            return ans;
        }

        public bool isSuperAdmin(string username)
        {
            List<string> input = new List<string>() { username };
            if (!Constants.isValidInput(input))
            {
                return false;
            }
            return username.Equals(_superAdmin.getUsername());
        }

        public void notifyUsersThatNeedToUpdatePassword(string forumName, int lifespan)
        {
            string msg = ", your password in forum " + forumName + " is expired.\nNest time you will enter this forum you will be requested to update your password.";
            foreach (User user in getForumAdmins(forumName))
            {
                sendMailIfNeedsToUpdatePassword(user, lifespan, msg);
            }
            foreach (User user in getForumMembers(forumName))
            {
                sendMailIfNeedsToUpdatePassword(user, lifespan, msg);
            }
        }

        public List<Tuple<string, List<Tuple<string, string>>>> getRepeatUsersByMail()
        {
            List<Tuple<string, List<Tuple<string, string>>>> ans = new List<Tuple<string, List<Tuple<string, string>>>>();
            List<Tuple<string, string, string, bool>> usersData = new List<Tuple<string, string, string, bool>>();
            getUsersDataFromAllForums(ans, usersData, _forumsAdmins);
            getUsersDataFromAllForums(ans, usersData, _forumsMembers);
            return ans;
        }

        public Tuple<string, string> getPasswordRestorationQuestion(string forumName, string username)
        {
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null)
            {
                return new Tuple<string, string>(Constants.forumDoesntExist(forumName), null);
            }
            User u = getUser(admins, username);
            if (u == null)
            {
                u = getUser(members, username);
            }
            if (u == null)
            {
                return new Tuple<string, string>(WRONG_USERNAME, null);
            }
            return new Tuple<string, string>("true", u.getSecurityQuestion());
        }

        public string answerSecurityQuestion(string forumName, string username, string answer)
        {
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            User u = getUser(admins, username);
            if (u == null)
            {
                u = getUser(members, username);
            }
            if (u == null)
            {
                return WRONG_USERNAME;
            }
            if (!answer.Equals(u.getAnswer()))
            {
                return "Wrong answer";
            }
            Constants.sendMail(u.getEMail(), "Password restoration", "Hello " + username + Environment.NewLine + "Your password is " + u.getPassword());
            return Constants.SUCCESS;
        }

        public string setPassword(string forumName, string username, string password)
        {
            List<User> admins = getForumAdmins(forumName);
            List<User> members = getForumMembers(forumName);
            if (admins == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            User u = getUser(admins, username);
            if (u == null)
            {
                u = getUser(members, username);
            }
            if (u == null)
            {
                return WRONG_USERNAME;
            }
            u.setPassword(password);
            if (!u.getPassword().Equals(password))
            {
                return Constants.FUNCTION_ERRROR;
            }
            return Constants.SUCCESS;
        }

        private void getUsersDataFromAllForums(List<Tuple<string, List<Tuple<string, string>>>> ans,
            List<Tuple<string, string, string, bool>> usersData, List<Tuple<string, List<User>>> _forumsAdmins)
        {
            foreach (Tuple<string, List<User>> t_user in _forumsAdmins)
            {
                foreach (User u in t_user.Item2)
                {
                    bool dataContainsUser = false;
                    Tuple<string, string, string, bool> found_data = null;
                    foreach (Tuple<string, string, string, bool> t_data in usersData)
                    {
                        if (t_data.Item1.Equals(u.getEMail()))
                        {
                            found_data = t_data;
                            dataContainsUser = true;
                            break;
                        }
                    }
                    if (dataContainsUser)
                    {
                        if (!found_data.Item4)
                        {
                            usersData.Remove(found_data);
                            usersData.Add(new Tuple<string, string, string, bool>
                                (found_data.Item1, found_data.Item2, found_data.Item3, true));
                            Tuple<string, string> t = new Tuple<string, string>(found_data.Item2, found_data.Item3);
                            List<Tuple<string, string>> l = new List<Tuple<string, string>> { t };
                            ans.Add(new Tuple<string, List<Tuple<string, string>>>(found_data.Item1, l));
                        }
                        // find relevant list in ans by email and add tuple
                        foreach (Tuple<string, List<Tuple<string, string>>> t in ans)
                        {
                            if (t.Item1.Equals(u.getEMail()))
                            {
                                t.Item2.Add(new Tuple<string, string>(t_user.Item1, u.getUsername()));
                                break;
                            }
                        }
                    }
                    usersData.Add(new Tuple<string, string, string, bool>(u.getEMail(), t_user.Item1, u.getUsername(), false));
                }
            }
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

        private List<Tuple<User, string, DateTime, int>> getSubForumModerators(string forumName, string subForumName)
        {
            List<Tuple<User, string, DateTime, int>> moderators = null;
            foreach (Tuple<string, string, List<Tuple<User, string, DateTime, int>>> subForumModerators in _subForumsModerators)
            {
                if (forumName.Equals(subForumModerators.Item1) && subForumName.Equals(subForumModerators.Item2))
                {
                    moderators = subForumModerators.Item3;
                    break;
                }
            }
            return moderators;
        }

        private User getUser(List<User> users, string username)
        {
            User user = null;
            foreach (User u in users)
            {
                if (username.Equals(u.getUsername()))
                {
                    user = u;
                    break;
                }
            }
            return user;
        }

        private void sendMailIfNeedsToUpdatePassword(User user, int lifespan, string msg)
        {
            if (DateTime.Now.Date.Subtract(user.getDateOfLastPasswordChange()).Days > lifespan)
            {
                Constants.sendMail(user.getEMail(), "Your password has expired", "Hello, " + user.getUsername() + msg);
            }
        }
    }
}
