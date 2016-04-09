using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP_service.forumManagementService;
using WSEP_doamin.forumManagementDomain.threadsHandler;
using WSEP_service.userManagementService;

namespace WSEP_tests.adapter
{
    class Adapter: IAdapter
    {

        IForumSystem fs = new ForumSystem("forum system", new WSEP_service.userManagementService.UserManager());
        IUserManager um = new UserManager();
        public bool addForum(string forumName)
        {
            um.addForum(forumName);
            return fs.addForum(forumName);
        }

        public bool addForumAdmin(string forumName, string adminName)
        {
            throw new NotImplementedException();
        }

        public bool addSubForum(string forumName, string subForumName, List<string> moderators)
        {
            try
            {
                fs.addSubForum(forumName, subForumName, moderators);
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        public bool addSubForumModerator(string forumName, string subForumName, string moderatorName)
        {
            throw new NotImplementedException();
        }

        public bool containForum(string forumName)
        {
            try
            {
                fs.getForum(forumName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool containSubForum(string forumName, string subForum)
        {
            throw new NotImplementedException();
        }

        public bool createReply(string forumName, string subForumName, string title, string content, string userName, string postIdToReplyTo)
        {
            try
            {
                fs.createReply(forumName, subForumName, title, content, userName, postIdToReplyTo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool createThread(string forumName, string subForumName, string threadTitle, string content, string userName)
        {
            try
            {
                string ans = fs.createThread(forumName, subForumName, threadTitle, content, userName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string createThreadAndGetID(string forumName, string subForumName, string threadTitle, string content, string userName)
        {
            try
            {
                string ans = fs.createThread(forumName, subForumName, threadTitle, content, userName);
                return ans;
            }
            catch (Exception)
            {
                return "false";
            }
        }

        public bool deletePost(string forumName, string subForumName, string postId)
        {
            try
            {
                fs.deletePost(forumName, subForumName, postId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool editModeratorTime(string ForumName, string SubForumName, string UserName, int time)
        {
            throw new NotImplementedException();
        }

        public bool forumLogIn(string forumName, string userName, string userPassword)
        {
            return um.login(forumName, userName, userPassword);
        }

        public List<string> getThreadsFromSubForum(string forumName, string subForumName)
        {
            try
            {
                List<string> ans = fs.getThreadIDSFromSubForum(forumName, subForumName);
                return ans;
            }
            catch
            {
                return new List<string>();
            }
        }

        public bool moderatorAppointment(string forumName, string subForumName, string moderatorName, int howMuchTime)
        {
            throw new NotImplementedException();
        }

        public bool registerToForum(string forumName, string userName, string userPassword, string userMail)
        {
            string ans = um.registerMemberToForum(forumName, userName, userPassword, userMail);
            if (ans.Equals("true"))
            {
                return true;
            }

            return false;
        }

        public bool sendingPrivateMassage(string forumName, string sender, string receiver, string content)
        {
            try
            {
                string ans = um.sendPM(forumName, sender, receiver, content);
                if (ans.Equals("true"))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool setPolicy(string forumName, int minNumAdmins, int maxNumAdmins, int minNummoderator, int maxNumModerator, string freeText)
        {
            try
            {
                fs.setForumPolicy(forumName,"name", minNumAdmins, maxNumAdmins, minNummoderator, maxNumModerator, freeText);
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }
    }
}
