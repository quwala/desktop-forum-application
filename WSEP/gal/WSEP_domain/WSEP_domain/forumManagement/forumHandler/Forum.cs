using System;
using System.Collections.Generic;
using WSEP_domain.forumSystem;
using WSEP_domain.userManagement;

namespace WSEP_domain.forumManagement.forumHandler
{
    public class Forum
    {

        private string _name;
        private ForumPolicy _policy;
        private List<SubForum> _subForums;
        private List<string> _interactiveNotifications;

        public static Forum create(string name)
        {
            List<string> input = new List<string>() { name };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            return new Forum(name);
        }

        public static Forum create(string name, ForumPolicy policy, List<SubForum> subForums)
        {
            List<string> input = new List<string>() { name };
            foreach (SubForum sf in subForums)
            {
                input.Add(sf.getName());
            }
            if (!Constants.isValidInput(input) || policy == null)
            {
                return null;
            }
            return new Forum(name, policy, subForums);
        }

        private Forum(string name)
        {
            _name = name;
            _policy = new ForumPolicy();
            _subForums = new List<SubForum>();
            _interactiveNotifications = new List<string>();
        }

        private Forum(string name, ForumPolicy policy, List<SubForum> subForums)
        {
            _name = name;
            _policy = policy;
            _subForums = subForums;
            _interactiveNotifications = new List<string>();
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
            if (!ForumSystem._testFlag)
            {
                if (!ForumSystem._db.addSubForum(_name, subForumName))
                {
                    return Constants.DB_ERROR;
                }
            }
            _subForums.Add(subForum);
            if (!_subForums.Contains(subForum))
            {
                return Constants.FUNCTION_ERRROR; // cannot cover this case
            }
            ForumSystem.notify("A new sub forum has been added to the forum you are currently in.", _interactiveNotifications, _name);
            return Constants.SUCCESS;
        }

        public string getName()
        {
            return _name;
        }

        public int getMaxAdmins()
        {
            return _policy.getMaxAdmins();
        }

        public int getMinAdmins()
        {
            return _policy.getMinAdmins();
        }

        public int getMaxModerators()
        {
            return _policy.getMaxModerators();
        }

        public int getMinModerators()
        {
            return _policy.getMinModerators();
        }

        public int getSeniorityLimit()
        {
            return _policy.getModeratorsSeniority();
        }

        public postDeletionPermission getPostDeletionPermissions()
        {
            return _policy.getPostDeletionPermissions();
        }

        public int getPasswordLifespan()
        {
            return _policy.getPasswordLifespan();
        }

        public int getModeratorsSeniority()
        {
            return _policy.getModeratorsSeniority();
        }

        public modUnassignmentPermission getModUnassignmentPermissions()
        {
            return _policy.getModUnassignmentPermissions();
        }

        public void setMaxAdmins(int maxAdmins)
        {
            if (!ForumSystem._testFlag)
            {
                ForumPolicy fp = new ForumPolicy();
                fp.setMaxAdmins(maxAdmins);
                if (!setForumPolicy(fp))
                {
                    return;
                }
            }
            _policy.setMaxAdmins(maxAdmins);
        }

        public void setMinAdmins(int minAdmins)
        {
            if (!ForumSystem._testFlag)
            {
                ForumPolicy fp = new ForumPolicy();
                fp.setMinAdmins(minAdmins);
                if (!setForumPolicy(fp))
                {
                    return;
                }
            }
            _policy.setMinAdmins(minAdmins);
        }

        public void setMaxModerators(int maxModerators)
        {
            if (!ForumSystem._testFlag)
            {
                ForumPolicy fp = new ForumPolicy();
                fp.setMaxModerators(maxModerators);
                if (!setForumPolicy(fp))
                {
                    return;
                }
            }
            _policy.setMaxModerators(maxModerators);
        }

        public void setMinModerators(int minModerators)
        {
            if (!ForumSystem._testFlag)
            {
                ForumPolicy fp = new ForumPolicy();
                fp.setMinModerators(minModerators);
                if (!setForumPolicy(fp))
                {
                    return;
                }
            }
            _policy.setMinModerators(minModerators);
        }

        public void setPostDeletionPermissions(postDeletionPermission permission)
        {
            if (!ForumSystem._testFlag)
            {
                ForumPolicy fp = new ForumPolicy();
                fp.setPostDeletionPermissions(permission);
                if (!setForumPolicy(fp))
                {
                    return;
                }
            }
            _policy.setPostDeletionPermissions(permission);
        }

        public void setPasswordLifespan(int lifespan)
        {
            if (!ForumSystem._testFlag)
            {
                ForumPolicy fp = new ForumPolicy();
                fp.setPasswordLifespan(lifespan);
                if (!setForumPolicy(fp))
                {
                    return;
                }
            }
            _policy.setPasswordLifespan(lifespan);
        }

        public void setModeratorsSeniority(int seniority)
        {
            if (!ForumSystem._testFlag)
            {
                ForumPolicy fp = new ForumPolicy();
                fp.setModeratorsSeniority(seniority);
                if (!setForumPolicy(fp))
                {
                    return;
                }
            }
            _policy.setModeratorsSeniority(seniority);
        }

        public void setModUnassignmentPermissions(modUnassignmentPermission permission)
        {
            if (!ForumSystem._testFlag)
            {
                ForumPolicy fp = new ForumPolicy();
                fp.setModUnassignmentPermission(permission);
                if (!setForumPolicy(fp))
                {
                    return;
                }
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

        public void getSubForumsList(List<string> list)
        {
            foreach(SubForum sf in _subForums)
            {
                list.Add(sf.getName());
            }
        }

        public void addThreadsToList(List<Tuple<string, DateTime, int>> list, string subForumName, string requestingUser)
        {
            SubForum sf = getSubForum(subForumName);
            if (sf != null)
            {
                sf.addThreadsToList(list);
                sf.observe(requestingUser);
            }
        }

        public void addPostsToList(List<Tuple<string, string, DateTime, int, int, string, DateTime>> list, string subForumName, int openPostNo, string requestingUser)
        {
            SubForum sf = getSubForum(subForumName);
            if (sf != null)
            {
                sf.addPostsToList(list, openPostNo, requestingUser);
            }
        }

        public void observe(string username)
        {
            _interactiveNotifications.Add(username);
            foreach (SubForum sf in _subForums)
            {
                sf.unobserve(username);
            }
        }

        public void unobserve(string username)
        {
            _interactiveNotifications.Remove(username);
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

        private bool setForumPolicy(ForumPolicy p)
        {
            int i_pdp = 2;
            postDeletionPermission e_pdp = p.getPostDeletionPermissions();
            switch (e_pdp)
            {
                case postDeletionPermission.MODERATOR:
                    i_pdp = 3;
                    break;
                case postDeletionPermission.ADMIN:
                    i_pdp = 4;
                    break;
                case postDeletionPermission.SUPER_ADMIN:
                    i_pdp = 5;
                    break;
            }
            int i_mup = 2;
            modUnassignmentPermission e_mup = p.getModUnassignmentPermissions();
            switch (e_mup)
            {
                case modUnassignmentPermission.ASSIGNING_ADMIN:
                    i_mup = 3;
                    break;
                case modUnassignmentPermission.SUPER_ADMIN:
                    i_mup = 4;
                    break;
            }
            if (!ForumSystem._db.changeForumPolicy(p.getMaxAdmins(), p.getMinAdmins(), p.getMaxModerators(), p.getMinModerators(), _name,
                i_pdp, p.getPasswordLifespan(), p.getModeratorsSeniority(), i_mup))
            {
                return false;
            }
            return true;
        }
    }
}
