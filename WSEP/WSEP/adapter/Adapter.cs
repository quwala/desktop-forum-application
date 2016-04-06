using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP.forumManagement;

namespace WSEP.adapter
{
    class Adapter: IAdapter
    {

        IForumSystem fs = new ForumSystem("forum system");

        public bool addForum(string forumName)
        {
            return fs.addForum(forumName); 
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

        public bool RegisterToForum(string forumName, string userName, string userPassword, string userMail)
        {
            throw new NotImplementedException();
        }

        public bool ForumLogIn(string forumName, string userName, string userPassword)
        {
            throw new NotImplementedException();
        }

        public bool SetPolicy(string forumName, int minNumAdmins, int maxNumAdmins, int minNummoderator, int maxNumModerator, string freeText)
        {
            throw new NotImplementedException();
        }

        public bool addSubForum(string forumName, string subForumName, List<string> moderatorsNames)
        {
            throw new NotImplementedException();
        }

        public bool addForumAdmin(string forumName, string adminName)
        {
            throw new NotImplementedException();
        }

        public bool addSubForumModerator(string forumName, string subForumName, string moderatorName)
        {
            throw new NotImplementedException();
        }

        public bool CreateThread(string forumName, string subForumName, string threadTitle, string content)
        {
            throw new NotImplementedException();
        }

        public List<forumManagement.forumHandler.Post> getThreadsFromSubForum(string forumName, string subForumName)
        {
            throw new NotImplementedException();
        }


        public bool DeletePost(string forumName, string subForumName, List<forumManagement.forumHandler.Post> posts)
        {
            throw new NotImplementedException();
        }

        public bool ModeratorAppointment(string forumName, string subForumName, string moderatorName, int howMuchTime)
        {
            throw new NotImplementedException();
        }

        public bool EditModeratorTime(string ForumName, string SubForumName, string UserName, int time)
        {
            throw new NotImplementedException();
        }

        public bool CreateReply(string forumName, string subForumName, forumManagement.forumHandler.Post post, string content)
        {
            throw new NotImplementedException();
        }

        public bool SendingPrivateMassage(string forumName, string sender, string receiver, string content)
        {
            throw new NotImplementedException();
        }

        public bool ContainSubForum(string forumName, string subForum)
        {
            throw new NotImplementedException();
        }


        public List<forumManagement.forumHandler.Post> getThreadFromSubForum(string forumName, string subForumName)
        {
            throw new NotImplementedException();
        }
    }
}
