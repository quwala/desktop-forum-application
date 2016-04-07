using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.userManagement
{
    class PrivateMessage
    {

        private string _writer;
        private string _msg;
        private DateTime _time;

        // add creator like user for input control

        public PrivateMessage(string writer, string msg)
        {
            _writer = writer;
            _msg = msg;
            _time = DateTime.Now;
        }

        public string toString()
        {
            return "";
        }
    }
}
