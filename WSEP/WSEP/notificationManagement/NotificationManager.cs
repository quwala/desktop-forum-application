using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WSEP.notificationManagement
{
    public class NotificationManager : INotificationsManager
    {
        private string systemsEmail;

        public NotificationManager()
        {
            systemsEmail = "magal@post.bgu.ac.il";
        }

        public bool NotifyPost(string postResponser_username, string postOwner_username, string postOwner_email)
        {
            if (postResponser_username == null || postOwner_username == null || postOwner_email == null)
            {
                Console.WriteLine("One of the parameters is null");
                return false;
            }
            if (postResponser_username == "" || postOwner_username == "" || postOwner_email == "")
            {
                Console.WriteLine("One of the parameters is empty");
                return false;
            }
            // ensures good email format by using regular expression
            if (!Regex.IsMatch(postOwner_email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
            {
                Console.WriteLine("Wrong email format");
                return false;
            }

            try
            {
                string Subject = "New Forums System Notification";
                string Body = "Hi " + postOwner_username + ", " + postResponser_username + " has comment your post in the Forums System";

                SmtpClient client = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(systemsEmail, "pjRpac5b"),
                    Timeout = 10000,
                };

                MailMessage mm = new MailMessage(systemsEmail, postOwner_email, Subject, Body);
                client.Send(mm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not send email: " + e.ToString());
                return false;
            }
            return true;
        }
    }
}
