using System;
using System.Collections.Generic;
using WSEP.forumManagement.forumHandler;
using WSEP.forumManagement.threadsHandler;
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

            string message = um.addForum(name);
            if (message.Equals("true"))
                return true;
            else
                throw new Exception(message); 

        }

        public bool addSubForum(string forumName, string subForumName, List<string> mods)
        {
            Forum forum = getForum(forumName);
               if(forum==null)
                     throw new Exception("Cannot add Sub Forum - Forum was not found");

            foreach (SubForum sf in forum.SubForums)
                if (sf.getName().Equals(subForumName))
                    throw new Exception("A Sub Forum with that name already exists");

            int minModerators = forum.getPolicy().MinModerators;
            int maxModerators = forum.getPolicy().MaxModerators;

            string message = um.addSubForum(forumName, subForumName, mods, minModerators, maxModerators);

            if (message.Equals("true"))
                return true;
            else
                throw new Exception(message); 
        }

        //need a user manager to perform
        public bool setForumPolicy(string forumName, int minAdmins, int maxAdmins, 
            int minModerators, int maxModerators, string forumRules)
        {
            Forum forum = getForum(forumName);
            if (forum == null)
                throw new Exception("Cannot set policy - Forum was not found");

            ForumPolicy nPolicy = new ForumPolicy(forumName, minAdmins, maxAdmins,
                minModerators, maxModerators, forumRules);

            string message = um.checkForumPolicy(forumName, minAdmins, maxAdmins, minModerators, maxModerators);

            if (!message.Equals("true"))
                throw new Exception(message);

            else
                return forum.setPolicy(nPolicy);
        }




        public bool createThread(string forumName, string subForumName, string title, string content
            ,string userName)
        {
            if (!hasForum(forumName))
                throw new Exception("Cannot create Thread - Forum was not found");

            Forum forum = getForum(forumName);
            SubForum subForum=null;
            foreach (SubForum sf in forum.SubForums)
                if (sf.getName().Equals(subForumName))
                    subForum = sf;

            if(subForum == null)
                throw new Exception("Cannot create Thread - Sub Forum was not found");

            Post thread = new Post(title,content,userName);

            return subForum.createThread(thread);

        }

        public bool createReply(string forumName, string subForumName, string title, string content,
            string userName, string postToReplyToID)
        {
            if (!hasForum(forumName))
                throw new Exception("Cannot create Reply - Forum was not found");

            Forum forum = getForum(forumName);
            SubForum subForum = null;
            foreach (SubForum sf in forum.SubForums)
                if (sf.getName().Equals(subForumName))
                    subForum = sf;

            if (subForum == null)
                throw new Exception("Cannot create Reply - Sub Forum was not found");

            Post replyTo = subForum.getPostById(postToReplyToID);
            if(replyTo== null)
                throw new Exception("Cannot create Reply - original Post was not found");

            Post reply = new Post(title, content, userName,replyTo );

            return replyTo.addReply(reply);

        }

        public List<string> getThreadIDSFromSubForum(string forumName, string subForumName)
        {
            if (!hasForum(forumName))
                throw new Exception("Cannot retrieve threads - Forum was not found");

            Forum forum = getForum(forumName);
            SubForum subForum = null;
            foreach (SubForum sf in forum.SubForums)
                if (sf.getName().Equals(subForumName))
                    subForum = sf;

            if (subForum == null)
                throw new Exception("Cannot retrieve threads - Sub Forum was not found");

            return subForum.getThreadIDS();
        }

        public bool deletePost(string forumName, string subForumName, string postId)
        {
            if (!hasForum(forumName))
                throw new Exception("Cannot delete post - Forum was not found");

            Forum forum = getForum(forumName);
            SubForum subForum = null;
            foreach (SubForum sf in forum.SubForums)
                if (sf.getName().Equals(subForumName))
                    subForum = sf;

            if (subForum == null)
                throw new Exception("Cannot delete post - Sub Forum was not found");

            return subForum.deletePost(postId);
        }
    }
    
}
