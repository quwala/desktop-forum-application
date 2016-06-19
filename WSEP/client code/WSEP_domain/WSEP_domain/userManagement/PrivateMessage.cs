using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP_domain.userManagement
{
    public class PrivateMessage
    {
        private string _writer;
        private string _msg;
        private DateTime _time;

        public static PrivateMessage create(string writer, string msg)
        {
            List<string> input = new List<string>() { writer };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            if (msg == null || msg.Equals("null"))
            {
                return null;
            }
            return new PrivateMessage(writer, msg);
        }

        public static PrivateMessage create(string writer, string msg, DateTime creationTime)
        {
            List<string> input = new List<string>() { writer };
            if (!Constants.isValidInput(input))
            {
                return null;
            }
            if (msg == null || msg.Equals("null"))
            {
                return null;
            }
            return new PrivateMessage(writer, msg, creationTime);
        }

        private PrivateMessage(string writer, string msg)
        {
            _writer = writer;
            _msg = msg;
            _time = DateTime.Now;
        }

        private PrivateMessage(string writer, string msg, DateTime creationTime)
        {
            _writer = writer;
            _msg = msg;
            _time = creationTime;
        }

        public DateTime getCreationDate()
        {
            return _time;
        }
    }
}
