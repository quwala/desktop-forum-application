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
        static specialServerMessage ret1;

        static void Main(string[] args)
        {
            c.start();
            bool res = true;
            addForum("Cars");
            registerMemberToForum("Cars", "carsUser1", "carsPass1", "email1@gmail.com");
            registerMemberToForum("Cars", "carsUser2", "carsPass2", "email2@gmail.com");
            registerMemberToForum("Cars", "carsUser3", "carsPass3", "email3@gmail.com");
            registerMemberToForum("Cars", "carsUser4", "carsPass4", "email4@gmail.com");
            registerMemberToForum("Cars", "carsUser5", "carsPass5", "email5@gmail.com");
            registerMemberToForum("Cars", "carsUser6", "carsPass6", "email6@gmail.com");
            assignAdmin("Cars", "carsUser2", "superAdmin");
            mods.Add(new Tuple<string, string, int>("carsUser1", "", 7));
            mods.Add(new Tuple<string, string, int>("carsUser5", "", 3));
            addSubForum("Cars", "Mercedes", "carsUser2", mods);
            mods.Clear();
            sendPM("Cars", "carsUser1", "superAdmin", "Roie is a douche");
            mods.Add(new Tuple<string, string, int>("superAdmin", "", 20));
            addSubForum("Cars", "BMW", "superAdmin", mods);
            mods.Clear();

            ret1 = c.sendMessage();
            ret1.writeData();
            // ask for users permissions

            // ret = c.sendMessage2();
            // ret.writeData();
            // Console.WriteLine(res);
            while (true) { }
        }

        static bool addForum(string forumName)
        {
            strIn.Add(forumName);
            strIn.Add("superAdmin");
            ret = c.sendMessage("addForum", strIn, intIn, DateTime.Now);
            // ret.writeData();
            strIn.Clear();
            return ret._messageType == serverMessage.messageType.success;
        }

        static bool addSubForum(string forumName, string subForumName, string requestingUser, List<Tuple<string, string, int>> mods)
        {
            strIn.Add(forumName);
            strIn.Add(subForumName);
            strIn.Add(requestingUser);
            ret = c.sendMessage("addSubForum", strIn, intIn, DateTime.Now);
            // ret.writeData();
            strIn.Clear();
            return ret._messageType == serverMessage.messageType.success;
        }

        static bool registerMemberToForum(string forumName, string username, string password, string eMail)
        {
            strIn.Add(forumName);
            strIn.Add(username);
            strIn.Add(password);
            strIn.Add(eMail);
            ret = c.sendMessage("registerMemberToForum", strIn, intIn, DateTime.Now);
            // ret.writeData();
            strIn.Clear();
            return ret._messageType == serverMessage.messageType.success;
        }

        static bool assignAdmin(string forumName, string username, string requestingUser)
        {
            strIn.Add(forumName);
            strIn.Add(username);
            strIn.Add(requestingUser);
            ret = c.sendMessage("assignAdmin", strIn, intIn, DateTime.Now);
            // ret.writeData();
            strIn.Clear();
            return ret._messageType == serverMessage.messageType.success;
        }

        static bool sendPM(string forumName, string from, string to, string msg)
        {
            List<string> input = new List<string>() { forumName, from, to, msg };
            ret = c.sendMessage("sendPM", input, intIn, DateTime.Now);
            // ret.writeData();
            strIn.Clear();
            return ret._messageType == serverMessage.messageType.success;
        }
    }
}
