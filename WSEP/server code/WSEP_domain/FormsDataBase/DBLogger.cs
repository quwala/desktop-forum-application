using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsDataBase
{
    class DBLogger
    {
        private const string TEXT = "Data Base log:";
        private string _filePath;

        public DBLogger()
        {
            DateTime dt = DateTime.Now;
            string fileName = "DB log " + dt.Day.ToString() + "_" + dt.Month.ToString() + "_" + dt.Year.ToString() + "_" + 
                dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + ".txt";
            _filePath = @"..\..\..\Logs\" + fileName;
            System.IO.File.WriteAllText(_filePath, TEXT + Environment.NewLine);
        }

        public void append(string s)
        {
            //s = s.Equals("true") ? "PASS: Opration succedded" : "FAIL: " + s;
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(_filePath, true))
            {
                DateTime dt = DateTime.Now;
                string toWrite = dt.Day.ToString() + "_" + dt.Month.ToString() + "_" + dt.Year.ToString() + "_" +
                dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + " " + s;
                file.WriteLine(s);
            }
        }
    }
}
