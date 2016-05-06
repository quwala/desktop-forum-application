using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP_domain.forumManagement.forumHandler;
using WSEP_domain.forumManagement;
using WSEP_domain.userManagement;
using WSEP_domain.forumManagement.threadsHandler;
using WSEP_domain;

namespace WSEP_service.forumManagement
{
    public class ForumSystem : IForumSystem
    {
        private IForumManager _fm;
        private IUserManager _um;
        public static bool _testFlag;
        public static Object o = new Object();

        public ForumSystem()
        {
            _fm = new WSEP_domain.forumManagement.ForumManager();
            _um = new WSEP_domain.userManagement.UserManager();
            _testFlag = false;
        }

        public ForumSystem(bool isTest)
        {
            _fm = new WSEP_domain.forumManagement.ForumManager();
            _um = new WSEP_domain.userManagement.UserManager();
            _testFlag = true;
        }

        public string addForum(string forumName, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    string res = _um.addForum(forumName, requestingUser);
                    if (res.Equals(Constants.SUCCESS))
                    {
                        return _fm.addForum(forumName);
                    }
                    return res;
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string addSubForum(string forumName, string subForumName, List<Tuple<string, string, int>> moderators, string requestingUser)
        {
            if (moderators == null)
            {
                return Constants.INVALID_INPUT;
            }
            List<string> input = new List<string>() { forumName, subForumName };
            foreach (Tuple<string, string, int> t in moderators)
            {
                input.Add(t.Item1);
                input.Add(t.Item2);
            }
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    int maxNumOfModerators = _fm.getForumMaxModerators(forumName);
                    if (maxNumOfModerators == -1)
                    {
                        return Constants.forumDoesntExist(forumName);
                    }
                    if (moderators.Count > maxNumOfModerators)
                    {
                        return Constants.ILLEGAL_ACTION;
                    }
                    int seniority = _fm.getForumSeniorityLimit(forumName);
                    string res = _um.addSubForum(forumName, subForumName, moderators, requestingUser, seniority);
                    if (res.Equals(Constants.SUCCESS))
                    {
                        return _fm.addSubForum(forumName, subForumName);
                    }
                    return res;
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string registerMemberToForum(string forumName, string username, string password, string eMail)
        {
            List<string> input = new List<string>() { forumName, username, password, eMail };
            if (Constants.isValidInput(input) && Constants.isValidEmail(eMail))
            {
                lock (o)
                {
                    return _um.registerMemberToForum(forumName, username, password, eMail);
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string assignAdmin(string forumName, string username, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, username, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    int maxNumOfAdmins = _fm.getForumMaxAdmins(forumName);
                    if (maxNumOfAdmins == -1)
                    {
                        return Constants.forumDoesntExist(forumName);
                    }
                    int currentAmountOfAdmins = _um.getNumOfAdmins(forumName);
                    if (currentAmountOfAdmins == -1)
                    {
                        throw new Exception("Forum was successfully added to one section of the system yet not to another. CRITICAL FAILURE");
                    }
                    if (currentAmountOfAdmins >= maxNumOfAdmins)
                    {
                        return Constants.ILLEGAL_ACTION;
                    }
                    return _um.assignAdmin(forumName, username, requestingUser);
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string unassignAdmin(string forumName, string username, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, username, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    int minNumOfAdmins = _fm.getForumMinAdmins(forumName);
                    if (minNumOfAdmins == -1)
                    {
                        return Constants.forumDoesntExist(forumName);
                    }
                    int currentAmountOfAdmins = _um.getNumOfAdmins(forumName);
                    if (currentAmountOfAdmins == -1)
                    {
                        throw new Exception("Forum was successfully added to one section of the system yet not to another. CRITICAL FAILURE");
                    }
                    if (currentAmountOfAdmins <= minNumOfAdmins)
                    {
                        return Constants.ILLEGAL_ACTION;
                    }
                    return _um.unassignAdmin(forumName, username, requestingUser);
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string assignModerator(string forumName, string subForumName, string username, string requestingUser, int days)
        {
            List<string> input = new List<string>() { forumName, subForumName, username, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    int maxNumOfModerators = _fm.getForumMaxModerators(forumName);
                    if (maxNumOfModerators == -1)
                    {
                        return Constants.forumDoesntExist(forumName);
                    }
                    int currentAmountOfModerators = _um.getNumOfModerators(forumName, subForumName);
                    if (currentAmountOfModerators >= maxNumOfModerators)
                    {
                        return Constants.ILLEGAL_ACTION;
                    }
                    return _um.assignModerator(forumName, subForumName, username, requestingUser, days);
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string unassignModerator(string forumName, string subForumName, string username, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, subForumName, username, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    int minNumOfModerators = _fm.getForumMinModerators(forumName);
                    if (minNumOfModerators == -1)
                    {
                        return Constants.forumDoesntExist(forumName);
                    }
                    int currentAmountOfModerators = _um.getNumOfModerators(forumName, subForumName);
                    if (currentAmountOfModerators <= minNumOfModerators)
                    {
                        return Constants.ILLEGAL_ACTION;
                    }
                    bool onlyAssigningAdmin = _fm.getForumModUnassignmentPermissions(forumName) >= modUnassignmentPermission.ASSIGNING_ADMIN;
                    return _um.unassignModerator(forumName, subForumName, username, requestingUser, onlyAssigningAdmin);
                }
            }
            return Constants.INVALID_INPUT;
        }
        
        public permission getUserPermissionsForForum(string forumName, string username)
        {
            List<string> input = new List<string>() { forumName, username };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    return _um.getUserPermissionsForForum(forumName, username);
                }
            }
            return permission.INVALID;
        }
        
        public permission getUserPermissionsForSubForum(string forumName, string subForumName, string username)
        {
            List<string> input = new List<string>() { forumName, subForumName, username };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    return _um.getUserPermissionsForSubForum(forumName, subForumName, username);
                }
            }
            return permission.INVALID;
        }
        
        public string sendPM(string forumName, string from, string to, string msg)
        {
            List<string> input = new List<string>() { forumName, from, to, msg };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    return _um.sendPM(forumName, from, to, msg);
                }
            }
            return Constants.INVALID_INPUT;
        }
        
        public string checkForumPolicy(string forumName, ForumPolicy policy)
        {
            List<string> input = new List<string>() { forumName };
            if (Constants.isValidInput(input) && policy != null)
            {
                lock (o)
                {
                    int numOfAdmins = _um.getNumOfAdmins(forumName);
                    int maxNumOfModerators = _um.getMaxModerators(forumName);
                    int minNumOfModerators = _um.getMinModerators(forumName);
                    int minModeratorSeniority = _um.getMinModeratorSeniority(forumName);
                    if (numOfAdmins > policy.getMaxAdmins())
                    {
                        return "Forum " + forumName + " has " + numOfAdmins + " admins. Cannot set max num of admins to " + policy.getMaxAdmins();
                    }
                    if (numOfAdmins < policy.getMinAdmins())
                    {
                        return "Forum " + forumName + " has " + numOfAdmins + " admins. Cannot set min num of admins to " + policy.getMinAdmins();
                    }
                    if (maxNumOfModerators > policy.getMaxModerators())
                    {
                        return "One of forum " + forumName + " sub forums has " + maxNumOfModerators + " moderators. Cannot set max num of moderators to " + policy.getMaxModerators();
                    }
                    if (minNumOfModerators < policy.getMinModerators())
                    {
                        return "One of forum " + forumName + " sub forums has " + minNumOfModerators + " moderators. Cannot set min num of moderators to " + policy.getMinModerators();
                    }
                    if (minModeratorSeniority < policy.getSeniority())
                    {
                        return "One of the moderators of forum " + forumName + " has a seniority of " + minModeratorSeniority + " days. Cannot set minimum seniotiry to " + policy.getSeniority();
                    }
                    return Constants.SUCCESS;
                }
            }
            return Constants.INVALID_INPUT;
        }

        public loginStatus login(string forumName, string username, string password)
        {
            List<string> input = new List<string>() { forumName, username, password };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    return _um.login(forumName, username, password, _fm.getForumPasswordLifespan(forumName));
                }
            }
            return loginStatus.FALSE;
        }

