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
       
        private string path;
        private string _text;

        public ForumLogger(string section)
        {
            path = section+"Log.txt";
            _text = "";

        }

        public void log(string text)
        {
            File.AppendAllText(path, DateTime.Now.ToShortDateString() 
                + ", " + DateTime.Now.ToShortTimeString() +
                ": " + text+"\n");
            _text = File.ReadAllText(path);
        }

      

        internal void logException(Exception e)
        {
            throw new NotImplementedException();
        }

        public string getLog()
        {
            return _text;
        }
    }
}
