using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.userManagement
{
    class User
    {

        private string _username;
        private string _password;
        private string _eMail;
        private List<Tuple<string, List<PrivateMessage>>> privateMessages;
        private UserManager _manager;

        public User(string username, string password, string eMail, UserManager manager)
        {
            _username = username;
            _password = password;
            _eMail = eMail;
        }

        #region getters
        public string getUsername()
        {
            return _username;
        }

        public string getPAssword()
        {
            return _password;
        }

        public string getEMail()
        {
            return _eMail;
        }
        #endregion

        #region setters
        public void setUsername(string username)
        {
            _username = username;
        }

        public void setPassword(string password)
        {
            _password = password;
        }

        public void setEMail(string eMail)
        {
            _eMail = eMail;
        }
        #endregion

        public bool sendPM(string forumName, string to, string msg)
        {
            if (_manager.sendPM(forumName, _username, to, msg))
            {
                // check if user has conversation with sendTo
                // if so simply add this message to the conversation
                // else create conversation and add this message
                return true;
            }
            return false;
        }

        public bool getPM(string from, string msg)
        {
            // check if user has a conversation with the sender
            // if so simply add this message to the conversation
            // else create conversation and add this message
            // verify message added to conversation
            // if so return true
            // else return false
            return false;
        }
    }
}
