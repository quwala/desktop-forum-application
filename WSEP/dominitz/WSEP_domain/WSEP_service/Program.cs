using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP_domain.userManagement;

namespace WSEP_service
{
    class Program
    {
        static void Main(string[] args)
        {
            List<PrivateMessage> l = new List<PrivateMessage>();
            DateTime d1 = DateTime.Now;
            DateTime d2 = DateTime.Now.AddDays(7);
            DateTime d3 = DateTime.Now.AddMonths(1);
            PrivateMessage pm1 = PrivateMessage.create("user1", "message1", d1);
            PrivateMessage pm2 = PrivateMessage.create("user2", "message2", d2);
            PrivateMessage pm3 = PrivateMessage.create("user3", "message3", d3);
            l.Add(pm3);
            l.Add(pm1);
            l.Add(pm2);
            l.Sort((x, y) => x.getCreationDate().CompareTo(y.getCreationDate()));
            while (true)
            {

            }
        }
    }
}
