using System;
using System.Collections.Generic;
using WSEP.forumManagement.forumHandler;
using WSEP.userManagement;
namespace WSEP.forumManagement
{
    public class ForumSystem : IForumSystem
    {

        private string _superAdmin;
        private List<Forum> _forums;
        private IUserManager um;

        //public ForumSystem(string superAdmin)
        //{
        //    _superAdmin = superAdmin;
        //    _forums = new List<Forum>();
        //     um = null;
        //}

        public ForumSystem(string superAdmin, UserManager um)
        {
            _superAdmin = superAdmin;
            _forums = new List<Forum>();
            this.um = um;
        }

        public Forum getForum(string name)
        {
            foreach (Forum f in _forums)
                if (f.getName().Equals(name))
                    return f;

            return null;
        }

        public bool hasForum(string name)
        {
            if (getForum(name) == null)
                return false;
            return true;
        }

        //need a user manager to perform
        public bool addForum(string name)
        {
            // verify there is no forum with that name
            foreach (Forum f in _forums)
                if (f.getName().Equals(name))
                    throw new Exception("A Forum with that name already exists");
            

            Forum nForum = new Forum(name);
            _forums.Add(nForum);
            if (!_forums.Contains(nForum))
                throw new Exception("Failed to add forum");

            if (um.addForum(name).Equals("true"))
                return true;
            else
                return false;

        }

        public bool addSubForum(string forumName, string subForumName)
        {
            Forum forum = getForum(forumName);
               if(forum==null)
                     throw new Exception("Cannot add Sub Forum - Forum was not found");

            
            return forum.addSubForum(subForumName);
        }

        //need a user manager to perform
        public bool changeForumPolicy(string forumName, int minAdmins, int maxAdmins, 
            int minModerators, int maxModerators, string forumRules)
        {
            Forum forum = getForum(forumName);
            if (forum == null)
                throw new Exception("Cannot change policy - Forum was not found");

            ForumPolicy nPolicy = new ForumPolicy(forumName, minAdmins, maxAdmins,
                minModerators, maxModerators, forumRules);

            string message = um.checkForumPolicy(forumName, minAdmins, maxAdmins, minModerators, maxModerators);

            if (!message.Equals("true"))
                throw new Exception(message);

            else
                return forum.setPolicy(nPolicy);
        }

        public bool addSubForum(string forumName, string subForumName, List<string> mods)
        {
            throw new NotImplementedException();
        }

        public bool setForumPolicy(string forumName, int minAdmins, int maxAdmins, int minModerators, int maxModerators, string forumRules)
        {
            throw new NotImplementedException();
        }


        public bool createThread(string forumName, string subForumName, string title, string content)
        {
            throw new NotImplementedException();
        }

        public bool createReply(string forumName, string subForumName, string threadID, string postToReplyToID)
        {
            throw new NotImplementedException();
        }

        public List<string> getThreadIDSFromSubForum(string forumName, string subForumName)
        {
            throw new NotImplementedException();
        }

      
    }
    
}
