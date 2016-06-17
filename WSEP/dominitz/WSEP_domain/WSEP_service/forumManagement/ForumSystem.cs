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
using ForumsDataBase;

namespace WSEP_service.forumManagement
{
    public class ForumSystem : IForumSystem
    {
        private static IForumManager _fm;
        private static IUserManager _um;
        public static bool _testFlag;
        public static Object o = new Object();
        public static bool _dbLoaded = false;
        public static DBI _db;

        public ForumSystem()
        {
            _fm = new ForumManager();
            _um = new UserManager();
            _testFlag = false;
            _db = new DB();
            try
            {
                setup();
            }
            catch (ShouldNotHappenException)
            {
                throw new Exception("DB is badly maintained and is irrelevant.");
            }
            catch (Exception)
            {
                throw new Exception("Could not load data from DB.");
            }
        }

        public ForumSystem(bool isTest)
        {
            _fm = new ForumManager();
            _um = new UserManager();
            _testFlag = true;
        }

        private void setup()
        {
            lock (o)
            {
                if (!_dbLoaded)
                {
                    loadDB();
                }
            }
        }

        private void loadDB()
        {
            List<string> forumsNames = _db.ReturnForumsList();
            foreach (string forumName in forumsNames)
            {
                ForumPolicy forumPolicy = getForumPolicy(forumName);
                List<string> subForumsNames = _db.ReturnSubForumList(forumName);
                List<SubForum> subForums = new List<SubForum>();
                List<User> forumUsers = getForumUsers(forumName);
                foreach (string subForumName in subForumsNames)
                {
                    List<Post> threads = managePosts(forumName, subForumName, forumUsers);
                    SubForum sf = SubForum.create(forumName, subForumName, threads);
                    subForums.Add(sf);
                    getSubForumModerators(forumName, subForumName, forumUsers);
                }
                Forum f = Forum.create(forumName, forumPolicy, subForums);
                _fm.readForum(f);
            }
        }

        private void getSubForumModerators(string forumName, string subForumName, List<User> forumUsers)
        {
            List<Tuple<string, string, int, DateTime>> modsData = _db.ReturnSubforumModerators(forumName, subForumName);
            List<Tuple<User, string, DateTime, int>> moderators = new List<Tuple<User, string, DateTime, int>>();
            foreach (Tuple<string, string, int, DateTime> t in modsData)
            {
                User u = getUser(t.Item1, forumUsers);
                moderators.Add(new Tuple<User, string, DateTime, int>(u, t.Item2, t.Item4, t.Item3));
            }
            _um.readSubForumModerators(forumName, subForumName, moderators);
        }

        private List<Post> managePosts(string forumName, string subForumName, List<User> users)
        {
            List<Tuple<string, string, string, DateTime, int, int>> posts = _db.ReturnSubforumPosts(forumName, subForumName);
            List<Post> threads = new List<Post>();
            foreach (Tuple<string, string, string, DateTime, int, int> t in posts)
            {
                if (t.Item6 == 0)
                {
                    User u = getUser(t.Item3, users);
                    Post p = Post.create(t.Item1, t.Item2, u, null, t.Item5, t.Item4);
                    threads.Add(p);
                }
            }
            threads.Sort((x, y) => x.getCreationDate().CompareTo(y.getCreationDate()));
            getAllTree(threads, posts, users);
            return threads;
        }

        private void getAllTree(List<Post> roots, List<Tuple<string, string, string, DateTime, int, int>> posts, List<User> users)
        {
            foreach (Post post in roots)
            {
                List<Post> replies = new List<Post>();
                foreach (Tuple<string, string, string, DateTime, int, int> t in posts)
                {
                    if (t.Item6 == post.getSN())
                    {
                        User u = getUser(t.Item3, users);
                        Post p = Post.create(t.Item1, t.Item2, u, post, t.Item5, t.Item4);
                        replies.Add(p);
                    }
                }
                replies.Sort((x, y) => x.getCreationDate().CompareTo(y.getCreationDate()));
                foreach (Post reply in replies)
                {
                    getAllTree(roots, posts, users);
                }
                post.readReplies(replies);
            }
        }

        private User getUser(string username, List<User> users)
        {
            foreach (User user in users)
            {
                if (username.Equals(user.getUsername()))
                {
                    return user;
                }
            }
            throw new ShouldNotHappenException();
        }

        private List<User> getForumUsers(string forumName)
        {
            List<Tuple<string, string, DateTime, string>> pmsData = _db.ReturforumMessages(forumName);
            List<Tuple<string, string, string, DateTime, DateTime>> usersData = _db.ReturnForumUsers(forumName);
            List<string> forumMembers = getForumMembers(forumName);
            List<string> forumAdmins = getForumAdmins(forumName);
            List<User> members = new List<User>();
            List<User> admins = new List<User>();
            List<User> users = new List<User>();
            addUserFromList(pmsData, usersData, members, admins, users, forumMembers);
            addUserFromList(pmsData, usersData, members, admins, users, forumAdmins);
            _um.readForumMembers(forumName, members);
            _um.readForumAdmins(forumName, admins);
            return users;
        }

