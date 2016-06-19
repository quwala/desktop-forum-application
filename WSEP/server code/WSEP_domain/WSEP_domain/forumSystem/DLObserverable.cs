using Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP_domain.forumSystem
{
    class DLObserverable
    {
        private IObserverable _commObserverable;
        private string _userID;
        private string _forum;

        public DLObserverable(IObserverable o, string userID, string forum)
        {
            _commObserverable = o;
            _userID = userID;
            _forum = forum;
        }

        public void send(string msg, List<string> usernames, string forum)
        {
            if (_forum.Equals(forum) && !_userID.Equals("guest") && usernames.Contains(_userID))
            {
                _commObserverable.send(msg);
            }
        }
    }
}
