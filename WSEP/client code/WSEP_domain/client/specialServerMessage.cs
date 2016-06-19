using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace client
{
    class specialServerMessage
    {
        public enum messageType { getListOfForummoderators, editPost, getNumOfPostsInSubForum, getListOfMemberMessages, writePost, setModeratorTrialTime, deletePost, setForumModeratorsSeniority, setForumModUnassignmentPermissions, setForumMaxModerators, setForumMinModerators, setForumPostDeletionPermissions, setForumPasswordLifespan, sendPM, checkForumPolicy, setForumMaxAdmins, setForumMinAdmins, unassignAdmin, assignModerator, unassignModerator, getUserPermissionsForForum, getUserPermissionsForSubForum, success, notSuccess, errorHappened, addForum, addSubForum, registerMemberToForum, assignAdmin }

        public messageType _messageType { get; set; }
        List<Tuple<string, string, DateTime, int>> data;


        public specialServerMessage(List<Tuple<string, string, DateTime, int>> data, messageType p1)
        {
            _messageType = p1;
            this.data = data;
        }

        public specialServerMessage()
        {
            _messageType = messageType.unassignAdmin;
            this.data = null;
        }

        public void writeData()
        {
         if(data.Count == 0)
            {
                Console.WriteLine("special message empty list");
            }
        }
    }
}
