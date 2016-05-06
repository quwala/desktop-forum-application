using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP_domain.forumManagement.threadsHandler;
using WSEP_domain.userManagement;

namespace WSEP_domain.forumManagement.forumHandler
{
    public class Forum
    {
        private const string SUCCESS = "true";
        private const string FUNCTION_ERRROR = "An error has occured with C# internal function.";

        private string _name;
        private ForumPolicy _policy;
        private List<SubForum> _subForums;

        public static Forum create(string name)
        {
            List<string> input = new List<string>() { name };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            return new Forum(name);
        }

        private Forum(string name)
        {
            _name = name;
            _policy = new ForumPolicy();
            _subForums = new List<SubForum>();
        }

        public string addSubForum(string subForumName)
        {
            SubForum subForum = getSubForum(subForumName);
            if (subForum != null)
            {
                return "Sub forum " + subForumName + " already exist.";
            }
            subForum = SubForum.create(_name, subForumName);
            if (subForum == null)
            {
                return "Invalid sub forum name.";
            }
            _subForums.Add(subForum);
            if (!_subForums.Contains(subForum))
            {
                return FUNCTION_ERRROR;
            }
            return SUCCESS;
        }

        public string getName()
        {
            return _name;
        }

        public int getMaxAdmins()
        {
            return (_policy == null) ? -1 : _policy.getMaxAdmins();
        }

        public int getMinAdmins()
        {
            return (_policy == null) ? -1 : _policy.getMinAdmins();
        }

        public int getMaxModerators()
        {
            return (_policy == null) ? -1 : _policy.getMaxModerators();
        }

        public int getMinModerators()
        {
            return (_policy == null) ? -1 : _policy.getMinModerators();
        }

        public int getSeniorityLimit()
        {
            return (_policy == null) ? -1 : _policy.getSeniority();
        }

        public postDeletionPermission getPostDeletionPermissions()
        {
            return (_policy == null) ? postDeletionPermission.INVALID : _policy.getPostDeletionPermissions();
        }

        public int getPasswordLifespan()
        {
            return (_policy == null) ? -1 : _policy.getPasswordLifespan();
        }

        public int getModeratorsSeniority()
        {
            return (_policy == null) ? -1 : _policy.getModeratorsSeniority();
        }

        public modUnassignmentPermission getModUnassignmentPermissions()
        {
            return (_policy == null) ? modUnassignmentPermission.INVALID : _policy.getModUnassignmentPermissions();
        }

        public void setMaxAdmins(int maxAdmins)
        {
            if (_policy == null)
            {
                return;
            }
            _policy.setMaxAdmins(maxAdmins);
        }

        public void setMinAdmins(int minAdmins)
        {
            if (_policy == null)
            {
                return;
            }
            _policy.setMinAdmins(minAdmins);
        }

        public void setMaxModerators(int maxModerators)
        {
            if (_policy == null)
            {
                return;
            }
            _policy.setMaxModerators(maxModerators);
        }

        public void setMinModerators(int minModerators)
        {
            if (_policy == null)
            {
                return;
            }
            _policy.setMinModerators(minModerators);
        }

        public void setPostDeletionPermissions(postDeletionPermission permission)
        {
            if (_policy == null)
            {
                return;
            }
            _policy.setPostDeletionPermissions(permission);
        }

        public void setPasswordLifespan(int lifespan)
        {
            if (_policy == null)
            {
                return;
            }
            _policy.setPasswordLifespan(lifespan);
        }

        public void setModeratorsSeniority(int seniority)
        {
            if (_policy == null)
            {
                return;
            }
            _policy.setModeratorsSeniority(seniority);
        }

        public void setModUnassignmentPermissions(modUnassignmentPermission permission)
        {
            if (_policy == null)
            {
                return;
            }
            _policy.setModUnassignmentPermission(permission);
        }

        public string writePost(string subForumName, int parentPostNo, User user, string title, string content)
        {
            SubForum sf = getSubForum(subForumName);
            if (sf == null)
            {
                return subForumDoesntExist(subForumName);
            }
            return sf.writePost(parentPostNo, user, title, content);
        }

        public string deletePost(string subForumName, int postNo, postDeletionPermission pdp, permission p, string requestingUser)
        {
            SubForum sf = getSubForum(subForumName);
            if (sf == null)
            {
                return subForumDoesntExist(subForumName);
            }
            return sf.deletePost(postNo, pdp, p, requestingUser);
        }

        public string editPost(string subForumName, int postNo, string requestingUser, permission p, string content)
        {
            SubForum sf = getSubForum(subForumName);
            if (sf == null)
            {
                return subForumDoesntExist(subForumName);
            }
            return sf.editPost(postNo, requestingUser, p, content);
        }

        public int getNumOfPostsInSubForum(string subForumName)
        {
            SubForum sf = getSubForum(subForumName);
            if (sf == null)
            {
                return -1;
            }
            return sf.getNumOfPOsts();
        }

        public List<Tuple<string, string, DateTime, int>> getListOfMemberMessages(List<Tuple<string, string, DateTime, int>> list, string memberName)
        {
            foreach (SubForum sf in _subForums)
            {
                sf.addUserMessages(list, memberName);
            }
            return list;
        }

        public void addSubForumsToList(List<string> list)
        {
            foreach(SubForum sf in _subForums)
            {
                list.Add(sf.getName());
            }
        }

        public void addThreadsToList(List<Tuple<string, DateTime, int>> list, string subForumName)
        {
            SubForum sf = getSubForum(subForumName);
            if (sf != null)
            {
                sf.addThreadsToList(list);
            }
        }

        public void addPostsToList(List<Tuple<string, string, DateTime, int, int, string, DateTime>> list, string subForumName, int openPostNo)
        {
            SubForum sf = getSubForum(subForumName);
            if (sf != null)
            {
                sf.addPostsToList(list, openPostNo);
            }
        }

        private SubForum getSubForum(string subForumName)
        {
            foreach (SubForum subForum in _subForums)
            {
                if (subForum.getName().Equals(subForumName))
                {
                    return subForum;
                }
            }
            return null;
        }

        private string subForumDoesntExist(string subForumName)
        {
            return "Sub forum " + subForumName + " doesn't exist.";
        }
    }
}
