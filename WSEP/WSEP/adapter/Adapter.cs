using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP.forumManagement;
using WSEP.forumManagement.threadsHandler;

namespace WSEP.adapter
{
    class Adapter: IAdapter
    {

        IForumSystem fs = new ForumSystem("forum system", new WSEP.userManagement.UserManager());

        public bool addForum(string forumName)
        {
            return fs.addForum(forumName); 
        }

        public bool addForumAdmin(string forumName, string adminName)
        {
            throw new NotImplementedException();
        }

        public bool addSubForum(string forumName, string subForumName, List<string> moderators)
        {
            throw new NotImplementedException();
        }

        public bool addSubForumModerator(string forumName, string subForumName, string moderatorName)
        {
            throw new NotImplementedException();
        }

        public bool containForum(string forumName)
        {
            try{
                fs.getForum(forumName);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        public bool containSubForum(string forumName, string subForum)
        {
            throw new NotImplementedException();
        }

        public bool createReply(string forumName, string subForumName, string postToReplyToId, string title, string content)
        {
            throw new NotImplementedException();
        }

        public bool createThread(string forumName, string subForumName, string threadTitle, string content)
        {
            throw new NotImplementedException();
        }

        public bool deletePost(string forumName, string subForumName, string postId)
        {
            throw new NotImplementedException();
        }

        public bool editModeratorTime(string ForumName, string SubForumName, string UserName, int time)
        {
            throw new NotImplementedException();
        }

        public bool forumLogIn(string forumName, string userName, string userPassword)
        {
            throw new NotImplementedException();
        }

        public List<string> getThreadsFromSubForum(string forumName, string subForumName)
        {
            throw new NotImplementedException();
        }

        public bool moderatorAppointment(string forumName, string subForumName, string moderatorName, int howMuchTime)
        {
            throw new NotImplementedException();
        }

        public bool registerToForum(string forumName, string userName, string userPassword, string userMail)
        {
            throw new NotImplementedException();
        }

        public bool sendingPrivateMassage(string forumName, string sender, string receiver, string content)
        {
            throw new NotImplementedException();
        }

        public bool setPolicy(string forumName, int minNumAdmins, int maxNumAdmins, int minNummoderator, int maxNumModerator, string freeText)
        {
            throw new NotImplementedException();
        }
    }
}
