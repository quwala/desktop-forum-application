using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.notificationManagement
{
    class NotificationManager : INotificationsManager
    {
        private string systemsEmail;
        private string emailAddressIP;

        public NotificationManager()
        {
            systemsEmail = "magal@post.bgu.ac.il";
            emailAddressIP = "216.58.211.69"; // gmail.com IP Address
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

            string To = postOwner_email;
            string From = systemsEmail;
            string Subject = "New Forums System Notification";
            string Body = "Hi " + postOwner_username + ", " + postResponser_username + " has comment your post in the Forums System";

            // create the email message
            MailMessage completeMessage = new MailMessage(From, To, Subject, Body);

            // create smtp client at mail server location
            SmtpClient client = new SmtpClient(emailAddressIP);

            // add credentials
            client.UseDefaultCredentials = true;

            try
            {
                // send message
                client.Send(completeMessage);
            }
            catch (Exception)
            {
                Console.WriteLine("Problem occured while trying to send an email");
                return false;
            }
            return true;
        }
    }
}
