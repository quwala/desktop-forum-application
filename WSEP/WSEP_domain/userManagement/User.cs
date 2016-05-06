using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP_domain.userManagement
{
    public class User
    {
        private string _username;
        private string _password;
        private string _eMail;
        private DateTime _registered;
        private DateTime _lastPasswordEdit;
        private List<Tuple<string, List<PrivateMessage>>> _privateMessages;

        private const string SUCCESS = "true";
        private const string FUNCTION_ERRROR = "An error has occured with C# internal function.";
        private const string INVALID_USERNAME = "Invalid username. Username cannot be null, \"null\" or empty.";
        private const string INVALID_MESSAGE = "Invalid message. Message cannot be null, \"null\" or empty.";

        public static User create(string username, string password, string eMail)
        {
            List<string> input = new List<string>() { username, password, eMail };
            if (!Constants.isValidInput(input) || !Constants.isValidEmail(eMail))
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
            _registered = DateTime.Now.Date;
            _lastPasswordEdit = DateTime.Now.Date;
            _privateMessages = new List<Tuple<string, List<PrivateMessage>>>();
        }

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

        public DateTime getRegistrationDate()
        {
            return _registered;
        }

        public DateTime getDateOfLastPasswordChange()
        {
            return _lastPasswordEdit;
        }

        public void setUsername(string username)
        {
            // validate input
            _username = username;
        }

        public void setPassword(string password)
        {
            List<string> input = new List<string>() { password };
            if (Constants.isValidInput(input))
            {
                _password = password;
                _lastPasswordEdit = DateTime.Now.Date;
            }
        }

        public void setEMail(string eMail)
        {
            // validate input
            _eMail = eMail;
        }

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
            PrivateMessage pm = PrivateMessage.create(from, msg);
            if (pm == null)
            {
                return "Invalid input. Could not create a new private message.";
            }
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
            PrivateMessage pm = PrivateMessage.create(_username, msg);
            if (pm == null)
            {
                return "Invalid input. Could not create a new private message.";
            }
            conversation.Item2.Add(pm);
            if (!conversation.Item2.Contains(pm))
            {
                return FUNCTION_ERRROR;
            }
            return SUCCESS;
        }

        private string messagesInputValidation(string username, string msg)
        {
            if (username == null || username.Equals("null") || username.Equals("") || username.IndexOf(' ') == 0)
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
