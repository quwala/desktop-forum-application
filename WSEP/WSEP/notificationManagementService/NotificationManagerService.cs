using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.notificationManagementService
{
    class NotificationManagerService : INotificationManagerObserver
    {
        notificationManagement.INotificationsManager notificationManager;

        public NotificationManagerService()
        {
            notificationManager = new notificationManagement.NotificationManager();
        }

        public bool NotifyPost(string postResponser_username, string postOwner_username, string postOwner_email)
        {
            return notificationManager.NotifyPost(postResponser_username, postOwner_username, postOwner_email);
        }
    }
}
