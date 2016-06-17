using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP_domain.forumManagement.forumHandler;

namespace server
{
    class serverMessage
    {

        public enum messageType
        {
            unsuccess, getThreads, getThread, getSubForums, numOfForums, getForums,
            getListOfForummoderators, editPost, getNumOfPostsInSubForum, getListOfMemberMessages,
            writePost, setModeratorTrialTime, deletePost, setForumModeratorsSeniority,
            setForumModUnassignmentPermissions, setForumMaxModerators, setForumMinModerators,
            setForumPostDeletionPermissions, setForumPasswordLifespan, sendPM, checkForumPolicy,
            setForumMaxAdmins, setForumMinAdmins, unassignAdmin, assignModerator, unassignModerator,
            getUserPermissionsForForum, getUserPermissionsForSubForum, success, notSuccess,
            errorHappened, addForum, addSubForum, registerMemberToForum, assignAdmin, login,
            updateLocation,
            numOfForumsByUser,
            ForumsByUser,
        }



        public messageType _messageType { get; set; }
        public List<string> stringContent { get; set; }

        public List<int> intContent { get; set; }

        public DateTime time { get; set; }

        public ForumPolicy policy { get; set; }

        public List<Tuple<string, string, DateTime, int>> ListOfMemberMessages { get; set; }

        public List<Tuple<string, string, DateTime, string>> ListOfForumModerators { get; set; }

        public List<Tuple<string, DateTime, int>> Threads { get; set; }

        List<Tuple<string, string, DateTime, int, int, string, DateTime>> getThread { get; set; }

        public serverMessage(messageType p1, List<string> p2, List<int> p3, DateTime p4)
        {
            // TODO: Complete member initialization
            _messageType = p1;
            stringContent = p2;
            intContent = p3;
            time = p4;
            policy = null;
            ListOfMemberMessages = null;
            ListOfForumModerators = null;
            Threads = null;
            getThread = null;

        }

        public serverMessage(messageType p1, ForumPolicy p2, string forumName)
        {
            // TODO: Complete member initialization
            _messageType = p1;
            policy = p2;
            stringContent = new List<string>();
            stringContent.Add(forumName);
            intContent = null;
            time = DateTime.Now; 
            policy = null;
            ListOfMemberMessages = null;
            ListOfForumModerators = null;
            Threads = null;
            getThread = null;
        }


        public serverMessage(int num, List<string> list)
        {
            switch(num){
                case 1:
                    _messageType = messageType.getListOfMemberMessages;
                    break;
                case 2:
                    _messageType = messageType.getListOfForummoderators;
                    break;
                case 3:
                    _messageType = messageType.getThreads;
                    break;
            }
            policy = null;
            getThread = null;
            stringContent = list;
            intContent = null;
            time = DateTime.Now;
            policy = null;
            ListOfMemberMessages = new List<Tuple<string,string,DateTime,int>>();
            ListOfForumModerators = new List<Tuple<string, string, DateTime, string>>();
            Threads = new List<Tuple<string, DateTime, int>>();
        }
        public serverMessage()
        {
            _messageType = messageType.errorHappened;
            stringContent = new List<string>();
            intContent = new List<int>();
            time = DateTime.Now;
        }

        public serverMessage(string p)
        {
            if (p.Equals("error"))
            {
                _messageType = messageType.errorHappened;
                stringContent = new List<string>();
                intContent = new List<int>();
                time = DateTime.Now;
            }
            else
            {
                _messageType = messageType.errorHappened;
                stringContent = new List<string>();
                intContent = new List<int>();
                time = DateTime.Now;
            }
        }

        public serverMessage(messageType messageType, List<Tuple<string, string, DateTime, int>> ListOfMemberMessages)
        {
            _messageType = messageType;
            policy = null;
            stringContent = null;
            intContent = null;
            time = DateTime.Now;
            policy = null;
            Threads = null;
            getThread = null;
            ListOfForumModerators = null;
            this._messageType = messageType;
            this.ListOfMemberMessages = ListOfMemberMessages;
        }

        public serverMessage(messageType messageType, List<Tuple<string, string, DateTime, string>> ListOfForumModerators)
        {
            policy = null;
            stringContent = null;
            intContent = null;
            time = DateTime.Now;
            policy = null;
            Threads = null;
            getThread = null;
            ListOfMemberMessages = null;
            this._messageType = messageType;
            this.ListOfForumModerators = ListOfForumModerators;
        }

        public serverMessage(messageType messageType, List<Tuple<string, DateTime, int>> returnForMemMesages3)
        {
            policy = null;
            stringContent = null;
            intContent = null;
            time = DateTime.Now;
            policy = null;
            getThread = null;
            ListOfForumModerators = null;
            ListOfMemberMessages = null;
            this._messageType = messageType;
            this.Threads = returnForMemMesages3;
        }

        public serverMessage(List<string> list, int num)
        {
            policy = null;
            getThread = null;
            stringContent = list;
            intContent = new List<int>();
            intContent.Add(num);
            time = DateTime.Now;
            policy = null;
            ListOfMemberMessages = null;
            ListOfForumModerators = null;
            Threads = null;
            getThread = new List<Tuple<string, string, DateTime, int, int, string, DateTime>>();
        }

        public serverMessage(messageType messageType, List<Tuple<string, string, DateTime, int, int, string, DateTime>> thread)
        {
            policy = null;
            getThread = null;
            stringContent = null;
            intContent = null;
            time = DateTime.Now;
            policy = null;
            ListOfMemberMessages = null;
            ListOfForumModerators = null;
            Threads = null;
            this._messageType = messageType;
            this.getThread = thread;
        }

        public void writeData()
        {
            Console.Write("type:" + _messageType + "\ncontent:");
            for (int i = 0; i < stringContent.Count; i++)
            {
                Console.WriteLine(stringContent.ElementAt(i));
            }
            if (stringContent.Count == 0)
            {
                Console.Write("\n");
            }
            Console.Write("int content:");
            for (int i = 0; i < intContent.Count; i++)
            {
                Console.WriteLine(intContent.ElementAt(i));
            }
            if (intContent.Count == 0)
            {
                Console.Write("\n");
            }
            Console.WriteLine("time: " + time.ToString());
        }
    }
}
