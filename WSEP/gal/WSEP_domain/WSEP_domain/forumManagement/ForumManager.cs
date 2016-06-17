using System;
using System.Collections.Generic;
using WSEP_domain.forumManagement.forumHandler;
using WSEP_domain.forumSystem;
using WSEP_domain.userManagement;

namespace WSEP_domain.forumManagement
{
    public class ForumManager
    {

        private UserManager _um;
        private List<Forum> _forums;

        public ForumManager()
        {
            _um = new UserManager();
            _forums = new List<Forum>();
        }
        
        public void setUserManager(UserManager um)
        {
            _um = um;
        }

        public void readForum(Forum forum)
        {
            _forums.Add(forum);
        }
        
        public string addForum(string forumName, string requestingUser)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            string res = _um.addForum(forumName, requestingUser);
            if (!res.Equals(Constants.SUCCESS))
            {
                return res;
            }
            Forum f = getForum(forumName);
            if (f != null)
            {
                return "Forum " + forumName + " already exist.";
            }
            f = Forum.create(forumName);
            if (f == null)
            {
                return "Invalid forum name.";
            }
            if (!ForumSystem._testFlag)
            {
                if (ForumSystem._db.addForum(forumName))
                {
                    if (!ForumSystem._db.addForumPolicy(ForumPolicy.DEFAULT_MAX_ADMINS, ForumPolicy.DEFAULT_MIN_ADMINS,
                        ForumPolicy.DEFAULT_MAX_MODS, ForumPolicy.DEAFULT_MIN_MODS, forumName, ForumPolicy.DEFAULT_PDP, ForumPolicy.DEFAULT_LIFESPAN,
                        ForumPolicy.DEFAULT_SENIORITY, ForumPolicy.DEFAULT_MUP))
                    {
                        if (ForumSystem._db.removeForum(forumName))
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
            _forums.Add(f);
            if (!_forums.Contains(f))
            {
                return Constants.FUNCTION_ERRROR;
            }
            return Constants.SUCCESS;
        }

        public string addSubForum(string forumName, string subForumName, List<Tuple<string, string, int>> moderators,
            string requestingUser)
        {
            if (moderators == null)
            {
                return Constants.INVALID_INPUT;
            }
            List<string> input = new List<string>() { forumName, subForumName };
            foreach (Tuple<string, string, int> t in moderators)
            {
                input.Add(t.Item1);
                //input.Add(t.Item2);
            }
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            int maxNumOfModerators = f.getMaxModerators();
            int minNumOfModerators = f.getMinModerators();
            if (moderators.Count > maxNumOfModerators || moderators.Count < minNumOfModerators)
            {
                return Constants.ILLEGAL_ACTION;
            }
            int seniority = f.getSeniorityLimit();
            string res = _um.addSubForum(forumName, subForumName, moderators, requestingUser, seniority);
            if (!res.Equals(Constants.SUCCESS))
            {
                return res;
            }
            return f.addSubForum(subForumName);
        }

        public string registerMemberToForum(string forumName, string username, string password, string eMail, string securityQuestion, string answer)
        {
            List<string> input = new List<string>() { forumName, username, password, eMail, securityQuestion, answer };
            if (!Constants.isValidInput(input) || !Constants.isValidEmail(eMail))
            {
                return Constants.INVALID_INPUT;
            }
            return _um.registerMemberToForum(forumName, username, password, eMail, securityQuestion, answer);
        }

        public string assignAdmin(string forumName, string username, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, username, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            int maxNumOfAdmins = f.getMaxAdmins();
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

        public string unassignAdmin(string forumName, string username, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, username, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            int minNumOfAdmins = f.getMinAdmins();
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

        public string assignModerator(string forumName, string subForumName, string username, string requestingUser, int days)
        {
            List<string> input = new List<string>() { forumName, subForumName, username, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            int maxNumOfModerators = f.getMaxModerators();
            int currentAmountOfModerators = _um.getNumOfModerators(forumName, subForumName);
            if (currentAmountOfModerators >= maxNumOfModerators)
            {
                return Constants.ILLEGAL_ACTION;
            }
            return _um.assignModerator(forumName, subForumName, username, requestingUser, days);
        }

        public string unassignModerator(string forumName, string subForumName, string username, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, subForumName, username, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            int minNumOfModerators = f.getMinModerators();
            int currentAmountOfModerators = _um.getNumOfModerators(forumName, subForumName);
            if (currentAmountOfModerators <= minNumOfModerators)
            {
                return Constants.ILLEGAL_ACTION;
            }
            bool onlyAssigningAdmin = f.getModUnassignmentPermissions() >= modUnassignmentPermission.ASSIGNING_ADMIN;
            return _um.unassignModerator(forumName, subForumName, username, requestingUser, onlyAssigningAdmin);
        }

        public permission getUserPermissionsForForum(string forumName, string username)
        {
            List<string> input = new List<string>() { forumName, username };
            if (!Constants.isValidInput(input))
            {
                return permission.INVALID;
            }
            return _um.getUserPermissionsForForum(forumName, username);
        }

        public permission getUserPermissionsForSubForum(string forumName, string subForumName, string username)
        {
            List<string> input = new List<string>() { forumName, subForumName, username };
            if (!Constants.isValidInput(input))
            {
                return permission.INVALID;
            }
            return _um.getUserPermissionsForSubForum(forumName, subForumName, username);
        }

        public string sendPM(string forumName, string from, string to, string msg)
        {
            List<string> input = new List<string>() { forumName, from, to, msg };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            return _um.sendPM(forumName, from, to, msg);
        }

        public string checkForumPolicy(string forumName, ForumPolicy policy)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input) && policy != null)
            {
                return Constants.INVALID_INPUT;
            }
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
            if (minModeratorSeniority < policy.getModeratorsSeniority())
            {
                return "One of the moderators of forum " + forumName + " has a seniority of " + minModeratorSeniority + " days. Cannot set minimum seniotiry to " + policy.getModeratorsSeniority();
            }
            return Constants.SUCCESS;
        }

        public loginStatus login(string forumName, string username, string password)
        {
            List<string> input = new List<string>() { forumName, username, password };
            if (!Constants.isValidInput(input))
            {
                return loginStatus.FALSE;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return loginStatus.FALSE;
            }
            loginStatus ls = _um.login(forumName, username, password, f.getPasswordLifespan());
            if (ls != loginStatus.FALSE)
            {
                f.observe(username);
            }
            return ls;
        }
        // checked coverage up to here
        public string setForumMaxAdmins(string forumName, int maxAdmins, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            if (_um.getUserPermissionsForForum(forumName, requestingUser) < permission.ADMIN)
            {
                return "Unauthorized user";
            }
            int numOfAdmins = _um.getNumOfAdmins(forumName);
            if (numOfAdmins > maxAdmins)
            {
                return Constants.ILLEGAL_ACTION;
            }
            f.setMaxAdmins(maxAdmins);
            return maxAdmins == f.getMaxAdmins() ? Constants.SUCCESS : Constants.FUNCTION_ERRROR; 
        }

        public string setForumMinAdmins(string forumName, int minAdmins, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (!Constants.isValidInput(input) && minAdmins >= 1)
            {
                return Constants.INVALID_INPUT;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            if (_um.getUserPermissionsForForum(forumName, requestingUser) < permission.ADMIN)
            {
                return "Unauthorized user";
            }
            int numOfAdmins = _um.getNumOfAdmins(forumName);
            if (numOfAdmins < minAdmins)
            {
                return Constants.ILLEGAL_ACTION;
            }
            f.setMinAdmins(minAdmins);
            return minAdmins == f.getMinAdmins() ? Constants.SUCCESS : Constants.FUNCTION_ERRROR;
        }

        public string setForumMaxModerators(string forumName, int maxModerators, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            if (_um.getUserPermissionsForForum(forumName, requestingUser) < permission.ADMIN)
            {
                return "Unauthorized user";
            }
            int maxNumOfModerators = _um.getMaxModerators(forumName);
            if (maxNumOfModerators > maxModerators)
            {
                return Constants.ILLEGAL_ACTION;
            }
            f.setMaxModerators(maxModerators);
            return maxModerators == f.getMaxModerators() ? Constants.SUCCESS : Constants.FUNCTION_ERRROR;
        }

        public string setForumMinModerators(string forumName, int minModerators, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            if (_um.getUserPermissionsForForum(forumName, requestingUser) < permission.ADMIN)
            {
                return "Unauthorized user";
            }
            int minNumOfModerators = _um.getMinModerators(forumName);
            if (minNumOfModerators < minModerators)
            {
                return Constants.ILLEGAL_ACTION;
            }
            f.setMinModerators(minModerators);
            return minModerators == f.getMinModerators() ? Constants.SUCCESS : Constants.FUNCTION_ERRROR;
        }

        public string setForumPostDeletionPermissions(string forumName, postDeletionPermission permission,
            string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            if (_um.getUserPermissionsForForum(forumName, requestingUser) < userManagement.permission.ADMIN)
            {
                return "Unauthorized user";
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            f.setPostDeletionPermissions(permission);
            return permission == f.getPostDeletionPermissions() ? Constants.SUCCESS : Constants.FUNCTION_ERRROR;
        }

        public string setForumPasswordLifespan(string forumName, int lifespan, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (!Constants.isValidInput(input) && lifespan >= 0)
            {
                return Constants.INVALID_INPUT;
            }
            if (_um.getUserPermissionsForForum(forumName, requestingUser) < userManagement.permission.ADMIN)
            {
                return "Unauthorized user";
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            f.setPasswordLifespan(lifespan);
            if (lifespan != f.getPasswordLifespan())
            {
                return Constants.FUNCTION_ERRROR;
            }
            _um.notifyUsersThatNeedToUpdatePassword(forumName, lifespan);
            return Constants.SUCCESS;
        }

        public string setForumModeratorsSeniority(string forumName, int seniority, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (!Constants.isValidInput(input) && seniority >= 0)
            {
                return Constants.INVALID_INPUT;
            }
            if (_um.getUserPermissionsForForum(forumName, requestingUser) < userManagement.permission.ADMIN)
            {
                return "Unauthorized user";
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            f.setModeratorsSeniority(seniority);
            return seniority == f.getModeratorsSeniority() ? Constants.SUCCESS : Constants.FUNCTION_ERRROR;
        }

        public string setForumModUnassignmentPermissions(string forumName, modUnassignmentPermission permission,
            string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            if (_um.getUserPermissionsForForum(forumName, requestingUser) < userManagement.permission.ADMIN)
            {
                return "Unauthorized user";
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            f.setModUnassignmentPermissions(permission);
            return permission == f.getModUnassignmentPermissions() ? Constants.SUCCESS : Constants.FUNCTION_ERRROR;
        }

        public string writePost(string forumName, string subForumName, int parentPostNo, string username, string title,
            string content)
        {
            List<string> input = new List<string>() { forumName, subForumName, username };
            // title and content are not in input list as they are checked seperately since one of them can be empty
            if (!Constants.isValidInput(input) || !Constants.isValidPost(title, content))
            {
                return Constants.INVALID_INPUT;
            }
            User user = _um.getUserFromForum(forumName, username);
            if (user == null)
            {
                return "Wrong forum name or username.";
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            return f.writePost(subForumName, parentPostNo, user, title, content);
        }

        public string setModeratorTrialTime(string forumName, string subForumName, string moderator, int newTime,
            string requestingUser)
        {
            List<string> input = new List<string>() { forumName, subForumName, moderator, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            return _um.setModeratorTrialTime(forumName, subForumName, moderator, newTime, requestingUser);
        }

        public string deletePost(string forumName, string subForumName, int postNo, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, subForumName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            postDeletionPermission pdp = f.getPostDeletionPermissions();
            if (pdp == postDeletionPermission.INVALID)
            {
                return Constants.noPermissionToDeletePost(requestingUser);
            }
            permission p = _um.getUserPermissionsForSubForum(forumName, subForumName, requestingUser);
            if (p == permission.INVALID)
            {
                return Constants.noPermissionToDeletePost(requestingUser);
            }
            return f.deletePost(subForumName, postNo, pdp, p, requestingUser);
        }

        public string editPost(string forumName, string subForumName, int postNo, string requestingUser, string content)
        {
            List<string> input = new List<string>() { forumName, subForumName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            permission p = _um.getUserPermissionsForSubForum(forumName, subForumName, requestingUser);
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            return f.editPost(subForumName, postNo, requestingUser, p, content);
        }

        public int getNumOfPostsInSubForum(string forumName, string subForumName, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, subForumName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return -1;
            }
            permission p = _um.getUserPermissionsForForum(forumName, requestingUser);
            if (p < permission.ADMIN)
            {
                return -1;
            }
            return f.getNumOfPostsInSubForum(subForumName);
        }

        public int getNumOfForums(string requestingUser)
        {
            List<string> input = new List<string>() { requestingUser };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            return _um.isSuperAdmin(requestingUser) ? _forums.Count : -1;
        }

        public List<Tuple<string, string, DateTime, int>> getListOfMemberMessages(string forumName, string memberName,
            string requestingUser)
        {
            List<string> input = new List<string>() { forumName, memberName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            permission p = _um.getUserPermissionsForForum(forumName, requestingUser);
            if (p < permission.ADMIN)
            {
                return null;
            }
            List<Tuple<string, string, DateTime, int>> list = new List<Tuple<string, string, DateTime, int>>();
            Forum f = getForum(forumName);
            if (f == null)
            {
                return list;
            }
            return f.getListOfMemberMessages(list, memberName);
        }

        public List<Tuple<string, string, DateTime, string>> getListOfForumModerators(string forumName, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            permission p = _um.getUserPermissionsForForum(forumName, requestingUser);
            if (p < permission.ADMIN)
            {
                return null;
            }
            return _um.getListOfForumModerators(forumName);
        }

        public List<string> getForums()
        {
            List<string> ans = new List<string>();
            foreach (Forum f in _forums)
            {
                ans.Add(f.getName());
            }
            return ans;
        }

        public List<string> getSubForums(string forumName, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            //  verify user has permissions
            List<string> list = new List<string>();
            Forum f = getForum(forumName);
            if (f != null)
            {
                f.getSubForumsList(list);
            }
            f.observe(requestingUser);
            return list;
        }

        public List<Tuple<string, DateTime, int>> getThreads(string forumName, string subForumName, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, subForumName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            // verify user has permissions
            List<Tuple<string, DateTime, int>> list = new List<Tuple<string, DateTime, int>>();
            Forum f = getForum(forumName);
            if (f != null)
            {
                f.addThreadsToList(list, subForumName, requestingUser);
            }
            f.unobserve(requestingUser);
            return list;
        }

        public List<Tuple<string, string, DateTime, int, int, string, DateTime>> getThread(string forumName, string subForumName, int openPostNo, string requestingUser)
        {
            List<string> input = new List<string>() { forumName, subForumName, requestingUser };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            // verify user has permissions
            List<Tuple<string, string, DateTime, int, int, string, DateTime>> list = new List<Tuple<string, string, DateTime, int, int, string, DateTime>>();
            Forum f = getForum(forumName);
            if (f != null)
            {
                f.addPostsToList(list, subForumName, openPostNo, requestingUser);
            }
            return list;
        }

        public List<Tuple<string, List<Tuple<string, string>>>> getRepeatUsersByMail(string requestingUser)
        {
            List<string> input = new List<string>() { requestingUser };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            if (!_um.isSuperAdmin(requestingUser))
            {
                return null;
            }
            return _um.getRepeatUsersByMail();
        }

        public Tuple<string, string> getPasswordRestorationQuestion(string forumName, string username)
        {
            List<string> input = new List<string>() { forumName, username };
            if (!Constants.isValidInput(input))
            {
                return new Tuple<string, string>(Constants.INVALID_INPUT, null);
            }
            return _um.getPasswordRestorationQuestion(forumName, username);
        }

        public string answerSecurityQuestion(string forumName, string username, string answer)
        {
            List<string> input = new List<string>() { forumName, username, answer };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            return _um.answerSecurityQuestion(forumName, username, answer);
        }

        public string setPassword(string forumName, string username, string password)
        {
            List<string> input = new List<string>() { forumName, username, password };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            return _um.setPassword(forumName, username, password);
        }

        private Forum getForum(string forumName)
        {
            foreach (Forum forum in _forums)
            {
                if (forum.getName().Equals(forumName))
                {
                    return forum;
                }
            }
            return null;
        }
    }
}
