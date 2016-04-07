using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security;

using System.Threading.Tasks;


namespace WSEP.loggingUtilities
{
    public class ForumLogger
    {
        StreamWriter logFile;

        public ForumLogger(string section)
        {
            logFile = new System.IO.StreamWriter("/log" + section + ".txt", true);
        }

        public void log(string text)
        {
            logFile.WriteLine(DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToShortTimeString() + ": " + text);
        }

        public void close() { logFile.Close(); }

        internal void logException(Exception e)
        {
            throw new NotImplementedException();
        }
    }
}