        private void addUserFromList(List<Tuple<string, string, DateTime, string>> pmsData,
            List<Tuple<string, string, string, DateTime, DateTime>> usersData, List<User> members, List<User> admins, 
            List<User> users, List<string> listToSearch)
        {
            foreach (string username in listToSearch)
            {
                User user = null;
                List<Tuple<string, string, PrivateMessage>> pms = new List<Tuple<string, string, PrivateMessage>>();
                foreach (Tuple<string, string, DateTime, string> data in pmsData)
                {
                    if (username.Equals(data.Item1) || username.Equals(data.Item2))
                    {
                        pms.Add(new Tuple<string, string, PrivateMessage>(data.Item1, data.Item2,
                            PrivateMessage.create(data.Item1, data.Item4, data.Item3)));
                    }
                }
                List<Tuple<string, List<PrivateMessage>>> userPMs = makePMTuples(username, pms);
                foreach (Tuple<string, string, string, DateTime, DateTime> t in usersData)
                {
                    if (username.Equals(t.Item1))
                    {
                        user = User.create(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, userPMs);
                        break;
                    }
                }
                if (user == null)
                {
                    throw new ShouldNotHappenException();
                }
                members.Add(user);
                users.Add(user);
            }
        }

        private List<Tuple<string, List<PrivateMessage>>> makePMTuples(string username, List<Tuple<string, string, PrivateMessage>> pms)
        {
            List<Tuple<string, List<PrivateMessage>>> tuples = new List<Tuple<string, List<PrivateMessage>>>();
            List<string> users = new List<string>();
            foreach (Tuple<string, string, PrivateMessage> t in pms)
            {
                if (t.Item1.Equals(username))
                {
                    users.Add(t.Item2);
                }
                else
                {
                    users.Add(t.Item1);
                }
            }
            foreach (string user in users)
            {
                List<PrivateMessage> msgs = new List<PrivateMessage>();
                foreach (Tuple<string, string, PrivateMessage> t in pms)
                {
                    if (user.Equals(t.Item1) || user.Equals(t.Item2))
                    {
                        msgs.Add(t.Item3);
                    }
                }
                msgs.Sort((x, y) => x.getCreationDate().CompareTo(y.getCreationDate()));
                tuples.Add(new Tuple<string, List<PrivateMessage>>(user, msgs));
            }
            return tuples;
        }

        private List<string> getForumAdmins(string forumName)
        {
            List<Tuple<string, string>> forumAdmins = _db.ReturnforumAdmins(forumName);
            List<string> admins = new List<string>();
            foreach (Tuple<string, string> t in forumAdmins)
            {
                admins.Add(t.Item2);
            }
            return admins;
        }

        private List<string> getForumMembers(string forumName)
        {
            List<Tuple<string, string>> forumMembers = _db.ReturnforumMembers(forumName);
            List<string> members = new List<string>();
            foreach (Tuple<string, string> t in forumMembers)
            {
                members.Add(t.Item2);
            }
            return members;
        }

        private ForumPolicy getForumPolicy(string forumName)
        {
            List<Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>>> policies =
                    _db.ReturnforumPolicy(forumName);
            Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>> policy = policies.ElementAt(0);
            postDeletionPermission pdp = postDeletionPermission.WRITER;
            switch (policy.Item3.Item1)
            {
                case 2:
                    pdp = postDeletionPermission.WRITER;
                    break;
                case 3:
                    pdp = postDeletionPermission.MODERATOR;
                    break;
                case 4:
                    pdp = postDeletionPermission.ADMIN;
                    break;
                case 5:
                    pdp = postDeletionPermission.SUPER_ADMIN;
                    break;
            }
            modUnassignmentPermission mup = modUnassignmentPermission.ADMIN;
            switch (policy.Item4.Item2)
            {
                case 2:
                    mup = modUnassignmentPermission.ADMIN;
                    break;
                case 3:
                    mup = modUnassignmentPermission.ASSIGNING_ADMIN;
                    break;
                case 4:
                    mup = modUnassignmentPermission.SUPER_ADMIN;
                    break;
            }
            return new ForumPolicy(policy.Item1.Item1, policy.Item1.Item2, policy.Item2.Item1,
                policy.Item2.Item2, pdp, policy.Item3.Item2, policy.Item4.Item1, mup);
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

        public string setForumPostDeletionPermissions(string forumName, postDeletionPermission permission, 
            string requestingUser)
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

        public string setForumModUnassignmentPermissions(string forumName, modUnassignmentPermission permission, 
            string requestingUser)
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

        public string writePost(string forumName, string subForumName, int parentPostNo, string username, string title, 
            string content)
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

        public string setModeratorTrialTime(string forumName, string subForumName, string moderator, int newTime, 
            string requestingUser)
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

        public List<Tuple<string, string, DateTime, int>> getListOfMemberMessages(string forumName, string memberName,
            string requestingUser)
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

        public List<Tuple<string, string, DateTime, int, int, string, DateTime>>
            getThread(string forumName, string subForumName, int openPostNo, string requestingUser)
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
