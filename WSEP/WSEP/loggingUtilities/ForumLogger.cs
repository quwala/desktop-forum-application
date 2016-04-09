using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security;

using System.Threading.Tasks;


namespace WSEP_doamin.loggingUtilities
{
    public class ForumLogger
    {
       
        private string path;
        private string _text;
        public ForumLogger(string section)
        {
            path = section+"Log.txt";
            _text = "";
            if (File.Exists(path))//some concurrency issues here
                File.Delete(path);
        }

        public void log(string text)
        {
       
            File.AppendAllText(path, DateTime.Now.ToShortDateString() 
                + ", " + DateTime.Now.ToShortTimeString() +
                ": " + text+".\r\n");
            _text = File.ReadAllText(path);
        }

      

        public void logException(Exception e)
        {
            log("Exception caught: "+e.Message);
        }

        public string getLog()
        {
            return _text;
        }
    }
}
