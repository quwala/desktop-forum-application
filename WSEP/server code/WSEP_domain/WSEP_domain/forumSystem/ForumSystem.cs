using LogServices;
using Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSEP_domain.forumManagement;
using WSEP_domain.forumManagement.forumHandler;
using WSEP_domain.forumManagement.threadsHandler;
using WSEP_domain.userManagement;

namespace WSEP_domain.forumSystem
{
    public class ForumSystem :IForumSystem
    {
        private static ForumManager _fm;
        private static UserManager _um;
        private ServerLog _sl;
        private static Object o = new Object();
        private static bool _dbLoaded = false;
        private static List<DLObserverable> _allConnectedUsers = new List<DLObserverable>();

        public static bool _testFlag = true;
        public static ForumsDataBase.DBI _db;

        public ForumSystem()
        {
            _fm = new ForumManager();
            _um = new UserManager();
            _sl = new ServerLog();
            _allConnectedUsers = new List<DLObserverable>();
            /*
            _testFlag = false;
            _db = new ForumsDataBase.DB();
            try
            {
                setup();
                _sl.appendResult("Data loaded from DB");
            }
            catch (ShouldNotHappenException)
            {
                _sl.appendResult("DB is badly maintained and is irrelevant.");
                throw new Exception("DB is badly maintained and is irrelevant.");
            }
            catch (Exception)
            {
                _sl.appendResult("Could not load data from DB.");
                throw new Exception("Could not load data from DB.");
            }
            _fm.setUserManager(_um);
            */
        }

        public ForumSystem(bool isTest)
        {
            _fm = new ForumManager();
            _um = new UserManager();
            _testFlag = true;
            _sl = new ServerLog();
            _allConnectedUsers = new List<DLObserverable>();
        }

