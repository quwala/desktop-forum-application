using System;
using System.Collections.Generic;

namespace WSEP_domain.userManagement
{
    public class User
    {
        private string _username;
        private string _password;
        private string _eMail;
        private string _securityQuestion;
        private string _answer;
        private DateTime _registered;
        private DateTime _lastPasswordEdit;
        private List<Tuple<string, List<PrivateMessage>>> _privateMessages;

        private const string INVALID_USERNAME = "Invalid username. Username cannot be null, \"null\" or empty.";
        private const string INVALID_MESSAGE = "Invalid message. Message cannot be null, \"null\" or empty.";

        public static User create(string username, string password, string eMail, string securityQuestion, string answer)
        {
            List<string> input = new List<string>() { username, password, eMail, securityQuestion, answer };
            if (!Constants.isValidInput(input) || !Constants.isValidEmail(eMail))
            {
                return null;
            }
            return new User(username, password, eMail, securityQuestion, answer);
        }

        public static User create(string username, string password, string eMail, DateTime registration, 
            DateTime passwordChange, List<Tuple<string, List<PrivateMessage>>> PMs, string securityQuestion, string answer)
        {
            List<string> input = new List<string>() { username, password, eMail, securityQuestion, answer };
            if (!Constants.isValidInput(input) || !Constants.isValidEmail(eMail) || PMs == null)
            {
                return null;
            }
            return new User(username, password, eMail, registration, passwordChange, PMs, securityQuestion, answer);
        }

        private User(string username, string password, string eMail, string securityQuestion, string answer)
        {
            _username = username;
            _password = password;
            _eMail = eMail;
            _securityQuestion = securityQuestion;
            _answer = answer;
            _registered = DateTime.Now.Date;
            _lastPasswordEdit = DateTime.Now.Date;
            _privateMessages = new List<Tuple<string, List<PrivateMessage>>>();
        }

        private User(string username, string password, string eMail, DateTime registration,
            DateTime passwordChange, List<Tuple<string, List<PrivateMessage>>> PMs, string securityQuestion, string answer)
        {
            _username = username;
            _password = password;
            _eMail = eMail;
            _registered = registration;
            _lastPasswordEdit = passwordChange;
            _privateMessages = PMs;
            _securityQuestion = securityQuestion;
            _answer = answer;
        }

        public string getUsername()
        {
            return _username;
        }

        public string getPassword()
        {
            return _password;
        }

        public string getEMail()
        {
            return _eMail;
        }

        public string getSecurityQuestion()
        {
            return _securityQuestion;
        }

        public string getAnswer()
        {
            return _answer;
        }

        public void setPassword(string password)
        {
            if (Constants.isValidInput(new List<string>() { password }))
            {
                _password = password;
            }
        }

        public DateTime getRegistrationDate()
        {
            return _registered;
        }

        public DateTime getDateOfLastPasswordChange()
        {
            return _lastPasswordEdit;
        }

        public string getMessage(string from, string msg)
        {
            string inputStatus = messagesInputValidation(from, msg);
            if (!inputStatus.Equals(Constants.SUCCESS))
            {
                return inputStatus;
            }
            // check if user has a conversation with the other user
            bool found = false;
            Tuple<string, List<PrivateMessage>> conversation = null;
            foreach (Tuple<string, List<PrivateMessage>> t in _privateMessages)
            {
                if (t.Item1.Equals(from))
                {
                    conversation = t;
                    found = true;
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
                return "Invalid input. Could not create a new private message."; // cannot cover this case
            }
            conversation.Item2.Add(pm);
            if (!conversation.Item2.Contains(pm))
            {
                return Constants.FUNCTION_ERRROR; // cannot cover this case
            }
            if (!found)
            {
                _privateMessages.Add(conversation);
                if (!_privateMessages.Contains(conversation))
                {
                    return Constants.FUNCTION_ERRROR; // cannot cover this case
                }
            }
            return Constants.SUCCESS;
        }

        public string sendMessage(string to, string msg)
        {
            string inputStatus = messagesInputValidation(to, msg);
            if (!inputStatus.Equals(Constants.SUCCESS))
            {
                return inputStatus;
            }
            // check if user has a conversation with the other user
            bool found = false;
            Tuple<string, List<PrivateMessage>> conversation = null;
            foreach (Tuple<string, List<PrivateMessage>> t in _privateMessages)
            {
                if (t.Item1.Equals(to))
                {
                    conversation = t;
                    found = true;
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
                return "Invalid input. Could not create a new private message."; // cannot cover this case
            }
            conversation.Item2.Add(pm);
            if (!conversation.Item2.Contains(pm))
            {
                return Constants.FUNCTION_ERRROR; // cannot cover this case
            }
            if (!found)
            {
                _privateMessages.Add(conversation);
                if (!_privateMessages.Contains(conversation))
                {
                    return Constants.FUNCTION_ERRROR; // cannot cover this case
                }
            }
            return Constants.SUCCESS;
        }

        private string messagesInputValidation(string username, string msg)
        {
            if (!Constants.isValidInput(new List<string> { username }))
            {
                return INVALID_USERNAME;
            }
            if (msg == null || msg.Equals("null") || msg.Equals(""))
            {
                return INVALID_MESSAGE;
            }
            return Constants.SUCCESS;
        }
    }
}
