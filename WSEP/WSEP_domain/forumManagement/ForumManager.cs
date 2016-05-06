using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP_domain.forumManagement.forumHandler;
using WSEP_domain.forumManagement.threadsHandler;
using WSEP_domain.userManagement;

namespace WSEP_domain.forumManagement
{
    public class ForumManager : IForumManager
    {
        private const string SUCCESS = "true";

        private List<Forum> _forums;

        public ForumManager()
        {
            _forums = new List<Forum>();
        }

        public string addForum(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
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
            _forums.Add(f);
            if (!_forums.Contains(f))
            {
                return Constants.FUNCTION_ERRROR;
            }
            return SUCCESS;
        }

        public string addSubForum(string forumName, string subForumName)
        {
            List<string> input = new List<string>() { forumName, subForumName };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            return f.addSubForum(subForumName);
        }

        public int getForumMaxAdmins(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return -1;
            }
            return f.getMaxAdmins();
        }

        public int getForumMinAdmins(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return -1;
            }
            return f.getMinAdmins();
        }

        public int getForumMaxModerators(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return -1;
            }
            return f.getMaxModerators();
        }

        public int getForumMinModerators(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return -1;
            }
            return f.getMinModerators();
        }

        public int getForumSeniorityLimit(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return -1;
            }
            return f.getSeniorityLimit();
        }

        public modUnassignmentPermission getForumModUnassignmentPermissions(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return modUnassignmentPermission.INVALID;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return modUnassignmentPermission.INVALID;
            }
            return f.getModUnassignmentPermissions();
        }

        public postDeletionPermission getForumPostDeletionPermission(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return postDeletionPermission.INVALID;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return postDeletionPermission.INVALID;
            }
            return f.getPostDeletionPermissions();
        }

        public bool setForumMaxAdmins(string forumName, int maxAdmins)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return false;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return false;
            }
            f.setMaxAdmins(maxAdmins);
            return (maxAdmins == f.getMaxAdmins());
        }

        public bool setForumMinAdmins(string forumName, int minAdmins)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return false;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return false;
            }
            f.setMinAdmins(minAdmins);
            return (minAdmins == f.getMinAdmins());
        }

        public bool setForumMaxModerators(string forumName, int maxModerators)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return false;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return false;
            }
            f.setMaxModerators(maxModerators);
            return (maxModerators == f.getMaxModerators());
        }

        public bool setForumMinModerators(string forumName, int minModerators)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return false;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return false;
            }
            f.setMinModerators(minModerators);
            return (minModerators == f.getMinModerators());
        }

        public bool setForumPostDeletionPermissions(string forumName, postDeletionPermission permission)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return false;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return false;
            }
            f.setPostDeletionPermissions(permission);
            return (f.getPostDeletionPermissions() == permission);
        }

        public bool setForumPasswordLifespan(string forumName, int lifespan)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return false;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return false;
            }
            f.setPasswordLifespan(lifespan);
            return (f.getPasswordLifespan() == lifespan);
        }

        public bool setForumModeratorsSeniority(string forumName, int seniority)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return false;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return false;
            }
            f.setModeratorsSeniority(seniority);
            return (f.getModeratorsSeniority() == seniority);
        }

        public bool setForumModUnassignmentPermissions(string forumName, modUnassignmentPermission permission)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return false;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return false;
            }
            f.setModUnassignmentPermissions(permission);
            return (f.getModUnassignmentPermissions() == permission);
        }

        public string writePost(string forumName, string subForumName, int parentPostNo, User user, string title, string content)
        {
            if (user == null)
            {
                return Constants.INVALID_INPUT;
            }
            List<string> input = new List<string>() { forumName, subForumName, user.getUsername() };
            if (!Constants.isValidInput(input))
            {
                return Constants.INVALID_INPUT;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return Constants.forumDoesntExist(forumName);
            }
            return f.writePost(subForumName, parentPostNo, user, title, content);
        }

        public string deletePost(string forumName, string subForumName, int postNo, postDeletionPermission pdp, permission p, string requestingUser)
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
            return f.deletePost(subForumName, postNo, pdp, p, requestingUser);
        }

        public string editPost(string forumName, string subForumName, int postNo, string requestingUser, permission p, string content)
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
            return f.editPost(subForumName, postNo, requestingUser, p, content);
        }

        public int getNumOfPostsInSubForum(string forumName, string subForumName)
        {
            List<string> input = new List<string>() { forumName, subForumName };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            Forum f = getForum(forumName);
            if (f == null)
            {
                return -1;
            }
            return f.getNumOfPostsInSubForum(subForumName);
        }

        public int getNumOfForums()
        {
            return _forums.Count;
        }

        public List<Tuple<string, string, DateTime, int>> getListOfMemberMessages(string forumName, string memberName)
        {
            List<string> input = new List<string>() { forumName, memberName };
            if (!Constants.isValidInput(input))
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

        public List<string> getForums()
        {
            List<string> ans = new List<string>();
            foreach (Forum f in _forums)
            {
                ans.Add(f.getName());
            }
            return ans;
        }

        public List<string> getSubForums(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            List<string> list = new List<string>();
            Forum f = getForum(forumName);
            if (f != null)
            {
                f.addSubForumsToList(list);
            }
            return list;
        }

        public List<Tuple<string, DateTime, int>> getThreads(string forumName, string subForumName)
        {
            List<string> input = new List<string>() { forumName, subForumName };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            List<Tuple<string, DateTime, int>> list = new List<Tuple<string, DateTime, int>>();
            Forum f = getForum(forumName);
            if (f != null)
            {
                f.addThreadsToList(list, subForumName);
            }
            return list;
        }

        public List<Tuple<string, string, DateTime, int, int, string, DateTime>> getThread(string forumName, string subForumName, int openPostNo)
        {
            List<string> input = new List<string>() { forumName, subForumName };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            List<Tuple<string, string, DateTime, int, int, string, DateTime>> list = new List<Tuple<string, string, DateTime, int, int, string, DateTime>>();
            Forum f = getForum(forumName);
            if (f != null)
            {
                f.addPostsToList(list, subForumName, openPostNo);
            }
            return list;
        }

        public int getForumPasswordLifespan(string forumName)
        {
            List<string> input = new List<string>() { forumName };
            if (!Constants.isValidInput(input))
            {
                return -1;
            }
            Forum f = getForum(forumName);
            if (f != null)
            {
                return f.getPasswordLifespan();
            }
            return -1;
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