        public static void notify(string msg, List<string> usernames, string forum)
        {
            foreach (DLObserverable dlo in _allConnectedUsers)
            {
                dlo.send(msg, usernames, forum);
            }
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
                    List<string> l = _db.ReturnPostFollowers(t.Item5);
                    User u = getUser(t.Item3, users);
                    Post p = Post.create(t.Item1, t.Item2, u, null, t.Item5, t.Item4, l);
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
                        List<string> l = _db.ReturnPostFollowers(t.Item5);
                        User u = getUser(t.Item3, users);
                        Post p = Post.create(t.Item1, t.Item2, u, post, t.Item5, t.Item4, l);
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
            List<Tuple<string, string, string, DateTime, DateTime, string, string>> usersData = _db.ReturnForumUsers(forumName);
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
            List<Tuple<string, string, string, DateTime, DateTime, string, string>> usersData, List<User> members, List<User> admins,
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
                foreach (Tuple<string, string, string, DateTime, DateTime, string, string> t in usersData)
                {
                    if (username.Equals(t.Item1))
                    {
                        user = User.create(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, userPMs, t.Item6, t.Item7);
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
            return ForumPolicy.create(policy.Item1.Item1, policy.Item1.Item2, policy.Item2.Item1,
                policy.Item2.Item2, pdp, policy.Item3.Item2, policy.Item4.Item1, mup);
        }

        public string addForum(string forumName, string requestingUser)
        {
            _sl.append(requestingUser + " wants to add forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.addForum(forumName, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }
        
        public string addSubForum(string forumName, string subForumName, List<Tuple<string, string, int>> moderators,
            string requestingUser)
        {
            _sl.append(requestingUser + " wants to add sub forum " + subForumName + " to forum " + forumName);
            try
            {
                if (moderators == null)
                {
                    _sl.appendResult(Constants.INVALID_INPUT);
                    return Constants.INVALID_INPUT;
                }

                List<string> input = new List<string>() { forumName, subForumName };
                foreach (Tuple<string, string, int> t in moderators)
                {
                    input.Add(t.Item1);
                    //input.Add(t.Item2);
                }
                StringBuilder sb = new StringBuilder("\tModerators data:");
                foreach (Tuple<string, string, int> t in moderators)
                {
                    sb.Append(Environment.NewLine);
                    sb.Append("\t\tUsername is " + t.Item1 + ", trial time is " + t.Item3);
                }
                _sl.appendResult(sb.ToString());
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.addSubForum(forumName, subForumName, moderators, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }

        public string registerMemberToForum(string forumName, string username, string password, string eMail, string securityQuestion, string answer)
        {
            _sl.append("Try rgistration of new user " + username + " with eMail " + eMail + " to forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, username, password, eMail, securityQuestion, answer };
                if (Constants.isValidInput(input) && Constants.isValidEmail(eMail))
                {
                    lock (o)
                    {
                        string ans = _fm.registerMemberToForum(forumName, username, password, eMail, securityQuestion, answer);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }
        
        public string assignAdmin(string forumName, string username, string requestingUser)
        {
            _sl.append(requestingUser + " wants to assign " + username + " as admin of forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, username, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.assignAdmin(forumName, username, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }
        
        public string unassignAdmin(string forumName, string username, string requestingUser)
        {
            _sl.append(requestingUser + " wants to unassign " + username + " from being an admin of forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, username, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans =  _fm.unassignAdmin(forumName, username, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }
        
        public string assignModerator(string forumName, string subForumName, string username, string requestingUser, int days)
        {
            _sl.append(requestingUser + " wants to assign " + username + " as moderator of sub forum " + subForumName + " in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, subForumName, username, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.assignModerator(forumName, subForumName, username, requestingUser, days);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }
        
        public string unassignModerator(string forumName, string subForumName, string username, string requestingUser)
        {
            _sl.append(requestingUser + " wants to unassign " + username + " from being a moderator of sub forum " + subForumName + " in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, subForumName, username, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.unassignModerator(forumName, subForumName, username, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }
        
        public permission getUserPermissionsForForum(string forumName, string username)
        {
            _sl.append("Get " + username + " permission in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, username };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        permission p = _fm.getUserPermissionsForForum(forumName, username);
                        string ans = "Invalid";
                        switch (p)
                        {
                            case permission.GUEST:
                                ans = "Guset";
                                break;
                            case permission.MEMBER:
                                ans = "Member";
                                break;
                            case permission.MODERATOR:
                                ans = "Moderator";
                                break;
                            case permission.ADMIN:
                                ans = "Admin";
                                break;
                            case permission.SUPER_ADMIN:
                                ans = "Super Admin";
                                break;
                        }
                        _sl.append(username + " permission in forum " + forumName + " is " + ans);
                        return p;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return permission.INVALID;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                throw e;
            }
        }

        public permission getUserPermissionsForSubForum(string forumName, string subForumName, string username)
        {
            _sl.append("Get " + username + " permission in sub forum " + subForumName + " in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, subForumName, username };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        permission p = _fm.getUserPermissionsForSubForum(forumName, subForumName, username);
                        string ans = "Invalid";
                        switch (p)
                        {
                            case permission.GUEST:
                                ans = "Guset";
                                break;
                            case permission.MEMBER:
                                ans = "Member";
                                break;
                            case permission.MODERATOR:
                                ans = "Moderator";
                                break;
                            case permission.ADMIN:
                                ans = "Admin";
                                break;
                            case permission.SUPER_ADMIN:
                                ans = "Super Admin";
                                break;
                        }
                        _sl.append(username + " permission in forum " + forumName + " in sub forum " + subForumName + " is " + ans);
                        return p;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return permission.INVALID;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                throw e;
            }
        }

        public string sendPM(string forumName, string from, string to, string msg)
        {
            _sl.append(from + " wants to send a private msg with content " + Environment.NewLine
                + "\t" + msg + Environment.NewLine + "\tto " + to + " in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, from, to, msg };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.sendPM(forumName, from, to, msg);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }
        
        public string checkForumPolicy(string forumName, ForumPolicy policy)
        {
            _sl.append("Checking if the given forum policy doesn't contradict current situation in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName };
                if (Constants.isValidInput(input) && policy != null)
                {
                    lock (o)
                    {
                        string ans = _fm.checkForumPolicy(forumName, policy);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }

        public loginStatus login(string forumName, string username, string password, IObserverable co)
        {
            _sl.append("User " + username + "tries to login to forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, username, password };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        loginStatus ls = _fm.login(forumName, username, password);
                        string ans = "Logged in";
                        switch (ls)
                        {
                            case loginStatus.FALSE:
                                ans = "Wrong username or password";
                                break;
                            case loginStatus.UPDATE_PASSWORD:
                                ans = "Logged in, must update password";
                                break;
                        }
                        _sl.append(username + " login status to forum " + forumName + " is " + ans);
                        if (ls != loginStatus.FALSE)
                        {
                            _allConnectedUsers.Add(new DLObserverable(co, username, forumName));
                        }
                        return ls;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return loginStatus.FALSE;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                throw e;
            }
        }

        public string setForumMaxAdmins(string forumName, int maxAdmins, string requestingUser)
        {
            _sl.append(requestingUser + " wants to set maximal number of admins in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.setForumMaxAdmins(forumName, maxAdmins, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }

        public string setForumMinAdmins(string forumName, int minAdmins, string requestingUser)
        {
            _sl.append(requestingUser + " wants to set minimal number of admins in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, requestingUser };
                if (Constants.isValidInput(input) && minAdmins >= 1)
                {
                    lock (o)
                    {
                        string ans = _fm.setForumMinAdmins(forumName, minAdmins, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }

        public string setForumMaxModerators(string forumName, int maxModerators, string requestingUser)
        {
            _sl.append(requestingUser + " wants to set maximal number of moderators in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.setForumMaxModerators(forumName, maxModerators, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }

        public string setForumMinModerators(string forumName, int minModerators, string requestingUser)
        {
            _sl.append(requestingUser + " wants to set minimal number of moderators in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.setForumMinModerators(forumName, minModerators, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }

        public string setForumPostDeletionPermissions(string forumName, postDeletionPermission permission,
            string requestingUser)
        {
            _sl.append(requestingUser + " wants to set post deletion permissions in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.setForumPostDeletionPermissions(forumName, permission, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }

        public string setForumPasswordLifespan(string forumName, int lifespan, string requestingUser)
        {
            _sl.append(requestingUser + " wants to set password lifespan in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, requestingUser };
                if (Constants.isValidInput(input) && lifespan >= 0)
                {
                    lock (o)
                    {
                        string ans = _fm.setForumPasswordLifespan(forumName, lifespan, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }

        public string setForumModeratorsSeniority(string forumName, int seniority, string requestingUser)
        {
            _sl.append(requestingUser + " wants to set minimal moderators seniority in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, requestingUser };
                if (Constants.isValidInput(input) && seniority >= 0)
                {
                    lock (o)
                    {
                        string ans = _fm.setForumModeratorsSeniority(forumName, seniority, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }

        public string setForumModUnassignmentPermissions(string forumName, modUnassignmentPermission permission,
            string requestingUser)
        {
            _sl.append(requestingUser + " wants to set moderators unassignment permissions in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.setForumModUnassignmentPermissions(forumName, permission, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }

        public string writePost(string forumName, string subForumName, int parentPostNo, string username, string title,
            string content)
        {
            _sl.append(username + " wants to write a post in sub forum " + subForumName + " in forum " + forumName);
            _sl.append("\ttitle is " + title);
            _sl.append("\tcontent is " + content);
            try
            {
                List<string> input = new List<string>() { forumName, subForumName, username, title, content };
                if (Constants.isValidInput(input) && Constants.isValidPost(title, content))
                {
                    lock (o)
                    {
                        string ans = _fm.writePost(forumName, subForumName, parentPostNo, username, title, content);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }
        
        public string setModeratorTrialTime(string forumName, string subForumName, string moderator, int newTime,
            string requestingUser)
        {

            _sl.append(requestingUser + " wants to set the moderating trial time of " + moderator + " in sub forum " +
                subForumName + " in forum " + forumName + " to " + newTime + " days");
            try
            {
                List<string> input = new List<string>() { forumName, subForumName, moderator, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.setModeratorTrialTime(forumName, subForumName, moderator, newTime, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }
        
        public string deletePost(string forumName, string subForumName, int postNo, string requestingUser)
        {
            _sl.append(requestingUser + " wants to delete post number " + postNo + " in sub forum " + subForumName + " in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, subForumName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.deletePost(forumName, subForumName, postNo, requestingUser);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }

        public string editPost(string forumName, string subForumName, int postNo, string requestingUser, string content)
        {
            _sl.append(requestingUser + " wants to set the content of post number " + postNo + " in sub forum " + 
                subForumName + " in forum " + forumName + " to " + content);
            try
            {
                List<string> input = new List<string>() { forumName, subForumName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.editPost(forumName, subForumName, postNo, requestingUser, content);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return Constants.INVALID_INPUT;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }

        public int getNumOfPostsInSubForum(string forumName, string subForumName, string requestingUser)
        {
            _sl.append(requestingUser + " wants to know how many posts are in sub forum " + subForumName + " in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, subForumName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        int n = _fm.getNumOfPostsInSubForum(forumName, subForumName, requestingUser);
                        _sl.appendResult("There are " + n + " messages in sub forum " + subForumName + " in forum " + forumName);
                        return n;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return -1;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                throw e;
            }
        }

        public List<Tuple<string, string, DateTime, int>> getListOfMemberMessages(string forumName, string memberName,
            string requestingUser)
        {
            _sl.append(requestingUser + " asks for a list of all of " + memberName + " posts in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, memberName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        return _fm.getListOfMemberMessages(forumName, memberName, requestingUser);
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return null;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                throw e;
            }
        }

        public List<Tuple<string, string, DateTime, string>> getListOfForumModerators(string forumName, string requestingUser)
        {
            _sl.append(requestingUser + " asks for a list of all moderators data in forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        return _fm.getListOfForumModerators(forumName, requestingUser);
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return null;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                throw e;
            }
        }

        public int getNumOfForums(string requestingUser)
        {
            _sl.append(requestingUser + " asks for the total number of forums in the system");
            try
            {
                List<string> input = new List<string>() { requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        int n = _fm.getNumOfForums(requestingUser);
                        _sl.appendResult("There are " + n + " forums in the system");
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return -1;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                throw e;
            }
        }
        
        public List<string> getForums()
        {
            _sl.append("List of all forums have been requested");
            try
            {
                lock (o)
                {
                    return _fm.getForums();
                }
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                throw e;
            }
        }

        public List<string> getSubForums(string forumName, string requestingUser)
        {
            _sl.append(requestingUser + "requested a list of all sub forums of forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        return _fm.getSubForums(forumName, requestingUser);
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return null;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                throw e;
            }
        }

        public List<Tuple<string, DateTime, int>> getThreads(string forumName, string subForumName, string requestingUser)
        {
            _sl.append(requestingUser + "requested a list of all threads of sub forums " + subForumName + " of forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, subForumName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        return _fm.getThreads(forumName, subForumName, requestingUser);
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return null;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                throw e;
            }
        }
        
        public List<Tuple<string, string, DateTime, int, int, string, DateTime>>
            getThread(string forumName, string subForumName, int openPostNo, string requestingUser)
        {
            _sl.append(requestingUser + "requested a the thread starting with post number " + openPostNo 
                + " of sub forums " + subForumName + " of forum " + forumName);
            try
            {
                List<string> input = new List<string>() { forumName, subForumName, requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        return _fm.getThread(forumName, subForumName, openPostNo, requestingUser);
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return null;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                throw e;
            }
        }

        public List<Tuple<string, List<Tuple<string, string>>>> getRepeatUsersByMail(string requestingUser)
        {
            _sl.append(requestingUser + "requested a list of all users registered to multiple forums with the same eMail");
            try
            {
                List<string> input = new List<string>() { requestingUser };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        return _fm.getRepeatUsersByMail(requestingUser);
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return null;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                throw e;
            }
        }


        public Tuple<string, string> getPasswordRestorationQuestion(string forumName, string username)
        {
            _sl.append(username + "asked to restore password");
            try
            {
                List<string> input = new List<string>() { forumName, username };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        Tuple<string, string> t = _fm.getPasswordRestorationQuestion(forumName, username);
                        _sl.appendResult(t.Item1);
                        return t;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return null;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return new Tuple<string,string>(e.Message, null);
            }
        }

        public string answerSecurityQuestion(string forumName, string username, string answer)
        {
            _sl.append(username + "asked to restore password");
            try
            {
                List<string> input = new List<string>() { forumName, username, answer };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.answerSecurityQuestion(forumName, username, answer);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return null;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }

        public string setPassword(string forumName, string username, string password)
        {
            _sl.append(username + "asked to set a new password");
            try
            {
                List<string> input = new List<string>() { forumName, username, password };
                if (Constants.isValidInput(input))
                {
                    lock (o)
                    {
                        string ans = _fm.setPassword(forumName, username, password);
                        _sl.appendResult(ans);
                        return ans;
                    }
                }
                _sl.appendResult(Constants.INVALID_INPUT);
                return null;
            }
            catch (Exception e)
            {
                _sl.appendResult(e.Message);
                return e.Message;
            }
        }
    }
}