        public string setForumMaxAdmins(string forumName, int maxAdmins, string requestingUser)
        {
            List<string> input = new List<string>() { forumName };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    if (_um.getUserPermissionsForForum(forumName, requestingUser) != permission.SUPER_ADMIN && _um.getUserPermissionsForForum(forumName, requestingUser) != permission.ADMIN)
                    {
                        return "Unauthorized user";
                    }
                    int numOfAdmins = _um.getNumOfAdmins(forumName);
                    if (numOfAdmins > maxAdmins)
                    {
                        return Constants.ILLEGAL_ACTION;
                    }
                    if (!_fm.setForumMaxAdmins(forumName, maxAdmins))
                    {
                        return Constants.forumDoesntExist(forumName);
                    }
                    return Constants.SUCCESS;
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string setForumMinAdmins(string forumName, int minAdmins, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (Constants.isValidInput(input) && minAdmins >= 1)
            {
                lock (o)
                {
                    if (_um.getUserPermissionsForForum(forumName, requestingUser) != permission.SUPER_ADMIN && _um.getUserPermissionsForForum(forumName, requestingUser) != permission.ADMIN)
                    {
                        return "Unauthorized user";
                    }
                    int numOfAdmins = _um.getNumOfAdmins(forumName);
                    if (numOfAdmins < minAdmins)
                    {
                        return Constants.ILLEGAL_ACTION;
                    }
                    if (!_fm.setForumMinAdmins(forumName, minAdmins))
                    {
                        return Constants.forumDoesntExist(forumName);
                    }
                    return Constants.SUCCESS;
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string setForumMaxModerators(string forumName, int maxModerators, string requestingUser)
        {
            List<string> input = new List<string>() { forumName };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    if (_um.getUserPermissionsForForum(forumName, requestingUser) != permission.SUPER_ADMIN && _um.getUserPermissionsForForum(forumName, requestingUser) != permission.ADMIN)
                    {
                        return "Unauthorized user";
                    }
                    int maxNumOfModerators = _um.getMaxModerators(forumName);
                    if (maxNumOfModerators > maxModerators)
                    {
                        return Constants.ILLEGAL_ACTION;
                    }
                    if (!_fm.setForumMaxModerators(forumName, maxModerators))
                    {
                        return Constants.forumDoesntExist(forumName);
                    }
                    return Constants.SUCCESS;
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string setForumMinModerators(string forumName, int minModerators, string requestingUser)
        {
            List<string> input = new List<string>() { forumName };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    if (_um.getUserPermissionsForForum(forumName, requestingUser) != permission.SUPER_ADMIN && _um.getUserPermissionsForForum(forumName, requestingUser) != permission.ADMIN)
                    {
                        return "Unauthorized user";
                    }
                    int minNumOfModerators = _um.getMinModerators(forumName);
                    if (minNumOfModerators < minModerators)
                    {
                        return Constants.ILLEGAL_ACTION;
                    }
                    if (!_fm.setForumMaxModerators(forumName, minModerators))
                    {
                        return Constants.forumDoesntExist(forumName);
                    }
                    return Constants.SUCCESS;
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string setForumPostDeletionPermissions(string forumName, postDeletionPermission permission, string requestingUser)
        {
            List<string> input = new List<string>() { forumName };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    if (_um.getUserPermissionsForForum(forumName, requestingUser) != WSEP_domain.userManagement.permission.SUPER_ADMIN && _um.getUserPermissionsForForum(forumName, requestingUser) != WSEP_domain.userManagement.permission.ADMIN)
                    {
                        return "Unauthorized user";
                    }
                    if (!_fm.setForumPostDeletionPermissions(forumName, permission))
                    {
                        return Constants.forumDoesntExist(forumName);
                    }
                    return Constants.SUCCESS;
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string setForumPasswordLifespan(string forumName, int lifespan, string requestingUser)
        {
            List<string> input = new List<string>() { forumName };
            if (Constants.isValidInput(input) && lifespan >= 0)
            {
                lock (o)
                {
                    if (_um.getUserPermissionsForForum(forumName, requestingUser) != permission.SUPER_ADMIN && _um.getUserPermissionsForForum(forumName, requestingUser) != permission.ADMIN)
                    {
                        return "Unauthorized user";
                    }
                    if (!_fm.setForumPasswordLifespan(forumName, lifespan))
                    {
                        return Constants.forumDoesntExist(forumName);
                    }
                    // notify users with expired passwords
                    _um.notifyUsersThatNeedToUpdatePassword(forumName, lifespan);
                    return Constants.SUCCESS;
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string setForumModeratorsSeniority(string forumName, int seniority, string requestingUser)
        {
            List<string> input = new List<string>() { forumName };
            if (Constants.isValidInput(input) && seniority >= 0)
            {
                lock (o)
                {
                    if (_um.getUserPermissionsForForum(forumName, requestingUser) != permission.SUPER_ADMIN && _um.getUserPermissionsForForum(forumName, requestingUser) != permission.ADMIN)
                    {
                        return "Unauthorized user";
                    }
                    int minModeratorSeniority = _um.getMinModeratorSeniority(forumName);
                    if (minModeratorSeniority < seniority)
                    {
                        return Constants.ILLEGAL_ACTION;
                    }
                    if (!_fm.setForumModeratorsSeniority(forumName, seniority))
                    {
                        return Constants.forumDoesntExist(forumName);
                    }
                    return Constants.SUCCESS;
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string setForumModUnassignmentPermissions(string forumName, modUnassignmentPermission permission, string requestingUser)
        {
            List<string> input = new List<string>() { forumName };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    if (_um.getUserPermissionsForForum(forumName, requestingUser) != WSEP_domain.userManagement.permission.SUPER_ADMIN && _um.getUserPermissionsForForum(forumName, requestingUser) != WSEP_domain.userManagement.permission.ADMIN)
                    {
                        return "Unauthorized user";
                    }
                    if (!_fm.setForumModUnassignmentPermissions(forumName, permission))
                    {
                        return Constants.forumDoesntExist(forumName);
                    }
                    return Constants.SUCCESS;
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string writePost(string forumName, string subForumName, int parentPostNo, string username, string title, string content)
        {
            List<string> input = new List<string>() { forumName, subForumName, username, title, content };
            if (Constants.isValidInput(input) && Constants.isValidPost(title, content))
            {
                lock (o)
                {
                    User user = _um.getUserFromForum(forumName, username);
                    if (user == null)
                    {
                        return "Wrong forum name or username.";
                    }
                    return _fm.writePost(forumName, subForumName, parentPostNo, user, title, content);
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string setModeratorTrialTime(string forumName, string subForumName, string moderator, int newTime, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, subForumName, moderator, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    return _um.setModeratorTrialTime(forumName, subForumName, moderator, newTime, requestingUser);
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string deletePost(string forumName, string subForumName, int postNo, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, subForumName, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    postDeletionPermission pdp = _fm.getForumPostDeletionPermission(forumName);
                    if (pdp == postDeletionPermission.INVALID)
                    {
                        return Constants.noPermissionToDeletePost(requestingUser);
                    }
                    permission p = _um.getUserPermissionsForSubForum(forumName, subForumName, requestingUser);
                    if (p == permission.INVALID)
                    {
                        return Constants.noPermissionToDeletePost(requestingUser);
                    }
                    return _fm.deletePost(forumName, subForumName, postNo, pdp, p, requestingUser);
                }
            }
            return Constants.INVALID_INPUT;
        }

        public string editPost(string forumName, string subForumName, int postNo, string requestingUser, string content)
        {
            List<string> input = new List<string>() { forumName, subForumName, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    permission p = _um.getUserPermissionsForSubForum(forumName, subForumName, requestingUser);
                    return _fm.editPost(forumName, subForumName, postNo, requestingUser, p, content);
                }
            }
            return Constants.INVALID_INPUT;
        }

        public int getNumOfPostsInSubForum(string forumName, string subForumName, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, subForumName, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    permission p = _um.getUserPermissionsForForum(forumName, requestingUser);
                    if (p < permission.ADMIN)
                    {
                        return -1;
                    }
                    return _fm.getNumOfPostsInSubForum(forumName, subForumName);
                }
            }
            return -1;
        }

        public List<Tuple<string, string, DateTime, int>> getListOfMemberMessages(string forumName, string memberName, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, memberName, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    permission p = _um.getUserPermissionsForForum(forumName, requestingUser);
                    if (p < permission.ADMIN)
                    {
                        return null;
                    }
                    return _fm.getListOfMemberMessages(forumName, memberName);
                }
            }
            return null;
        }

        public List<Tuple<string, string, DateTime, string>> getListOfForumModerators(string forumName, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    permission p = _um.getUserPermissionsForForum(forumName, requestingUser);
                    if (p < permission.ADMIN)
                    {
                        return null;
                    }
                    return _um.getListOfForummoderators(forumName);
                }
            }
            return null;
        }

        public int numOfForums(string requestingUser)
        {
            List<string> input = new List<string>() { requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    if (!_um.isSuperAdmin(requestingUser))
                    {
                        return -1;
                    }
                    return _fm.getNumOfForums();
                }
            }
            return -1;
        }

        public List<string> getForums()
        {
            lock (o)
            {
                return _fm.getForums();
            }
        }

        public List<string> getSubForums(string forumName, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    // get user permissions and verify can get a list of sub forums
                    return _fm.getSubForums(forumName);
                }
            }
            return null;
        }

        public List<Tuple<string, DateTime, int>> getThreads(string forumName, string subForumName, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, subForumName, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    // get user permissions and verify can get a list of threads
                    return _fm.getThreads(forumName, subForumName);
                }
            }
            return null;
        }

        public List<Tuple<string, string, DateTime, int, int, string, DateTime>> getThread(string forumName, string subForumName, int openPostNo, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, subForumName, requestingUser };
            if (Constants.isValidInput(input))
            {
                lock (o)
                {
                    // get user permissions and verify can get a thread
                    return _fm.getThread(forumName, subForumName, openPostNo);
                }
            }
            return null;
        }
    }
}
