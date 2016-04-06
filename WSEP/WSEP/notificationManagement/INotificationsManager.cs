using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.notificationManagement
{
    public interface INotificationsManager
    {
        // returns true if the email sent successfuly, otherwise false
        bool NotifyPost(string postResponser_username, string postOwner_username, string postOwner_email);
    }
}
