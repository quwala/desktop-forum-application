using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class serverMessage
    {
        private string p;


        public enum messageType {login, getThreads, getThread, getSubForums, numOfForums, getForums, getListOfForummoderators, editPost, getNumOfPostsInSubForum, getListOfMemberMessages, writePost, setModeratorTrialTime, deletePost, setForumModeratorsSeniority, setForumModUnassignmentPermissions, setForumMaxModerators, setForumMinModerators, setForumPostDeletionPermissions, setForumPasswordLifespan, sendPM, checkForumPolicy, setForumMaxAdmins, setForumMinAdmins, unassignAdmin, assignModerator, unassignModerator, getUserPermissionsForForum, getUserPermissionsForSubForum, success, notSuccess, errorHappened, addForum, addSubForum, registerMemberToForum, assignAdmin }
        public messageType _messageType { get; set; }
        public List<string> stringContent { get; set; }

        public List<int> intContent { get; set; }

        public DateTime time { get; set; }

        public serverMessage(messageType p1, List<string> p2, List<int> p3, DateTime p4)
        {

            _messageType = p1;
            stringContent = p2;
            intContent = p3;
            time = p4;

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

        public void writeData()
        {
            Console.Write("type:" + _messageType + "\ncontent:");
            for (int i = 0; i < stringContent.Count; i++)
            {
                Console.WriteLine(stringContent.ElementAt(i));
            }
            if(stringContent.Count == 0)
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
