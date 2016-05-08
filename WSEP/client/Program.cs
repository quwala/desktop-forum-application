using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
//using System.Text;
using System.Net.Sockets;
using System.Web.Script.Serialization;
using System.Threading;


namespace client
{
    class Program
    {

        static client c = new client();
        static List<string> strIn = new List<string>();
        static List<int> intIn = new List<int>();
        static List<Tuple<string, string, int>> mods = new List<Tuple<string, string, int>>();
        static serverMessage ret;
        

        static void Main(string[] args)
        {
            c.start();
            bool res = true;
            c.addForum("Cars", "superAdmin");
            c.registerMemberToForum("Cars", "carsUser1", "carsPass1", "email1@gmail.com");
            c.registerMemberToForum("Cars", "carsUser2", "carsPass2", "email2@gmail.com");
            c.registerMemberToForum("Cars", "carsUser3", "carsPass3", "email3@gmail.com");
            c.registerMemberToForum("Cars", "carsUser4", "carsPass4", "email4@gmail.com");
            c.registerMemberToForum("Cars", "carsUser5", "carsPass5", "email5@gmail.com");
            c.registerMemberToForum("Cars", "carsUser6", "carsPass6", "email6@gmail.com");
            c.assignAdmin("Cars", "carsUser2", "superAdmin");
            mods.Add(new Tuple<string, string, int>("carsUser1", "", 7));
            mods.Add(new Tuple<string, string, int>("carsUser5", "", 3));
            c.addSubForum("Cars", "Mercedes", mods, "carsUser2");
            mods.Clear();
            c.sendPM("Cars", "carsUser1", "superAdmin", "Roie is a douche");
            mods.Add(new Tuple<string, string, int>("superAdmin", "", 20));
            c.addSubForum("Cars", "BMW", mods, "superAdmin");
            mods.Clear();

         
            while (true) { }
        }

      
    }
}
