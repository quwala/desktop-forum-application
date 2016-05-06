using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace WSEP_domain
{
    public static class Constants
    {
        public const string NULL = "null";
        public const string EMPTY = "";
        private static readonly IList<string> ILLEGAL_VALUES = new ReadOnlyCollection<string> (new List<string>() { NULL, EMPTY });

        public const string NEWLINE = "\n";
        private static readonly IList<string> ILLEGAL_CONTENT = new ReadOnlyCollection<string> (new List<string>() { NEWLINE });

        public const char SPACE = ' ';
        private static readonly IList<char> ILLEGAL_STARTERS = new ReadOnlyCollection<char> (new List<char>() { SPACE });

        private const string SYSTEM_EMAIL = "gmrrstests@gmail.com";
        private static string SYSTEM_EMAIL_PASSWORD = "gmrrs123";

        public const string SUCCESS = "true";
        public const string INVALID_INPUT = "Invalid input.";
        public const string FUNCTION_ERRROR = "An error has occured with C# internal function.";
        public const string ILLEGAL_ACTION = "Illegal action due to forum policy.";
        public const string UNAUTHORIZED = "Unauthorized user.";

        public static void sendMail(string eMail, string subject, string content)
        {
            try
            {
                SmtpClient client = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(SYSTEM_EMAIL, SYSTEM_EMAIL_PASSWORD),
                    Timeout = 10000,
                };
                MailMessage mm = new MailMessage(SYSTEM_EMAIL, eMail, subject, content);
                client.Send(mm);
            }
            catch (Exception) { }
        }

        public static bool isValidEmail(string eMail)
        {
            List<string> input = new List<string>() { eMail };
            if (!isValidInput(input))
            {
                return false;
            }
            if (!eMail.Contains("@") || eMail.IndexOf('@') == 0)
            {
                return false;
            }
            string eMailSuffix = eMail.Substring(eMail.IndexOf('@') + 1);
            if (eMailSuffix.Contains("@") || !eMailSuffix.Contains(".") || eMailSuffix.IndexOf('.') == 0 || eMailSuffix.IndexOf('.') == eMailSuffix.Length - 1)
            {
                return false;
            }
            return true;
        }

        public static bool isValidInput(List<string> input)
        {
            foreach (string str in input)
            {
                if (str == null)
                {
                    return false;
                }
            }
            foreach (string str in input)
            {
                if (ILLEGAL_VALUES.Contains(str))
                {
                    return false;
                }
            }
            foreach (string str in input)
            {
                foreach (char bad in ILLEGAL_STARTERS)
                {
                    if (str.IndexOf(bad) == 0)
                    {
                        return false;
                    }
                }
                foreach (string bad in ILLEGAL_CONTENT)
                {
                    if (str.Contains(bad))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool isValidPost(string title, string content)
        {
            return !(title == null || content == null || title.Equals("null") || content.Equals("null") || (title.Equals("") && content.Equals("")));
        }

        public static string forumDoesntExist(string forumName)
        {
            return "Forum " + forumName + " doesn't exist.";
        }

        public static string noPermissionToDeletePost(string requestingUser)
        {
            return requestingUser + " does not have permission to delete posts in this forum.";
        }
    }
}
