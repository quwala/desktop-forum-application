using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class specialServerMessage
    {
        public enum messageType { unsuccess, getListOfForummoderators, editPost, getNumOfPostsInSubForum, getListOfMemberMessages, writePost, setModeratorTrialTime, deletePost, setForumModeratorsSeniority, setForumModUnassignmentPermissions, setForumMaxModerators, setForumMinModerators, setForumPostDeletionPermissions, setForumPasswordLifespan, sendPM, checkForumPolicy, setForumMaxAdmins, setForumMinAdmins, unassignAdmin, assignModerator, unassignModerator, getUserPermissionsForForum, getUserPermissionsForSubForum, success, notSuccess, errorHappened, addForum, addSubForum, registerMemberToForum, assignAdmin }

        public messageType _messageType { get; set; }
        List<Tuple<string, string, DateTime, int>> data;


        public specialServerMessage(messageType p1, List<Tuple<string, string, DateTime, int>> data)
        {
            _messageType = p1;
            this.data = data;
        }
    }
}
