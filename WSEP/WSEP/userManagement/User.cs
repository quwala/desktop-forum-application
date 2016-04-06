using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.userManagement
{
    public class User
    {

        private string _username;
        private string _password;
        private string _eMail;
        private List<Tuple<string, List<PrivateMessage>>> _privateMessages;

        private const string SUCCESS = "true";
        private const string FUNCTION_ERRROR = "An error has occured with C# internal function.";
        private const string INVALID_USERNAME = "Invalid username. Username cannot be null, \"null\" or empty.";
        private const string INVALID_MESSAGE = "Invalid message. Message cannot be null, \"null\" or empty.";

        public static User create(string username, string password, string eMail)
        {
            if (username == null || username.Equals("null") || username.Equals("") || username.IndexOf(' ') == 0)
            {
                return null;
            }
            if (password == null || password.Equals("null") || password.Equals("") || password.Contains(" "))
            {
                return null;
            }
            if (eMail == null || eMail.Equals("null") || eMail.Equals("") || !eMail.Contains("@") || eMail.IndexOf('@') == 0)
            {
                return null;
            }
            string eMailSuffix = eMail.Substring(eMail.IndexOf('@') + 1);
            if (eMailSuffix.Contains("@") || !eMailSuffix.Contains(".") || eMailSuffix.IndexOf('.') == 0 || eMailSuffix.IndexOf('.') == eMailSuffix.Length - 1)
            {
                return null;
            }
            return new User(username, password, eMail);
        }

        private User(string username, string password, string eMail)
        {
            _username = username;
            _password = password;
            _eMail = eMail;
            _privateMessages = new List<Tuple<string, List<PrivateMessage>>>();
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
            // validate input
            _username = username;
        }

        public void setPassword(string password)
        {
            // validate input
            _password = password;
        }

        public void setEMail(string eMail)
        {
            // validate input
            _eMail = eMail;
        }
        #endregion

        public string getMessage(string from, string msg)
        {
            string inputStatus = messagesInputValidation(from, msg);
            if (!inputStatus.Equals(SUCCESS))
            {
                return inputStatus;
            }
            // check if user has a conversation with the other user
            Tuple<string, List<PrivateMessage>> conversation = null;
            foreach (Tuple<string, List<PrivateMessage>> t in _privateMessages)
            {
                if (t.Item1.Equals(from))
                {
                    conversation = t;
                    break;
                }
            }
            if (conversation == null)
            {
                conversation = new Tuple<string, List<PrivateMessage>>(from, new List<PrivateMessage>());
            }
            PrivateMessage pm = new PrivateMessage(from, msg);
            conversation.Item2.Add(pm);
            if (!conversation.Item2.Contains(pm))
            {
                return FUNCTION_ERRROR;
            }
            return SUCCESS;
        }

        public string sendMessage(string to, string msg)
        {
            string inputStatus = messagesInputValidation(to, msg);
            if (!inputStatus.Equals(SUCCESS))
            {
                return inputStatus;
            }
            // check if user has a conversation with the other user
            Tuple<string, List<PrivateMessage>> conversation = null;
            foreach (Tuple<string, List<PrivateMessage>> t in _privateMessages)
            {
                if (t.Item1.Equals(to))
                {
                    conversation = t;
                    break;
                }
            }
            if (conversation == null)
            {
                conversation = new Tuple<string, List<PrivateMessage>>(to, new List<PrivateMessage>());
            }
            PrivateMessage pm = new PrivateMessage(_username, msg);
            conversation.Item2.Add(pm);
            if (!conversation.Item2.Contains(pm))
            {
                return FUNCTION_ERRROR;
            }
            return SUCCESS;
        }

        private string messagesInputValidation(string username, string msg)
        {
            if (username == null || username.Equals("null") || username.Equals(""))
            {
                return INVALID_USERNAME;
            }
            if (msg == null || msg.Equals("null") || msg.Equals(""))
            {
                return INVALID_MESSAGE;
            }
            return SUCCESS;
        }
    }
}
